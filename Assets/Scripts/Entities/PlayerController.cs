using UnityEngine;
using System.Collections;

public enum MovementState {
	
	Idle,
	Walking,
	Running,
	Dashing,
	Crouching
}


public class PlayerController : MonoBehaviour {
	
	// Editor parametrs
	public float walkSpeed;
	public float runSpeed;
	public float dashSpeed;
	public float dashDuration;
	public float gravity;
	public float jumpStrength;
	public float maxHealth;

	// Components and child objects
	private CharacterController player;
	
	// Private properties
	private Vector2 movementInput;
	private Vector2 dashMovementInput;
	private float horizontalSpeed = 0f;
	private float health;
	private MovementState movementState;
	private bool isJumping = false;
	private bool isGrounded = false;
	private bool isDashing = false;
	
	private float gravityAcceleration = 0f;

    void Start() {
		
		player = GetComponent<CharacterController>();
		health = maxHealth;
	
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

			case MovementState.Dashing:
				horizontalSpeed = dashSpeed;
				break;

		}

	}

	public void SetMovementState(MovementState movementState) {

		if (movementState != MovementState.Idle && movementInput == Vector2.zero) {

			movementState = MovementState.Idle;
		}

		this.movementState = movementState;

	}

	public void ResolveMovementState(MovementState movementState) {

		if (this.movementState == MovementState.Dashing) return;

		if (this.movementState == MovementState.Running &&
			movementState != MovementState.Dashing) {

			return;

		}

		SetMovementState(movementState);

	}

	private void Move() {
			
		SetSpeed();

		if (isDashing) {
			movementInput = dashMovementInput;
		}
		
		Vector3 horizontalMovementVector = new Vector3(
				movementInput.x,
				0,
				movementInput.y
		);
		Vector3 verticalMovementVector = new Vector3();
		Vector3 resultMovementVector = new Vector3();

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
	
	public IEnumerator Dash() {
		
		MovementState preservedMovemetState = movementState;
		SetMovementState(MovementState.Dashing);

		dashMovementInput = movementInput;
		
		PlayerManager.Instance.DisableCameraShaking();

		yield return new WaitForSeconds(dashDuration);
		
		SetMovementState(preservedMovemetState);
		PlayerManager.Instance.EnableCameraShaking();

	}

	public void TakeDamage(float damage) {

		health -= damage;

		if (health == 0) Die();

	}

	public void Die() {
	
		health = 0;

	}

	void FixedUpdate() {

		// Checks if character touches the ground
		isGrounded = Physics.Raycast(
			player.transform.position,
			Vector3.down,
			player.height / 2 + 0.1f,
			LayerMask.GetMask("Ground")	
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
