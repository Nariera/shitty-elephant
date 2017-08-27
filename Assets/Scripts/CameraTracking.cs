using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraTracking : MonoBehaviour
{

	public Transform player;
	public bool colorifyMeCapn = true;
	PostProcessingBehaviour post;

	public static List<GameObject> planets = new List<GameObject>();

	void Update()
	{
		float bestDistance = -1;
		Transform nearestPlanet = transform;

		//Find nearest planet
		foreach (var t in planets)
		{
			if (bestDistance == -1)
			{
				bestDistance = (t.transform.position - player.position).sqrMagnitude;
				nearestPlanet = t.transform;
			}
			else
			{
				var thisDistance = (t.transform.position - player.position).sqrMagnitude;
				if (thisDistance < bestDistance)
				{
					bestDistance = thisDistance;
					nearestPlanet = t.transform;
				}
			}
		}

		if (nearestPlanet != transform)
		{
			//Camera position is the middle between the player and the nearest planet
			transform.position = (player.position + nearestPlanet.position) / 2;
			transform.position = new Vector3(transform.position.x, transform.position.y, -20);

			var settings = post.profile.colorGrading.settings;

			if (colorifyMeCapn)
			{
				float something = 200 - (nearestPlanet.transform.position - player.position).magnitude * 5;
				settings.basic.temperature = Mathf.Clamp(something, -100, 100);
				settings.basic.temperature = Mathf.Abs(settings.basic.temperature) < 40 ? settings.basic.temperature / 3 : settings.basic.temperature;
				settings.basic.temperature = Mathf.Abs(settings.basic.temperature) < 60 ? settings.basic.temperature / 2 : settings.basic.temperature;
				settings.basic.temperature = Mathf.Abs(settings.basic.temperature) < 80 ? settings.basic.temperature / 1.5f : settings.basic.temperature;
			}

			post.profile.colorGrading.settings = settings;
		}
	}

	void Awake()
	{
		foreach (var t in GameObject.FindGameObjectsWithTag("Planetary"))
		{
			if (!planets.Contains(t))
			{
				planets.Add(t);
			}
		}

		post = GetComponent<PostProcessingBehaviour>();
	}
}
