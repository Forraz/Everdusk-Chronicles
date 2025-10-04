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
		playerInput.PlayerMovement.Dash.performed += DashHandler;

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

		PlayerManager.Instance.Look(ctx.ReadValue<Vector2>());

	}

	private void MovementHandler(InputAction.CallbackContext ctx) {

		PlayerManager.Instance.Move(ctx.ReadValue<Vector2>());

	}

	private void StopMovementHandler(InputAction.CallbackContext ctx) {
		
		PlayerManager.Instance.StopMove();

	}

	private void RunHandler(InputAction.CallbackContext ctx) {
		
		PlayerManager.Instance.Run(true);

	}
	private void StopRunHandler(InputAction.CallbackContext ctx) {

		PlayerManager.Instance.Run(false);

	}

	private void JumpHandler(InputAction.CallbackContext ctx) {

		PlayerManager.Instance.Jump();

	}

	private void DashHandler(InputAction.CallbackContext ctx) {

		PlayerManager.Instance.Dash();

	}
}
