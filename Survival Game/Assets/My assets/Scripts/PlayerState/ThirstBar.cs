using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirstBar : MonoBehaviour
{
    private Slider thirstSlider;

    public GameObject playerState;

    private float currentThirst, maxThirst;

    private void Start()
    {
        thirstSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        ThirstUpdate();
    }

    private void ThirstUpdate()
    {
        currentThirst = playerState.GetComponent<PlayerState>().currentThirst;
        maxThirst = playerState.GetComponent<PlayerState>().maxThirst;

        float fillValue = currentThirst / maxThirst;
        thirstSlider.value = fillValue;
    }
}
