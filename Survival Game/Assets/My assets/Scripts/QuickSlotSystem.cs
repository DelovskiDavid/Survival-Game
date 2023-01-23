using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotSystem : MonoBehaviour
{
    public static QuickSlotSystem Instance;

    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotList = new List<GameObject>();

    public GameObject numbersHolder;

    public int selectedNumber = -1;
    public GameObject selectedItem;
    public GameObject toolHolder;
    public GameObject selectedItemModel;

    [HideInInspector]
    public string selectedItemName;
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
        PopulateQuickSlotList();
    }

    private void Update()
    {
        ButtonPressedToEquip();
    }
    public void PopulateQuickSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlot(GameObject itemToEquip)
    {
        GameObject emptySlot = FindNextEmptyQuickSlot();
        itemToEquip.transform.SetParent(emptySlot.transform, false);

        InventorySystem.Instance.ReCalculateList();
    }

    private GameObject FindNextEmptyQuickSlot()
    {
        foreach (GameObject slot in quickSlotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;

        foreach (GameObject slot in quickSlotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ButtonPressedToEquip()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectQuickSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuickSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuickSlot(7);
        }
    }

    void SelectQuickSlot(int number)
    {
        if (CheckIfSlotFull(number) == true)
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;
                // Unselect previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }

                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;

                SetEquippedModel(selectedItem);

                //changing the color 
                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.grey;
                }
                Text numberColorToBeChanged = numbersHolder.transform.Find("Number" + number).transform.Find("Text").GetComponent<Text>();
                numberColorToBeChanged.color = Color.white;
            }
            else
            {
                selectedNumber = -1;

                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }

                if (selectedItemModel != null)
                {
                    DestroyImmediate(selectedItemModel.gameObject);
                    selectedItemModel = null;
                }

                foreach (Transform child in numbersHolder.transform)
                {
                    child.transform.Find("Text").GetComponent<Text>().color = Color.grey;
                }
            }
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null)
        {
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }

        selectedItemName = selectedItem.name.Replace("(Clone)", "");
        if (selectedItemName == "Axe")
        {
            selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"), new Vector3(0.35f, -0.1f, 1.3f)
          , Quaternion.Euler(0, -14, -103));
        }
        else if (selectedItemName == "Spear")
        {
            selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"), new Vector3(0.25f, -0.2f, 1)
          , Quaternion.Euler(27, -94, -82));
        }
        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }

    private GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotList[slotNumber - 1].transform.GetChild(0).gameObject;
    }

    private bool CheckIfSlotFull(int slotNumber)
    {
        if (quickSlotList[slotNumber - 1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
