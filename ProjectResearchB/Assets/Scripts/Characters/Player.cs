using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;

//PCに関するメインのクラス
public class Player : Character
{
    public static Player instance;

    public Inventory inventory = new Inventory();
    public bool acting = false;

    private bool modeFlag = false, menuFlag=false;
    private Animator animator;
    private IActionable[] actionList;
    private List<string>[] callNames = new List<string>[3];

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
        if (acting) return;
        acting = true;
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

    public void RegisterActions(List<Item> items, List<Skill> skills, List<Magic> magics)
    {
        var temp = new List<IActionable>();
        var names = new List<string>();
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
        vr.SetRecognition(names.ToArray(), items.Count);        
    }

    public Arm GetArm(bool right)
    {
        if (right) return ArmR.GetComponent<Arm>();
        return ArmL.GetComponent<Arm>();
    }
}