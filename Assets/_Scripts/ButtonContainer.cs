using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonContainer : MonoBehaviour
{
    public void NextLevelButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            AsyncOperation asyn = SceneManager.LoadSceneAsync(Random.Range(0, 3));
        }

        else if (SceneManager.GetActiveScene().buildIndex < 3)
        {
            AsyncOperation asyn = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
