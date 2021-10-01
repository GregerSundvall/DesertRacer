using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizInput;
    public float vertInput;
    private float steeringAngle;
    private float speedCompensation;
    [SerializeField] Vector3 centerOfMass;

    public WheelCollider frontLeftWC, frontRightWC;
    public WheelCollider rearLeftWC, rearRightWC;
    public Transform frontLeftTr, frontRightTr;
    public Transform rearLeftTr, rearRightTr;
    public float maxSteer = 30;
    public float motorForce = 50;
    public float tqe;
    public float frontRPM;
    public float rearRPM;
    public float rearWheelSpeed;

    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
        frontRPM = frontLeftWC.rpm;
        rearRPM = rearLeftWC.rpm;
        rearWheelSpeed = rearRPM * rearLeftWC.radius * 0.377f;
        tqe = rearLeftWC.motorTorque;
        speedCompensation = 1 - (rearWheelSpeed / 200);
    }


    private void GetInput()
    {
        horizInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");
    }
    
    private void Steer()
    {
        steeringAngle = maxSteer * horizInput * speedCompensation;
        frontLeftWC.steerAngle = steeringAngle;
        frontRightWC.steerAngle = steeringAngle;
    }

    private void Accelerate()
    {
        if (vertInput == 0)
        {
            rearLeftWC.motorTorque = 0;
            rearRightWC.motorTorque = 0;

            if (rearWheelSpeed > 1)
            {
                rearLeftWC.brakeTorque = 50;
                rearRightWC.brakeTorque = 50;
            }
        }
        
        if (vertInput > 0)
        {
            frontLeftWC.brakeTorque = 0;
            frontRightWC.brakeTorque = 0;
            rearLeftWC.brakeTorque = 0;
            rearRightWC.brakeTorque = 0;
            
            if (rearWheelSpeed < 160)
            {
                rearLeftWC.motorTorque = vertInput * motorForce;
                rearRightWC.motorTorque = vertInput * motorForce;
            }
        }

        if (vertInput < 0)
        {
            if (rearWheelSpeed > 0)
            {
                frontLeftWC.brakeTorque = (vertInput * -1) * (80 * rearWheelSpeed * speedCompensation);
                frontRightWC.brakeTorque = (vertInput * -1) * (80 * rearWheelSpeed * speedCompensation);
                rearLeftWC.brakeTorque = (vertInput * -1) * (20 * rearWheelSpeed * speedCompensation);
                rearRightWC.brakeTorque = (vertInput * -1) * (20 * rearWheelSpeed * speedCompensation);
            }
            else
            {
                frontLeftWC.brakeTorque = 0;
                frontRightWC.brakeTorque = 0;
                rearLeftWC.brakeTorque = 0;
                rearRightWC.brakeTorque = 0;

                if (rearWheelSpeed > - 50)
                {
                    rearLeftWC.motorTorque = vertInput * motorForce;
                    rearRightWC.motorTorque = vertInput * motorForce;
                }
            }
        }
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWC, frontLeftTr);
        UpdateWheelPose(frontRightWC, frontRightTr);
        UpdateWheelPose(rearLeftWC, rearLeftTr);
        UpdateWheelPose(rearRightWC, rearRightTr);
    }

    private void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation;
        
        collider.GetWorldPose(out pos, out quat);
        transform.position = pos;
        transform.rotation = quat;
    }
    
}
