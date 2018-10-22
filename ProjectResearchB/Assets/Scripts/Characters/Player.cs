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

    [SerializeField]
    private bool move;

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
        UDPReceiver.RotCallBack += RotAction;
        UDPMove.AccelCallBack += AccelAction;
        UDPDirection.DirCallBack += DirAction;
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
        if (move == true)
        {
            double x = - Math.Sin((userDir.eulerAngles.y) * (Math.PI / 180));
            double y = - Math.Cos((userDir.eulerAngles.y) * (Math.PI / 180));
            userPosi = new Vector3((float)x,0,(float)y);
            userPosi = Quaternion.Euler(0, Vector3.Angle(transform.forward, Vector3.up), 0) * userPosi;
            transform.position += userPosi * moveSpeed;
            animator.SetBool("Running", true);
        }
        else animator.SetBool("Running", false);
        float cameraX = transform.position.x - transform.forward.x;
        float cameraY = transform.forward.y + 1.0f;
        float cameraZ = transform.position.z - transform.forward.z;
        transform.rotation = userRot;
        userCamera.transform.position = new Vector3(cameraX,cameraY,cameraZ);
        userCamera.transform.rotation = transform.rotation;
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

    //加速度に応じて移動フラグ変更
    public void AccelAction(float xx, float yy, float zz)
    {
        if (xx > 2) move = !move;
        //if((move == 0) && (xx > 2)){
        //    move = 1;
        //    Debug.Log(xx+ "for");
        //} else if ((move == 0) && (xx < -2)){
         //   move = -1;
         //   Debug.Log(xx+ "back");
        //}
        //else if ((move != 0) && (Math.Abs(zz) > 2)){
        //    move = 0;
        //    Debug.Log(zz+ "stop");
        //}
    }

    //回転によってカメラ方向を変更
    public void RotAction(float xx, float yy, float zz, float ww)
    {
        var newQut = new Quaternion(0, -zz, 0, ww);
        var newRot = newQut * Quaternion.Euler(0, 90f, 0);
        userRot = newRot;
    }

    //回転によって移動方向を変更
    public void DirAction(float xx, float yy, float zz, float ww)
    {
        var newQut2 = new Quaternion(0, -zz, 0, ww);
        var newDir = newQut2 * Quaternion.Euler(0, 90f, 0);
        userDir = newDir;
    }
}