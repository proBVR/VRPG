using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public sealed class StorageData : Storage.ISerializer {

    [System.NonSerialized]
    private const string magic_ = "StorageData_181212";
    [System.NonSerialized]
    private const string fileName_ = "/StorageData";

    public string magic { get { return StorageData.magic_; } }    // マジックNo.
    public int version { get { return 1; } }                      // バージョンNo.
    public string fileName { get { return StorageData.fileName_; } } // 保存先
    public System.Type type { get { return typeof(StorageData); } }   // JSONデシリアライズ用型宣言
    public Storage.FORMAT format { get { return Storage.FORMAT.JSON; } } // 保存形式
    public bool encrypt { get { return true; } }                  // 暗号化するか（任意）
    public bool backup { get { return true; } }

    public bool UpdateVersion(int oldVersion)
    {
        //switch (oldVersion) {
        //  case 1:
        //      break;
        //}
        return true;
    }

    /// 複製
    public Storage.ISerializer Clone()
    {
        return this.MemberwiseClone() as Storage.ISerializer;
    }

    //保存データの内容example
    public long date = 0L;  // 更新時間(64bit)
    public int count = 0;   // 更新回数
    public int level = 1;

    //簡易的な初期化
    public void Clear()
    {
        this.date = System.DateTime.MinValue.ToBinary();
        this.count = 0;
        this.level = 1;
    }
}
