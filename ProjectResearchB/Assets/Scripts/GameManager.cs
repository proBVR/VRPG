﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Item[] itemList;
    public Skill[] skillList;
    public Magic[] magicList;
    public EnemyData[] enemyDataList;

    private List<Player> players = new List<Player>();
    private short lives = 0;

    [SerializeField]
    private MagicObject[] magics;
    [SerializeField]
    private SkillObject[] skills;
    [SerializeField]
    private Enemy[] enemies;

    // Use this for initialization
    void Start()
    {
        instance = this;
        itemList = (Resources.Load("DataList/ItemList") as ItemList).list;
        skillList = (Resources.Load("DataList/SkillList") as SkillList).list;
        magicList = (Resources.Load("DataList/MagicList") as MagicList).list;
        enemyDataList = (Resources.Load("DataList/EnemyDataList") as EnemyDataList).list;
        StartGameScene();
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
        //リスポーンないしシーン切替
    }

    public MagicObject GenMagic(int index)
    {
        if (magics.Length > index && index >= 0)
        {
            return Instantiate(magics[index]);
        }
        Debug.Log("index error: " + index);
        return null;
    }

    public SkillObject GenSkill(int index)
    {
        if (skills.Length > index && index >= 0)
        {
            return Instantiate(skills[index]);
        }
        Debug.Log("index error: " + index);
        return null;
    }

    public Enemy GenEnemy(int index, Vector3 pos, Quaternion rot,Transform manager)
    {
        if (enemies.Length > index && index >= 0)
            return Instantiate(enemies[index], pos, rot, manager);
        Debug.Log("index error");
        return null;
    }

    private void StartGameScene()
    {
        var useAction = new List<IActionable>[Player.kindOfAction];
        useAction[0] = new List<IActionable>();
        useAction[1] = new List<IActionable>();
        useAction[2] = new List<IActionable>();

        foreach (IActionable action in itemList)
            useAction[0].Add(action);
        foreach (IActionable action in skillList)
            useAction[1].Add(action);
        foreach (IActionable action in magicList)
            useAction[2].Add(action);

        for (int i = 0; i < magicList.Length; i++)
            magicList[i].RegisterNode(i);
        Player.instance.RegisterActions(useAction);
    }
}
