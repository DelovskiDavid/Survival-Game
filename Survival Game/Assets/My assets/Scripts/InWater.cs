using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWater : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerState.Instance.currentThirst += 100;
        }
    }
}
