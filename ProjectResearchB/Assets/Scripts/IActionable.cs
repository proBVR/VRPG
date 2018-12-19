using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionable
{
    void Use();
    int GetCost();
    string GetName();
}
