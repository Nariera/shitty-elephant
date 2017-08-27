using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteroidSpawner : MonoBehaviour
{
	public List<GameObject> activeAsteroids = new List<GameObject>();
	private List<GameObject> asteroidPool = new List<GameObject>();

	void Update()
	{
		activeAsteroids.RemoveAll(obj => obj == null || !obj.activeSelf);
		asteroidPool.RemoveAll(obj => obj == null);

		if (activeAsteroids.Count < 20)
		{
			SpawnAsteroid();
		}
	}

	void SpawnAsteroid()
	{
		GameObject asteroid = asteroidPool.Find(obj => !obj.activeSelf);

		if (asteroid == null)
		{
			asteroid = Instantiate(Resources.Load("Orbital")) as GameObject;
			asteroid.transform.SetParent(transform);
			asteroid.transform.localScale = Vector3.zero;
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

		asteroid.transform.position = new Vector3(xStart, yStart, 0);
		var boost = asteroid.GetComponent<Boosters>();

		boost.initialBoost = new Vector2(-xStart / Random.Range(10, 100), -yStart / Random.Range(10, 100));

		StartCoroutine(ReturnToPool(asteroid));
	}

	IEnumerator ReturnToPool(GameObject asteroid)
	{
		yield return new WaitUntil(() => asteroid == null || Mathf.Abs(asteroid.transform.position.x) > 800 || Mathf.Abs(asteroid.transform.position.y) > 800);

		if (asteroid == null)
			yield break;

		asteroid.SetActive(false);
	}
}
