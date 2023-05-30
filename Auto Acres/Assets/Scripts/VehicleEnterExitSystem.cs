using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehicleEnterExitSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float enterRadius;

    [Header("Components")]
    private VehicleController vehicle;
    private GameObject player;
    private SphereCollider enterZone;
    private Cinemachine.CinemachineVirtualCamera virtualCamera;
    private Transform vehicleCameraRoot;
    private PlayerInput playerInputComponent;

    [Header("Local Variables")]
    private bool m_driving;
    private bool m_isDrivable;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameObject.TryGetComponent<VehicleController>(out vehicle);
        gameObject.TryGetComponent<SphereCollider>(out enterZone);
        GameObject.FindGameObjectWithTag("InputManager").TryGetComponent<PlayerInput>(out playerInputComponent);

        virtualCamera = GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        vehicleCameraRoot = gameObject.transform.Find("CameraRoot");

        enterZone.radius = enterRadius;

        m_driving = false;
        if (!m_driving)
            vehicle.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && m_isDrivable && !m_driving)
        {
            m_driving = true;
            vehicle.enabled = true;
            player.GetComponent<StarterAssets.ThirdPersonController>().enabled = false;
            player.transform.SetParent(gameObject.transform);
            player.SetActive(false);
            virtualCamera.Follow = vehicleCameraRoot;
            playerInputComponent.SwitchCurrentActionMap("Vehicle");
            Debug.Log(playerInputComponent.currentActionMap);
        }
        else if (Input.GetKeyDown(KeyCode.F) && m_driving)
        {
            m_driving = false;
            vehicle.enabled = false;
            player.SetActive(true);
            player.GetComponent<StarterAssets.ThirdPersonController>().enabled = true;
            player.transform.SetParent(null);
            virtualCamera.Follow = player.transform.Find("PlayerCameraRoot");
            playerInputComponent.SwitchCurrentActionMap("Player");
            Debug.Log(playerInputComponent.currentActionMap);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_driving)
        {
            m_isDrivable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!m_driving)
        {
            m_isDrivable = false;
        }
    }
}
