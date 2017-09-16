using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class RomancerController : MonoBehaviour {

    public bool startsMoving = true;
    private NavMeshAgent agent;
    
    private float buildingDistance = 1000;

    int count = 0;

    float cdTime = 0;

    bool avoiding = false;

    private Vector3 overallDestination;
    private Vector3 originalDir;

	// Use this for initialization
	void Start () {
        originalDir = transform.forward;
        overallDestination = transform.position + originalDir * 40;
        
        Debug.DrawLine(transform.position, overallDestination, Color.red, 5);

	    agent = GetComponent<NavMeshAgent> ();	
        agent.SetDestination(NearestPointOnMesh(overallDestination));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Debug.DrawLine(transform.position, agent.destination, Color.red);
	    if(count > 3 && cdTime < Time.time && !agent.isOnOffMeshLink) {
            Debug.Log("Crossing road");
            CrossRoad();
            cdTime = Time.time + 10;
            avoiding = true;
        }
        if((agent.destination - agent.nextPosition).magnitude < 1f) {
            if(avoiding) {
                agent.SetDestination(transform.position + originalDir * 20);
                avoiding = false;
            } else {
                Debug.Log("wah");
                agent.SetDestination(overallDestination);
            }
        }
	}

    Vector3 NearestPointOnMesh(Vector3 point) {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas)) {
            return hit.position;
            //Debug.DrawLine(transform.position, hit.position, Color.red, 5);
        }
        return point;
    }

    void CrossRoad() {
        RaycastHit rightHit;
        bool rightCast = Physics.Raycast(transform.position, transform.right, out rightHit);

        RaycastHit leftHit;
        bool leftCast = Physics.Raycast(transform.position, -transform.right, out leftHit);

        if(rightCast && rightHit.transform.CompareTag("Buildings") && rightHit.distance < buildingDistance) {
            agent.SetDestination(NearestPointOnMesh(leftHit.point));
        }
    }

    void OnTriggerEnter(Collider col) {
        count++;
        Debug.Log("Enter " + count);
    }

    void OnTriggerExit(Collider col) {
        count--;
    }


}
