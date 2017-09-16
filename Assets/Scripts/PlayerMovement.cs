using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour {
	public Vector3 goal;
    NavMeshAgent agent;
       
    void Start () {
        agent = GetComponent<NavMeshAgent>();
		goal = new Vector3 (0, 0, 0);
		goal = agent.transform.position;
    }

    public void SetGoal(Vector3 goal){
        agent.destination = NearestPointOnMesh(goal);
    }

    void Update() {
		//if (Input.GetButtonDown("Fire1")){
		//	RaycastHit hit;
		//	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//	if (Physics.Raycast (ray, out hit)) {
		//		goal = NearestPointOnMesh(hit.point);
		//	}
		//}

		//agent.destination = goal; 
    }

	Vector3 NearestPointOnMesh(Vector3 point) {
		NavMeshHit hit;
		if (NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas)) {
			return hit.position;
		}
		return point;
	}
}
