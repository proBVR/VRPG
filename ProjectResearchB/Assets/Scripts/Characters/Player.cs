using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

//PCに関するメインのクラス
public class Player : Character
{
    public static Player instance;

    private bool modeFlag = false, menuFlag=false;
    private Animator animator;
    private IActionable[] actionList;
    private List<string>[] callNames = new List<string>[3];

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private GameObject ArmR, ArmL, Menu, ContR, ContL;

    protected override void Start()
    {
        base.Start();
        instance = this;
        animator = GetComponent<Animator>();
        Menu.SetActive(false);
        ArmR.SetActive(false);
        ArmL.SetActive(false);
        ContR.SetActive(true);
        ContL.SetActive(true);        
    }

    private void Update()
    {
        if (!menuFlag && Input.GetButtonDown("Change"))
        {
            modeFlag = !modeFlag;
            ArmR.SetActive(modeFlag);
            ArmL.SetActive(modeFlag);
            ContR.SetActive(modeFlag);
            ContL.SetActive(modeFlag);
        }
        else if(!modeFlag && Input.GetButtonDown("Menu"))
        {
            menuFlag = !menuFlag;
            Menu.SetActive(menuFlag);
        }
        Move();
    }    

    protected override void Move()
    {
        var vector = ImageRecognition.instance.GetDirection();
        if (vector != Vector3.zero)
        {
            vector = Quaternion.Euler(0, Vector3.Angle(transform.forward, Vector3.up), 0) * vector;
            transform.position += vector * moveSpeed;
            transform.forward = vector;
            animator.SetBool("Running", true);
            //Debug.Log("dir: " + vector);
        }
        else animator.SetBool("Running", false);        
    }

    protected override void Action(int index)
    {
        actionList[index].Use();
    }

    protected override void Death()
    {
        MyGameManager.instance.DecLives();
    }

    public void OpeAction(int index)
    {
        Action(index);
    }

    public List<string>[] GetNames()
    {
        return callNames;
    }
}