using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitDeathCollider : MonoBehaviour
{
	static List<GameObject> boomBooms = new List<GameObject>();

	PlanetaryAtmos atmos;


	void BoomBoom(Collider2D other)
	{
		var boomBoomInMyRoomRoom = boomBooms.Find(obj => obj != null && !obj.GetComponentInChildren<ParticleSystem>().isPlaying);

		if (boomBoomInMyRoomRoom == null)
		{
			boomBoomInMyRoomRoom = Instantiate(Resources.Load("Explosion")) as GameObject;
		}
		
		boomBoomInMyRoomRoom.transform.position = other.transform.position;

		boomBoomInMyRoomRoom.SetActive(true);
		boomBoomInMyRoomRoom.GetComponentInChildren<ParticleSystem>().Play();

		if (!boomBooms.Contains(boomBoomInMyRoomRoom))
		{
			boomBooms.Add(boomBoomInMyRoomRoom);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//We probably want this to check for general orbital objects if those will exist and can burn up.
		if (other.tag == "Orbital" || other.tag == "Player")
		{
			//End the beautiful trail that brought them here
			if (atmos != null)
				atmos.DisconnectBurnFromParent(other);

			other.gameObject.SetActive(false);
			BoomBoom(other);
		}
	}

	void Awake()
	{
		atmos = GetComponentInChildren<PlanetaryAtmos>();
	}
}
