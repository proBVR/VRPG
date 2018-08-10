using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character {
    protected int counter;
    [SerializeField]
    protected int interval;

	// Use this for initialization
	void Start () {
        Initialize();
        counter = interval;
	}
	
	// Update is called once per frame
	void Update () {
        counter--;
        if (counter == 0)
        {
            Action();
            counter = interval;
        }
        Move();
	}
}
