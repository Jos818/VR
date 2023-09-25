//NOT USED IN PROJECT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionReactivate : MonoBehaviour
{
    public Collider trigger;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            trigger.enabled = false;
               trigger.enabled = true;
        }
    }
}
