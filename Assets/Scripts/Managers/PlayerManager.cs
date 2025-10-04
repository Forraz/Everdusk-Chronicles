using UnityEngine;


// PlayerController and Camera Controller interface
public class PlayerManager : MonoBehaviour {

	public static PlayerManager Instance;

	private PlayerController Controller { get; set; }	
	private CameraController Camera { get; set; }

    void Start() {

		Instance ??= this;

		Controller = FindFirstObjectByType<PlayerController>();
		Camera = FindFirstObjectByType<CameraController>();

    }


	public void Move(Vector2 input) {

		Controller.SetMovementInput(input);
		Controller.ResolveMovementState(MovementState.Walking);

	}

	public void StopMove() {

		Controller.SetMovementInput(Vector2.zero);
		Controller.SetMovementState(MovementState.Idle);
	
	}

	public void Jump() {

		Controller.Jump();

	}

	public void Dash() {

		StartCoroutine(Controller.Dash());

	}

	public void Run(bool isRunning) {
	
		if (isRunning) {

			Controller.ResolveMovementState(MovementState.Running);

		} else {

			Controller.SetMovementState(MovementState.Walking);

		}
	
	}

	public void Look(Vector2 input) {

		Camera.Look(input);

	}

	public void DealDamage(float damage) {
	
		Controller.TakeDamage(damage);
	
	}


	public void EnableCameraShaking() {

		Camera.EnableShaking();

	}

	public void DisableCameraShaking() {


		Camera.DisableShaking();

	}

}
