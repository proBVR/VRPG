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

    public UDPReceiver updReceiver;
    public UDPMove udpMove;
    public UDPDirection udpDirection;
    public Quaternion phoneRot;
    public Vector3 phonePosi;
    public Quaternion phoneDir;
    bool move;

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
        /*
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
        */

        if (move)
        {
            double x = Math.Sin(phoneDir.eulerAngles.y * (Math.PI / 180));
            double y = Math.Cos(phoneDir.eulerAngles.y * (Math.PI / 180));
            phonePosi = new Vector3((float)x,0,(float)y);
            phonePosi = Quaternion.Euler(0, Vector3.Angle(transform.forward, Vector3.up), 0) * phonePosi;
            Debug.Log(phonePosi);
            //phonePosi = transform.forward * moveSpeed;
            transform.position += phonePosi * moveSpeed;
            animator.SetBool("Running", true);
            //Debug.Log(phonePosi);
        }
        else animator.SetBool("Running", false);
        //transform.localRotation = phoneRot;
        //Debug.Log(Vector3.Angle(transform.forward, Vector3.up));
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
        if (xx > 2)
        {
            move = !move;
            Debug.Log(xx);
        }
    }

    public void GyroAction(float xx, float yy, float zz, float ww)
    {
        var newQut = new Quaternion(0, -zz, 0, ww);
        var newRot = newQut * Quaternion.Euler(0, 90f, 0);
        phoneRot = newRot;
    }

    public void GyroAction2(float xx, float yy, float zz, float ww)
    {
        var newQut2 = new Quaternion(0, -zz, 0, ww);
        var newRot2 = newQut2 * Quaternion.Euler(0, 90f, 0);
        phoneDir = newRot2;
        //Debug.Log(phoneDir.eulerAngles);
    }
}