using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : MonoBehaviour
{
    private Slider hungerSlider;

    public GameObject playerState;

    private float currentHunger, maxHunger;

    private void Start()
    {
        hungerSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        HungerUpdate();
    }

    private void HungerUpdate()
    {
        currentHunger = playerState.GetComponent<PlayerState>().currentHunger;
        maxHunger = playerState.GetComponent<PlayerState>().maxHunger;

        float fillValue = currentHunger / maxHunger;
        hungerSlider.value = fillValue;
    }
}
