using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public Vector3 destiny;
    public float velocity = 1;
    public float closeDistance = 5.0f;

    private CharacterController controller;
    private TargetComponent target;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        target = GetComponent<TargetComponent>();
    }
	
	// Update is called once per frame
	void Update () {
        destiny = target.GetTargetPosition(); //Camera.main.ScreenToWorldPoint(Input.mousePosition);
        destiny.z = 0;
        Vector3 origin = transform.position;
        origin.z = 0;
        Vector3 move = destiny - origin;
        
        //TODO when it arrives (close) stop moving and it not move again until it pass a higher number of close

        move.z = 0;
        move = move.normalized;
        controller.Move(move * Time.deltaTime * velocity);
    }

    public void FixedUpdate() {
        
    }
}
