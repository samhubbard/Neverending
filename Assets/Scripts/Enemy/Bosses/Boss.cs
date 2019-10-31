using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss
{
    // member variables
    private readonly string bossName;
    private readonly int baseHealth;
    private readonly int baseAttackDamageOne;
    private readonly int baseAttackDamageTwo;

    // public constructor
    public Boss(string _name, int _health, int _damageOne, int _damageTwo)
    {
        bossName = _name;
        baseHealth = _health;
        baseAttackDamageOne = _damageOne;
        baseAttackDamageTwo = _damageTwo;
    }

    // getters
    public string GetBossName
    {
        get
        {
            return bossName;
        }
    }

    public int GetBossHealth
    {
        get
        {
            return baseHealth;
        }
    }

    public int GetAttackDamageOne
    {
        get
        {
            return baseAttackDamageOne;
        }
    }

    public int GetAttackDamageTwo
    {
        get
        {
            return baseAttackDamageTwo;
        }
    }
}
