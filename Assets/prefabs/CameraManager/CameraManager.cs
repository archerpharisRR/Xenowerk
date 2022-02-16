using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform cameraFollowTransform;
    [SerializeField] float rotateSpeed = 20.0f;
    [SerializeField] [Range(0f, 100f)] float rotateDamping = 0.5f;
    [SerializeField] [Range(0f, 100f)] float moveDamping = 0.1f;
    Vector3 velocity = Vector3.zero;
    private Camera MainCamera;
    


    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        
        

    }

    public void UpdateCamera(Vector3 playePosition, Vector2 moveInput, bool LockCamera)
    {
        transform.position = playePosition;

        if(!LockCamera)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime * moveInput.x);
        }

        //make the actual camera follow
        //MainCamera.transform.position = cameraFollowTransform.position;
        //MainCamera.transform.rotation = cameraFollowTransform.rotation;

        float tempSpeed = moveDamping;

        float distance = Vector3.Distance(cameraFollowTransform.position, MainCamera.transform.position);
        float close = 50.0f;

        if(distance < close)
        {
            float slowestCameraSpeed = 0.01f;
            float alpha = distance / close;

            tempSpeed = Mathf.Lerp(slowestCameraSpeed, tempSpeed, alpha);
        }

        tempSpeed = tempSpeed * Time.deltaTime;

        MainCamera.transform.position = Vector3.MoveTowards(MainCamera.transform.position, cameraFollowTransform.position, tempSpeed);
        MainCamera.transform.rotation = Quaternion.Lerp(MainCamera.transform.rotation, cameraFollowTransform.rotation, rotateDamping * Time.deltaTime);






    }
}
