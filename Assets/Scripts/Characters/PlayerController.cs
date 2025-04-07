using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInputAction input;

    Vector3 _walk;
    Vector3 _cameraMovement;

    CharacterController _characterController;

    public float rotationFactorPerFrame = 10.0f;

    void Awake(){
        input = GetComponent<PlayerInputAction>();
        _characterController = GetComponent<CharacterController>();
    }    
    void Start()
    {   
        input.EnableOn();
    }

    // Update is called once per frame
    void Update()
    {
        characterRotation();
        _cameraMovement = ConvertToCameraSpace(_walk);
        _characterController.Move(_cameraMovement * Time.deltaTime);
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

    void characterRotation(){
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
}
