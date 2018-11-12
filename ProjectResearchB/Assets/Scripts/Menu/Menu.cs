using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR;

public abstract class Menu : MonoBehaviour {    
    private float preUd = 0, preLr = 0;

    [SerializeField]
    protected bool udFlag, lrFlag;
    protected int selectLine;
    protected MenuManager manager;
    protected readonly float height = 1.17f;
    [SerializeField]
    protected RectTransform selecter;
    [SerializeField]
    protected int lineSize;
    [SerializeField]
    protected TextMeshPro title, text;    
    [SerializeField]
    protected Menu prePanel;
    [SerializeField]
    protected Menu[] nextPanels;    

    // Use this for initialization
    void Start () {
        manager = transform.parent.GetComponent<MenuManager>();
        lineSize--;
        if (!udFlag && !lrFlag) selecter.gameObject.SetActive(false);
        SetData();
    }

    // Update is called once per frame
    void Update()
    {
        if (udFlag)//選択行を変更
        {            
            var ud = Input.GetAxis("UpDn");
            if (ud != preUd)
            {
                if ((ud == 1 && selectLine != 0) || (ud == -1 && selectLine != lineSize))
                {
                    selecter.localPosition += new Vector3(0, 0, height * ud);
                    selectLine -= (int)ud;
                }
                preUd = ud;
            }
        }

        if (lrFlag)//値の変更
        {
            var lr = Input.GetAxis("LR");
            if (lr != preLr) 
            {
                if(Mathf.Abs(lr)==1) ChangeValue((int) lr);
                preLr = lr;
            }
        }
        
        if (SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.RightHand))//決定時
        {
            Debug.Log("fire1 pushed");
            Decide(selectLine);
        }

        if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Debug.Log("buck pushed");
            if (prePanel != null) manager.PanelChamge(this, prePanel);
        }
    }

    protected virtual void Decide(int index) { }//決定時の操作

    protected virtual void ChangeValue(int dir) { }//値の変更

    protected virtual void SetData() { }//データを更新
}