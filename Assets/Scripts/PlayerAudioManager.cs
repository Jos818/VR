//PlayerAudioManager.cs by UF Digital Worlds
//Adapted by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages player audio. Added audio functions for the charge mechanic were written by Joseph Panara
public class PlayerAudioManager : MonoBehaviour
{
    public AudioClip WalkAudioClip;
    public bool LoopWalkAudio = false;
    public AudioClip ThrowAudioClip;
    public bool LoopThrowAudio = false;
    public AudioClip ChargeAudioClip;
    public bool LoopChargeAudio = false;

    [Range(0, 1)]
    public float WalkVolumeLevel = 1;
    [Range(0, 1)]
    public float ThrowVolumeLevel = 1;
    [Range(0, 1)]
    public float ChargeVolumeLevel = 1;

    [Range(0.1f, 3)]
    public float WalkAudioSpeed = 1;
    [Range(0.1f, 3)]
    public float ThrowAudioSpeed = 1;
    [Range(0.1f, 3)]
    public float ChargeAudioSpeed = 1;


    //And here is where you should create the respective AudioSource
    [HideInInspector] public AudioSource WalkSource;
    [HideInInspector] public AudioSource ThrowSource;
    [HideInInspector] public AudioSource ChargeSource;
    //The whole [HideInInspector] thing just makes it so that way you can't see these public variables in editor

    void Start()
    {
        SetUpAudio();
    }

    //Here is where you can add more audio sources and the like
    void SetUpAudio()
    {
        //First you have to make a new GameObject with a name
        GameObject WalkGameObject = new GameObject("WalkAudioSource");
        GameObject ThrowGameObject = new GameObject("ThrowAudioSource");
        GameObject ChargeGameObject = new GameObject("ChargeAudioSource");

        //Next you have to Assign the parent so it's all organized
        AssignParent(WalkGameObject);
        AssignParent(ThrowGameObject);
        AssignParent(ChargeGameObject);

        //Then you have to add the actual audiosource to each gameobject
        WalkSource = WalkGameObject.AddComponent<AudioSource>();
        ThrowSource = ThrowGameObject.AddComponent<AudioSource>();
        ChargeSource = ChargeGameObject.AddComponent<AudioSource>();
        //And finally you assign the clip to the audio source
        WalkSource.clip = WalkAudioClip;
        ThrowSource.clip = ThrowAudioClip;
        ChargeSource.clip = ChargeAudioClip;

        //And here is just where we assign the global volume level, you can make these individualized if you want
        WalkSource.volume = WalkVolumeLevel;
        ThrowSource.volume = ThrowVolumeLevel;
        ChargeSource.volume = ChargeVolumeLevel;

        WalkSource.loop = LoopWalkAudio;
        ThrowSource.loop = LoopThrowAudio;
        ChargeSource.loop = LoopChargeAudio;

        WalkSource.pitch = WalkAudioSpeed;
        ThrowSource.pitch = ThrowAudioSpeed;
        ChargeSource.pitch = ChargeAudioSpeed;
    }

    //Just a helper function that assigns whatever object as a child of this gameObject
    void AssignParent(GameObject obj)
    {
        obj.transform.parent = transform;
    }
}
