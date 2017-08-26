using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOrbitalObject : MonoBehaviour {

    public Transform GravityWell;
    public float GravAcceleration = 2;

    public Transform selfTransform;
    Rigidbody2D self;

	// Use this for initialization
	void Start () {
        selfTransform = gameObject.transform;
        self = gameObject.GetComponent<Rigidbody2D>();

        //Just want to add some initial propulsion
        self.AddForce(selfTransform.up*100);
    }
	
	// Update is called once per frame
	void Update () {
        //Gravity.  Add Force of Accelaration * direction
        Vector2 dir = selfTransform.position - GravityWell.position;
        self.AddForce(dir.normalized * -GravAcceleration);


    }
}
