﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading;

namespace Storage
{
    public enum IO_RESULT
    {
        NONE = 0,

        SAVE_SUCCESS = 1,
        SAVE_FAILED = -1,

        LOAD_SUCCESS = 10,
        LOAD_FAILED = -10,
    }

    public enum FORMAT
    {
        BINARY,
        JSON,
    }

    /// シリアライズするクラスで要求する設定インターフェイス
    public interface ISerializer
    {
        string magic { get; }       // マジックNo. ※見られてもいいものにする
        int version { get; }        // データver.
        string fileName { get; }    // 保存先
        FORMAT format { get; }      // 保存形式
        System.Type type { get; }   // JSONデシリアライズ用型宣言
        bool encrypt { get; }       // 暗号化するか
        bool backup { get; }        // バックアップを取るか

        bool UpdateVersion(int oldVersion); // 旧ver.からの更新
        ISerializer Clone();        // インスタンスの複製
    }

    public struct DataInfo
    {
        public ISerializer serializer;      // シリアライズクラス
        public string filePath;             // 保存先
        public FinishHandler finishHandler; // アクセス完了ハンドラ
        public bool async;                  // 非同期
    }

    /// 保存完了ハンドラ
    /// "ret"  結果
    /// "serializer"  保存クラス参照
    public delegate void FinishHandler(IO_RESULT ret, ref DataInfo dataInfo);


    /// セーブデータを保存・読込するクラス
    /// 保存形式はデフォルトはバイナリ
    public sealed class StorageManager
    {
        private const int HASH_SIZE = 16;   // MD5は128bit
        private const int SALT_SIZE = 32;   // ソルトの長さ(byte) ※AesManagedの要求値に固定
        private const int IV_SIZE = 16;     // 初期化ベクターの長さ(byte) ※AesManagedの要求値に固定
        private const int ITERATIONS = 50;  // 多いほど強度が高いが処理時間とトレードオフ
        private const string BACKUP_KEY = ".dup";       // バックアップファイル追加キー
        private volatile object sync = new object();    // 同期オブジェクト

        private readonly WaitCallback saveThreadHandler = null; // 保存処理デリゲートキャッシュ
        private readonly WaitCallback loadThreadHandler = null; // 読込処理デリゲートキャッシュ
        private readonly MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider(); // 改ざんチェック用ハッシュジェネレータ
        private readonly Rfc2898DeriveBytes deriveBytes = null;         // キー作成用乱数ジェネレータ
        private readonly byte[] mySalt = null;                          // 更新ソルトキャッシュ
        private readonly byte[] saltBuffer = new byte[SALT_SIZE];       // ソルト読込用バッファ
        private readonly byte[] ivBuffer = new byte[IV_SIZE];           // 初期化ベクター読込用バッファ
        private readonly BinaryFormatter bf = new BinaryFormatter();    // クラスシリアライザー

        private bool isAccessing = false;               // 保存中フラグ（必ずクリティカルセクションをはる）


        /// コンストラクタ
        public StorageManager()
        {
            // デリゲートキャッシュ（暗黙のアロケートの削減）
            this.saveThreadHandler = new WaitCallback(this.SaveThreadMain);
            this.loadThreadHandler = new WaitCallback(this.LoadThreadMain);
            // パスワードをPASSWORD等の変数名や名前で持つと抜かれやすいので注意
            // 今回ソルト値はアプリ起動毎に更新
            this.deriveBytes = new Rfc2898DeriveBytes("10cA1_IJUf_ZOUn118_DeM0", SALT_SIZE, ITERATIONS);
            this.mySalt = this.deriveBytes.Salt;
        }

        /// シリアライズ保存
        /// "saveInterface"  シリアライズするクラス
        /// "finishHandler"  終了ハンドラ（null指定可）
        /// "async"  非同期実行するか
        public void Save(ISerializer saveInterface, FinishHandler finishHandler, bool async = true)
        {
            DataInfo dataInfo = new DataInfo();
            dataInfo.serializer = saveInterface;
            dataInfo.filePath = Application.persistentDataPath + saveInterface.fileName;
            dataInfo.finishHandler = finishHandler;
            dataInfo.async = async;

            if (saveInterface == null || string.IsNullOrEmpty(saveInterface.fileName))
            {
                this.FinishAccessing(IO_RESULT.NONE, ref dataInfo);
                return;
            }

            // 非同期処理
            if (async)
                ThreadPool.QueueUserWorkItem(this.saveThreadHandler, dataInfo);
            else
                this.SaveThreadMain(dataInfo);
        }

