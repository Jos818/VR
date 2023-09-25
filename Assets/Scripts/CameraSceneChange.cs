//CameraSceneChange.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Ensures that the player is holding the camera in order for scene changes to occur
public class CameraSceneChange : MonoBehaviour
{
    public Player_Move_Update player;
    public GameObject warningtext;
    public int scene;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Move_Update>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (player.grabbedobject != null)
            {
                if (player.grabbedobject.name == "CameraObj")
                {
                    SceneManager.LoadScene(scene);
                }
                else
                {
                    warningtext.SetActive(true);
                }
            }
            else
            {
                warningtext.SetActive(true);
            }

        }
       
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            warningtext.SetActive(false);
        }
    }
}

