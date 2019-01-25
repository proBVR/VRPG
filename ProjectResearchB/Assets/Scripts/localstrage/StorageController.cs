using UnityEngine;
using System.Collections;
using Storage;



public class StorageController : MonoBehaviour {
    public const string INFO_FORMAT = "バージョン\t\t{0}\n" +
                                        "保存時刻\t\t{1}\n" +
                                        "保存回数\t\t{2}\n" +
                                        "Level: {3}" + 
		                                "length: {4}";


	private enum STATE {
		IDLE,
		SAVING,
		LOADING,
		ASYNC_WAIT,
		FINISH,
	}


	private StorageManager storageManager = null;	// ストレージ制御
	private StorageData usedSettings = null;		// 現在のデータ
	private StorageData procSettings = null;		// 処理中のデータ
	private FinishHandler ioHandler = null;			// ストレージアクセスコールバック
	private STATE state = STATE.IDLE;				// 処理状態
	private string accessMessage = "NOTHING";
	private IO_RESULT result = IO_RESULT.NONE;		// 結果
	private float ioTime = 0f;                      // 処理開始時刻
	private float accessTime = 0f;
    public Player player;
    private UserCamera ucamera;


    void Start() {

		this.ioHandler = new FinishHandler(this.IOHandler);
		this.storageManager = new StorageManager();
        this.usedSettings = this.procSettings = new StorageData();

		// 例外
		this.UpdateDataInfo((IO_RESULT)999);
	}


	void Update() {

		switch (this.state) {
			case STATE.IDLE:
				break;
			case STATE.ASYNC_WAIT:
				StartCoroutine(this.AsyncWait());
				this.state = STATE.FINISH;
				break;
			case STATE.FINISH:
				this.accessTime += Time.deltaTime;

                Debug.Log(this.accessMessage);
				int count = (int)(this.accessTime / 0.1f) % 4;
				for (int i = 0; i < count; ++i)
					Debug.Log(".");
				break;
		}
	}


	/// 完了コールバック
	private void IOHandler(IO_RESULT ret, ref DataInfo dataInfo) {
		this.result = ret;
		switch (ret) {
			case IO_RESULT.SAVE_SUCCESS:
				// 保存成功
				if (dataInfo.async) {
					this.state = STATE.ASYNC_WAIT;
					break;
				}
				this.UpdateDataInfo(ret);
				this.state = STATE.IDLE;
				break;
			case IO_RESULT.SAVE_FAILED:
				// 保存失敗
				if (dataInfo.async) {
					this.state = STATE.ASYNC_WAIT;
					break;
				}
				this.state = STATE.IDLE;
				break;
			case IO_RESULT.LOAD_SUCCESS:
				// 読込成功
				this.procSettings = dataInfo.serializer as StorageData;
				if (dataInfo.async) {
					this.state = STATE.ASYNC_WAIT;
					break;
				}
				this.UpdateDataInfo(ret);
				this.state = STATE.IDLE;
				break;
			case IO_RESULT.LOAD_FAILED:
				// 読込失敗
				if (dataInfo.async) {
					this.state = STATE.ASYNC_WAIT;
					break;
				}
				this.state = STATE.IDLE;
				break;
			case IO_RESULT.NONE:
				// データ不備
				this.state = STATE.IDLE;
				break;
		}
	}

	/// 非同期完了待ち
	private IEnumerator AsyncWait() {
		// 非同期最低待ち時間（※保存や読込のアニメーションを一定時間表示させるため）
		while ((Time.realtimeSinceStartup - this.ioTime) < 1.8f)
			yield return null;

		this.UpdateDataInfo(this.result);
	}

	/// データ情報更新
	private void UpdateDataInfo(IO_RESULT result) {
		StorageData us = this.usedSettings = this.procSettings;
		if (us.count > 0) {
            Debug.Log("datainfo: " + string.Format(INFO_FORMAT, us.version, System.DateTime.FromBinary(us.date).ToString(), us.count, us.level, us.length));
            Debug.Log("filepath: " + Application.persistentDataPath + us.fileName);
		} else {
            Debug.Log("datainfo: " + string.Format(INFO_FORMAT, "--", "-/-/---- --:--:--", "--", "-", "-"));
            Debug.Log("filepath: " + "----");
		}
		this.state = STATE.IDLE;
        var temp = new CharacterStatus("Player", 1000, 100, 100, 50, AttackAttribute.normal);
        switch (result) {
			case IO_RESULT.NONE:
				this.accessMessage = "NOTHING";
                player.Init(temp, 1);
				break;
			case IO_RESULT.SAVE_SUCCESS:
			case IO_RESULT.LOAD_SUCCESS:
                this.accessMessage = "SUCCESS";
                player.Init(temp, us.level);
                if(us.count != 0){
                    UserCamera.length = us.length;
                }
                break;
			case IO_RESULT.SAVE_FAILED:
			case IO_RESULT.LOAD_FAILED:
                this.accessMessage = "FAILED";
				break;
			default:
				this.accessMessage = "NOTHING";
				break;
		}
		Debug.Log(this.accessMessage);
	}


	/// 保存
	public void Save() {
		this.ioTime = Time.realtimeSinceStartup;
		this.accessTime = 0f;

		// 保存設定（※デモ用の設定変更）
		this.procSettings = this.usedSettings.Clone() as StorageData;
		StorageData us = this.procSettings;
		// 内容更新（適当に内容を更新）
		System.DateTime date = System.DateTime.Now;
		us.date = date.ToBinary();
		us.count += 1;
        us.level = player.GetLevel();
        us.length = UserCamera.length;

        // 保存（※FinishHandlerはnullでも可）
        bool async = true;
		if (async) {
			this.accessMessage = "Now Saving";
		}
		this.storageManager.Save(this.procSettings, this.ioHandler, async);
	}

	/// 読込
	public void Load() {
		this.ioTime = Time.realtimeSinceStartup;
		this.accessTime = 0f;

		// 読込
        bool async = true; // ture:非同期
		if (async) {
			this.accessMessage = "Now Loading";
		}
		this.storageManager.Load(this.usedSettings, this.ioHandler, async);
	}

	/// 削除
	public void Delete() {
		if (!this.storageManager.Exists(this.usedSettings)) {
			return;
		}

		this.storageManager.Delete(this.usedSettings);
		this.usedSettings.Clear();
		this.procSettings.Clear();
		this.UpdateDataInfo(IO_RESULT.NONE);
	}

	/// 初期化
	public void Clear() {
		if (this.usedSettings.count == 0 || !this.storageManager.Exists(this.usedSettings)) {
			return;
		}

		this.usedSettings.Clear();
		this.procSettings.Clear();
		this.UpdateDataInfo(IO_RESULT.NONE);
	}
}
