using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceRecognition : MonoBehaviour
{
    public static VoiceRecognition instance;
    private KeywordRecognizer keyRecognizer;
    private readonly string[] category = {"アイテム", "スペル", "スキル"};
    private readonly string[] items = { "アイテム", "スペル", "スキル" };
    private readonly string[] magics = { "アイテム", "スペル", "スキル" };
    private readonly string[] skills = { "アイテム", "スペル", "スキル" };

    private int state = 0;
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
        keyRecognizer = new KeywordRecognizer(category);
        keyRecognizer.OnPhraseRecognized += OnPhraseRecognized;
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        switch (state)
        {
            case 0:
                state = Array.IndexOf<string>(category, args.text) + 1;
                break;
            case 1:
                state = 0;
                //Player.instance.UseItem();
                break;
            case 2:
                state = 0;
                //Player.instance.DoSkill();
                break;
            case 3:
                p.NextNode(0);
                //Player.instance.DoMagic();
                break;
        }
    }

    private void StateChange(int index)
    {

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
