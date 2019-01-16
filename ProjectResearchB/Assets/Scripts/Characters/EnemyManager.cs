using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private Enemy[] models;
    //enemyの初期化用データ(from excel)

    private List<Enemy> enemies = new List<Enemy>();
    private int max, interval, counter;
    private bool existPlayer = true;

	// Use this for initialization
	void Start () {
        counter = interval;
	}
	
	// Update is called once per frame
	void Update ()
    {
        counter--;
        if (counter == 0)
        {
            counter = interval;

        }
	}

    private void Spone(int index)
    {

    }

    public bool ExistPlayer()
    {
        return existPlayer;
    }
}
