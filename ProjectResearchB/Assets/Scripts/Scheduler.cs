using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//スケジュール管理用のクラス
public class Scheduler : MonoBehaviour
{
    private static Scheduler instance;
    private List<Event> list = new List<Event>();

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.Log("eroor: scheduler already exists");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        while (list.Count > 0 && list[0].time < Time.time)
        {
            try
            {
                list[0].action();
            }
            catch (Exception e)
            {
                Debug.Log("error: Scheduler event");
            }
            finally
            {
                list.RemoveAt(0);
            }
        }
    }

    ///<summary>
    ///timeFromNow秒後にactionを実行する。
    ///</summary>    
    public static void AddEvent(Action action, float timeFromNow)
    {
        if (instance == null)
        {
            Debug.Log("error: scheduler instance");
            return;
        }

        int i;
        timeFromNow += Time.time;
        for (i = 0; i < instance.list.Count; i++)
        {
            if (instance.list[i].time > timeFromNow)
            {
                instance.list.Insert(i, new Event(timeFromNow, action));
                break;
            }
        }
        if (i == instance.list.Count)
            instance.list.Add(new Event(timeFromNow, action));
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
