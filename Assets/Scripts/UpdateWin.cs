using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateWin : MonoBehaviour
{
    private string PlanetName = "";

    private void Start()
    {
        PlanetName = transform.parent.transform.parent.name;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.GetComponent<PlayerControl>() != null)
        {
            Debug.Log(PlanetName + "'s Atmo trigger!");
            Achievement.instance.Trigger(PlanetName + " approached.");
			int totalVisits = 0;
			bool earthVisited = false;
			if (Achievement.instance.IsTriggered("Facey McFace approached."))
				totalVisits++;
			if (Achievement.instance.IsTriggered("Blue Giant approached."))
				totalVisits++;
			if (Achievement.instance.IsTriggered("Mudders Anonymopus approached."))
				totalVisits++;
			if (Achievement.instance.IsTriggered("Dinosaur Egg approached."))
				totalVisits++;
			if (Achievement.instance.IsTriggered("Cratered My Yoshi approached."))
				totalVisits++;
			if (Achievement.instance.IsTriggered("Cam's Vacation approached."))
				totalVisits++;
			if (Achievement.instance.IsTriggered("Jackie approached."))
				totalVisits++;
			if (Achievement.instance.IsTriggered("The Shining approached."))
				totalVisits++;
			if (Achievement.instance.IsTriggered("Arf approached."))
			{
				totalVisits++;
				earthVisited = true;
			}
			if (earthVisited && totalVisits > 4 && PlanetName == "Arf")
			{
				Achievement.instance.Trigger("VictoryScreen");
			}
        }
    }
}
