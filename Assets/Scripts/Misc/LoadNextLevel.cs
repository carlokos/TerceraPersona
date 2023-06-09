using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    //script basico que carga el siguiente nivel 
    private Scene SceneToLoad;
    [SerializeField] private bool FinalLevel;

    private void Awake()
    {
        SceneToLoad = SceneManager.GetActiveScene();
    }

    public void loadNextLevelScene()
    {
        SceneManager.LoadScene(SceneToLoad.buildIndex + 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!FinalLevel)
                loadNextLevelScene();
            else
                SceneManager.LoadScene(0);
        }
    }
}
