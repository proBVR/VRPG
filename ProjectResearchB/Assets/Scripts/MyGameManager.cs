using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    public static MyGameManager instance;

    private List<Player> players = new List<Player>();
    private short lives = 0;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
        lives++;
    }

    public void DecLives()
    {
        lives--;
        if (lives == 0) GameOver();
    }

    private void GameOver()
    {

    }
}
