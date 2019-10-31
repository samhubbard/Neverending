using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    // member variables
    private readonly string enemyName;
    private readonly int enemyHealth;
    private readonly double speedModifier;
    private readonly double baseAttack;

    // public constructor
    public Enemy(string _name, int _health, double _speed, double _attack)
    {
        enemyName = _name;
        enemyHealth = _health;
        speedModifier = _speed;
        baseAttack = _attack;
    }

    // getters
    public string getEnemyName {
        get {
            return enemyName;
        }
    }

    public int getEnemyHealth {
        get {
            return enemyHealth;
        }
    }

    public double getSpeedModifier {
        get {
            return speedModifier;
        }
    }

    public double getBaseAttack {
        get {
            return baseAttack;
        }
    }
}
