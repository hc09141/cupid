using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingChecker : MonoBehaviour {

    private int count = 0;

    public bool TouchingWall() {
        return count > 0;
    }


    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Buildings")) {
            count++;
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Buildings")) {
            count--;
        }
    }

}
