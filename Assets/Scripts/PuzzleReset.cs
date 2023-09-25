//PuzzleReset.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script resets all the movable objects for a particular puzzle
public class PuzzleReset : MonoBehaviour
{
    public GameObject camera;
    public GameObject player;
    private bool playerinArea;
    private bool camerainArea;
    public bool active;
    [SerializeField]private List<GameObject> objects;
    [SerializeField] private List<Vector3> positions;

    //Gets a list of all objects involved in the puzzle
    void Start()
    {
        active = false;
        for (int i = 0; i<gameObject.transform.childCount; i++)
        {
            objects.Add(gameObject.transform.GetChild(i).gameObject);
            positions.Add(objects[i].transform.position);
        }
    }
    //Resets all puzzle objects to their original positions
    public void Reset()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.position = positions[i];
        }
    }
}
