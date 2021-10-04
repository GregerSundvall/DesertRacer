using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizInput;
    public float vertInput;
    private float steeringAngle;
    private float speedCompensation;
    [SerializeField] Vector3 centerOfMass;

    public TextMeshProUGUI speedometer;
    public TextMeshProUGUI tachometer;
    public TextMeshProUGUI gearDisplay;
    public TextMeshProUGUI barrelsDisplay;
    public WheelCollider frontLeftWC, frontRightWC, rearLeftWC, rearRightWC;
    public Transform frontLeftTr, frontRightTr;
    public Transform rearLeftTr, rearRightTr;
    public float maxSteer = 30;
    public float motorForce = 50;
    public float tqe;
    public float frontRPM;
    public float rearRPM;
    public float rearWheelSpeed;
    public float engineRPM;
    public int gear = 1;
    public bool isGrounded;

    public int barrelsHit;
    public Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.centerOfMass = centerOfMass;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "barrel")
        {
            barrelsHit++;
        }

        
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
        frontRPM = frontLeftWC.rpm;
        rearRPM = rearLeftWC.rpm;
        engineRPM = rearLeftWC.rpm * 10;
        rearWheelSpeed = rearRPM * rearLeftWC.radius * 0.377f;
        tqe = rearLeftWC.motorTorque;
        gear = Mathf.RoundToInt(engineRPM / 2800);
        gearDisplay.text = gear.ToString();
        speedCompensation = 1 - rearWheelSpeed / 200;
        speedometer.text = $"<mspace=60>{Mathf.RoundToInt(Mathf.Clamp(rearWheelSpeed, 0, 300)).ToString()}</mspace>";
        tachometer.text = $"<mspace=50>{Mathf.RoundToInt(engineRPM % 4000 + 900).ToString()}</mspace>";
        barrelsDisplay.text = barrelsHit.ToString();
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
            
            if (rearWheelSpeed < 150 
                && rearLeftWC.GetGroundHit(out WheelHit hitL) 
                || rearWheelSpeed < 150 
                && rearRightWC.GetGroundHit(out WheelHit hitR))
            {
                Debug.Log(rearLeftWC.GetGroundHit(out WheelHit hitLl));
                rearLeftWC.motorTorque = vertInput * motorForce;
                rearRightWC.motorTorque = vertInput * motorForce;
            }
        }

        if (vertInput < 0)
        {
            if (frontLeftWC.rpm > 0 || frontRightWC.rpm > 0 || rearLeftWC.rpm > 0 || rearRightWC.rpm > 0)
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

                rearLeftWC.motorTorque = vertInput * motorForce;
                rearRightWC.motorTorque = vertInput * motorForce;
                // if (rearWheelSpeed > - 50)
                // {
                //     
                // }
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
