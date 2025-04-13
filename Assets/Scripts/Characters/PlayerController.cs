using UnityEngine;


public class PlayerController : MonoBehaviour
{
    PlayerInputAction input;

    Vector3 _walk;
    Vector3 _cameraMovement;

    CharacterController _characterController;

    public Cinemachine.CinemachineFreeLook freeLookCam;
    public Cinemachine.CinemachineVirtualCamera computerCam;
    public ChairInteraction currentChair;

    private AudioSource[] AudioSources;
    internal AudioSource footstepSource;
    internal AudioSource chaseSource;
    public AudioClip walkClip;
    public AudioClip chaseClip;
    public float ChaseVolume;

    private bool isNearChair = false;
    private bool isSitting = false;

    public bool isWork;
    public bool isSlack;
    
    public bool isIdle;

    private bool isBusy = false;

    public bool IsBusy{get{return isBusy;} set {isBusy = value;}}

    public bool IsIdle{ get { return isIdle; } set { isIdle = value; }} 

    public bool IsWork{ get { return isWork; } set { isWork = value; }}
    public bool IsSlack{ get { return isSlack; } set { isSlack = value; }}

    private bool sitRequested = false;
    public bool SitRequested{get{return sitRequested;} set{ sitRequested = value; }}
    public bool IsSitting{ get { return isSitting; } set { isSitting = value; }}
    public bool IsNearChair { get { return isNearChair; } set { isNearChair = value; }}

    public float rotationFactorPerFrame = 10.0f;

    void Awake(){
        input = GetComponent<PlayerInputAction>();
        _characterController = GetComponent<CharacterController>();
        AudioSources = GetComponents<AudioSource>();
        footstepSource = AudioSources[0]; // 脚步音效
        footstepSource.clip = walkClip;
        chaseSource = AudioSources[1]; // 追逐音效
        chaseSource.clip = chaseClip;
        chaseSource.volume = ChaseVolume; // 设置音量
        

    }    
    void Start()
    {   
        LockCursor();
        input.EnableOn();
    }


    void Update()
    {
        HandleChairInteraction();
        HandleGravity();
         if (!isSitting && !isBusy){
            CharacterRotation();
            _cameraMovement = ConvertToCameraSpace(_walk);
            _characterController.Move(_cameraMovement * Time.deltaTime);
         }
        
    }

    void HandleGravity(){
        if(_characterController.isGrounded){
            float groundGravity = -0.05f;
            _walk.y =groundGravity;
        }else{
            float gravity = -9.8f;
            _walk.y = gravity;
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

    void CharacterRotation(){
        Vector3 positionLookAt;
        positionLookAt.x = _cameraMovement.x;
        positionLookAt.y = 0.0f;
        positionLookAt.z = _cameraMovement.z;

        Quaternion currentRotation = transform.rotation;

        if(input.isWalkPressed){
            Quaternion targetRotation = Quaternion.LookRotation(positionLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation,targetRotation,rotationFactorPerFrame * Time.deltaTime);
        }
    }

    public void SetVelocity(float speed){
        _walk.x = input.walkInput.x * speed;
        _walk.z = input.walkInput.y * speed;

    }

    void HandleChairInteraction()
    {
        if (input.isInteractPressed)
        {
            if (isSitting)
            {
                isSitting = false;
            }
            else if (isNearChair)
            {
                sitRequested = true; 
            }
        }
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
