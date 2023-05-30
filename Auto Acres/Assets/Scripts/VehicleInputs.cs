using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class VehicleInputs : MonoBehaviour
{
	[Header("Vehicle Input Values")]
	public Vector2 drive;
	public Vector2 look;

	[Header("Movement Settings")]
	public bool analogMovement;

	[Header("Mouse Cursor Settings")]
	public bool cursorLocked = true;
	public bool cursorInputForLook = true;
	public bool currentDeviceIsMouse = true;

	[Header("References")]
	[SerializeField] private InputManagerSO inputManagerSO;

	private void OnEnable()
	{
		inputManagerSO.driveEvent += DriveInput;
		inputManagerSO.lookEvent += LookInput;
	}

	private void OnDisable()
	{
		inputManagerSO.driveEvent -= DriveInput;
		inputManagerSO.rotateEvent -= LookInput;
	}

	public void DriveInput(Vector2 newDriveDirection)
    {
		drive = newDriveDirection;
    }

	public void LookInput(Vector2 newLookDirection)
    {
		look = newLookDirection;
    }
}
