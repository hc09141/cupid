using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (NavMeshAgent))]
public class RomancerController : MonoBehaviour {

    public bool startsMoving = true;
	public string SuccessScene;
    private NavMeshAgent agent;
    public Transform desiredLocation;
    
	public AudioSource Theme;
	public AudioSource Success;
	public AudioSource Failure;

    private float buildingDistance = 10;

    int count = 0;

    float cdTime = 0;

    bool avoiding = false;

    int stress = 0;

    private Vector3 overallDestination;
    private Vector3 originalDir;


    private RomancerEffects effects;

	void Awake() {
        if (Success != null)
        {
            Success.Play();
            Success.Pause();
        }
		DontDestroyOnLoad (Success);
		DontDestroyOnLoad (Failure);
	}

	// Use this for initialization
	void Start () {
        originalDir = transform.forward;
        if(startsMoving) {
            if (desiredLocation != null)
            {
                overallDestination = desiredLocation.position;
            }else {
                overallDestination = transform.position + originalDir * 40;
            }
        } else {
            overallDestination = transform.position;
        }
        
        Debug.DrawLine(transform.position, overallDestination, Color.red, 5);

	    agent = GetComponent<NavMeshAgent> ();	
        agent.SetDestination(NearestPointOnMesh(overallDestination));

        effects = GetComponentInChildren<RomancerEffects>();
	}
	
	void FixedUpdate () {
        Debug.DrawLine(transform.position, agent.destination, Color.red);
        if (count >= 5  && cdTime < Time.time && !agent.isOnOffMeshLink) {
            count = 0;
            Debug.Log("Crossing");
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
        count+=HandleUndesirables(col.gameObject) * 2;
        //Debug.Log(count + " " + col.gameObject.name );
        if (col.transform.root.gameObject.GetComponent<GreyPersonAnimationController>() != null)
            count++;

    }

    void OnTriggerExit(Collider col) {
        count -= HandleUndesirables(col.gameObject);
        if (col.transform.root.gameObject.GetComponent<GreyPersonAnimationController>() != null)
            count--;
    }

    // returns additional stress of seeing this thing
    int HandleUndesirables(GameObject o) {
        if (o.GetComponent<Clipboarder>() != null)
        {
            return 4;
        } else
        if(o.GetComponent<Expartner>() != null) {
            BadEffects();
            Debug.Log("GAME OVER :(");
            return 10;
        } else
        if(o.GetComponent<RomancerController>() != null) {
            Debug.Log("You win");
            overallDestination = o.transform.position;
            agent.SetDestination(o.transform.position);
            if(Success != null)
			Success.Play ();
			Invoke( "success", 10 );
            effects.HappyEffects();
            return -10;
        }
        return 0;
    }

	void success() {
		SceneManager.LoadScene (SuccessScene);
	}

}
