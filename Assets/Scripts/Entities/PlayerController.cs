using UnityEngine;

public enum MovementState {
	
	Idle,
	Walking,
	Running,
	Crouching
}


public class PlayerController : MonoBehaviour {
	
	// Editor parametrs
	public float walkSpeed;
	public float runSpeed;
	public float gravity;
	public float jumpStrength;

	// Components and child objects
	private CharacterController player;
	
	// Private properties
	private Vector2 movementInput;
	private float horizontalSpeed = 0f;
	private MovementState movementState;
	private bool isRunning = false;
	private bool isJumping = false;
	private bool isGrounded = false;
	
	private float gravityAcceleration = 0f;
	
	// Controller initialization
    void Start() {
		
		FindObjects();

    }

	private void FindObjects() {

		player = GetComponent<CharacterController>();
	}

	public void SetMovementInput(Vector2 movementInput) {

		this.movementInput = movementInput;
	}

	public float GetSpeed() {

		return this.horizontalSpeed;

	}

	private void SetSpeed() {
	
		switch (movementState) {

			case MovementState.Idle:
				horizontalSpeed = 0f;
				break;

			case MovementState.Walking:
				horizontalSpeed = walkSpeed;
				break;

			case MovementState.Running:
				horizontalSpeed = runSpeed;
				break;

		}

	}

	public void SetMovementState(MovementState movementState) {

		if ((movementState == MovementState.Crouching ||
			movementState == MovementState.Running) &&
			movementInput == Vector2.zero

		) return;

		this.movementState = movementState;

	}

	public void ResolveMovementState(MovementState movementState) {


		if (this.movementState != MovementState.Running) {

			SetMovementState(movementState);
				
		}

	}

	private void Move() {
		
		Vector3 horizontalMovementVector = new Vector3(
				movementInput.x,
				0,
				movementInput.y
		);
		Vector3 verticalMovementVector = new Vector3();
		Vector3 resultMovementVector = new Vector3();

		SetSpeed();

		if (isJumping) { verticalMovementVector.y += jumpStrength; }	
		if (!isGrounded) { verticalMovementVector.y += gravityAcceleration; }

		horizontalMovementVector = transform.rotation * horizontalMovementVector * horizontalSpeed;
		resultMovementVector = horizontalMovementVector + verticalMovementVector;

		player.Move(resultMovementVector * Time.deltaTime);

		
	}

	public void Jump() {

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

			gravityAcceleration += gravity / 5f;

		}

	}

    void Update() {
		
		Move();
		
    }

	
}
