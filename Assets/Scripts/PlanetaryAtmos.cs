using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetaryAtmos : MonoBehaviour
{
	Dictionary<Collider2D, GameObject> burnTargs = new Dictionary<Collider2D, GameObject>();
	List<GameObject> burnInPool = new List<GameObject>();

	void OnTriggerEnter2D(Collider2D other)
	{
		burnInPool.RemoveAll(obj => obj == null);

		if (other.tag == "Orbital")
		{
			burnTargs.Add(other, BeginBurn(other));
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		burnInPool.RemoveAll(obj => obj == null);

		if (other.tag == "Orbital")
		{
			GameObject burn;
			burnTargs.TryGetValue(other, out burn);
			if (burn)
			{
				StartCoroutine(EndBurn(burn));
			}

			burnTargs.Remove(other);
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		UpdateBurnPos(other);
	}

	void UpdateBurnPos(Collider2D other)
	{
		GameObject burn;
		burnTargs.TryGetValue(other, out burn);
		if (burn)
		{
			//Put it in front of movement
			var rigid = other.GetComponent<Rigidbody2D>();
			var part = burn.GetComponentInChildren<ParticleSystem>();
			burn.transform.position = other.transform.position + (Vector3)rigid.velocity.normalized * other.bounds.extents.magnitude;

			//Angle the circle front
			var shape = part.shape;
			shape.rotation = new Vector3(0, 0, Vector3.Angle(Vector3.up, rigid.velocity.normalized) + 10);
		}
	}

	public void DisconnectBurnFromParent(Collider2D parent)
	{
		GameObject burn;
		burnTargs.TryGetValue(parent, out burn);
		if (burn)
		{
			burn.transform.SetParent(transform);
			StartCoroutine(EndBurn(burn));
		}
	}

	GameObject BeginBurn(Collider2D other)
	{
		var burn = burnInPool.Find(obj => !obj.activeSelf);

		if (burn == null)
		{
			burn = Instantiate(Resources.Load<GameObject>("Atmos Burn In")) as GameObject;

			burnInPool.Add(burn);
		}	


		burn.transform.SetParent(other.transform);
		burn.transform.localScale = Vector3.one;

		//Velocity relative to the planet
		UpdateBurnPos(other);

		burn.SetActive(true);

		return burn;
	}

	IEnumerator EndBurn(GameObject burn)
	{
		//Stop the anim
		var part = burn.GetComponentInChildren<ParticleSystem>();
		part.Stop();

		//Wait till it's wound down
		yield return new WaitUntil(() => part == null || part.isStopped);

		if (part == null)
			yield break;

		//Goodbye, return to pool
		burn.SetActive(false);
	}
}
