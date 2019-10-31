using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootLinking : MonoBehaviour {

    public GameObject healthPickup;
    public GameObject rateOfFire;
    public GameObject roomClear;
    public GameObject greaterHealthPickup;
    public GameObject maxHealthIncreasePickup;
    public GameObject maxDamageIncrease;
    public GameObject bulletSpeedIncrease;
    public GameObject runSpeedIncrease;
    public GameObject timeSlowInventoryItem;
    public GameObject invincibilityInventoryItem;
    public GameObject damageReflectionInventoryItem;
    public GameObject healingTotemInventoryItem;
    public GameObject attackTotemInventoryItem;
    public GameObject areaAttackTotemInventoryItem;
    public GameObject slowingTotemInventoryItem;

    // takes in the loot item name and returns the actual loot item
    public GameObject GetLootItem(string name)
    {
        switch (name)
        {
            case "healthPickup":
                return healthPickup;
            case "shootSpeed":
                return rateOfFire;
            case "roomClear":
                return roomClear;
            case "greaterHealth":
                return greaterHealthPickup;
            case "maxHealthIncrease":
                return maxHealthIncreasePickup;
            case "attackDamageIncrease":
                return maxDamageIncrease;
            case "projectileFlightTimeIncrease":
                return bulletSpeedIncrease;
            case "runSpeedIncrease":
                return runSpeedIncrease;
            case "timeBendingBomb":
                return timeSlowInventoryItem;
            case "invincibilityBuff":
                return invincibilityInventoryItem;
            case "damageReflectionBuff":
                return damageReflectionInventoryItem;
            case "healingTotem":
                return healingTotemInventoryItem;
            case "shootingTotem":
                return attackTotemInventoryItem;
            case "damageAreaTotem":
                return areaAttackTotemInventoryItem;
            case "slowingAreaTotem":
                return slowingTotemInventoryItem;
            default:
                return null;
        }
    }
}
