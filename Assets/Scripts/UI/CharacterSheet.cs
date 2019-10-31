using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSheet : MonoBehaviour
{
    public Text className;
    public Text successfulRuns;
    public Text score;
    public Text health;
    public Text runSpeed;
    public Text runSpeedIncreases;
    public Text rateOfFire;
    public Text rateOfFireIncreases;
    public Text attackDamage;
    public Text bulletSpeed;
    public Image inventoryOneItem;
    public Image inventoryTwoItem;

    public Sprite aoeTotem;
    public Sprite damageReflection;
    public Sprite healingTotem;
    public Sprite invincibility;
    public Sprite roomClear;
    public Sprite shootingTotem;
    public Sprite slowingTotem;
    public Sprite slowTime;

    // Displays all of the player stats onto the screen
    public void SetCharacterSheet(PlayerStats stats)
    {
        className.text = stats.Class;
        successfulRuns.text = stats.SuccessfulRuns.ToString();
        score.text = stats.TotalScore.ToString();
        health.text = stats.TotalHealth.ToString();
        runSpeed.text = stats.TotalRunSpeed.ToString();
        if (stats.MaxRunSpeedReached)
        {
            runSpeedIncreases.text = "Max (4)";
        }
        else
        {
            runSpeedIncreases.text = stats.TotalRunSpeedIncreases.ToString();
        }
        float calculateIncrease = 0.5f / stats.TotalRateOfFire;
        string convertToString = calculateIncrease.ToString("#.##");
        string rateOfFireText = convertToString + "x";
        rateOfFire.text = rateOfFireText;
        if (stats.MaxRateOfFireReached)
        {
            rateOfFireIncreases.text = "Max (4)";
        }
        else
        {
            rateOfFireIncreases.text = stats.TotalRateOfFireIncreases.ToString();
        }
        attackDamage.text = stats.AttackDamageIncreaseAmount.ToString();
        bulletSpeed.text = stats.ProjectileFlightSpeed.ToString();
        if (stats.InventoryOneFilled)
        {

            switch (stats.InventoryOneItem)
            {
                case "AOEAttackTotem":
                    inventoryOneItem.sprite = aoeTotem;
                    inventoryOneItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "DamageReflection":
                    inventoryOneItem.sprite = damageReflection;
                    inventoryOneItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "HealingTotem":
                    inventoryOneItem.sprite = healingTotem;
                    inventoryOneItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "InvincibilityBuff":
                    inventoryOneItem.sprite = invincibility;
                    inventoryOneItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "RoomClear":
                    inventoryOneItem.sprite = roomClear;
                    inventoryOneItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "ShootingTotem":
                    inventoryOneItem.sprite = shootingTotem;
                    inventoryOneItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "SlowingTotem":
                    inventoryOneItem.sprite = slowingTotem;
                    inventoryOneItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "TimeSlow":
                    inventoryOneItem.sprite = slowTime;
                    inventoryOneItem.color = new Color(1f, 1f, 1f, 1f);
                    break;


                default:
                    break;
            }
        }

        if (stats.InventoryTwoFilled)
        {
            switch (stats.InventoryTwoItem)
            {
                case "AOEAttackTotem":
                    inventoryTwoItem.sprite = aoeTotem;
                    inventoryTwoItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "DamageReflection":
                    inventoryTwoItem.sprite = damageReflection;
                    inventoryTwoItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "HealingTotem":
                    inventoryTwoItem.sprite = healingTotem;
                    inventoryTwoItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "InvincibilityBuff":
                    inventoryTwoItem.sprite = invincibility;
                    inventoryTwoItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "RoomClear":
                    inventoryTwoItem.sprite = roomClear;
                    inventoryTwoItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "ShootingTotem":
                    inventoryTwoItem.sprite = shootingTotem;
                    inventoryTwoItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "SlowingTotem":
                    inventoryTwoItem.sprite = slowingTotem;
                    inventoryTwoItem.color = new Color(1f, 1f, 1f, 1f);
                    break;
                case "TimeSlow":
                    inventoryTwoItem.sprite = slowTime;
                    inventoryTwoItem.color = new Color(1f, 1f, 1f, 1f);
                    break;


                default:
                    break;
            }
        }
    }
}
