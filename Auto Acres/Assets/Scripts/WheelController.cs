using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [Header("Wheel Settings")]
    [SerializeField] private bool powered;
    [SerializeField] private bool steerable;
    [SerializeField] private bool hasBrakes;

    [Header("References")]
    private WheelCollider m_wheelCollider;
    [SerializeField]private Transform m_wheelMesh;

    private void Start()
    {
        m_wheelCollider = GetComponentInChildren<WheelCollider>();
        //m_wheelMesh = transform.Find("WheelMesh");
    }

    public bool IsPowered()
    {
        return powered;
    }

    public bool IsSteerable()
    {
        return steerable;
    }

    public bool HasBrakes()
    {
        return hasBrakes;
    }

    public void ApplyTorque(float _powerInput)
    {
        m_wheelCollider.motorTorque = _powerInput;
    }

    public void ApplySteering(float _steerInput)
    {
        m_wheelCollider.steerAngle = _steerInput;
    }

    public void ApplyBrakes(float _brakeInput)
    {
        m_wheelCollider.brakeTorque = _brakeInput;
    }

    public void UpdatePosition()
    {
        Vector3 _wheelPos = transform.position;
        Quaternion _wheelRotation = transform.rotation;

        m_wheelCollider.GetWorldPose(out _wheelPos, out _wheelRotation);
        m_wheelMesh.transform.position = _wheelPos;
        m_wheelMesh.transform.rotation = _wheelRotation;
    }

    public float GetWheelRPM()
    {
        return m_wheelCollider.rpm;
    }
}
