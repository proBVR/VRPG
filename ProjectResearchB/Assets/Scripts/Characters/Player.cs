using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using Valve.VR;

//PCに関するメインのクラス
public class Player : Character
{
    public static Player instance;

    public Inventory inventory = new Inventory();
    public bool acting = false;

    private bool modeFlag = false, menuFlag=false;
    private Animator animator;
    private Rigidbody rb;
    private IActionable[] actionList;
    private List<string>[] callNames = new List<string>[3];

    public StorageController storagecont;
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
    private float moveAngle;

    [SerializeField]
    private VoiceRecognition vr;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private GameObject ArmR, ArmL, Menu, ContR, ContL;

    protected void Start()
    {
        instance = this;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
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
        moveAngle = 0;
        var temp = new CharacterStatus("Player", 1000, 100, 100, 50, AttackAttribute.normal);
        Init(temp);//仮のステータス
        //transform.localScale *= 1.2f;
    }

    private void Update()
    {        
        if (!menuFlag && SteamVR_Input._default.inActions.MenuAction.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            Debug.Log("change pushed");
            modeFlag = !modeFlag;
            ArmR.SetActive(modeFlag);
            ArmL.SetActive(modeFlag);
            ContR.SetActive(!modeFlag);
            ContL.SetActive(!modeFlag);
        }
        else if(!modeFlag && SteamVR_Input._default.inActions.MenuAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Debug.Log("menu pushed");
            menuFlag = !menuFlag;
            Menu.SetActive(menuFlag);
            if (menuFlag) Menu.GetComponent<MenuManager>().MenuReset();
        }

        if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            vr.StartRecognition();
        }
        else if (SteamVR_Input._default.inActions.InteractUI.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            vr.StopRecognition();
        }

        if(Input.GetKey(KeyCode.Space)){
            storagecont.Save();
            Debug.Log("save!");
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        if (move == true)
        {
            double x =  Math.Sin((/*userDir.eulerAngles.y +*/  moveAngle) * (Math.PI / 180));
            double y =  Math.Cos((/*userDir.eulerAngles.y +*/  moveAngle) * (Math.PI / 180));
            userPosi = new Vector3((float)x,0,(float)y);
            rb.velocity = userPosi * moveSpeed;
            animator.SetBool("Running", true);
        }
        else{
            rb.velocity = Vector3.zero;
            animator.SetBool("Running", false);
        }

        //var pivot = userCamera.transform.position - camera.position + new Vector3(0, 1.35f, 0.05f);
        //var pivot = userCamera.offset;
        //float cameraX = transform.position.x + pivot.x;
        //float cameraY = transform.position.y + pivot.y;
        //float cameraZ = transform.position.z + pivot.z;
        transform.rotation = userRot;
        //userCamera.transform.position = new Vector3(cameraX, cameraY, cameraZ);
        //userCamera.transform.rotation = transform.rotation;
    }

    void MovePosi(){
        rb.velocity = userPosi * moveSpeed;
    }

    protected override void Action(int index)
    {
        if (acting) return;
        acting = true;
        actionList[index].Use();
    }

    protected override void Death()
    {
        GameManager.instance.DecLives();
    }

    public void OpeAction(int index)
    {
        Action(index);
    }

    public List<string>[] GetNames()
    {
        return callNames;
    }

    public void RegisterActions(Item[] items, Skill[] skills, Magic[] magics)
    {
        var temp = new List<IActionable>();
        var names = new List<string>();

        for (int i = 0; i < 3; i++) callNames[i] = new List<string>();
        foreach (Item t in items)
        {
            temp.Add(t);
            callNames[0].Add(t.GetName());
            names.Add(t.GetName());
        }
        foreach (Skill t in skills)
        {
            temp.Add(t);
            callNames[1].Add(t.GetName());
            names.Add(t.GetName());
        }
        foreach (Magic t in magics)
        {
            temp.Add(t);
            callNames[2].Add(t.GetName());
        }

        actionList = temp.ToArray();
        vr.SetRecognition(names.ToArray(), items.Length+skills.Length);
        Debug.Log("register");
    }

    public Arm GetArm(bool right)
    {
        if (right) return ArmR.GetComponent<Arm>();
        return ArmL.GetComponent<Arm>();
    }

    //加速度に応じて移動フラグ変更
    public void AccelAction(float xx, float yy, float zz)
    {
        var max = MaxFloat(xx, yy, zz);
        if (EquFloat(max, Math.Abs(zz)))
        {
            if ((move == false) && (zz < -2))
            {
                move = true;
                moveAngle = 0;
            }
            else if ((move == false) && (zz > 2))
            {
                move = true;
                moveAngle = 180f;
            }
        }
        else if (EquFloat(max, Math.Abs(xx)))
        {
            if ((move == false) && (xx > 2)){
                move = true;
                moveAngle = 90f;
            }
            else if ((move == false) && (xx < -2)){
                move = true;
                moveAngle = 270f;
            }
        }
        else{
            if (yy > 1.0 && Math.Abs(xx) < 0.5 && Math.Abs(zz) < 0.5) move = false;
        }
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

    //加速度最大値方向特定用
    public float MaxFloat(float x, float y, float z){
        float max = Math.Abs(x);
        if (max < Math.Abs(y)) max = Math.Abs(y);
        if (max < Math.Abs(z)) max = Math.Abs(z);
        return max;
    }

    //floatイコール判定用
    public bool EquFloat(float xx, float yy){
        float test = 0.0001f;
        return (test > Math.Abs(xx - yy));
    }
}
