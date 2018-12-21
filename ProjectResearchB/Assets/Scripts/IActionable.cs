using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionable
{
    void Use(Character user);
    int GetCost();
    string GetName();
}
