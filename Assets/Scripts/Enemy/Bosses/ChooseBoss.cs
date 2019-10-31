using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseBoss : MonoBehaviour
{
    // the whole point of this script is to enable the handling of multiple bosses
    // if and when I add them to the game

    public GameObject flameKnight;
    //public GameObject SecondBoss;
    //public GameObject ThirdBoss

    // set up all of the variables I will need to get a randomly chosen boss
    private DBAccess database;
    private int bossTableSize;
    private int random;
    private Boss chosenBoss;

    // Start is called before the first frame update
    void Start()
    {
        // get a randomly chosen boss from the database
        database = GetComponent<DBAccess>();
        bossTableSize = database.GetBossTableSize();
        random = Random.Range(1, bossTableSize + 1);
        chosenBoss = database.GetChosenBoss(random);

        // since there is currently only one boss, it's obviously the only one that will be 
        // found, however... futurecasting
        if (chosenBoss.GetBossName == "FlameKnight")
        {
            Instantiate(flameKnight, transform.position, Quaternion.identity);
        }
    }

    public Boss GetBossStats()
    {
        // return the chosen boss to the room...
        return chosenBoss;
    }
}
