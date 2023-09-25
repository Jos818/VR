using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            SceneReload();
        }
    }
    /*IEnumerator SceneReload()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        yield return new WaitForSeconds(1f);
        StartCoroutine(SceneManager.FadeOut());
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(currentScene);
    }*/
    void SceneReload()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }



}
