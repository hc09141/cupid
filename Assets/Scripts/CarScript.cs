using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarScript : MonoBehaviour {

    public bool moving = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(moving)
            transform.position += transform.forward * Time.deltaTime * 10;
	}

    void OnTriggerEnter(Collider col) {
        GameObject root = col.transform.root.gameObject;
        if(root.GetComponent<GreyPersonAnimationController>() != null) {
            root.GetComponent<GreyPersonAnimationController>().Ragdoll(transform.forward * 5);
            //root.GetComponent<NavMeshAgent>().enabled = false;
            //root.GetComponent<Animator>().enabled = false;
            //root.transform.position += Vector3.up;
            //root.GetComponent<Rigidbody>().AddForce((transform.forward + Vector3.up) * 2);
        }
    }
}
