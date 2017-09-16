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
            if (worldDeltaPosition.magnitude > agent.radius * 1)
                agent.isStopped = true;
            else
                agent.isStopped = false;
        } else {
            agent.isStopped = false;
        }
               // Pull agent towards character
        if (worldDeltaPosition.magnitude > agent.radius * 2)
            agent.nextPosition = transform.position + 0.9f*worldDeltaPosition;
    }

    IEnumerator OffMeshLinkStart(){
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        while (true){
            if (agent.isOnOffMeshLink) {
                Debug.Log("On link");
                yield return StartCoroutine(NormalSpeed(agent));
                agent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }

    IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        while ((agent.transform.position - endPos).magnitude < 0.001f){
            Debug.Log("Moving along");
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime * 0.001f);
            yield return null;
        }
        Debug.Log("Crossed");
        agent.CompleteOffMeshLink();
    }



   void OnAnimatorMove (){
        // Update position to agent position
        Vector3 position = walkAnimator.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;
    }
}

