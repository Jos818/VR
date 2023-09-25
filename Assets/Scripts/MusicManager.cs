//MusicManager.cs by Joseph Panara for VR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//Allows the background music GameObject to remain through different scenes and continue playing music, and is deleted when a duplicate would be formed
public class MusicManager : MonoBehaviour
{
    public int currentscene;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        currentscene = SceneManager.GetActiveScene().buildIndex;
        if (currentscene == 4)
        {
            Destroy(this.gameObject);
        }


    }

    void Update()
    {
        currentscene = SceneManager.GetActiveScene().buildIndex;
        if (currentscene == 4)
        {
            Destroy(this.gameObject);
        }
 
    }
}
