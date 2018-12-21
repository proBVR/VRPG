using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : Menu
{
    private List<string> contents = new List<string>();
    //private int pageMax;
    private readonly int contentsPerPage = 6;
    private int page = 0;
    private string itemName;

    protected override void SetData()
    {
        contents.Clear();
        //pageMax = 0;
        var list = Player.instance.inventory.GetContents();
        int count = 0;
        var temp = "";
        
        foreach(string str in list)
        {
            temp += str + "\n";
            count++;
            if (count == contentsPerPage)
            {
                contents.Add(temp);
                temp = "";
                count = 0;
            }
        }
        if (count > 0) contents.Add(temp);

        text.text = contents[0];
    }

    protected override void ChangeValue(int dir)
    {
        if(page > 0 && dir == -1)
        {
            page--;
            text.text = contents[page];
        }
        else if(page < contents.Count-1 && dir == 1)
        {
            page++;
            text.text = contents[page];
        }
    }

    protected override void Decide(int index)
    {
        int i=0;
        string item = "";
        bool flag = false;
        foreach (char c in contents[page])
        {
            if (c == '\n') i++;
            if (!flag && i == index) flag = true;
            else if (flag && c == '\t') break;
            else if (flag) item += c;            
        }
        itemName = item;
        manager.Confirm("Use "+itemName, UseItem);
    }

    private void UseItem()
    {
        Player.instance.inventory.UseItem(itemName);
    }
}