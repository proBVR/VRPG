using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;

//PCに関するメインのクラス
public class Player : Character
{
    public static Player instance;
    public static readonly int kindOfAction = 3;

    public readonly Inventory inventory = new Inventory();


    public bool acting = false;

    private bool modeFlag = false, menuFlag = false, speedFlag = false;
    private Animator animator;
    private Rigidbody rb;
    private List<IActionable>[] actionList;
    private int exp, nextExp = 10;
    private float expRate = 2;

    public UserCamera userCamera;
    public UDPReceiver updReceiver;
    public UDPMove udpMove;
    public UDPDirection udpDirection;
    public Quaternion userRot;
    public Vector3 userPosi;
    public Quaternion userDir;

    public UDPReceiver2 udpReceiver2;

    [SerializeField]
    private bool move;

    [SerializeField]
    private float moveAngle;

    [SerializeField]
    private VoiceRecognition vr;

    [SerializeField]
    private float moveSpeed;
    private float moveRate = 1;

    [SerializeField]
    private GameObject ArmR, ArmL, Menu, ContR, ContL;

    private bool listening = false;
    private float mvRate = 1;
    private Vector3 offset = Vector3.forward;

    public float dirOffset = 0, angle = 0;

    [SerializeField]
    private Transform pivot;

    [SerializeField]
    private StorageController storage;



    protected void Start()
    {
        storage.Load();
        userCamera.LoadWidth();
        userCamera.HeightReset();
        IsPlayer = true;
        instance = this;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        actionList = new List<IActionable>[kindOfAction];
        Menu.SetActive(false);
        ArmR.SetActive(false);
        ArmL.SetActive(false);
        ContR.SetActive(true);
        ContL.SetActive(true);
        //UDPReceiver.RotCallBack += RotAction;
        //UDPMove.AccelCallBack += AccelAction;
        //UDPDirection.DirCallBack += DirAction;
        //updReceiver.UDPStart();
        //udpMove.UDPStart();
        //udpDirection.UDPStart();
        UDPReceiver2.AccelCallBack += AccelAction;
        UDPReceiver2.RotCallBack += RotAction;
        udpReceiver2.UDPStart();

        move = false;
        moveAngle = 0;
        var temp = new CharacterStatus("Player", 1000, 100, 100, 50, AttackAttribute.normal);
        Init(temp, 1);//仮のステータス
        //transform.localScale *= 1.2f;
        luRate = 1.1f;
    }

    private void Update()
    {
        //test.rotation = testQua;
        if (!menuFlag && SteamVR_Input._default.inActions.MenuAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            //Debug.Log("change pushed");
            modeFlag = !modeFlag;
            ArmR.SetActive(modeFlag);
            ArmL.SetActive(modeFlag);
            ContR.SetActive(!modeFlag);
            ContL.SetActive(!modeFlag);
        }
        else if (!modeFlag && SteamVR_Input._default.inActions.MenuAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            //Debug.Log("menu pushed");
            if (!listening)
            {
                menuFlag = !menuFlag;
                Menu.SetActive(menuFlag);
                if (menuFlag) Menu.GetComponent<MenuManager>().MenuReset();
            }
        }

        if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            if (!menuFlag)
            {
                vr.StartRecognition();
                listening = true;
            }
        }
        else if (SteamVR_Input._default.inActions.InteractUI.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            vr.StopRecognition();
            listening = false;
        }

