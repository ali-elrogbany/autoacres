using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Vehicle Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private AnimationCurve motorTorque;
    [SerializeField] private AnimationCurve steeringAngle;
    [SerializeField] private float idleRPM;
    [SerializeField] private float maxRPM;
    [SerializeField] private float differentialRatio;
    [SerializeField] private float maxBrakeTorque;
    [SerializeField] private float dragFactor;
    [SerializeField] private gears[] gears;

    [Header("References")]
    [SerializeField] private WheelController[] wheels;
    private VehicleInputs vehicleInputs;
    private new Rigidbody rigidbody;

    [Header("Local Variables")]
    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_throttleInput;
    private float m_brakeInput;
    private float m_steeringInput;
    private float m_movingDirection;
    private float m_currentRPM;
    private int m_currentGear;

    private void Start()
    {
        TryGetComponent<Rigidbody>(out rigidbody);
        TryGetComponent<VehicleInputs>(out vehicleInputs);
        maxRPM = motorTorque.keys[^1].time;
        m_currentRPM = idleRPM;
        m_currentGear = 0;
        foreach (gears gear in gears)
        {
            gear.gearMaxSpeed = gear.speedToRPM.keys[^1].time;
        }
    }

    private void Update()
    {
        m_horizontalInput = vehicleInputs.drive.x;
        m_verticalInput = vehicleInputs.drive.y;
    }

    private void FixedUpdate()
    {
        Debug.Log("Current RPM: " + m_currentRPM.ToString() + " Current speed: " + GetCarSpeed() + " Current Torque: " + GetCurrentTorque() + " Current Gear: " + (m_currentGear + 1).ToString());
        m_throttleInput = m_verticalInput;
        m_steeringInput = m_horizontalInput;

        AutomaticGearShifting();

        m_currentRPM = GetCurrentRPM();

        //To determine if car is breaking or reversing
        m_movingDirection = Vector3.Dot(transform.forward, rigidbody.velocity);
        if (m_movingDirection < -0.5f && m_throttleInput > 0)
        {
            m_brakeInput = Mathf.Abs(m_throttleInput);
        }
        else if (m_movingDirection > 0.5f && m_throttleInput < 0)
        {
            m_brakeInput = Mathf.Abs(m_throttleInput);
        }
        else
        {
            m_brakeInput = 0;
        }

        //To limit the car from going over its topspeed
        if (GetCarSpeed() > 0.1f && m_throttleInput == 0f)
            AddDrag();
        else if (GetCarSpeed() > maxSpeed && m_throttleInput > 0f && GetAverageWheelRotation() > 0)
            AddDrag();
        else if (GetCarSpeed() > maxSpeed * 0.5f && m_throttleInput < 0f && GetAverageWheelRotation() < 0)
            AddDrag();
        else rigidbody.drag = 0;

        //To Apply Power, Breaking and steering to the car
        foreach (WheelController wheel in wheels)
        {
            if (wheel.IsPowered())
            {
                wheel.ApplyTorque(GetCurrentTorque() * m_throttleInput);
            }
            if (wheel.HasBrakes())
            {
                wheel.ApplyBrakes(maxBrakeTorque * m_brakeInput);
            }
            if (wheel.IsSteerable())
            {
                wheel.ApplySteering(GetSteeringAngle() * m_steeringInput);
            }
            wheel.UpdatePosition();
        }
    }

    //Helper function to add drag
    private void AddDrag()
    {
        rigidbody.drag = Mathf.Lerp(rigidbody.drag, dragFactor, Time.deltaTime);
    }

    //To get the car's speed in KM/H
    private float GetCarSpeed()
    {
        Vector3 _localVelocity = rigidbody.velocity;
        Vector3 _worldVelocity = transform.TransformDirection(_localVelocity);
        float _speedKMH = _worldVelocity.magnitude * 3.6f;
        if (_speedKMH < 0)
            _speedKMH = 0;
        return _speedKMH;
    }

    //To determine the average wheel rotation per minute for all wheels attached to the vehicle
    private float GetAverageWheelRotation()
    {
        float _sumOfRotation = 0;
        foreach (WheelController wheel in wheels)
        {
            _sumOfRotation += wheel.GetWheelRPM();
        }
        return Mathf.RoundToInt(_sumOfRotation / wheels.Length);
    }

    //To determine the steering angle
    private float GetSteeringAngle()
    {
        float _steeringSpeed = GetCarSpeed();
        if (_steeringSpeed > 100f)
            _steeringSpeed = 100f;
        return steeringAngle.Evaluate(_steeringSpeed);
    }

    //To calculate current RPM
    private float GetCurrentRPM()
    {
        return gears[m_currentGear].speedToRPM.Evaluate(GetCarSpeed());
    }

    //To calculate the torque based on the RPM and current gear
    private float GetCurrentTorque()
    {
        return motorTorque.Evaluate(m_currentRPM) * gears[m_currentGear].gearRatio * differentialRatio;
    }

    //Automatic Gearbox simulation
    private void AutomaticGearShifting()
    {
        if (GetCarSpeed() >= gears[m_currentGear].optimalShiftingSpeed && m_currentGear < gears.Length - 1 && m_movingDirection > 0.5f)
        {
            m_currentGear += 1;
            maxSpeed = gears[m_currentGear].gearMaxSpeed;
        }
        else if (m_currentGear > 0)
        {
            if (GetCarSpeed() < gears[m_currentGear-1].optimalShiftingSpeed)
            {
                m_currentGear -= 1;
                maxSpeed = gears[m_currentGear].gearMaxSpeed;
            }
        }
    }
}

[System.Serializable]
public class gears
{
    [SerializeField] public AnimationCurve speedToRPM;
    [SerializeField] public float gearRatio;
    [SerializeField] public float optimalShiftingSpeed;
    [SerializeField] public float gearMaxSpeed;
}