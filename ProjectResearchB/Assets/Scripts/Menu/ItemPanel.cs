using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPanel : Menu
{
    private List<string> contents = new List<string>();
    //private int pageMax;
    private readonly int contentsPerPage = 2;
    private int page = 0;
    private string itemName;

    protected override void SetData()
    {
        lineSize = contentsPerPage;
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

        if (contents.Count != 0) text.text = contents[0];
        else text.text = "";
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
        if (text.text == "") return;
        int i=0;
        string item = "";
        bool flag = false;
        foreach (char c in contents[page])
        {
            Debug.Log(c);
            if (c == '\n') i++;
            if (!flag && i == index) flag = true;
            else if (flag && c == '\t') break;
            if (flag && c!='\n') item += c;            
        }
        itemName = item;
        manager.Confirm("Use item?\n", UseItem);
    }

    private void UseItem()
    {
        Debug.Log("use item from Menu: "+itemName);
        Player.instance.inventory.UseItem(itemName);
        SetData();
    }
}