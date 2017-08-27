using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpin : MonoBehaviour
{
	public float spedAngle;
	private float realSpedAngle = 0;

	void Update()
	{
		transform.Rotate(0, 0, realSpedAngle);
	}

	void Start()
	{
		realSpedAngle = Random.Range(spedAngle / 2, spedAngle);
		if (Random.Range(0, 2) == 0)
			realSpedAngle = -realSpedAngle;
	}
}
