using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent (typeof (NavMeshAgent))]
public class GreyPersonAnimationController : MonoBehaviour {

    public Vector3 desiredDirection = Vector3.forward;
    private Animator walkAnimator;
    private NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    private bool crossing = false;

    float linkTrav = 0;

    public void Start() {
        walkAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent> ();
        agent.updatePosition = false;
    }

    public void SetSpeed(float speed) {
        walkAnimator.SetFloat("Speed", speed);
    }

    public void FixedUpdate() {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
        float dx = Vector3.Dot (transform.right, worldDeltaPosition);
        float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2 (dx, dy);
        float smooth = Mathf.Min(1.0f, Time.deltaTime/0.15f);
        smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, smooth);
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;
        bool x = (agent.velocity.magnitude < 1f && !agent.isOnOffMeshLink) || (agent.destination - transform.position).magnitude < 0.5f;
        walkAnimator.SetFloat("Speed", x ? 0 : 2);
        // prevents odd stuff over links
        if(agent.isOnOffMeshLink) {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            if (data.valid) {
                if (!crossing  && worldDeltaPosition.magnitude < 0.5f) {
                    Debug.Log("Starting");
                    agent.nextPosition = data.endPos;
                    agent.isStopped = true;
                    //agent.updatePosition = false;
                    crossing = true;
                } else if (crossing) {
                    agent.transform.position = Vector3.Lerp(data.startPos, data.endPos, linkTrav);
                    linkTrav += 0.005f;
                    agent.transform.rotation = Quaternion.LookRotation(agent.nextPosition - transform.position, Vector3.up);
                        //agent.nextPosition = Vector3.Lerp(data.startPos, data.endPos, linkTrav);
                    }
                
        
            //if (worldDeltaPosition.magnitude > agent.radius)
            //    agent.isStopped = true;
            //else
            //    agent.isStopped = false;
                if((data.endPos - transform.position).magnitude < 1f) {
                    Debug.Log("Complete");
                    agent.CompleteOffMeshLink();
                    agent.isStopped = false;
                    //agent.updatePosition = true;
                    crossing = false;
                    linkTrav = 0;
                }
            }
        } else {
            //agent.isStopped = false;
        }
        // Pull agent towards character
        if (worldDeltaPosition.magnitude > agent.radius * 1.5f) {
            agent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
        }
    }

   void OnAnimatorMove (){
        // Update position to agent position
        Vector3 position = walkAnimator.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;
    }
}

