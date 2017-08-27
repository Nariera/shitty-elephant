using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStorySegment : MonoBehaviour {

    public StorySegment segment;
  
    // Use this for initialization
    void Start () {
        Achievement.instance.Register(segment.name, PlayStory);
	}

    private void PlayStory()
    {
        //Somehow access Subtitle Text
        StoryManager.Instance.PlayStorySegment(segment);
        enabled = false;
    }
	
    public void TriggerStory()
    {
        Achievement.instance.Trigger(segment.name);
    }
}
