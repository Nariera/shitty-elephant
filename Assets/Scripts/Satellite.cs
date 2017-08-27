using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Satellite : MonoBehaviour
{
	public Canvas canvas;

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.collider.GetComponent<PlayerControl>() != null)
		{
			canvas.gameObject.SetActive(true);
		}
	}
}
