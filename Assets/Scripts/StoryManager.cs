using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour {

    public static StoryManager Instance;

    public bool IsPlaying = false;

    public Text SubtitleText;

    // Use this for initialization
    void Awake () {
        //Check if instance already exists
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        //Get Text Component
        SubtitleText = GetComponent<Text>();
        if (SubtitleText == null)
        {
            Debug.Log("There is no Text object on Story Manager");
        }
        SubtitleText.enabled = false;
    }

    public void PlayStorySegment(StorySegment segment)
    {
        //Probably want to add this to a Queue or something.
        Debug.Log("Triggered story segment: " + segment.name);
        StartCoroutine("Play", segment);
    }

    private IEnumerator Play(StorySegment segment)
    {
        while (IsPlaying)
        {
            yield return null;
        }
        Debug.Log("Started Playing: " + segment.name);

        IsPlaying = true;
        SubtitleText.text = segment.StoryText;
        SubtitleText.enabled = true;
        yield return new WaitForSeconds(segment.duration);
        SubtitleText.text = "";
        SubtitleText.enabled = false;
        IsPlaying = false;
        yield return null;
    }

	
}