        /// デシリアライズ読込
        /// "saveInterface"  デシリアライズするクラス
        /// "finishHandler"  終了ハンドラ（null指定可）
        /// "async"  非同期で実行するか
        public void Load(ISerializer loadInterface, FinishHandler finishHandler, bool async = true)
        {
            DataInfo dataInfo = new DataInfo();
            dataInfo.serializer = loadInterface;
            dataInfo.filePath = Application.persistentDataPath + loadInterface.fileName;
            dataInfo.finishHandler = finishHandler;
            dataInfo.async = async;

            if (loadInterface == null || string.IsNullOrEmpty(loadInterface.fileName) || !File.Exists(dataInfo.filePath))
            {
                this.FinishAccessing(IO_RESULT.NONE, ref dataInfo);
                return;
            }

            if (async)
                ThreadPool.QueueUserWorkItem(this.loadThreadHandler, dataInfo);
            else
                this.LoadThreadMain(dataInfo);
        }

        /// 削除
        /// "serializer"  シリアライズクラス
        public void Delete(ISerializer serializer)
        {
            // ロードしたら即削除
            File.Delete(Application.persistentDataPath + serializer.fileName);
        }

        /// ファイルがあるか
        /// "serializer"  シリアライズクラス
        public bool Exists(ISerializer serializer)
        {
            string path = Application.persistentDataPath + serializer.fileName;
            if (File.Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                // ※ファイル書き込み中にクラッシュすると0byteのファイルが出来る
                if (fi.Length > 0)
                    return true;
            }

            return false;
        }


        /// ファイルアクセス中か
        public bool IsAccessing()
        {
            // 複数リクエストが来てもいいように外部からの参照もロック
            lock (this.sync)
            {
                return this.isAccessing;
            }
        }


        /// ストレージアクセス終了処理
        /// "ret"  結果
        /// "dataInfo"  保存情報
        private void FinishAccessing(IO_RESULT ret, ref DataInfo dataInfo)
        {
            // 失敗時はバックアップを参照
            switch (ret)
            {
                case IO_RESULT.LOAD_FAILED:
                    if (!dataInfo.filePath.Contains(BACKUP_KEY))
                    {
                        string backupPath = dataInfo.filePath + BACKUP_KEY;
                        if (File.Exists(backupPath))
                        {
                            lock (this.sync)
                            {
                                this.isAccessing = false;
                            }

                            dataInfo.filePath = backupPath;
                            if (dataInfo.async)
                                ThreadPool.QueueUserWorkItem(this.loadThreadHandler, dataInfo);
                            else
                                this.LoadThreadMain(dataInfo);

                            return;
                        }
                    }
                    break;
                case IO_RESULT.SAVE_SUCCESS:
                    // バックアップ生成
                    if (dataInfo.serializer.backup)
                    {
                        try
                        {
                            File.Copy(dataInfo.filePath, dataInfo.filePath + BACKUP_KEY, true);
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError("BACKUP FAILED : " + dataInfo.filePath + "\n" + e.Message);
                        }
                    }
                    else
                    {
                        File.Delete(dataInfo.filePath + BACKUP_KEY);
                    }
                    break;
            }

            if (dataInfo.finishHandler != null)
                dataInfo.finishHandler(ret, ref dataInfo);

            lock (this.sync)
            {
                this.isAccessing = false;
            }
        }

