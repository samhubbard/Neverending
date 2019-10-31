using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEvent : MonoBehaviour {

    public GameObject physicalGrid;
    GameObject grid;
    PFGrid gridPoints;
    public GameObject gridSpawnPoint;
    private GameObject lootSpawnPoint;
    public GameObject meleeEnemy;
    public GameObject rangedEnemy;
    public Transform player;
    public Transform playerSphere;
    private AudioSource audioSource;
    public AudioClip roomEventStartSound;
    public AudioClip lootDropSound;
    public Transform room;
    public GameObject fire;
    public bool hasActivated;
    int numberOfMelee;
    int numberOfRanged;
    float startTime;
    float endTime;
    public static bool roomEventActive;
    float healthSnapshot;
    public Unit enemy;
    private Node lootDropPoint;
    private int roomValue;
    float timeToClear;
    public static int healthLost;
    float hitPercentage;
    public static float bulletsHitEnemy;
    public static float bulletsFired;
    private bool roomEventEnded;
    private bool enemiesHaveSpawned;
    private bool lootHasDropped;
    private bool doorsHaveOpened;
    private bool generalRequirements;
    private bool finalClosingRequirementsMet;
    private bool readyToResetRoomScript;

    private void Start()
    {
        // get all of the initials setup
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 1f * GameController.sfxVolume;

        ResetScript();
    }

    private void Update()
    {
        if (player != null)
        {
            // keep the invisible cube on the player so that the A* knows where to send the skeletons
            playerSphere.position = player.position;
        }

        if (enemiesHaveSpawned)
        {
            // This fix appears to be working very well now that I restructured the rest of the script
            GameObject[] secondaryListOfMeleeEnemies = GameObject.FindGameObjectsWithTag("meleeEnemy");
            GameObject[] secondaryListOfRangedEnemies = GameObject.FindGameObjectsWithTag("rangedEnemy");

            if (secondaryListOfMeleeEnemies.Length <= 0 && secondaryListOfRangedEnemies.Length <= 0)
            {
                EndRoomEvent();
            }
        }

        if (readyToResetRoomScript)
        {
            ResetScript();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // once the player crosses the threshold, start the room event
        if (collision.CompareTag("Player") && !readyToResetRoomScript) {

            // get an instance of the player
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();

            // as long as the room event has not happened yet, start it
            if (!hasActivated)
            {
                hasActivated = true;
                roomEventActive = true;
                RoomEventSpawner();
            }
        }
    }

    private void ResetScript()
    {
        healthLost = 0;
        bulletsHitEnemy = 0;
        bulletsFired = 0;
        roomEventEnded = false;
        enemiesHaveSpawned = false;
        lootHasDropped = false;
        doorsHaveOpened = false;
        generalRequirements = false;
        finalClosingRequirementsMet = false;
        readyToResetRoomScript = false;
    }

    void RoomEventSpawner() {

        // get a current snapshot of the player's health for calculating the DDA
        healthSnapshot = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().currentHealth;

        // play a sound effect to alert the player that the room event has started
        audioSource.clip = roomEventStartSound;
        audioSource.volume = 0.4f * GameController.sfxVolume;
        audioSource.Play();

        // get an instance of the room spawner's parent
        room = transform.parent.transform;

        // instantiate the grid
        grid = Instantiate(physicalGrid, gridSpawnPoint.transform.position, Quaternion.identity, room);
        gridPoints = grid.GetComponent<PFGrid>();
        GameController.currentLootDropPoint = gridPoints.grid[gridPoints.gridSizeX / 2, gridPoints.gridSizeY / 2];

        // this will get rid of a game object that is no longer needed
        Destroy(gridSpawnPoint, 2.0f);

        // create the player sphere
        playerSphere.position = player.position;

        // Spawn enemies
        // First, determine how many enemies will spawn
        numberOfMelee = Random.Range(1, 4);
        numberOfRanged = Random.Range(0, 3);

        int enemyCount = numberOfMelee + numberOfRanged;

        // this will set the value of the room to determine what level of loot is available after the room event is over
        switch (enemyCount)
        {
            case(1):
            roomValue = 4;
            break;

            case(2):
            roomValue = 6;
            break;

            case(3):
            roomValue = 8;
            break;

            case(4):
            roomValue = 9;
            break;

            case(5):
            roomValue = 10;
            break;

            default:
            break;
        }

        startTime = Time.time;

        // spawn in the melee enemies in random positions in the room (as long as the area is spawnable.
        Invoke("SpawnEnemies", 1.0f);
    }

    void SpawnEnemies() {

        // spawn in all of the melee enemies
        for (int i = 1; i <= numberOfMelee; i++)
        {
            // get a random location to set the enemy
            Node spawnNode = gridPoints.grid[Random.Range(0, gridPoints.gridSizeX), 
                                             Random.Range(0, gridPoints.gridSizeY)];

            // ensure that the location isn't going to get the enemy stuck
            if (spawnNode.walkable)
            {
                // instantiate the enemy and set the melee enemy upon the player via the A*
                GameObject spawnedMelee = Instantiate(meleeEnemy, spawnNode.worldPosition,
                                                      transform.rotation, room) as GameObject;
                spawnedMelee.GetComponent<Unit>().player = playerSphere;
            }
            else
            {
                // if the area wasn't walkable, just roll back the for loop by one
                i--;
            }
        }

        if (numberOfRanged > 0)
        {
            for (int i = 1; i <= numberOfRanged; i++)
            {
                // get a random location
                Node spawnNode = gridPoints.grid[Random.Range(0, gridPoints.gridSizeX),
                                                 Random.Range(0, gridPoints.gridSizeY)];

                // ensure that the node is walkable
                if (spawnNode.walkable)
                {
                    // spawn the enemy in
                    GameObject spawnedRanged = Instantiate(rangedEnemy, spawnNode.worldPosition,
                                                           transform.rotation, room) as GameObject;
                }
                else
                {
                    // if the node isn't walkable, roll the for loop back by one
                    i--;
                }
            }
        }
        enemiesHaveSpawned = true;
    }

    void EndRoomEvent()
    {
        if (!generalRequirements)
        {
            GeneralClosingRequirements();
        }
        
        if (!lootHasDropped)
        {
            Droploot();
        }

        if (!doorsHaveOpened)
        {
            OpenDoors();
        }

        if (!finalClosingRequirementsMet && generalRequirements && lootHasDropped && doorsHaveOpened)
        {
            FinalRoomClosingRequirements();
        }
    }

    private void GeneralClosingRequirements()
    {
        // update the player stats and instantiate the loot item variable
        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gameController.playerStats.RoomsClearedCounter++;
        gameController.UpdatePlayerStats();

        // get the end time for DDA calculations
        endTime = Time.time;

        generalRequirements = true;
    }

    private void Droploot()
    {
        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        Loot lootItem;

        // Get the loot drop item
        if (gameController.calcHealth <= 0.15f)
        {
            // if the player has less that 15% health, increase the chance at a health pot
            int rand = Random.Range(1, 11);
            if (rand <= 3)
            {
                // there is the chance that the player will just get a normal loot item
                lootItem = GetLoot();
            }
            else if (rand > 3 && rand <= 7)
            {
                // normal health pickup
                lootItem = new Loot("healthPickup", 2);
            }
            else 
            {
                // greater health pickup
                lootItem = new Loot("greaterHealth", 2);
            }
        }
        else
        {
            // if the player is above 15% health, get a normal loot drop
             lootItem = GetLoot();
        }

        // instantiate the item into the screen
        LootLinking allLoot = GameObject.FindGameObjectWithTag("Loot").GetComponent<LootLinking>();
        GameObject lootDrop = Instantiate(allLoot.GetLootItem(lootItem.GetItemName), 
                                            GameController.currentLootDropPoint.worldPosition,
                                            Quaternion.identity, room);
        audioSource.clip = lootDropSound;
        audioSource.volume = 0.4f * GameController.sfxVolume;
        audioSource.Play();

        lootHasDropped = true;
    }

    Loot GetLoot()
    {
        // get all of the player stats that will affect loot drops
        GameController c = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        bool maxRunSpeedReached = c.playerStats.MaxRunSpeedReached;
        bool maxROFReached = c.playerStats.MaxRateOfFireReached;

        // pull a loot item from the database
        DBAccess database = GameObject.FindGameObjectWithTag("Loot").GetComponent<DBAccess>();
        int sizeOfLootTable = database.GetLootTableSize() + 1;
        int randomDropNumber = Random.Range(1, sizeOfLootTable);
        Loot lootItem = database.GetLootDrop(randomDropNumber);

        // check to see if the player has hit the max on either of the below items
        // if they have and it is slated to drop, rerun the function
        if (lootItem.GetItemName == "shootSpeed" && maxROFReached)
        {
            GetLoot();
        }
        else if (lootItem.GetItemName == "runSpeedIncrease" && maxRunSpeedReached)
        {
            GetLoot();
        }
        else
        {
            // check to see that the loot meets the value criteria
            // if ti does, return it, if it doesn't, rerun the function
            switch (roomValue)
            {
                case(4):
                if (lootItem.GetItemValue >= 5)
                {
                    GetLoot();
                    break;
                }
                else
                {
                    return lootItem;
                }

                case(6):
                if (lootItem.GetItemValue >= 7)
                {
                    GetLoot();
                    break;
                }
                else
                {
                    return lootItem;
                }

                case(8):
                return lootItem;

                case(9):
                if (lootItem.GetItemValue <= 5)
                {
                    GetLoot();
                    break;
                }
                else
                {
                    return lootItem;
                }

                case(10):
                if (lootItem.GetItemValue <= 3)
                {
                    GetLoot();
                    break;
                }
                else
                {
                    return lootItem;
                }

                default:
                break;
            }
            return lootItem;
        }

        return null;
    }

    private void OpenDoors()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (GameObject door in doors)
        {
            Destroy(door);
        }
    
        doorsHaveOpened = true;
    }

    private void FinalRoomClosingRequirements()
    {
        // set the room event to inactive
        roomEventActive = false;

        // destroy the grid components
        Destroy(GameObject.FindGameObjectWithTag("Grid"));

        // update the DDA
        UpdateDDA();
        
        // Flash message
        string messageToSend = "Room Cleared!";
        DisplayMessage.MessageToQueue(messageToSend);

        finalClosingRequirementsMet = true;
        readyToResetRoomScript = true;
    }

    void UpdateDDA() {
        // gather all pertinent information
        timeToClear = endTime - startTime;
        hitPercentage = bulletsHitEnemy / bulletsFired;

        // update the modifier
        GameController.DDA(timeToClear, healthLost, hitPercentage, healthSnapshot);
    }

    public Node TeleportLocation()
    {
        // get a random teleport location
        Node teleportNode = gridPoints.grid[Random.Range(0, gridPoints.gridSizeX), Random.Range(0, gridPoints.gridSizeY)];

        // ensure that the location is not going to get the enemy stuck
        if (teleportNode.walkable)
        {
            return teleportNode;
        }
        else
        {
            TeleportLocation();
        }

        return null;
    }

    // get the loot drop location (center of the current room)
    public Node LootDropLocation()
    {
        Node lootNode = gridPoints.grid[gridPoints.gridSizeX / 2, gridPoints.gridSizeY / 2];
        return lootNode;
    }

}
