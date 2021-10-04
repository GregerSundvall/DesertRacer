using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCam : MonoBehaviour
{
    public Transform car;
    public float cameraFrontBack = -5;
    public float cameraUpDown = 2;
    private Vector3 velocity = Vector3.zero;


    public void FixedUpdate()
    {
        Vector3 camPos = car.position + car.forward * cameraFrontBack + car.up * cameraUpDown;
        
        
        
        transform.rotation = car.localRotation;
        //transform.position = camPos;
        transform.position = Vector3.SmoothDamp(transform.position, camPos, ref velocity, 0.2f * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.rotation.eulerAngles, car.localRotation.eulerAngles, ref velocity, 0.2f * Time.deltaTime));

        //LookAtTarget();
    }
    
}
