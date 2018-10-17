using System;
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
    private readonly string[] keyword = {"アイテム", "スペル", "スキル"};
    private readonly string[] items = { "アイテム", "スペル", "スキル" };
    private readonly string[] magics = {"いち", "に", "さん", "し", "ご", "ろく" };
    private readonly string[] skills = { "アイテム", "スペル", "スキル" };

    private int state = 0, pivot=5;
    private Node p;
    /*
    state0: 初期状態
    state1: アイテム
    state2: スキル
    state3: マジック        
    */

    void Start()
    {
        instance = this;
        keyRecognizer = new KeywordRecognizer(keyword);
        keyRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        recognizers[1] = new KeywordRecognizer(magics);
        recognizers[1].OnPhraseRecognized += OnPhraseRecognized;
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        int index;
        switch (state)
        {            
            case 0:
                index = Array.IndexOf<string>(keyword, args.text);
                if (index < 2) state = 1;
                else if (index == 2) state = 2;
                break;
            case 1:
                index = Array.IndexOf<string>(keyword, args.text);
                if (index > 2) Player.instance.OpeAction(index);
                state = 0;
                break;
            case 2:
                index = Array.IndexOf<string>(magics, args.text);
                p = p.NextNode(index);
                if (p == null) p = Node.root;
                else if (p.GetMagic() != -1)
                {
                    Player.instance.OpeAction(p.GetMagic() + pivot);
                    state = 0;
                }
                break;
        }
    }

    public void StartRecognition()
    {
        if (!keyRecognizer.IsRunning) keyRecognizer.Start();
        state = 0;
        p = Node.root;
    }

    public void StopRecognition()
    {
        if (keyRecognizer.IsRunning) keyRecognizer.Stop();
    }
}
