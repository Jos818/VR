//Timeline Resume.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interacts with the TimelineController script. When the object is enabled, the timeline resumes playing from the point it was paused
public class TimelineResume : MonoBehaviour
{

    public TimelineController timelinecontroller;

    void OnEnable()
    {
        timelinecontroller.Resume();
    }
}
