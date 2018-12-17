using System.Collections;
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

    // Use this for initialization
    void Start()
    {
        instance = this;
        itemList = (Resources.Load("DataList/ItemList") as ItemList).list;
        skillList = (Resources.Load("DataList/SkillList") as SkillList).list;
        magicList = (Resources.Load("DataList/MagicList") as MagicList).list;
        enemyDataList = (Resources.Load("DataList/EnemyDataList") as EnemyDataList).list;
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
            var pos = magics[index].GetPos();
            var rot = magics[index].GetRot();
            Debug.Log("pos: "+pos);
            Debug.Log("rot: "+rot.eulerAngles);
            return Instantiate(magics[index], pos, rot);
        }
        Debug.Log("index error: " + index);
        return null;
    }

    public SkillObject GenSkill(int index)
    {
        if (skills.Length > index && index >= 0)
        {
            var pos = skills[index].GetPos();
            var rot = skills[index].GetRot();
            return Instantiate(skills[index], pos, rot);
        }
        Debug.Log("index error: " + index);
        return null;
    }

    private void StartGameScene()
    {
        for (int i = 0; i < magicList.Length; i++)
            magicList[i].RegisterNode(i);
        Player.instance.RegisterActions(itemList, skillList, magicList);
    }
}
