using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

//PCに関するメインのクラス
public class Player : Character
{
    public static Player instance;

    private bool battle = false;

    private void Start()
    {
        Player.instance = this;
    }

    private void Update()
    {       
       
    }    

    protected override void Move() { }

    protected override void Action() { }

    protected override void Death()
    {

    }
}