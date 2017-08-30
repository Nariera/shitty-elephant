using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWin : MonoBehaviour
{
	private string PlanetName = "";
	[SerializeField]
	private Text PlanetText;
	private static int _totalVisits = 0;
	private bool _hasBeenVisited = false;

	private void Start()
	{
		PlanetName = transform.parent.transform.parent.name;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{        
		if (collision.GetComponent<PlayerControl>() != null)
		{
			Debug.Log(PlanetName + "'s Atmo trigger!");
			bool earthVisited = false;

			Achievement.instance.Trigger(PlanetName + " approached.");

			if (!_hasBeenVisited && Achievement.instance.IsTriggered(PlanetName + " approached."))
			{
				_totalVisits++;
				_hasBeenVisited = true;
			}

			if (Achievement.instance.IsTriggered("Arf approached."))
				earthVisited = true;

			if (earthVisited && _totalVisits > 4 && PlanetName == "Arf")
			{
				Achievement.instance.Trigger("VictoryScreen");
			}

			if (PlanetText != null)
				PlanetText.text = "Planets visited : " + _totalVisits;
		}
	}
}
