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
        //StartCoroutine(OffMeshLinkStart());
    }

    //public void SetDirection(Vector3 vec) {
    //    Vector3 right = Vector3.Cross(Vector3.up, Vector3.right);
    //    Vector3 forward = Vector3.Cross(Vector3.right, Vector3.up);
    //    desiredDirection = Mathf.Atan2(Vector3.Dot(vec, right), Vector3.Dot(vec, forward));
    //}

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
        //Vector3 dirVec = Quaternion.AngleAxis(desiredDirection, Vector3.up) * Vector3.forward;
        walkAnimator.SetFloat("Speed", agent.remainingDistance);
        //if (worldDeltaPosition.magnitude > agent.radius)
        //    transform.position = agent.nextPosition - 0.9f * worldDeltaPosition;
        if (worldDeltaPosition.magnitude > agent.radius)
            //    agent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
            agent.isStopped = true;
        else
            agent.isStopped = false;

        if (agent.isOnOffMeshLink) {
                Debug.Log("On link");

        }
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

