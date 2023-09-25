//GreenShift.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

//Manages a Post-Processing effect that tells the player when they are in range to grab the camera
public class GreenShift : MonoBehaviour
{
    public Player_Move_Update player;
    public PostProcessProfile normal;
    public PostProcessProfile greenshift;
    public GameObject camera;

    //Finds the camera
    void Start()
    {
        camera = gameObject;
    }

    //Applies the Post-Processing effect when the player is in range to grab the camera
    void Update()
    {
        if (player.grabtarget != null)
        {
            if (player.grabtarget.name == "CameraObj")
            {
                camera.GetComponent<PostProcessVolume>().profile = greenshift;
            }
        }
        else
        {
            camera.GetComponent<PostProcessVolume>().profile = normal;
        }
        
    }
}
