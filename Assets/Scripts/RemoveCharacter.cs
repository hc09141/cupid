using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RemoveCharacter : MonoBehaviour {
	private double DistanceThreshold = 2;

	void OnTriggerEnter(Collider other) {
        if(other.GetComponent<Clipboarder>() != null)
        {
            return;
        }
        NavMeshAgent ag = other.GetComponentInParent<NavMeshAgent>();

		if (ag != null && (ag.destination - transform.position).magnitude <= DistanceThreshold) { // if this is the target
			Destroy (other.transform.root.gameObject);
		}
	}
}
