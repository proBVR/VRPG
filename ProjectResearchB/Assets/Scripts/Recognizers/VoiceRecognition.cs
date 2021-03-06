﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{
    public static VoiceRecognition instance;
    private KeywordRecognizer[] recognizers = new KeywordRecognizer[2];
    private KeywordRecognizer keyRecognizer;
    private readonly string[] category = {"アイテム", "スキル", "マジック"};
    private readonly string[] magic = { "アズール", "グリス", "ネグロ", "ブラッタ", "ベルデ", "モラド" };
    //{ "ワン", "ツー", "スリー", "フォー", "ファイブ", "シックス"};
    //private string[] names;
    private string[] keyword;

    private int state = 0, pivot=6;
    private readonly int categoryCount = 3;
    private Node p;

    /*
    state0: 初期状態
    state1: アイテム, スキル
    state2: マジック        
    */

    void Start()
    {
        instance = this;
        recognizers[1] = new KeywordRecognizer(magic);
        recognizers[1].OnPhraseRecognized += OnPhraseRecognized;
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Debug.Log("recognize(" + state + "): " + args.text);
        int index;
        switch (state)
        {            
            case 0:
                index = Array.IndexOf<string>(keyword, args.text);
                if (index < 2){
                    state = 1;
                    SoundController.decide_sound.PlayOneShot(SoundController.decide_sound.clip);
                }
                else if (index == 2)
                {
                    SoundController.decide_sound.PlayOneShot(SoundController.decide_sound.clip);
                    state = 2;
                    p = Node.root;
                    keyRecognizer.Stop();
                    recognizers[1].Start();
                }
                break;
            case 1:
                index = Array.IndexOf<string>(keyword, args.text);
                if (index > 2){
                    Player.instance.OpeAction(index - 3);
                    SoundController.decide_sound.PlayOneShot(SoundController.decide_sound.clip);
                }
                state = 0;
                break;
            case 2:
                index = Array.IndexOf<string>(magic, args.text);
                p = p.NextNode(index);
                if (p == null) p = Node.root;
                else if (p.GetMagic() != -1)
                {
                    SoundController.decide_sound.PlayOneShot(SoundController.decide_sound.clip);
                    Debug.Log("magic: "+p.GetMagic()+", "+pivot);
                    Player.instance.OpeAction(p.GetMagic() + pivot);
                    recognizers[1].Stop();
                    state = 0;
                }else{
                    SoundController.decide_sound.PlayOneShot(SoundController.decide_sound.clip);
                }
                break;
        }
    }

    public void StartRecognition()
    {
        if (!keyRecognizer.IsRunning) keyRecognizer.Start();
        state = 0;
        Debug.Log("start recognition");
        //p = Node.root;
    }

    public void StopRecognition()
    {
        if (keyRecognizer.IsRunning) keyRecognizer.Stop();
        Debug.Log("stop recognition");
    }

    public void SetRecognition(List<IActionable>[] actionList)
    {
        pivot = actionList[0].Count + actionList[1].Count;

        var temp = new List<string>();
        for (int i = 0; i < 2; i++)
            foreach (IActionable action in actionList[i])
                temp.Add(action.GetName());
        keyword = new string[category.Length + temp.Count];
        category.CopyTo(keyword, 0);
        temp.CopyTo(keyword, category.Length);

        keyRecognizer = new KeywordRecognizer(keyword);
        keyRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        foreach (string i in keyword) Debug.Log(i);
    }
}
