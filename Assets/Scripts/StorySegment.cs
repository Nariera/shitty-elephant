using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class StorySegment : ScriptableObject {

    public string Descript_Name;
    public string StoryText;
    public float duration = 5.0f;
    //And in the future, add Audio Clips and images and whatever else we want.
}
