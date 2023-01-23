using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesHealthBar : MonoBehaviour
{
    private Slider slider;

    private float currentHealth, maxHealth;

    public GameObject globalState;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        HealthUpdate();
    }

    private void HealthUpdate()
    {
        currentHealth = globalState.GetComponent<GlobalState>().resourceHealth;
        maxHealth = globalState.GetComponent<GlobalState>().resourceMaxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;
    }
}
