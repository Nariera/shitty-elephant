using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{

	public float rate;

	float currentTime = 0;

	public int temp	{ get; private set; }

	public int tempReadOnly;

	void Start()
	{
		temp = 100;
	}

	internal void Reset()
	{
		temp = 100;
	}

	void Update()
	{
		tempReadOnly = temp;

		if (rate == 0)
		{
			Debug.LogWarning("Hey idiot, make rate > 0 or this might blow up.");
			return;
		}

		if (currentTime > 1 / rate)
		{
			Tick();
			currentTime = 0;
		}
		else
		{
			currentTime += Time.deltaTime;
		}
	}

	void Tick()
	{
		int dist = (int)(CameraTracking.nearestPlanetToPlayer.position - transform.position).magnitude;

		temp -= dist / 100;

		if (dist / 50 == 0)
			temp += 5;

		if (temp < 0)
			gameObject.SetActive(false);
		if (temp > 100)
			temp = 100;
	}
}
