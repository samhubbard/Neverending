using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryLinkingForSave : MonoBehaviour
{
    public GameObject aoeTotem;
    public GameObject damageReflection;
    public GameObject healingTotem;
    public GameObject invincibility;
    public GameObject roomClear;
    public GameObject shootingTotem;
    public GameObject slowingTotem;
    public GameObject timeSlow;

    public GameObject GetInventoryItem(string identifier)
    {
        // this will check to see what item was in the player prefs for the inventory
        // and return the actual loot item to be placed in the inventory
        switch (identifier)
        {
            case "AOEAttackTotem":
            return aoeTotem;

            case "DamageReflection":
            return damageReflection;

            case "HealingTotem":
            return healingTotem;

            case "InvincibilityBuff":
            return invincibility;

            case "RoomClear":
            return roomClear;

            case "ShootingTotem":
            return shootingTotem;

            case "SlowingTotem":
            return slowingTotem;

            case "TimeSlow":
            return timeSlow;

            default:
            return null;
        }
    }
}
