using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private enum EnemyIdx
    {
        Boar,
        Zombie,
        CanonTower,
        Dragon
    }

    //[SerializeField]
    //private Enemy[] models;
    //enemyの初期化用データ(from excel)

    [SerializeField]
    private EnemyIdx[] sponeList;
    [SerializeField]
    private int max;

    private List<Enemy> enemies = new List<Enemy>();
    private int interval=30, same = 5;
    private float sponeRange=25;
    private bool existPlayer = true, ready = true;
	
	// Update is called once per frame
	void Update ()
    {
        if (ready)
        {
            ready = false;
            Scheduler.instance.AddEvent(interval, FinReady);
            for (int i = 0; i < same && enemies.Count < max; i++)
            {
                var index = Random.Range(0, sponeList.Length);
                Spone(index);
            }
        }
	}

    private void FinReady()
    {
        ready = true;
    }

    private void Spone(int index)
    {
        var x = Random.Range(-sponeRange, sponeRange);
        var y = Random.Range(-sponeRange, sponeRange);
        var deg = Random.Range(0, 360);
        var pos = new Vector3(x, 1, y);
        var rot = Quaternion.Euler(0, deg, 0);
        var temp = GameManager.instance.GenEnemy(index, pos+transform.position, rot, transform);
        temp.Init(GameManager.instance.enemyDataList[index].GenStatus(), 1);
        enemies.Add(temp);
    }

    public bool ExistPlayer()
    {
        return existPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("player in");
            existPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("player out");
            existPlayer = false;
        }
    }

    public void DecEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}
