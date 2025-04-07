using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputAction : MonoBehaviour
{
    PlayerInput playerInput;

   public Vector2 walkInput => playerInput.CharacterControls.Walk.ReadValue<Vector2>();

   public bool isRunPressed => playerInput.CharacterControls.Run.ReadValue<float>() > 0.5f;

   public bool isWalkPressed => walkInput.x != 0f || walkInput.y != 0f;
   
   void Awake(){
    playerInput = new PlayerInput();
   }

   public void EnableOn(){
    playerInput.CharacterControls.Enable();
   }
}
