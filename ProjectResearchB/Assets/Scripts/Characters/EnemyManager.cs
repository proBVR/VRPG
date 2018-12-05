using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private Enemy[] models;
    //enemyの初期化用データ(from excel)

    private List<Enemy> enemies = new List<Enemy>();
    private int max=20, interval=30, same = 5;
    private float counter, sponeRange=25;
    private bool existPlayer = true;

	// Use this for initialization
	void Start () {
        counter = interval;
	}
	
	// Update is called once per frame
	void Update ()
    {
        counter += Time.deltaTime;
        if (counter > interval)
        {
            counter = 0;
            for (int i = 0; i < same && enemies.Count < max; i++)
            {
                Spone(0);
            }
        }
	}

    private void Spone(int index)
    {
        var x = Random.Range(-sponeRange, sponeRange);
        var y = Random.Range(-sponeRange, sponeRange);
        var deg = Random.Range(0, 360);
        var pos = new Vector3(x, 2, y);
        var rot = Quaternion.Euler(0, deg, 0);
        var temp = Instantiate(models[index], pos+transform.position, rot, this.transform);
        enemies.Add(temp);
    }

    public bool ExistPlayer()
    {
        return existPlayer;
    }
}
