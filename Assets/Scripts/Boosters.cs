using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boosters : MonoBehaviour
{

	public Vector2 initialBoost;
	public float initialTorque;

	void OnEnable()
	{
		var rigid = GetComponent<Rigidbody2D>();
		rigid.AddForce(initialBoost, ForceMode2D.Impulse);
		rigid.AddTorque(initialTorque, ForceMode2D.Impulse);
	}
}
