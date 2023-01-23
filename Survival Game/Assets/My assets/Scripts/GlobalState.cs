using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalState : MonoBehaviour
{
    public static GlobalState Instance { get; set; }

    public UnityEvent delayedFirstQuest;

    public float resourceHealth;
    public float resourceMaxHealth;

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
    private void Start()
    {
        Invoke("DelayedFirstQuest", 2);
    }

    private void DelayedFirstQuest()
    {
        delayedFirstQuest?.Invoke();
    }

}
