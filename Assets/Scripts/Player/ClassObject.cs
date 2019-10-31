using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassObject
{
    // member variables
    public readonly string className;
    public readonly int health;
    public readonly int playerSpeed;
    public readonly int projectileSpeed;
    public readonly int projectileDamage;

    // public constructor
    public ClassObject(string _className, int _health, int _playerSpeed, int _projectileSpeed, 
        int _projectileDamage)
    {
        className = _className;
        health = _health;
        playerSpeed = _playerSpeed;
        projectileSpeed = _projectileSpeed;
        projectileDamage = _projectileDamage;
    }

    // getters
    public string GetClassName
    {
        get
        {
            return className;
        }
    }

    public int GetPlayerHealth
    {
        get
        {
            return health;
        }
    }

    public int GetPlayerSpeed
    {
        get
        {
            return playerSpeed;
        }
    }

    public int GetProjectileSpeed
    {
        get
        {
            return projectileSpeed;
        }
    }

    public int GetProjectileDamage
    {
        get
        {
            return projectileDamage;
        }
    }
}
