using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

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
			inputManagerSO.moveEvent += MoveInput;
			inputManagerSO.rotateEvent += LookInput;
			inputManagerSO.jumpEvent += JumpInput;
			inputManagerSO.sprintEvent += SprintInput;
        }

        private void OnDisable()
        {
			inputManagerSO.moveEvent -= MoveInput;
			inputManagerSO.rotateEvent -= LookInput;
			inputManagerSO.jumpEvent -= JumpInput;
			inputManagerSO.sprintEvent -= SprintInput;
		}

        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}