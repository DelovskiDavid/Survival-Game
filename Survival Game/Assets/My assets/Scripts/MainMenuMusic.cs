using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    public static MainMenuMusic Instance;

    public AudioSource mainMenuMusic;

    private void Awake()
    {
        Instance = this;
    }
}
