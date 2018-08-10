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
    [SerializeField]
    private string[] keywords;    

    void Start()
    {
        instance = this;
        keyRecognizer = new KeywordRecognizer(keywords);
        keyRecognizer.OnPhraseRecognized += OnPhraseRecognized;
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args) { }

    public void StartRecognition()
    {
        if (!keyRecognizer.IsRunning) keyRecognizer.Start();
    }

    public void StopRecognition()
    {
        if (keyRecognizer.IsRunning) keyRecognizer.Stop();
    }
}
