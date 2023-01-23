using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private GameObject itemInfoUI;

    private Text itemInfoUI_itemName;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;

    public string thisName, thisDescription, thisFunctionality;

    private GameObject itemPendingConsumption;

    public bool isConsumable;
    public bool isTrashable;
    public bool isEquipable;
    public bool isBuildable;
    public bool isInsideQuickSlot;
    public bool isSelected;

    public float healthEffect;
    public float hungerEffect;
    public float thirstEffect;

    private void Start()
    {

        itemInfoUI = InventorySystem.Instance.itemInfoUIInstance;
        itemInfoUI_itemName = itemInfoUI.transform.Find("ItemName").GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("ItemDescription").GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("ItemFunctionality").GetComponent<Text>();
    }

    private void Update()
    {
        CheckIfSelected();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        itemInfoUI.SetActive(true);
        itemInfoUI_itemName.text = thisName;
        itemInfoUI_itemDescription.text = thisDescription;
        itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfoUI.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable)
            {
                itemPendingConsumption = gameObject;
                consumingFunction(healthEffect, hungerEffect, thirstEffect);
            }
        

            if (isEquipable && isInsideQuickSlot == false && QuickSlotSystem.Instance.CheckIfFull() == false)
            {
                QuickSlotSystem.Instance.AddToQuickSlot(gameObject);
                isInsideQuickSlot = true;
            }

            if (isBuildable)
            {
                ConstructionManager.Instance.itemToBeDestroyed = gameObject;
                gameObject.SetActive(false);
                UseItem();
            }
        }
    }

    private void UseItem()
    {
        itemInfoUI.SetActive(false);

        InventorySystem.Instance.isOpen = false;
        InventorySystem.Instance.inventoryScreenUI.SetActive(false);

        CraftingSystem.Instance.isOpen = false;
        CraftingSystem.Instance.craftingScreenUI.SetActive(false);
        CraftingSystem.Instance.toolsCategoryScreenUI.SetActive(false);
        CraftingSystem.Instance.weaponsCategoryScreenUI.SetActive(false);
        CraftingSystem.Instance.refineryCategoryScreenUI.SetActive(false);
        CraftingSystem.Instance.constructionScreenUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.enabled = true;

        switch (gameObject.name)
        {
            case "Foundation(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
                break;
            case "Foundation":
                ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel"); //For testing
                break;
            case "Wall(Clone)":
                ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
                break;
            case "Wall":
                ConstructionManager.Instance.ActivateConstructionPlacement("WallModel"); //For testing
                break;
            default:
                //do nothing
                break;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                DestroyImmediate(gameObject);
                InventorySystem.Instance.ReCalculateList();
                CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }

    private void consumingFunction(float healthEffect, float hungerEffect, float thirstEffect)
    {
        itemInfoUI.SetActive(false);

        healthEffectCalculation(healthEffect);
        hungerEffectCalculation(hungerEffect);
        thirstEffectCalculation(thirstEffect);
    }

    private void thirstEffectCalculation(float thirstEffect)
    {
        float thirstBeforeConsumption = PlayerState.Instance.currentThirst;
        float maxThirst = PlayerState.Instance.maxThirst;

        if (thirstEffect != 0)
        {
            if ((thirstBeforeConsumption + thirstEffect) > maxThirst)
            {
                PlayerState.Instance.setThirst(maxThirst);
            }
            else
            {
                PlayerState.Instance.setThirst(thirstBeforeConsumption + thirstEffect);
            }
        }
    }

    private void hungerEffectCalculation(float hungerEffect)
    {
        float hungerBeforeConsumption = PlayerState.Instance.currentHunger;
        float maxHunger = PlayerState.Instance.maxHunger;

        if (hungerEffect != 0)
        {
            if ((hungerBeforeConsumption + hungerEffect) > maxHunger)
            {
                PlayerState.Instance.setHunger(maxHunger);
            }
            else
            {
                PlayerState.Instance.setHealth(hungerBeforeConsumption + hungerEffect);
            }
        }
    }

    private void healthEffectCalculation(float healthEffect)
    {
        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.setHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.setHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }

    private void CheckIfSelected()
    {
        if (isSelected)
        {
            gameObject.GetComponent<DragDrop>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<DragDrop>().enabled = true;
        }
    }
}
