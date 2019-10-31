using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    //public constructor
    public PlayerStats() { }
    
    // getters/setters
    public string Class { get; set; }
    public int SuccessfulRuns { get; set; }
    public bool FlaggedForDeletion { get; set; }
    public float MinimumDifficultyModifier { get; set; }
    public float MaximumDifficultyModifier { get; set; }
    public float CurrentDifficultyModifier { get; set; }
    public int TotalScore { get; set; }
    public int TotalHealth { get; set; }
    public int CurrentHealth { get; set; }
    public float TotalRunSpeed { get; set; }
    public int TotalRunSpeedIncreases { get; set; }
    public bool MaxRunSpeedReached { get; set; }
    public float TotalRateOfFire { get; set; }
    public int TotalRateOfFireIncreases { get; set; }
    public bool MaxRateOfFireReached { get; set; }
    public float AttackDamageIncreaseAmount { get; set; }
    public float ProjectileFlightSpeed { get; set; }
    public bool InventoryOneFilled { get; set; }
    public string InventoryOneItem { get; set; }
    public bool InventoryTwoFilled { get; set; }
    public string InventoryTwoItem { get; set; }
    public int CurrentLevel { get; set; }
    public int RoomsClearedCounter { get; set; }
}
