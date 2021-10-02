using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCam : MonoBehaviour
{
    public Transform car;
    public float cameraFrontBack = -5;
    public float cameraUpDown = 2;
    
    // public Vector3 angleOffset;
    // public Vector3 positionOffset;
    // public float followSpeed = 10;
    // public float lookSpeed = 10;
    //public Vector3 camPos;
    //public Vector3 turnOffset = new Vector3(-2, 0, 0);


    public void FixedUpdate()
    {
        Vector3 camPos = car.position + car.forward * cameraFrontBack + car.up * cameraUpDown;
        //Vector3 offset = new Vector3(0, objectToFollow.up.y + 1, objectToFollow.forward.z * -3);
        //transform.LookAt(objectToFollow);
        
        //transform.Rotate(objectToFollow.localEulerAngles * -1);
        transform.rotation = car.localRotation;
        //transform.Rotate(objectToFollow.localRotation.x, objectToFollow.localRotation.y, objectToFollow.localRotation.z);
        transform.position = camPos;

        //LookAtTarget();
        //MoveToTarget();
    }
    
    // public void LookAtTarget()
    // {
    //     Vector3 lookDirection = car.position - transform.position + angleOffset;
    //     Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);
    //     transform.rotation = Quaternion.Lerp(transform.rotation, 
    //                                             rot, 
    //                                             lookSpeed * Time.deltaTime);
    // }
    //
    // public void MoveToTarget()
    // {
    //     Vector3 targetPos = car.position +
    //                         car.forward +
    //                         car.right +
    //                         car.up;
    //     transform.position = Vector3.Lerp(transform.position, 
    //                                         targetPos + positionOffset, 
    //                                         followSpeed * Time.deltaTime);
    // }
}
