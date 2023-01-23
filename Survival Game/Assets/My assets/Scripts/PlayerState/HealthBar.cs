using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider healthSlider;
    
    public GameObject playerState;

    private float currentHealth, maxHealth;

    private void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        HealthUpdate();
    }

    private void HealthUpdate()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;
        healthSlider.value = fillValue;
    }
}
