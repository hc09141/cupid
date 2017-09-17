using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionController : MonoBehaviour {

    public GameObject clipboarder;
    private float nextTime = 0;

    void Update(){
        if (Input.GetButtonDown("Fire1") && Time.time > nextTime || (Input.GetButtonDown("Fire1") && Input.GetKey(KeyCode.LeftShift))){
            RaycastHit hit;
            nextTime = Time.time + 5;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)){
                SpawnClipboarder(hit.point);
            }
        }
    }

    void SpawnClipboarder(Vector3 position) {
        Vector3 spawnPoint = NearestSpawnpoint(position);
        GameObject clippy = GameObject.Instantiate<GameObject>(clipboarder);
        clippy.transform.position = spawnPoint;
        clippy.GetComponent<NavMeshAgent>().destination = NearestPointOnMesh(position);
    }

    Vector3 NearestSpawnpoint(Vector3 point)
    {
        Transform[] SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(sp => sp.transform).ToArray();
        Vector3 closest = SpawnPoints[0].position;
        for(int i = 1; i < SpawnPoints.Length; i++)
        {
            if (Vector3.Distance(closest, point) > Vector3.Distance(SpawnPoints[i].position, point))
            {
                closest = SpawnPoints[i].position;
            }
        }
        return closest;
    }

    Vector3 NearestPointOnMesh(Vector3 point)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return point;
    }
}
