using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Alec is tired, this script is definitely a mistake.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour {

    LineRenderer LaserLine;
    public float forceOfLaser = 1;

    public GameObject orbitalTarget = null;
    public Rigidbody2D selfRB;

	// Use this for initialization
	void Start () {
        LaserLine = GetComponent<LineRenderer>();
        LaserLine.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (orbitalTarget == null)
        {
            LaserLine.enabled = false;
            return;
        }

        LaserLine.enabled = true;
        LaserLine.SetPosition(0, transform.position);
        LaserLine.SetPosition(1, orbitalTarget.transform.position);

        Vector2 dir = transform.position - orbitalTarget.transform.position;
        Rigidbody2D targetRB = orbitalTarget.GetComponent<Rigidbody2D>();
        if (targetRB != null)
            targetRB.AddForce(-dir.normalized * forceOfLaser);
        if(selfRB != null)
            selfRB.AddForce(dir.normalized * forceOfLaser);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Orbital" || other.tag == "Player")
        {
            orbitalTarget = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Orbital" || other.tag == "Player")
        {
            orbitalTarget = null;
        }
    }
}
