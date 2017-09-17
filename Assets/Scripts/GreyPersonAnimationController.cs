using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
[RequireComponent (typeof (NavMeshAgent))]
public class GreyPersonAnimationController : MonoBehaviour {

    public Vector3 desiredDirection = Vector3.forward;
    private Animator walkAnimator;
    private NavMeshAgent agent;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;
	public string DeathScene;
	public AudioSource Theme;
	public AudioSource DeathSound;

    private bool crossing = false;
    public bool ragdolled = false;

    float linkTrav = 0;

    public void Start() {
        walkAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent> ();
        agent.updatePosition = false;
		DontDestroyOnLoad (DeathSound);
    }

    public void SetSpeed(float speed) {
        walkAnimator.SetFloat("Speed", speed);
    }

    public void FixedUpdate() {
        if (ragdolled){
            Rigidbody rb = GetComponent<Rigidbody>();
            if(rb.velocity.magnitude < 10f){
                rb.isKinematic = true;
                rb.useGravity = false;
                Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
                foreach(Rigidbody r in rbs){
                    r.useGravity = false;
                    r.isKinematic = true;
                }
            }
        }else {

            Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2(dx, dy);
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);
            if (Time.deltaTime > 1e-5f)
                velocity = smoothDeltaPosition / Time.deltaTime;
            bool x = (agent.velocity.magnitude < 1f && !agent.isOnOffMeshLink) || (agent.destination - transform.position).magnitude < 0.5f;
            walkAnimator.SetFloat("Speed", x ? 0 : 2);
            // prevents odd stuff over links
            if (agent.isOnOffMeshLink)
            {
                OffMeshLinkData data = agent.currentOffMeshLinkData;
                if (data.valid || true)
                {
                    if (!crossing && worldDeltaPosition.magnitude < 1f)
                    {
                        Debug.Log("Starting");
                        agent.nextPosition = data.endPos;
                        agent.isStopped = true;
                        //agent.updatePosition = false;
                        crossing = true;
                    }
                    else if (!crossing)
                    {
                        crossing = true;
                    }
                    else if (crossing)
                    {
                        //agent.transform.position = Vector3.Lerp(data.startPos, data.endPos, linkTrav);
                        //linkTrav += 0.005f;
                        agent.transform.position = Vector3.MoveTowards(agent.transform.position, data.endPos, agent.speed * Time.deltaTime * 0.5f);
                        agent.transform.rotation = Quaternion.LookRotation(data.endPos - transform.position, Vector3.up);
                        //agent.nextPosition = Vector3.Lerp(data.startPos, data.endPos, linkTrav);
                        Debug.DrawLine(data.endPos, transform.position, Color.cyan);
                    }

                    if ((data.endPos - transform.position).magnitude < 0.5f && crossing)
                    {
                        Debug.Log("Complete");
                        agent.CompleteOffMeshLink();
                        agent.isStopped = false;
                        crossing = false;
                        //agent.updatePosition = true;
                        linkTrav = 0;
                    }
                }
            }
            // Pull agent towards character
            if (worldDeltaPosition.magnitude > agent.radius * 1.5f)
            {
                agent.nextPosition = transform.position + 0.9f * worldDeltaPosition;
            }
        }
    }

    public void Ragdoll(Vector3 force) {
        if(!ragdolled) {
			if (GetComponent<RomancerController> () != null) {
				DeathSound.Play();
				Theme.Stop ();
				Invoke ("Die", 4);
			}
            GetComponent<Animator>().enabled = false;
            agent.enabled = false;
            transform.position += Vector3.up;
            ragdolled = true;
            Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
            foreach(Rigidbody rb in rbs) {
                rb.AddForce(force);
            }
        }
    }
   void OnAnimatorMove (){
        // Update position to agent position
        Vector3 position = walkAnimator.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;
    }

	void Die() {
		SceneManager.LoadScene (DeathScene);
	}
}

