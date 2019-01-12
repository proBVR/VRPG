using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR;

public abstract class Menu : MonoBehaviour {    
    private float preUd = 0, preLr = 0;

    [SerializeField]
    protected bool udFlag, lrFlag;
    private int selectLine = 0;
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
    protected Vector3 basePosi;

    // Use this for initialization
    void Start () {
        basePosi = selecter.localPosition;
        manager = transform.parent.GetComponent<MenuManager>();
        lineSize--;
        if (!udFlag && !lrFlag) selecter.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    { 
        if (SteamVR_Input._default.inActions.Teleport.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            var posi = SteamVR_Input._default.inActions.TrackPosi.GetAxis(SteamVR_Input_Sources.RightHand);
            if (udFlag && Mathf.Abs(posi.y) > 0.5)//選択行を変更
            {
                if ((posi.y > 0 && selectLine != 0) || (posi.y < 0 && selectLine != lineSize))
                {
                    selecter.localPosition += new Vector3(0, 0, height * Mathf.Sign(posi.y));
                    selectLine -= (int)Mathf.Sign(posi.y);

                }
            }
            else if (lrFlag && Mathf.Abs(posi.x) > 0.5)//値の変更
            {
                ChangeValue((int)Mathf.Sign(posi.x));                    
            }
            else//決定時
            {
                Debug.Log("fire1 pushed");
                Decide(selectLine);
            }
        }

        if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            Debug.Log("back pushed");
            if (prePanel != null) manager.PanelChange(prePanel, false);
        }
    }

    protected virtual void Decide(int index) { }//決定時の操作

    protected virtual void ChangeValue(int dir) { }//値の変更

    public void Reset()
    {
        if (selecter.gameObject.activeSelf)
        {
            selecter.localPosition = basePosi;
            selectLine = 0;
        }
        SetData();
    }

    protected virtual void SetData() { }//データを更新
}