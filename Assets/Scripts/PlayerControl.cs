using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{

    Animator animator;
    int isWalkingHash;
    int isRunningHash;



    PlayerInput playerInput;
    CharacterController characterController;

    Vector2 walkInput;
    Vector3 walk;
    Vector3 run;
    
    bool isWalkPressed;
    bool isRunPressed;
    public float rotationFactorPerFrame = 1.0f;
    public float walkSpeed = 1.0f;
    public float runSpeed = 2.0f;


    void Awake(){
        
        playerInput = new PlayerInput();

        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        
        characterController = GetComponent<CharacterController>();

        playerInput.CharacterControls.Walk.started += walkAndRunInput;
        playerInput.CharacterControls.Walk.performed += walkAndRunInput;
        playerInput.CharacterControls.Walk.canceled += walkAndRunInput;

        playerInput.CharacterControls.Run.started += getRunInput;
        playerInput.CharacterControls.Run.canceled += getRunInput;

    }   

    void Update()
    {
        if(isRunPressed){
            characterController.Move(run * Time.deltaTime);
            
        }else{
            characterController.Move(walk * Time.deltaTime);
        }
        
        animationWalk();
        characterRotation();
        gravity();
    
    }

    void walkAndRunInput(InputAction.CallbackContext context){
        walkInput = context.ReadValue<Vector2>();
        walk.x = walkInput.x * walkSpeed;
        walk.z = walkInput.y * walkSpeed; 

        run.x = walkInput.x * runSpeed;
        run.z = walkInput.y * runSpeed;

        isWalkPressed = walkInput.x != 0 || walkInput.y != 0;

    }

    void getRunInput(InputAction.CallbackContext context){
        isRunPressed = context.ReadValueAsButton();
    }

    void characterRotation(){
        Vector3 positionLookAt;
        positionLookAt.x = walk.x;
        positionLookAt.y = 0.0f;
        positionLookAt.z = walk.z;

        Quaternion currentRotation = transform.rotation;

        if(isWalkPressed){
            Quaternion targetRotation = Quaternion.LookRotation(positionLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation,targetRotation,rotationFactorPerFrame * Time.deltaTime);
        }
    }


    void animationWalk(){
        bool isWalking = animator.GetBool(isWalkingHash); 
        bool isRunning = animator.GetBool(isRunningHash);

        if(!isWalking && isWalkPressed){
             animator.SetBool(isWalkingHash,true);
        }
        
        else if(isWalking && !isWalkPressed){
            animator.SetBool(isWalkingHash,false);
        }

        if(!isRunning && (isWalking && isRunPressed)){
            animator.SetBool(isRunningHash,true);
        }

        else if(isRunning && (!isWalkPressed || !isRunPressed)){
            animator.SetBool(isRunningHash,false);
        }

    }

    void gravity(){
        if(characterController.isGrounded){
            walk.y = -0.05f;
            run.y = -0.05f;
        }else{
            walk.y -= 9.8f * Time.deltaTime;
            run.y -= 9.8f * Time.deltaTime;
        }
    }

    void OnEnable(){
        playerInput.CharacterControls.Enable();
    }

    void OnDisable(){
        playerInput.CharacterControls.Disable();
    }  

}