        if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            speedFlag = true;
        }
        else if (SteamVR_Input._default.inActions.InteractUI.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            speedFlag = false;
        }
    }

    void FixedUpdate()
    {
        if (acting)
        {
            //Debug.Log("acting");
            rb.velocity = Vector3.zero;
            animator.SetBool("Running", false);
            return;
        }
        Move();
    }

    protected override void Move()
    {
        if (move == true)
        {
            double x = Math.Sin((/*userDir.eulerAngles.y +*/  moveAngle) * (Math.PI / 180));
            double y = Math.Cos((/*userDir.eulerAngles.y +*/  moveAngle) * (Math.PI / 180));
            var dir = Quaternion.AngleAxis(moveAngle, Vector3.up) * transform.forward;
            userPosi = new Vector3((float)x, 0, (float)y);
            rb.velocity = dir * moveSpeed * (speedFlag ? 0.5f : 1);
            animator.SetBool("Running", true);
        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("Running", false);
        }

        //var pivot = userCamera.transform.position - camera.position + new Vector3(0, 1.35f, 0.05f);
        //var pivot = userCamera.offset;
        //float cameraX = transform.position.x + pivot.x;
        //float cameraY = transform.position.y + pivot.y;
        //float cameraZ = transform.position.z + pivot.z;
        if (Input.GetButtonDown("Fire1"))
            Debug.Log("angle: " + (int)angle);
        transform.eulerAngles = Vector3.up * (angle - dirOffset);
        pivot.eulerAngles = Vector3.zero;
        //userCamera.transform.position = new Vector3(cameraX, cameraY, cameraZ);
        //userCamera.transform.rotation = transform.rotation;
    }

    void MovePosi()
    {
        rb.velocity = userPosi * moveSpeed;
    }

    protected override void Action(int index)
    {
        if (acting) return;

        for (int i = 0; i < actionList.Length; i++)
        {
            if (actionList[i].Count <= index) index -= actionList[i].Count;
            else
            {
                var action = actionList[i][index];
                switch (i)
                {
                    case 0:
                        if (inventory.IsInclude(action.GetName()))
                        {
                            inventory.DecInventory(action.GetName());
                            action.Use(this);
                            acting = true;
                        }
                        else Debug.Log("you dont have this item");
                        break;
                    case 1:
                    case 2:
                        var cost = action.GetCost();
                        if (status.UseMp(cost))
                        {
                            action.Use(this);
                            acting = true;
                        }
                        else Debug.Log("mp shotage");
                        break;
                }
                break;
            }
        }
    }

    protected override void Death()
    {
        exp -= 10;
        if (exp < 0) exp = 0;
        GameManager.instance.GameOver();
    }

    public void OpeAction(int index)
    {
        Action(index);
    }

    public List<string>[] GetNames()
    {
        var actionNames = new List<string>[kindOfAction];
        for (int i = 0; i < kindOfAction; i++)
        {
            foreach (IActionable action in actionList[i])
            {
                actionNames[i].Add(action.GetName());
            }
        }
        return actionNames;
    }

    public void RegisterActions(List<IActionable>[] actionList)
    {
        this.actionList = actionList;
        vr.SetRecognition(actionList);
        //Debug.Log("register");
    }

    public Arm GetArm(bool right)
    {
        if (right) return ArmR.GetComponent<Arm>();
        return ArmL.GetComponent<Arm>();
    }

    public List<IActionable> GetAction(int index)
    {
        return actionList[index];
    }

    public void AddExp(int add)
    {
        if (level >= maxLevel) return;
        exp += add;
        if (exp >= nextExp)
        {
            level++;
            status.LevelUp(luRate);
            exp -= nextExp;
            nextExp = (int)(nextExp * expRate);
        }
    }

    public int GetExp()
    {
        return exp;
    }

    public int GetNextExp()
    {
        return nextExp;
    }

    //加速度に応じて移動フラグ変更
    public void AccelAction(float xx, float yy, float zz)
    {
        if (move && yy > 1.5 && Math.Abs(xx) < 0.5f && Math.Abs(zz) < 0.5f) move = false;
        else if(!move)
        {
            if(Mathf.Abs(xx) < Mathf.Abs(zz))
            {
                if(zz < -1.8f)
                {
                    move = true;
                    moveAngle = 0;
                }
                else if(zz > 1.8f)
                {
                    move = true;
                    moveAngle = 180;
                }
            }
            else
            {
                if(xx < -2f)
                {
                    move = true;
                    moveAngle = 90;
                }
                else if(xx > 2f)
                {
                    move = true;
                    moveAngle = 270;
                }
            }
        }

        //Debug.Log("accel");
        //var max = MaxFloat(xx, yy, zz);
        //if (EquFloat(max, Math.Abs(zz)))
        //{
        //    if ((move == false) && (zz < -2))
        //    {
        //        move = true;
        //        moveAngle = 0;
        //    }
        //    else if ((move == false) && (zz > 2))
        //    {
        //        move = true;
        //        moveAngle = 180f;
        //    }
        //}
        //else if (EquFloat(max, Math.Abs(xx)))
        //{
        //    if ((move == false) && (xx > 2))
        //    {
        //        move = true;
        //        moveAngle = 90f;
        //    }
        //    else if ((move == false) && (xx < -2))
        //    {
        //        move = true;
        //        moveAngle = 270f;
        //    }
        //}
        //else
        //{
        //    if (yy > 1.0 && Math.Abs(xx) < 0.5 && Math.Abs(zz) < 0.5) move = false;
        //}
    }

    public void ResetRot()
    {
        var dir = userCamera.mycamera.transform.forward;
        dir.y = 0;
        transform.forward = dir;
        dirOffset = angle - transform.eulerAngles.y;
    }

    //回転によってカメラ方向を変更
    public void RotAction(float xx, float yy, float zz, float ww)
    {
        //Debug.Log("rot");
        var newQut = new Quaternion(0, -zz, 0, ww);
        var newRot = newQut * Quaternion.Euler(0, 90f, 0);
        //userRot = newRot;
        angle = newRot.eulerAngles.y;
        //testQua = newRot;
    }

    //回転によって移動方向を変更
    public void DirAction(float xx, float yy, float zz, float ww)
    {
        //var newQut2 = new Quaternion(0, -zz, 0, ww);
        //var newDir = newQut2 * Quaternion.Euler(0, 90f, 0);
        //userDir = newDir;
        var qua = new Quaternion(xx, yy, zz, ww);
        var eul = qua.eulerAngles;
        dirOffset = eul.y;
    }

    //加速度最大値方向特定用
    public float MaxFloat(float x, float y, float z)
    {
        float max = Math.Abs(x);
        if (max < Math.Abs(y)) max = Math.Abs(y);
        if (max < Math.Abs(z)) max = Math.Abs(z);
        return max;
    }

    //floatイコール判定用
    public bool EquFloat(float xx, float yy)
    {
        float test = 0.0001f;
        return (test > Math.Abs(xx - yy));
    }
}