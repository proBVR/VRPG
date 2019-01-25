using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public static AudioSource decide_sound;
    public static AudioSource cancel_sound;


	// Use this for initialization
	void Start () {
        AudioSource[] sounds = GetComponents<AudioSource>();
		decide_sound = sounds[0];
		cancel_sound = sounds[1];
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
