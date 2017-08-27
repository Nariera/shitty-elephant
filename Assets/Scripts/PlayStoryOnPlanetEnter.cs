using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStoryOnPlanetEnter : PlayStorySegment{

    /// <summary>
    /// Fire the Achievement the first time the player enters the planets gravity.
    /// </summary>
    /// <param name="other"></param>
	private void OnTriggerEnter2D(Collider2D other)
    {
        if(segment == null)
        {
            return;
        }
        PlayerControl thePlayer = other.GetComponent<PlayerControl>();
        if(thePlayer != null)
        {
            if (!Achievement.instance.IsTriggered(segment.name))
            {
                Achievement.instance.Trigger(segment.name);
            }
        }
    }
}
