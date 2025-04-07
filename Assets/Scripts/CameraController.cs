using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraController : MonoBehaviour
{
    public Transform playerObj;
    public Transform player;
    public Transform orientation;
    public float rotationSpeed;

    private Vector3 view;
    private Vector3 inputDir;

    private Vector2 walkInput;
    PlayerInput playerInput;

    void Awake(){
        playerInput = new PlayerInput();

        playerInput.CharacterControls.Walk.started += playerInputMethod;
        playerInput.CharacterControls.Walk.performed += playerInputMethod;
        playerInput.CharacterControls.Walk.canceled += playerInputMethod;

    }

    void start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    
    void Update()
    {
        view = player.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        orientation.forward = view.normalized;
        inputDir = orientation.forward * walkInput.y + orientation.right * walkInput.x;
        Debug.Log(walkInput.x);
        if(inputDir != Vector3.zero){
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

        }

    }

    void playerInputMethod(InputAction.CallbackContext context){
        walkInput = context.ReadValue<Vector2>();
    }


    void OnEnable(){
        playerInput.CharacterControls.Enable();
    }

    void OnDisable(){
        playerInput.CharacterControls.Disable();
    }  
}
