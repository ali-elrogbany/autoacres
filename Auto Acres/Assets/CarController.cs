using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Vehicle Settings")]

    public List<AxleInfo> axleInfos;
    [SerializeField] private float topSpeed;
    [SerializeField] private float maxMotorTorque;
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private float maxBrakeTorque;

    [Header("Local Variables")]
    private float horizontalInput;
    private float verticalInput;
    private float motor;
    private float steering;


    public void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        ApplyTorque();
        ApplySteering();

        foreach (AxleInfo axleInfo in axleInfos)
        {
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }
    }

    private void ApplyTorque()
    {
        motor = maxMotorTorque * verticalInput;
    }

    private void ApplySteering()
    {
        steering = maxSteeringAngle * horizontalInput;
    }

}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

