using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : MonoBehaviour {

    public GameObject targetObject;

    public Vector3 GetTargetPosition() {
        return targetObject.transform.position;
    }
}
