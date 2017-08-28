using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoptartSpawner : MonoBehaviour
{
	public GameObject PoptartPrefab;

	public List<GameObject> activePoptarts = new List<GameObject>();
	public float scale = 0.5f;
	private List<GameObject> poptartPool = new List<GameObject>();

	void Update()
	{
		activePoptarts.RemoveAll(obj => obj == null || !obj.activeSelf);
		poptartPool.RemoveAll(obj => obj == null);

		if (activePoptarts.Count < 50)
		{
			SpawnPoptart();
		}
	}

	void SpawnPoptart()
	{
		GameObject poptart = poptartPool.Find(obj => !obj.activeSelf);

		if (poptart == null)
		{
			poptart = Instantiate(PoptartPrefab) as GameObject;
			poptart.transform.SetParent(transform);
			poptart.transform.localScale = Vector3.one * scale;


			poptartPool.Add(poptart);
			Gravity.orbitalsTable.Add(poptart, poptart.GetComponent<Rigidbody2D>());
		}

		var xStart = Random.Range(-600, 600);
		var yStart = Random.Range(-600, 600);

		if (Mathf.Abs(xStart) > Mathf.Abs(yStart))
		{
			if (xStart < 0)
				xStart = -600 - Random.Range(0, 200);
			else
				xStart = 600 + Random.Range(0, 200);
		}
		else
		{
			if (yStart < 0)
				yStart = -600 - Random.Range(0, 200);
			else
				yStart = 600 + Random.Range(0, 200);
		}

		poptart.transform.position = new Vector3(xStart, yStart, 0);
		var boost = poptart.GetComponent<Boosters>();

		boost.initialBoost = new Vector2(-xStart / Random.Range(10, 100), -yStart / Random.Range(10, 100));
		boost.initialTorque = Random.Range(-6f, 6f);

		activePoptarts.Add(poptart);
		poptart.SetActive(true);

		StartCoroutine(ReturnToPool(poptart));
	}

	IEnumerator ReturnToPool(GameObject poptart)
	{
		yield return new WaitUntil(() => poptart == null || Mathf.Abs(poptart.transform.position.x) > 800 || Mathf.Abs(poptart.transform.position.y) > 800);

		if (poptart == null)
			yield break;

		poptart.SetActive(false);
	}
}
