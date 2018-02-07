using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillHeroComponent : MonoBehaviour {

    private GameObject targetSelected;

    private TargetComponent targetComponent;
	// Use this for initialization
	void Start () {
        targetSelected = null;
        targetComponent = GetComponent<TargetComponent>();
	}
	
	// Update is called once per frame
	void Update () {
        //TODO measure performance (quadtree of heroes?)
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");
        GameObject target = GetClosestEnemy(heroes);

        if(targetSelected==null || targetSelected != target) {
            targetSelected = target;
            targetComponent.targetObject = targetSelected;   
        }
	}

    GameObject GetClosestEnemy(GameObject[] enemies) {
        Transform bestTarget = null;
        GameObject target=null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potential in enemies) {
            Transform potentialTarget = potential.transform;
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
                target = potential;
            }
        }

        return target;
    }
}
