using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("FirstPersonView");
        MainMenuMusic.Instance.mainMenuMusic = null;
    }
    public void Quit()
    {
        Application.Quit();
    }

    


}
