using UnityEngine;


public class CameraController: MonoBehaviour {

	public float mouseSensivity;
	public float fov;

	private PlayerController playerController;
	private Camera playerCamera;

	private Vector3 lookVectorX;
	private Vector3 lookVectorY;

	private bool isShaking = true;
	private float shakeTimer = 0f;
	private float shakeMaxTime = 1f;
	private float shakeAmplitude = 2f;

	
	
	void Start() {

		FindObjects();

		Cursor.lockState = CursorLockMode.Locked;

	}

	private void FindObjects() {

		playerController = FindFirstObjectByType<PlayerController>();
		playerCamera = GetComponent<Camera>();

	}

	public void Look(Vector2 lookInput) {

		lookVectorX.x = -lookInput.y * mouseSensivity / 10;
		lookVectorX = ClampLookX(lookVectorX);

		lookVectorY.y = lookInput.x * mouseSensivity / 10;

	}

	private static float ShakeFunction(float shakeTime, float shakeMaxTime, float shakeAmplitude) {

		return Mathf.Sin(shakeTime / shakeMaxTime * 2 * Mathf.PI) * shakeAmplitude;	

	}

	public void Shake() {

		float prevShakeValue = ShakeFunction(shakeTimer, shakeMaxTime, shakeAmplitude);

		if (shakeTimer >= shakeMaxTime) shakeTimer = 0;
		shakeTimer += Time.deltaTime;

		float shakeValue = ShakeFunction(shakeTimer, shakeMaxTime, shakeAmplitude);

		lookVectorX.x += shakeValue - prevShakeValue;
	
	}

	private Vector2 ClampLookX(Vector3 lookVector) {

		float resultCameraRotation = lookVector.x + transform.eulerAngles.x;

		if (resultCameraRotation >= 180) resultCameraRotation -= 360;

		if (resultCameraRotation >= 90 || resultCameraRotation <= -90) {

			lookVector.x = 0;

		}

		return lookVector;

	}

	void Update() {	

		float playerSpeed = playerController.GetSpeed();
		
		float targetFOV = fov + playerSpeed * 1.5f;
		if (playerCamera.fieldOfView != targetFOV) {

			playerCamera.fieldOfView = Mathf.MoveTowards(
				playerCamera.fieldOfView,
				targetFOV,
				Time.deltaTime * 175f
			);

		}

		if (playerSpeed == 0f) { playerSpeed = 2f; } 
		shakeMaxTime =  5 / playerSpeed;
		shakeAmplitude = playerSpeed * 0.05f;	

		if (isShaking) {

			Shake();

		}
				

		transform.Rotate(lookVectorX);
		playerController.transform.Rotate(lookVectorY);

		lookVectorX = Vector3.zero;
		lookVectorY = Vector3.zero;

	}

}
