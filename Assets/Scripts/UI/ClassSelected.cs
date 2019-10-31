using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelected : MonoBehaviour
{

    private Text buttonText;

    public Text classArea;

    public GameObject classSelection;
    private DBAccess database;
    private ClassObject chosenClass;

    public void SelectClass()
    {
        // this is where the player stats object will get created and saved to playerprefs... enabling the player
        // to enter a dungeon
        
        string classText = GetComponent<ClassSelectionIdentifier>().identifier;
        database = GetComponent<DBAccess>();
        chosenClass = database.GetClassInfo(classText);

        // take that chosen class from the database and make a player prefs character
        CreateCharacter(chosenClass);

        Invoke("HideMenu", 0.2f);
    }

    // create a new player stats object, populate it, and send it to be saved to player prefs
    private void CreateCharacter(ClassObject chosenClass)
    {
        PlayerStats characterStats = new PlayerStats();

        characterStats.Class = chosenClass.className;
        characterStats.SuccessfulRuns = 0;
        characterStats.FlaggedForDeletion = false;
        characterStats.MinimumDifficultyModifier = 1.0f;
        characterStats.MaximumDifficultyModifier = 3.0f;
        characterStats.CurrentDifficultyModifier = 1.0f;
        characterStats.TotalScore = 0;
        characterStats.TotalHealth = chosenClass.health;
        characterStats.CurrentHealth = chosenClass.health;
        characterStats.TotalRunSpeed = chosenClass.playerSpeed;
        characterStats.TotalRunSpeedIncreases = 0;
        characterStats.MaxRunSpeedReached = false;
        characterStats.TotalRateOfFire = 0.5f;
        characterStats.TotalRateOfFireIncreases = 0;
        characterStats.MaxRateOfFireReached = false;
        characterStats.AttackDamageIncreaseAmount = chosenClass.projectileDamage;
        characterStats.ProjectileFlightSpeed = chosenClass.projectileSpeed;
        characterStats.InventoryOneFilled = false;
        characterStats.InventoryTwoFilled = false;
        characterStats.InventoryOneItem = "none";
        characterStats.InventoryTwoItem = "none";
        PlayerPrefs.SetInt("CurrentBestStreak", 0);
        characterStats.CurrentLevel = 1;
        characterStats.RoomsClearedCounter = 0;
        GetComponent<PlayerStatHandler>().SavePlayerStats(characterStats);
    }

    // a simple function to hide the menu
    // I'm leaving this here incase I decide to use it in the future
    private void HideMenu()
    {
        classSelection.SetActive(false);
        PlayerController.disabled = false;
    }
}
