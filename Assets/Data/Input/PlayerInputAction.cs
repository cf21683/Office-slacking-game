using UnityEngine;

public class PlayerInputAction : MonoBehaviour
{
    PlayerInput playerInput;

   public Vector2 walkInput => playerInput.CharacterControls.Walk.ReadValue<Vector2>();

   public bool isRunPressed => playerInput.CharacterControls.Run.ReadValue<float>() > 0.5f;

   public bool isWalkPressed => walkInput.x != 0f || walkInput.y != 0f;

   public bool isInteractPressed => playerInput.CharacterControls.Interaction.WasPressedThisFrame();
   
   private bool workPressedThisFrame = false;
   private bool slackPressedThisFrame = false;
    public bool isWorkPressed => GetAndReset(ref workPressedThisFrame);
    public bool isSlackPressed => GetAndReset(ref slackPressedThisFrame);

    private bool GetAndReset(ref bool flag)
    {
        if (flag)
        {
            flag = false;
            return true;
        }
        return false;
    }


    public void TriggerWork()
    {
        workPressedThisFrame = true;
    }

    public void TriggerSlack()
    {
        slackPressedThisFrame = true;
    }
   
   void Awake(){
    playerInput = new PlayerInput();
   }

   public void EnableOn(){
    playerInput.CharacterControls.Enable();
   }
}
