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

    public UserCamera userCamera;
    public UDPReceiver updReceiver;
    public UDPMove udpMove;
    public UDPDirection udpDirection;
    public Quaternion userRot;
    public Vector3 userPosi;
    public Quaternion userDir;
    public bool move;

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
        UDPReceiver.GyroCallBack += GyroAction;
        UDPMove.AccelCallBack += AccelAction;
        UDPDirection.GyroCallBack += GyroAction2;
        updReceiver.UDPStart();
        udpMove.UDPStart();
        udpDirection.UDPStart();
        move = false;
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
        if (move)
        {
            double x = Math.Sin((userDir.eulerAngles.y - 90f) * (Math.PI / 180));
            double y = Math.Cos((userDir.eulerAngles.y - 90f) * (Math.PI / 180));
            userPosi = new Vector3((float)x,0,(float)y);
            userPosi = Quaternion.Euler(0, Vector3.Angle(transform.forward, Vector3.up), 0) * userPosi;
            Debug.Log(userPosi);
            //phonePosi = transform.forward * moveSpeed;
            transform.localPosition = userPosi * moveSpeed;
            userCamera.transform.position = new Vector3(transform.position.x,1.2f,transform.position.z);
            transform.localPosition = new Vector3(0, -1.18f,0.2083f);
            animator.SetBool("Running", true);
        }
        else animator.SetBool("Running", false);
        userCamera.transform.localRotation = userRot;
        transform.rotation = userCamera.transform.localRotation;
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

    public void AccelAction(float xx, float yy, float zz)
    {
        if (xx > 2) move = !move;
    }

    public void GyroAction(float xx, float yy, float zz, float ww)
    {
        var newQut = new Quaternion(0, -zz, 0, ww);
        var newRot = newQut * Quaternion.Euler(0, 90f, 0);
        userRot = newRot;
    }

    public void GyroAction2(float xx, float yy, float zz, float ww)
    {
        var newQut2 = new Quaternion(0, -zz, 0, ww);
        var newDir = newQut2 * Quaternion.Euler(0, 90f, 0);
        userDir = newDir;
    }
}