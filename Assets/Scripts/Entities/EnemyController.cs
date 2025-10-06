using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

	PlayerController player;
	NavMeshAgent navAgent;

    void Start() {
        
		player = FindFirstObjectByType<PlayerController>();
		navAgent = GetComponent<NavMeshAgent>();

    }

    void Update() {
        
		navAgent.destination = player.transform.position;
    }
}
