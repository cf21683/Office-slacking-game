
using UnityEngine;

public class PlayerBusy : MonoBehaviour
{
    // Start is called before the first frame update

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
