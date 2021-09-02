using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCam : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;
    public float followSpeed = 10;
    public float lookSpeed = 10;
    //public Vector3 turnOffset = new Vector3(-2, 0, 0);

    public void Start()
    {
        
    }

    public void FixedUpdate()
    {
        LookAtTarget();
        MoveToTarget();
    }
    
    public void LookAtTarget()
    {
        Vector3 lookDirection = objectToFollow.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, 
                                                rot, 
                                                lookSpeed * Time.deltaTime);
    }

    public void MoveToTarget()
    {
        Vector3 targetPos = objectToFollow.position +
                            objectToFollow.forward * offset.z +
                            objectToFollow.right * offset.x +
                            objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, 
                                            targetPos, 
                                            followSpeed * Time.deltaTime);
    }
}
