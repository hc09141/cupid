using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public float speed = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 v = new Vector3(transform.forward.x, 0, transform.forward.z);
            transform.position += v * speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 v = new Vector3(transform.forward.x, 0, transform.forward.z);
            transform.position += -v * speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed;
        }
	}
}
