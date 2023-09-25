//NOT USED IN PROJECT
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookat : MonoBehaviour
{
    public GameObject camera;
    public Transform target;
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        direction = target.position - camera.transform.position;
        float step = 100 * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
