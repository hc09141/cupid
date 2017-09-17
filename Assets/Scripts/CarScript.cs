using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarScript : MonoBehaviour {

    public bool moving = false;
    public float maxDist = 100;
    public float speed = 10;
    private Vector3 op;
	// Use this for initialization
	void Start () {
        op = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (moving)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
            if(Vector3.Distance(op, transform.position) > maxDist)
            {
                transform.position = op;
            }
        }
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
