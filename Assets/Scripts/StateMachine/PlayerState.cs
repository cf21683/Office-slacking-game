using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerState : MonoBehaviour
{
    Animator _animator;
    int _isWalkingHash;
    int _isRunningHash;



    PlayerInput _playerInput;
    CharacterController _characterController;

    Vector2 _walkInput;
    Vector3 _walk;
    Vector3 _run;

    Vector3 _cameraMovement;
    
    bool _isWalkPressed;
    bool _isRunPressed;
    public float rotationFactorPerFrame = 10.0f;
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;

    PlayerBaseState _currentState;
    PlayerStateFactory _stateFactory;

    public PlayerBaseState CurrentState {get {return _currentState;} set {_currentState = value;}}
    public PlayerStateFactory StateFactory{get {return _stateFactory;} set {_stateFactory = value;}}
    public bool IsWalkPressed{get {return _isWalkPressed;}}
    public bool IsRunPressed{get {return _isRunPressed;}}
    public Animator Animator{get {return _animator;}}
    public int IsWalkingHash{get {return _isWalkingHash;}}
    public int IsRunningHash{get {return _isRunningHash;}}
    public Vector2 WalkInput{get {return _walkInput;}}

    public float WalkX{get {return _walk.x;} set {_walk.x = value;}}
    public float WalkZ{get {return _walk.z;} set {_walk.z = value;}}
    public float WalkInputX{get {return _walkInput.x;}}
    public float WalkInputY{get {return _walkInput.y;}}

    public float WalkSpeed{get {return walkSpeed;}}
    public float RunSpeed{get {return runSpeed;}}
    



    void Awake(){
        
        _playerInput = new PlayerInput();

        _animator = GetComponent<Animator>();
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        
        _characterController = GetComponent<CharacterController>();

        _stateFactory = new PlayerStateFactory(this);
        _currentState = _stateFactory.GroundedState();
        _currentState.EnterState();

        _playerInput.CharacterControls.Walk.started += walkAndRunInput;
        _playerInput.CharacterControls.Walk.performed += walkAndRunInput;
        _playerInput.CharacterControls.Walk.canceled += walkAndRunInput;

        _playerInput.CharacterControls.Run.started += getRunInput;
        _playerInput.CharacterControls.Run.canceled += getRunInput;


    }   

    void Update()
    {
        characterRotation();
        _currentState.UpdateStates();

        _cameraMovement = ConvertToCameraSpace(_walk);
        _characterController.Move(_cameraMovement * Time.deltaTime);
    }


    void walkAndRunInput(InputAction.CallbackContext context){
        _walkInput = context.ReadValue<Vector2>();
        _isWalkPressed = _walkInput.x != 0 || _walkInput.y != 0;
    }

    void getRunInput(InputAction.CallbackContext context){
        _isRunPressed = context.ReadValueAsButton();
    }

    

    void characterRotation(){
        Vector3 positionLookAt;
        positionLookAt.x = _cameraMovement.x;
        positionLookAt.y = 0.0f;
        positionLookAt.z = _cameraMovement.z;

        Quaternion currentRotation = transform.rotation;

        if(_isWalkPressed){
            Quaternion targetRotation = Quaternion.LookRotation(positionLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation,targetRotation,rotationFactorPerFrame * Time.deltaTime);
        }
    }

    Vector3 ConvertToCameraSpace(Vector3 vectorToRotate){
        float currentY = vectorToRotate.y;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZ = vectorToRotate.z * cameraForward;
        Vector3 cameraRightX = vectorToRotate.x * cameraRight;

        Vector3 finalRotation = cameraForwardZ + cameraRightX;
        finalRotation.y = currentY;
        return finalRotation;
    }   

    void OnEnable(){
        _playerInput.CharacterControls.Enable();
    }

    void OnDisable(){
        _playerInput.CharacterControls.Disable();
    }  
}
