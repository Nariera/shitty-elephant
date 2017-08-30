using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraTracking : MonoBehaviour
{

	public Transform player;
	private PlayerCondition condition;
	public bool colorifyMeCapn = true;
	PostProcessingBehaviour post;

	public static List<GameObject> planets = new List<GameObject>();
	public static Transform nearestPlanetToPlayer;
	public float camLerpSpeed = 1;
	static Transform lastNearestPlanet;
	float lerpVal = 0;
	bool initialCameraMove = true;

	float elephantDistanceFromEdge = 5;

	void Update()
	{
		float bestDistance = -1;
		nearestPlanetToPlayer = transform;

		//Find nearest planet
		foreach (var t in planets)
		{
			if (bestDistance == -1)
			{
				bestDistance = (t.transform.position - player.position).sqrMagnitude;
				nearestPlanetToPlayer = t.transform;
			}
			else
			{
				var thisDistance = (t.transform.position - player.position).sqrMagnitude;
				if (thisDistance < bestDistance)
				{
					bestDistance = thisDistance;
					nearestPlanetToPlayer = t.transform;
				}
			}
		}

		if (nearestPlanetToPlayer != transform)
		{
			MoveCameraTowardsNewPlanet(nearestPlanetToPlayer, instant: initialCameraMove);

			//Done with that
			initialCameraMove = false;

			var settings = post.profile.colorGrading.settings;

			if (colorifyMeCapn)
			{
				float something = 200 - (nearestPlanetToPlayer.transform.position - player.position).magnitude * 5;
				settings.basic.temperature = Mathf.Clamp(something, -100, 100);
				settings.basic.temperature = Mathf.Abs(settings.basic.temperature) < 40 ? settings.basic.temperature / 3 : settings.basic.temperature;
				settings.basic.temperature = Mathf.Abs(settings.basic.temperature) < 60 ? settings.basic.temperature / 2 : settings.basic.temperature;
				settings.basic.temperature = Mathf.Abs(settings.basic.temperature) < 80 ? settings.basic.temperature / 1.5f : settings.basic.temperature;

				settings.basic.contrast = condition.temp * 0.01f + 0.5f;
			}

			post.profile.colorGrading.settings = settings;
		}
	}

	public void MoveCameraTowardsNewPlanet(Transform planet, bool instant = false)
	{
		//Camera position is ideally the middle between the player and the nearest planet
		var idealPosition = (player.position + planet.position) / 2;

		//Keep player on screen
		var xClamped = ClampX(idealPosition.x);
		var yClamped = ClampY(idealPosition.y);

		//This is the real position to use
		var newPosition = new Vector3(xClamped, yClamped, -20);

		//This is a new target!
		if (planet != lastNearestPlanet)
		{
			lastNearestPlanet = planet;
			lerpVal = 0;
		}

		//Lerp pos
		if (!instant)
		{
			//Be sure to still force clamp (if the player is moving faster than the camera lerps)
			var currentPosClamped = new Vector3(ClampX(transform.position.x), ClampY(transform.position.y), -20);

			transform.position = Vector3.Lerp(currentPosClamped, newPosition, lerpVal);

			lerpVal += 0.00001f * camLerpSpeed;
		}
		//Instants and 
		else
		{
			transform.position = newPosition;
		}
	}

	float ClampX(float xPos)
	{
		return Mathf.Clamp(xPos, player.position.x - Camera.main.orthographicSize * Camera.main.aspect + elephantDistanceFromEdge, 
			player.position.x + Camera.main.orthographicSize * Camera.main.aspect - elephantDistanceFromEdge);
	}

	float ClampY(float yPos)
	{
		return Mathf.Clamp(yPos, player.position.y - Camera.main.orthographicSize + elephantDistanceFromEdge, 
			player.position.y + Camera.main.orthographicSize - elephantDistanceFromEdge);
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

		if (player != null)
			condition = player.GetComponent<PlayerCondition>();
	}
}
