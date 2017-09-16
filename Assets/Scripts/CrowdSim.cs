using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class CrowdSim : MonoBehaviour {
	public GameObject CharacterToSpawn;
	public double SpawnTime;

	private Transform[] SpawnPoints;
	private double NextSpawnTime;
	private System.Random random;

	// Use this for initialization
	void Start () {
		SpawnPoints = GameObject.FindGameObjectsWithTag ("SpawnPoint").Select(sp => sp.transform).ToArray();
		NextSpawnTime = Time.time;
		random = new System.Random ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= NextSpawnTime) { // Only spawn every x seconds
			NextSpawnTime += SpawnTime;
			int StartSpawn = random.Next (0, SpawnPoints.Length);
			int EndSpawn = random.Next (0, SpawnPoints.Length);
			while (StartSpawn == EndSpawn) { // ensures start and end spawn points are different
				EndSpawn = random.Next (0, SpawnPoints.Length);
			}
			GameObject SpawnedCharacter = (GameObject) Instantiate (CharacterToSpawn, SpawnPoints[StartSpawn]); // places at start point
			SpawnedCharacter.transform.position = SpawnPoints[StartSpawn].position;
			SpawnedCharacter.GetComponent<movery>().goal = SpawnPoints [EndSpawn];
			NavMeshAgent agent = SpawnedCharacter.GetComponent<NavMeshAgent>();
			agent.SetDestination (SpawnPoints [EndSpawn].position);
		}
	}
}
