using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public static AudioSource decide_sound;
    public static AudioSource cancel_sound;
    private static AudioSource normal_bgm;
    private static AudioSource battle_bgm;


	// Use this for initialization
	void Start () {
        AudioSource[] sounds = GetComponents<AudioSource>();
		decide_sound = sounds[0];
		cancel_sound = sounds[1];
        normal_bgm = sounds[2];
        battle_bgm = sounds[3];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
