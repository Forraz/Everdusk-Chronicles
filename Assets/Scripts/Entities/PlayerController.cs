using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
	
	// Editor parametrs
	public float walkSpeed;
	public float runSpeed;
	public float mouseSensivity;
	public float gravity;
	public float jumpStrength;

	// Components and child objects
	private CharacterController player;
	private Camera playerCamera;
	private PlayerInput input;
	
	// Private properties
	private Vector2 movementInput;
	private Vector2 rotationInput;
	private Vector2 cameraRotationInput;
	private bool isRunning = false;
	private bool isJumping = false;
	private bool isGrounded = false;
	
	private float gravityAcceleration = 0f;
	
	// Controller initialization
    void Awake() {
		
		FindObjects();
		BindInputs();	

		Cursor.lockState = CursorLockMode.Locked;
    }

	private void FindObjects() {

		this.player = GetComponent<CharacterController>();
		this.playerCamera = FindFirstObjectByType<Camera>();
		this.input = new PlayerInput();
	}

	// Handler to event mapping
	private void BindInputs() {

		input.PlayerMovement.Movement.performed += movementHandler; 
		input.PlayerMovement.Movement.canceled += movementHandler;
		
		input.PlayerMovement.Run.performed += isRunningHandler;
		input.PlayerMovement.Run.canceled += isRunningHandler;

		input.PlayerMovement.Look.performed += rotationHandler;
		input.PlayerMovement.Look.canceled += rotationHandler;

		input.PlayerMovement.Jump.performed += jumpHandler;
	}

	private void OnEnable() {

		input.PlayerMovement.Enable();

	}

	private void OnDisable() {

		input.PlayerMovement.Disable();

	}

	private void movementHandler(InputAction.CallbackContext ctx) {
		
		this.movementInput = ctx.ReadValue<Vector2>();
	}

	private void isRunningHandler(InputAction.CallbackContext ctx) {

		this.isRunning = ctx.ReadValueAsButton();

	}

	private void rotationHandler(InputAction.CallbackContext ctx) {

		this.rotationInput = ctx.ReadValue<Vector2>();

	}
	
	private void jumpHandler(InputAction.CallbackContext ctx) {
		
		if (isGrounded) {

			isJumping = true;

		}

	}

	void FixedUpdate() {

		// Check if character touches the ground
		isGrounded = Physics.CheckSphere(
				player.transform.position,
				player.height / 2 + 0.1f ,
				LayerMask.GetMask("Ground") | LayerMask.GetMask("Player")
		);

		if (isGrounded) {

			isJumping = false;
			gravityAcceleration = 0f;

		} else {

			gravityAcceleration += gravity / 50f;

		}

	}

    void Update() {
   
		Vector3 movementVector = new Vector3();
		Vector3 rotationVector = new Vector3();
		Vector3 cameraRotationVector = new Vector3();

		movementVector.x = movementInput.x;
		movementVector.z = movementInput.y;

		rotationVector.y = rotationInput.x;
		cameraRotationVector.x = -rotationInput.y;

		// Camera clamp
		float resultCameraRotation = cameraRotationVector.x + playerCamera.transform.eulerAngles.x;

		if (resultCameraRotation >= 180) resultCameraRotation -= 360;

		if (resultCameraRotation >= 90 || resultCameraRotation <= -90) {

			cameraRotationVector.x = 0;

		}
		
	

		if (isJumping) {

			movementVector.y += jumpStrength;

		}
		
		if (!isGrounded) {

			movementVector.y += gravityAcceleration;
		}
		
		// Movement states 
		if (isRunning) {

			movementVector *= runSpeed;

		} else {

			movementVector *= walkSpeed;

		}

		movementVector = transform.rotation * movementVector; 
		movementVector *= Time.deltaTime;

		rotationVector *= mouseSensivity;
		rotationVector *= Time.deltaTime;

		cameraRotationVector *= mouseSensivity;
		cameraRotationVector *= Time.deltaTime;


		player.Move(movementVector);
		player.transform.Rotate(rotationVector);
		playerCamera.transform.Rotate(cameraRotationVector);
    }

	
}
