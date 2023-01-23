using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    public float maxHealth;
    public float currentHealth;
    public float healthFallDownRate;

    public float maxHunger;
    public float currentHunger;
    public float hungerFallDownRate;
    float distanceTravelled = 0;
    float distanceTravelledThirst = 0;
    Vector3 lastPosition;
    public GameObject player;

    public float maxThirst;
    public float currentThirst;
    public float thirstFallDownRate;

    public float maxStamina;
    public float currentStamina;
    public float StaminaFallDownRate;

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
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        HungerUpdater();
        HealthUpdater();
        StaminaUpdater();
        ThirstUpdater();

        if (Input.GetKeyDown(KeyCode.I))
        {
            currentHealth -= 10;
        }

        if (UIManager.Instance.isWinPanelOpen == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HungerUpdater()
    {
        if (currentHunger >= 0)
        {
            currentHunger -= Time.deltaTime / hungerFallDownRate;
        }

        lastPosition = player.transform.position;
        distanceTravelled += Vector3.Distance(player.transform.position, lastPosition);

        if (distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentHunger -= 1f;
        }
    }

    private void ThirstUpdater()
    {
        if (currentThirst >= 0)
        {
            currentThirst -= Time.deltaTime / thirstFallDownRate;
        }

        lastPosition = player.transform.position;
        distanceTravelledThirst += Vector3.Distance(player.transform.position, lastPosition);

        if (distanceTravelledThirst >= 5)
        {
            distanceTravelledThirst = 0;
            currentThirst -= 1f;
        }
    }

    private void StaminaUpdater()
    {
        if (FirstPersonController.Instance.isSprinting == true)
        {
            currentStamina -= 0.06f;
        }
        else if (FirstPersonController.Instance.isSprinting == false)
        {
            currentStamina += 0.08f;
        }

        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
        else if (currentStamina <= 0)
        {
            FirstPersonController.Instance.sprintSpeed = FirstPersonController.Instance.walkSpeed;
            currentStamina = 0;
        }
        else if (currentStamina >= 0)
        {
            FirstPersonController.Instance.sprintSpeed = 10;
        }

    }

    private void HealthUpdater()
    {
        if (currentHunger <= 0 && currentThirst <= 0)
        {
            currentHealth -= Time.deltaTime / healthFallDownRate * 2;
        }
        else
        {
            if (currentHunger <= 0 || currentThirst <= 0)
            {
                currentHealth -= Time.deltaTime / healthFallDownRate;
            }
        }

        if (currentHealth <= 0)
        {
            PlayerDeath();
        }
    }



    public void setHealth(float newHealth)
    {
        currentHealth = newHealth;
    }

    public void setHunger(float newHunger)
    {
        currentHunger = newHunger;
    }

    public void setThirst(float newThirst)
    {
        currentThirst = newThirst;
    }

    private void PlayerDeath()
    {
        UIManager.Instance.ShowLosePanel();
        UIManager.Instance.isLosePanelOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
