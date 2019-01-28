using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool newGame = false;

    public Item[] itemList;
    public Skill[] skillList;
    public Magic[] magicList;
    public EnemyData[] enemyDataList;

    //ooder: attack attribute
    public Material[] materials;

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

    public void GameOver()
    {
        //リスポーンないしシーン切替
        Player.instance.GetStatus().RecoverHp(10000);
        Player.instance.transform.position = Vector3.zero;
        Debug.Log("Game Over");
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
