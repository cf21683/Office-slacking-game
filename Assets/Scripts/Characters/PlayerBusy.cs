
using UnityEngine;

public class PlayerBusy : MonoBehaviour
{
    public PlayerController player;
    public PlayerInputAction input;

    public void SetPlayerBusy(){
        player.IsBusy = true;
        Debug.Log("set player is busy");
    }

    public void SetPlayerNotBusy(){
        player.IsBusy = false;
        Debug.Log("set player is not busy");
    }

    public void IsWorkPressed(){
        input.TriggerWork();
    }
    public void IsSlackPressed(){
        input.TriggerSlack();
    }
}
