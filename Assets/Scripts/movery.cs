using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class movery : MonoBehaviour {
    public Transform goal;
    NavMeshAgent agent;
       
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position; 
    }

    void Update() {
        agent.destination = goal.position; 
    }
}
