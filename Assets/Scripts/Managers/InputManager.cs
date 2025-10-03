using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {

	private PlayerInput playerInput;

	private PlayerController playerController;
	private CameraController playerCameraController;

    void Awake() {

		BindObjects();
		BindEvents();

    }
	
	private void BindObjects() {

		playerCameraController = FindFirstObjectByType<CameraController>();
		playerController = FindFirstObjectByType<PlayerController>();

		playerInput = new PlayerInput();
		playerInput.Enable();
	}

	private void BindEvents() {
		
		playerInput.PlayerMovement.Look.performed += LookHandler;
		playerInput.PlayerMovement.Jump.performed += JumpHandler;

		playerInput.PlayerMovement.Movement.performed += MovementHandler;
		playerInput.PlayerMovement.Movement.canceled += StopMovementHandler;

		playerInput.PlayerMovement.Run.performed += RunHandler;
		playerInput.PlayerMovement.Run.canceled += StopRunHandler;
	}

	private void OnEnable() {

		playerInput.PlayerMovement.Enable();

	}

	private void OnDisable() {

		playerInput.PlayerMovement.Disable();

	}

	private void LookHandler(InputAction.CallbackContext ctx) {

		playerCameraController.Look(ctx.ReadValue<Vector2>());	

	}

	private void MovementHandler(InputAction.CallbackContext ctx) {
		
		playerController.SetMovementInput(ctx.ReadValue<Vector2>());
		playerController.ResolveMovementState(MovementState.Walking);

	}

	private void StopMovementHandler(InputAction.CallbackContext ctx) {
		
		playerController.SetMovementInput(Vector2.zero);
		playerController.SetMovementState(MovementState.Idle);

	}

	private void RunHandler(InputAction.CallbackContext ctx) {
		
		playerController.ResolveMovementState(MovementState.Running);

	}
	private void StopRunHandler(InputAction.CallbackContext ctx) {

		playerController.SetMovementState(MovementState.Walking);

	}

	private void JumpHandler(InputAction.CallbackContext ctx) {

		playerController.Jump();

	}
}
