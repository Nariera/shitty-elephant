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
			burnTargs.Add(other, BeginBurn(other.transform));
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
				burn.SetActive(false);
			burnTargs.Remove(other);
		}
	}

	GameObject BeginBurn(Transform target)
	{
		var burn = burnInPool.Find(obj => obj.activeSelf);

		if (burn == null)
		{
			burn = Instantiate(Resources.Load<GameObject>("Atmos Burn In")) as GameObject;

			burnInPool.Add(burn);
		}	

		burn.transform.SetParent(target);
		burn.transform.localScale = Vector3.one;

		var rigid = target.GetComponent<Rigidbody2D>();

		//Velocity relative to the planet
		burn.transform.localPosition = ((transform.position - target.position).normalized - (Vector3)rigid.velocity.normalized);

		print(burn.transform.localPosition);

		burn.SetActive(true);

		return burn;
	}
}
