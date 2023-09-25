//ActivationTrigger.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Activates and deactivates objects based on player location
public class ActivationTrigger : MonoBehaviour {

    [SerializeField] private GameObject displayed;
    void Start()
    {
        displayed.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            displayed.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            displayed.SetActive(false);
        }
    }
}
