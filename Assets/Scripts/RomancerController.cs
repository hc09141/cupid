using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class RomancerController : MonoBehaviour {

    public bool startsMoving = true;
    private NavMeshAgent agent;
    
    private float buildingDistance = 10;

    int count = 0;

    float cdTime = 0;

    bool avoiding = false;

    int stress = 0;

    private Vector3 overallDestination;
    private Vector3 originalDir;


    private RomancerEffects effects;
	// Use this for initialization
	void Start () {
        originalDir = transform.forward;
        overallDestination = transform.position + originalDir * 40;
        
        Debug.DrawLine(transform.position, overallDestination, Color.red, 5);

	    agent = GetComponent<NavMeshAgent> ();	
        agent.SetDestination(NearestPointOnMesh(overallDestination));

        effects = GetComponentInChildren<RomancerEffects>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Debug.DrawLine(transform.position, agent.destination, Color.red);
	    if(count >= 5  && cdTime < Time.time && !agent.isOnOffMeshLink) {
            CrossRoad();
            BadEffects();
            avoiding = true;
            cdTime = Time.time + 10;
        }
        if((agent.destination - agent.nextPosition).magnitude < 1f) {
            if(avoiding) {
                agent.SetDestination(transform.position + originalDir * 20);
                avoiding = false;
            } else {
                agent.SetDestination(overallDestination);
            }
        }
	}

    void BadEffects() {
        stress++;
        effects.CauseAngry();
        if(stress >= 2) {
            effects.CloudsPlay();
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
        if(leftCast && leftHit.transform.CompareTag("Buildings") && leftHit.distance < buildingDistance) {
            agent.SetDestination(NearestPointOnMesh(rightHit.point));
        }

    }

    void OnTriggerEnter(Collider col) {
        count++;
    }

    void OnTriggerExit(Collider col) {
        count--;
    }


    void HandleUndesirables(GameObject o) {
    }

}
