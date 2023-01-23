using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource dragDropItemSound;
    public AudioSource craftingItemSound;
    public AudioSource pickingUpObjectSound;
    public AudioSource choppingWoodSound;
    public AudioSource spearAttackSound;
    public AudioSource grassWalkSound;
    public AudioSource swooshSound;
    public AudioSource ambienceSound;
    public AudioSource treeFallingDownSound;
    public AudioSource bearAttackSound;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }
}
