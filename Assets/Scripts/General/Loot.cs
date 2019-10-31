using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot {

    // member variables
    private readonly string nameOfItem;
    private readonly int valueOfItem;

    // public constructor
    public Loot(string _name, int _value)
    {
        nameOfItem = _name;
        valueOfItem = _value;
    }

    // getters
    public string GetItemName
    {
        get
        {
            return nameOfItem;
        }
    }

    public int GetItemValue
    {
        get
        {
            return valueOfItem;
        }
    }
}
