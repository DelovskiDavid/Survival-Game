using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Slider staminaSlider;

    public GameObject playerState;

    private float currentStamina, maxStamina;

    private void Start()
    {
        staminaSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        StaminaUpdate();
    }

    private void StaminaUpdate()
    {
        currentStamina = playerState.GetComponent<PlayerState>().currentStamina;
        maxStamina = playerState.GetComponent<PlayerState>().maxStamina;

        float fillValue = currentStamina / maxStamina;
        staminaSlider.value = fillValue;
    }
}
