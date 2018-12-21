using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//スケジュール管理用のクラス
public class Scheduler : MonoBehaviour
{
    public static Scheduler instance;
    private List<Event> list = new List<Event>();

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if(list[0].time > Time.time)
        {
            list[0].action();
            list.RemoveAt(0);
        }
	}

    //イベント追加(今からtime秒後にactionを実行する)
    public void AddEvent(float timeFromNow, Action action)
    {
        int i;
        timeFromNow += Time.time;
        for (i = 0; i < list.Count; i++)
        {
            if (list[i].time > timeFromNow)
            {
                list.Insert(i, new Event(timeFromNow, action));
                break;
            }
        }
        if (i == list.Count)
            list.Add(new Event(timeFromNow, action));
    }

    //イベントデータを入れるクラス
    private class Event
    {
        public float time;
        public Action action;

        public Event(float time, Action action)
        {
            this.time = time;
            this.action = action;
        }
    }
}
