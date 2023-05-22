using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [Header("Vehicle Settings")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxMotorTorque;
    [SerializeField] private AnimationCurve steeringAngle;
    [SerializeField] private float maxBrakeTorque;
    [SerializeField] private float dragFactor;

    [Header("References")]
    [SerializeField] private WheelController []wheels;
    private new Rigidbody rigidbody;

    [Header("Local Variables")]
    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_throttleInput;
    private float m_brakeInput;
    private float m_steeringInput;
    private float m_movingDirection;

    private void Start()
    {
        TryGetComponent<Rigidbody>(out rigidbody);
    }

    private void Update()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Debug.Log(GetSteeringAngle());
        m_throttleInput = m_verticalInput;
        m_steeringInput = m_horizontalInput;

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

        if (GetCarSpeed() > 0.1f && m_throttleInput == 0f)
            AddDrag();
        else if (GetCarSpeed() > maxSpeed && m_throttleInput > 0f && GetAverageWheelRotation() > 0)
            AddDrag();
        else if (GetCarSpeed() > maxSpeed * 0.5f && m_throttleInput < 0f && GetAverageWheelRotation() < 0)
            AddDrag();
        else rigidbody.drag = 0;

        foreach (WheelController wheel in wheels)
        {
            if (wheel.IsPowered())
            {
                wheel.ApplyTorque(maxMotorTorque * m_throttleInput);
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

    private void AddDrag()
    {
        rigidbody.drag = Mathf.Lerp(rigidbody.drag, dragFactor, Time.deltaTime);
    }

    private float GetCarSpeed()
    {
        Vector3 _localVelocity = rigidbody.velocity;
        Vector3 _worldVelocity = transform.TransformDirection(_localVelocity);
        float _speedKMH = _worldVelocity.magnitude * 3.6f;
        return _speedKMH;
    }

    private float GetAverageWheelRotation()
    {
        float _sumOfRotation = 0;
        foreach (WheelController wheel in wheels)
        {
            _sumOfRotation += wheel.GetWheelRPM();
        }
        return Mathf.RoundToInt(_sumOfRotation / wheels.Length);
    }

    private float GetSteeringAngle()
    {
        float _steeringSpeed = GetCarSpeed();
        if (_steeringSpeed > 100f)
            _steeringSpeed = 100f;
        return steeringAngle.Evaluate(_steeringSpeed);
    }
}
