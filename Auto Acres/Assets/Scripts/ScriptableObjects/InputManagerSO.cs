using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(fileName = "InputManager" ,menuName = "ScriptableObjects/InputManager")]
public class InputManagerSO : ScriptableObject
{
    //Player Inputs
    public event UnityAction<Vector2> moveEvent;
    public event UnityAction<Vector2> rotateEvent;
    public event UnityAction<bool> jumpEvent;
    public event UnityAction<bool> sprintEvent;

    //Vehicle Inputs
    public event UnityAction<Vector2> driveEvent;
    public event UnityAction<Vector2> lookEvent;

    private void OnEnable()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (moveEvent != null)
        {
            moveEvent.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (rotateEvent != null)
        {
            rotateEvent.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (jumpEvent != null && context.phase == InputActionPhase.Performed)
        {
            jumpEvent.Invoke(true);
        }
        else if (context.phase == InputActionPhase.Waiting)
        {
            jumpEvent.Invoke(false);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (sprintEvent != null && context.phase == InputActionPhase.Performed)
        {
            sprintEvent.Invoke(true);
        }
        else if (context.phase != InputActionPhase.Performed)
        {
            sprintEvent.Invoke(false);
        }
    }

    public void OnDrive(InputAction.CallbackContext context)
    {
        if (driveEvent != null)
        {
            driveEvent.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (lookEvent != null)
        {
            lookEvent.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void EnableVehicleControls()
    {

    }

    public void EnablePlayerControls()
    {

    }

}
