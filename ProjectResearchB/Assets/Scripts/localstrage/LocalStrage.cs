using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;


public class LocalStrage : MonoBehaviour {

    //string myJson = "[{\"NUM\":\"1\",\"TEXT\":\"HELLO\"},{\"NUM\":\"2\",\"TEXT\":\"BONJOUR\"},]";
    string dataName = "data.txt";

    void Start()
    {
        LoadLocalStageData();
    }

    // ローカルストレージから読み込み
    public void LoadLocalStageData()
    {
        string savePath = GetPath();

        // ディレクトリが無い場合はディレクトリを作成して保存
        if (!Directory.Exists(savePath))
        {
            // ディレクトリ作成
            Directory.CreateDirectory(savePath);
            // ローカルに保存
            SaveToLocal(myJson);
        }
        else
        {
            // ローカルからデータを取得
            JsonNode json = LoadFromLocal();
            Debug.Log(json);
        }
    }

    // 保存
    void SaveToLocal(string json)
    {
        // jsonを保存
        File.WriteAllText(GetPath() + dataName, json);
    }

    // 取得
    JsonNode LoadFromLocal()
    {
        // jsonを読み込み
        string json = File.ReadAllText(GetPath() + dataName);
        JsonNode jnode = JsonNode.Parse(json);

        return jnode;
    }

    // パス取得
    string GetPath()
    {
        return Application.persistentDataPath + "/AppData/";
    }
}
