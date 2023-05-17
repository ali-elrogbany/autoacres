using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Vehicle Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxMotorTorque;
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private float maxBrakeTorque;

    [Header("References")]
    [SerializeField] private WheelController []wheels;

    [Header("Local Variables")]
    private float m_horizontalInput;
    private float m_verticalInput;

    private void Update()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        foreach (WheelController wheel in wheels)
        {
            if (wheel.IsPowered())
            {
                wheel.ApplyTorque(maxMotorTorque * m_verticalInput);
            }
            if (wheel.IsSteerable())
            {
                wheel.ApplySteering(maxSteeringAngle * m_horizontalInput);
            }
            wheel.UpdatePosition();
        }
    }
}
