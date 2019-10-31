using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class PlayAd : MonoBehaviour
{
    public void ShowAd()
    {
        // show the ad
        if (Advertisement.IsReady())
        {
            Advertisement.Show("rewardedVideo", new ShowOptions()
            {
                // send the player's interaction with the ad to the callback
                resultCallback = HandleAdResults
            });
        }
        
    }

    private void HandleAdResults(ShowResult results)
    {
        // link into the game controller
        GameController controller =
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        switch(results)
        {
            case ShowResult.Finished:
                // revive the player
                controller.RevivePlayer();
                break;
            case ShowResult.Skipped:
                // let the player know they didn't watch the ad so they are going back to the main menu
                controller.PlayerSkippedAd();
                break;
            case ShowResult.Failed:
                // Unknown right now
                print("Ad failed");
                break;
        }
    }
}
