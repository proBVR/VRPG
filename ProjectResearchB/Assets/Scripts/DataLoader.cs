using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    private enum Rank { high, middle, low}
    private enum Power { high, middle, low}

    private List<Item> items = new List<Item>();
    private List<Magic> magics = new List<Magic>();
    private List<Skill> skills = new List<Skill>();

	// Use this for initialization
	void Start () {
        items.Add(new Item("ポーション", 1, 200, ItemMethods.uses[0]));
        items.Add(new Item("エリクサー", 2, 100, ItemMethods.uses[1]));

        skills.Add(new Skill("ハヤブサ切り", 0, 200));
        skills.Add(new Skill("魔人剣", 1, 500));

        magics.Add(new Magic("メラ", 0, AttackAttribute.fire, 0, 1));
        magics.Add(new Magic("メラゾーマ", 2, AttackAttribute.fire, 0, 3));
        magics.Add(new Magic("メラゾーマ範囲", 2, AttackAttribute.fire, 1, 2));

        int[] temp1 = { 0, 0};
        Node.AddNode(temp1, 0);
        int[] temp2 = { 2, 0, 0, 3};
        Node.AddNode(temp2, 1);
        int[] temp3 = { 2, 0, 1, 2};
        Node.AddNode(temp3, 2);

        Player.instance.RegisterActions(items, skills, magics);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