        /// 保存処理
        private void SaveThreadMain(object state)
        {
            DataInfo dataInfo = (DataInfo)state;
            int size = 0;
            byte[] hash = null;
            byte[] binary = null;

            // 他のリクエスト消化待ち
            while (dataInfo.async && this.IsAccessing())
            {
                Thread.Sleep(30);
            }

            lock (this.sync)
            {
                this.isAccessing = true;
            }

            // 書き込むバイナリデータの作成
            try
            {
                using (MemoryStream inMs = new MemoryStream())
                {
                    switch (dataInfo.serializer.format)
                    {
                        case FORMAT.BINARY:
                            // シリアライズ
                            this.bf.Serialize(inMs, dataInfo.serializer);
                            break;
                        case FORMAT.JSON:
                            using (BinaryWriter bw = new BinaryWriter(inMs))
                            {
                                string json = JsonUtility.ToJson(dataInfo.serializer, true);
                                bw.Write(json);
                            }
                            break;
                    }
                    binary = inMs.ToArray();
                    //size = (int)inMs.Position;
                    size = binary.Length;
                    hash = this.md5.ComputeHash(binary);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("SERIALIZE FAILED\n" + e.Message);
                this.FinishAccessing(IO_RESULT.SAVE_FAILED, ref dataInfo);
                return;
            }

            // ストレージ書込
            try
            {
                using (FileStream outFs = File.Create(dataInfo.filePath))
                {
                    using (BinaryWriter writer = new BinaryWriter(outFs))
                    {
                        // ヘッダ書込
                        writer.Write(dataInfo.serializer.magic);
                        writer.Write(dataInfo.serializer.version);
                        writer.Write(size);
                        writer.Write(hash);

                        // 暗号化して書込
                        if (dataInfo.serializer.encrypt)
                        {
                            bool success = this.EncryptFile(outFs, binary, size);
                            // 暗号化失敗
                            if (!success)
                            {
                                this.FinishAccessing(IO_RESULT.SAVE_FAILED, ref dataInfo);
                                return;
                            }
                        }
                        // そのまま書込
                        else
                        {
                            writer.Write(binary);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("WRITE FAILED\n" + e.Message);
                this.FinishAccessing(IO_RESULT.SAVE_FAILED, ref dataInfo);
                return;
            }

            // 完了
            this.FinishAccessing(IO_RESULT.SAVE_SUCCESS, ref dataInfo);
        }

        /// 読込処理
        private void LoadThreadMain(object state)
        {
            DataInfo dataInfo = (DataInfo)state;
            int version = -1;
            int size = 0;
            byte[] hash = null;
            byte[] binary = null;
            bool encrypt = dataInfo.serializer.encrypt;
            FORMAT format = dataInfo.serializer.format;

            // 他のリクエスト消化待ち
            while (dataInfo.async && this.IsAccessing())
            {
                Thread.Sleep(30);
            }

            lock (this.sync)
            {
                this.isAccessing = true;
            }

            // ストレージ読込
            try
            {
                using (FileStream inFs = File.OpenRead(dataInfo.filePath))
                {
                    using (BinaryReader reader = new BinaryReader(inFs))
                    {
                        // ヘッダ読込
                        string magic = reader.ReadString();
                        // マジックNo.不一致
                        if (magic != dataInfo.serializer.magic)
                        {
                            Debug.LogWarning("CHANGED MAGIC NUMBER\n");
                            this.FinishAccessing(IO_RESULT.LOAD_FAILED, ref dataInfo);
                            return;
                        }
                        version = reader.ReadInt32();
                        size = reader.ReadInt32();
                        hash = reader.ReadBytes(HASH_SIZE);
                        binary = new byte[size];

                        // 復号化して読込
                        if (encrypt)
                        {
                            bool success = this.DecryptFile(inFs, binary, size);
                            // 復号化失敗
                            if (!success)
                            {
                                this.FinishAccessing(IO_RESULT.LOAD_FAILED, ref dataInfo);
                                return;
                            }
                        }
                        // そのまま読込
                        else
                        {
                            inFs.Read(binary, 0, size);
                        }
                    }

                    // ハッシュチェック
                    if (!this.CheckHash(this.md5.ComputeHash(binary), hash))
                    {
                        Debug.LogWarning("HASH MISMATCH\n");
                        this.FinishAccessing(IO_RESULT.LOAD_FAILED, ref dataInfo);
                        return;
                    }

                    // デシリアライズ
                    using (MemoryStream outMs = new MemoryStream(binary))
                    {
                        switch (format)
                        {
                            case FORMAT.BINARY:
                                dataInfo.serializer = this.bf.Deserialize(outMs) as ISerializer;
                                break;
                            case FORMAT.JSON:
                                using (BinaryReader br = new BinaryReader(outMs))
                                {
                                    string json = br.ReadString();
                                    System.Type type = dataInfo.serializer.type;
                                    dataInfo.serializer = JsonUtility.FromJson(json, type) as ISerializer;
                                }
                                break;
                        }
                    }

                    // バージョンチェック
                    int nowVersion = dataInfo.serializer.version;
                    if (nowVersion > version)
                    {
                        dataInfo.serializer.UpdateVersion(version);
                    }
                    else if (nowVersion < version)
                    {
                        Debug.LogError("DEGRADE VERSION\n" + version + "→" + nowVersion);
                        this.FinishAccessing(IO_RESULT.LOAD_FAILED, ref dataInfo);
                        return;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("READ FAILED\n" + e.Message);
                this.FinishAccessing(IO_RESULT.LOAD_FAILED, ref dataInfo);
                return;
            }

            // 完了
            this.FinishAccessing(IO_RESULT.LOAD_SUCCESS, ref dataInfo);
        }

        /// ファイルを暗号化して書き込む
        /// "outFs"  保存するファイルストリーム
        /// "bs"  保存するバイト列
        /// "size"  保存するサイズ
        private bool EncryptFile(Stream outFs, byte[] bs, int size)
        {
            bool success = true;

            using (AesManaged aes = new AesManaged())
            {
                try
                {
                    // 今回の起動時のソルトを準備
                    this.deriveBytes.Salt = this.mySalt;
                    this.deriveBytes.Reset();
                    // パスワードからキーの作成、ソルトは自動作成とする
                    outFs.Write(this.deriveBytes.Salt, 0, SALT_SIZE);
                    aes.Key = this.deriveBytes.GetBytes(aes.KeySize / 8);

                    // 初期化ベクターの生成
                    aes.GenerateIV();
                    outFs.Write(aes.IV, 0, IV_SIZE);

                    // 暗号化書き込み
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        using (CryptoStream cryptStrm = new CryptoStream(outFs, encryptor, CryptoStreamMode.Write))
                        {
                            cryptStrm.Write(bs, 0, size);
                        }//cryptStrm.Close();
                    }//encryptor.Dispose();
                }
                catch (System.Exception e)
                {
                    Debug.LogError("ENCRYPT FAILED\n" + e.Message);
                    success = false;
                }
            }//aes.Clear();

            return success;
        }

        /// ファイルを復号化して読み込む
        /// "inFs"  読み込むファイルストリーム
        /// "bs"  読み込むバイト列
        /// "size"  読み込むサイズ
        private bool DecryptFile(Stream inFs, byte[] bs, int size)
        {
            bool success = true;

            using (AesManaged aes = new AesManaged())
            {
                try
                {
                    inFs.Read(this.saltBuffer, 0, SALT_SIZE);
                    this.deriveBytes.Salt = this.saltBuffer;
                    this.deriveBytes.Reset();
                    aes.Key = this.deriveBytes.GetBytes(aes.KeySize / 8);

                    inFs.Read(this.ivBuffer, 0, IV_SIZE);
                    aes.IV = this.ivBuffer;

                    // 複合化読み込み
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        using (CryptoStream cryptStrm = new CryptoStream(inFs, decryptor, CryptoStreamMode.Read))
                        {
                            cryptStrm.Read(bs, 0, size);
                        }//cryptStrm.Close();
                    }//decryptor.Dispose();
                }
                catch (System.Exception e)
                {
                    Debug.LogError("DECRYPT FAILED\n" + e.Message);
                    success = false;
                }
            }//aes.Clear();

            return success;
        }

        /// ハッシュチェック
        /// returns  一致したか
        private bool CheckHash(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;

            int size = a.Length;
            for (int i = 0; i < size; ++i)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }
    }
}