using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPanel : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("FirstPersonView");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitTODesktop()
    {
        Application.Quit();
    }
}
