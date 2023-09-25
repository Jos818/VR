//Checkpoint.cs by UF Digital Worlds
//Adapted by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Player_Move_Update player;

    //Sets respawn points for the player when they enter the trigger while holding the Camera
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && player.grabbedobject.name == "CameraObj")
        {
            GameManager temp = FindObjectOfType<GameManager>();
            if (temp != null)
            {
                temp.SetNewRespawnPlace(collision.gameObject);
            }
            else
            {
                Debug.Log("Checkpoint: ERROR no GameManager found!");
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && player.grabbedobject.name == "CameraObj")
        {
            GameManager temp = FindObjectOfType<GameManager>();
            if (temp != null)
            {
                temp.SetNewRespawnPlace(collision.gameObject);
            }
            else
            {
                Debug.Log("Checkpoint: ERROR no GameManager found!");
            }
        }
    }
}
