using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPanel : Menu
{

    private List<string> contents = new List<string>();
    //private int pageMax;
    private readonly int contentsPerPage = 3;
    private int page = 0;
    //private string itemName;

    protected override void SetData()
    {
        //lineSize = contentsPerPage;
        contents.Clear();
        //pageMax = 0;
        var list = Player.instance.GetAction(2);
        int count = 0;
        var temp = "";

        foreach (IActionable action in list)
        {
            temp += action.GetName() + "\n";
            count++;
            if (count == contentsPerPage)
            {
                contents.Add(temp);
                temp = "";
                count = 0;
            }
        }
        if (count > 0) contents.Add(temp);

        if (contents.Count != 0) text.text = contents[0];
        else text.text = "";
    }

    protected override void ChangeValue(int dir)
    {
        if (page > 0 && dir == -1)
        {
            page--;
            text.text = contents[page];
        }
        else if (page < contents.Count - 1 && dir == 1)
        {
            page++;
            text.text = contents[page];
        }
    }
}
