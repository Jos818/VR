//TimelineController.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

//Manages the Unity Timeline-based cutscenes. Pauses and resumes the timeline
public class TimelineController : MonoBehaviour
{
    public GameObject timeline;
    float pausespeed = 0;
    float resumespeed = 1;
    public PlayableDirector director;

    void Start()
    {
        director = timeline.GetComponent<PlayableDirector>();
    }

    public void Pause()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(pausespeed);
    }

    public void Resume()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(resumespeed);
    }
}
