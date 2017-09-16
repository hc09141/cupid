using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GreyPersonAnimationController))]
public class GreyPersonController : MonoBehaviour {

    GreyPersonAnimationController anim;

    Vector3 originalDir;
    int colCount = 0;
    public bool turnsCorners = false;

    public bool caresAboutPersonalSpace = true;

    public float speed = 0;

    private bool moveTowardsPoint = false;
    public Vector3 desiredPos;

    public BuildingChecker left;
    public BuildingChecker right;

    int cornerTurnDir = 0; // 1 for right 2 for left

	// Use this for initialization
	void Start () {
        anim = GetComponent<GreyPersonAnimationController>();
        anim.desiredDirection = transform.forward;
        //MoveTowardsPoint(Vector3.zero);
        speed = 1;
        turnsCorners = true;
    }

    void FixedUpdate() {
        anim.SetSpeed(speed);
        if (moveTowardsPoint && (transform.position - desiredPos).magnitude < 4f) {
            speed = 0;
            Reset();
        } else if(turnsCorners) {
            // if right
            if(cornerTurnDir == 0) {
                if(right.TouchingWall()) {
                    Debug.Log("Found right wall");
                    cornerTurnDir = 1;
                } else if(left.TouchingWall()) {
                    cornerTurnDir = 2;
                }
            }
            if(cornerTurnDir == 1 && !right.TouchingWall()) {
                anim.desiredDirection = transform.right;
                Debug.Log("not Touching right wall");
            } else {
                anim.desiredDirection = transform.forward;
            }
            if(cornerTurnDir == 2 && right.TouchingWall()) {
                anim.desiredDirection = -transform.right;
            }
        }
    }

    void MoveTowardsPoint(Vector3 point) {
        desiredPos = point;
        moveTowardsPoint = true;
        Vector3 dir = (point - transform.position).normalized;
        dir.y = 0;
        dir.Normalize();
        originalDir = anim.desiredDirection;
        anim.desiredDirection = dir;
        caresAboutPersonalSpace = false;
    }

    void Reset() {
        anim.desiredDirection = originalDir;
        caresAboutPersonalSpace = true;
        moveTowardsPoint = false;
    }

    void OnTriggerEnter(Collider other) {
        if(!caresAboutPersonalSpace || other.CompareTag("Buildings")) {
            return;
        }

        if (colCount == 0) {
            originalDir = anim.desiredDirection;
            anim.desiredDirection = transform.right;
        }
        colCount++;
    }

    void OnTriggerExit(Collider other) {
        if(!caresAboutPersonalSpace || other.CompareTag("Buildings")) {
            return;
        }

        if(colCount == 1) {
            anim.desiredDirection = originalDir;
        }
        colCount--;
    }
}
