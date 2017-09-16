using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RemoveCharacter : MonoBehaviour {
	private double DistanceThreshold = 1;

	void OnTriggerEnter(Collider other) {
		if (other.GetComponentInParent<NavMeshAgent> ().remainingDistance <= DistanceThreshold) { // if this is the target
			Destroy (other);
		}
	}
}
