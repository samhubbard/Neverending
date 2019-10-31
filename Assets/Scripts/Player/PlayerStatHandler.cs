using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : MonoBehaviour
{
    // take in the updated (or new) player stats and save them all to PlayerPrefs
    public void SavePlayerStats(PlayerStats currentPlayerStats)
    {
        // this is the function that will save it to the PlayerPrefs
        PlayerPrefs.SetString("ClassName", currentPlayerStats.Class);
        PlayerPrefs.SetInt("successfulRuns", currentPlayerStats.SuccessfulRuns);
        if (currentPlayerStats.FlaggedForDeletion)
            PlayerPrefs.SetString("FlaggedForDeletion", "true");
        else
            PlayerPrefs.SetString("FlaggedForDeletion", "false");

        PlayerPrefs.SetFloat("minDiff", currentPlayerStats.MinimumDifficultyModifier);
        PlayerPrefs.SetFloat("maxDiff", currentPlayerStats.MaximumDifficultyModifier);
        PlayerPrefs.SetFloat("curDiff", currentPlayerStats.CurrentDifficultyModifier);
        PlayerPrefs.SetInt("totalScore", currentPlayerStats.TotalScore);
        PlayerPrefs.SetInt("maxHealth", currentPlayerStats.TotalHealth);
        PlayerPrefs.SetInt("curHealth", currentPlayerStats.CurrentHealth);
        PlayerPrefs.SetFloat("totalRunSpeed", currentPlayerStats.TotalRunSpeed);
        PlayerPrefs.SetInt("runSpeedPickups", currentPlayerStats.TotalRunSpeedIncreases);
        if (currentPlayerStats.MaxRunSpeedReached)
            PlayerPrefs.SetString("maxRunSpeedBool", "true");
        else
            PlayerPrefs.SetString("maxRunSpeedBool", "false");
        PlayerPrefs.SetFloat("totalROF", currentPlayerStats.TotalRateOfFire);
        PlayerPrefs.SetInt("totalROFPickups", currentPlayerStats.TotalRateOfFireIncreases);
        if (currentPlayerStats.MaxRateOfFireReached)
            PlayerPrefs.SetString("maxROFBool", "true");
        else
            PlayerPrefs.SetString("maxROFBool", "false");
        PlayerPrefs.SetFloat("attackDamage", currentPlayerStats.AttackDamageIncreaseAmount);
        PlayerPrefs.SetFloat("projectileSpeed", currentPlayerStats.ProjectileFlightSpeed);

        if (currentPlayerStats.InventoryOneFilled)
            PlayerPrefs.SetString("invOneFilledBool", "true");
        else
            PlayerPrefs.SetString("invOneFilledBool", "false");

        if (currentPlayerStats.InventoryTwoFilled)
            PlayerPrefs.SetString("invTwoFilledBool", "true");
        else
            PlayerPrefs.SetString("invTwoFilledBool", "false");

        PlayerPrefs.SetString("invOneItem", currentPlayerStats.InventoryOneItem);
        PlayerPrefs.SetString("invTwoItem", currentPlayerStats.InventoryTwoItem);
        PlayerPrefs.SetInt("currentLevel", currentPlayerStats.CurrentLevel);
        PlayerPrefs.SetInt("roomCounter", currentPlayerStats.RoomsClearedCounter);

    }

    // pull all of the values out of the PlayerPrefs, setup a new PlayerStats object and return it
    public PlayerStats GetPlayerStats()
    {
        if (PlayerPrefs.GetString("ClassName") == "Knight" 
            || PlayerPrefs.GetString("ClassName") == "Mage"
            || PlayerPrefs.GetString("ClassName") == "Rogue")
        {
            // this is the function that will pull the information from the player prefs to create the object
            PlayerStats pullPlayerStats = new PlayerStats();

            // Set all of the information from the PlayerPrefs
            pullPlayerStats.Class = PlayerPrefs.GetString("ClassName");
            pullPlayerStats.SuccessfulRuns = PlayerPrefs.GetInt("successfulRuns");
            string flagged = PlayerPrefs.GetString("FlaggedForDeletion");
            if (flagged == "true")
                pullPlayerStats.FlaggedForDeletion = true;
            else
                pullPlayerStats.FlaggedForDeletion = false;
            pullPlayerStats.MinimumDifficultyModifier = PlayerPrefs.GetFloat("minDiff");
            pullPlayerStats.MaximumDifficultyModifier = PlayerPrefs.GetFloat("maxDiff");
            pullPlayerStats.CurrentDifficultyModifier = PlayerPrefs.GetFloat("curDiff");
            pullPlayerStats.TotalScore = PlayerPrefs.GetInt("totalScore");
            pullPlayerStats.TotalHealth = PlayerPrefs.GetInt("maxHealth");
            pullPlayerStats.CurrentHealth = PlayerPrefs.GetInt("curHealth");
            pullPlayerStats.TotalRunSpeed = PlayerPrefs.GetFloat("totalRunSpeed");
            pullPlayerStats.TotalRunSpeedIncreases = PlayerPrefs.GetInt("runSpeedPickups");
            string maxRunSpeedBool = PlayerPrefs.GetString("maxRunSpeedBool");
            if (maxRunSpeedBool == "true")
                pullPlayerStats.MaxRunSpeedReached = true;
            else
                pullPlayerStats.MaxRunSpeedReached = false;
            pullPlayerStats.TotalRateOfFire = PlayerPrefs.GetFloat("totalROF");
            pullPlayerStats.TotalRateOfFireIncreases = PlayerPrefs.GetInt("totalROFPickups");
            string maxROFBool = PlayerPrefs.GetString("maxROFBool");
            if (maxROFBool == "true")
                pullPlayerStats.MaxRateOfFireReached = true;
            else
                pullPlayerStats.MaxRateOfFireReached = false;
            pullPlayerStats.AttackDamageIncreaseAmount = PlayerPrefs.GetFloat("attackDamage");
            pullPlayerStats.ProjectileFlightSpeed = PlayerPrefs.GetFloat("projectileSpeed");

            string invOneFilledBool = PlayerPrefs.GetString("invOneFilledBool");
            if (invOneFilledBool == "true")
                pullPlayerStats.InventoryOneFilled = true;
            else
                pullPlayerStats.InventoryOneFilled = false;
            string invTwoFilledBool = PlayerPrefs.GetString("invTwoFilledBool");
            if (invTwoFilledBool == "true")
                pullPlayerStats.InventoryTwoFilled = true;
            else
                pullPlayerStats.InventoryTwoFilled = false;

            // this portion may need some work
            pullPlayerStats.InventoryOneItem = PlayerPrefs.GetString("invOneItem");
            pullPlayerStats.InventoryTwoItem = PlayerPrefs.GetString("invTwoItem");
            pullPlayerStats.CurrentLevel = PlayerPrefs.GetInt("currentLevel");
            pullPlayerStats.RoomsClearedCounter = PlayerPrefs.GetInt("roomsCounter");

            return pullPlayerStats;
        }
        else
        {
            return null;
        }
    }
}
