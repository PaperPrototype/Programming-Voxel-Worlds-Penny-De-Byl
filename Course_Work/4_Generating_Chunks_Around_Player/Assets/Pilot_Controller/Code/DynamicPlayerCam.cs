using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DynamicPlayerCam : MonoBehaviour
{
    [Header("The Camera")]
    [SerializeField] Transform Camera_Trans;


    [Header("To be rotated or positioned")]
    [SerializeField] Transform CameraAngle_Trans;
    [SerializeField] [Range(0f, 1f)] float CameraAngle_InterpolateTime = 0.5f;


    [Header("To be set to")]
    [SerializeField] Transform Position;
    [SerializeField] Transform Rotation;

    [SerializeField] Transform CameraFollow_Pos;
    [SerializeField] [Range(0f, 0.5f)] float CameraFollowPos_SmoothTime = 0.1f;


    [Header("Looked At")]
    [SerializeField] Transform LookAt_Pos;


    [Header("World up")]
    [SerializeField] Transform WorldUp_Trans;


    private Vector3 refVelocity = Vector3.zero;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void LateUpdate()
    {
        // setting the cameraAngle(Object) rotation
        CameraAngle_Trans.rotation = Quaternion.Lerp(CameraAngle_Trans.rotation, Rotation.rotation, CameraAngle_InterpolateTime);
        // setting the cameraAngle(Object) position
        CameraAngle_Trans.position = Position.position;


        // setting the Camera's position
        Camera_Trans.position = Vector3.SmoothDamp(Camera_Trans.position, CameraFollow_Pos.position, ref refVelocity, CameraFollowPos_SmoothTime);
        // setting the Camera's rotation(LookAt)
        Camera_Trans.LookAt(LookAt_Pos.position, WorldUp_Trans.up);
    }
}
