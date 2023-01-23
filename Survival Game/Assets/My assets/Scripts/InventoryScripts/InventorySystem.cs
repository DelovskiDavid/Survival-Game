using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }



    private IEnumerator PopUpCoroutine;
    public float popUpDuration;
    private float popUpTimer;
    public string waterName;

    public GameObject inventoryScreenUI;
    public GameObject itemInfoUIInstance;
    public GameObject escapeMenu;

    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    public List<KeyValuePair> winCondition = new List<KeyValuePair>();

    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;
    [HideInInspector]
    public bool isOpen;
    [HideInInspector]
    public bool isEscapeMenuOpen;
    //public bool isFull;

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
        isOpen = false;
        isEscapeMenuOpen = false;

        PopulateSlotList();

        Cursor.visible = false;
    }

    private void Update()
    {
        InventoryOpen();
        EscapeOptions();
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    public void AddToInventory(string itemName)
    {
        whatSlotToEquip = FindNextEmtySlot();

        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();

        waterName = itemName;
    }

    private GameObject FindNextEmtySlot()
    {
        foreach (GameObject slot in slotList)
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

        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }
        if (counter == 28)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void InventoryOpen()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
        }
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                }
            }
        }
        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    public void ReCalculateList()
    {
        itemList.Clear();
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str1 = "(Clone)";
                string result = name.Replace(str1, "");

                itemList.Add(result);
            }
        }
    }
    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);

        pickupName.text = "You picked up a " + itemName;
        pickupImage.sprite = itemSprite;
        popUpTimer = popUpDuration;
        if (PopUpCoroutine == null)
        {
            PopUpCoroutine = ClosePopUp();
            StartCoroutine(PopUpCoroutine);
        }
    }

    private IEnumerator ClosePopUp()
    {
        while (popUpTimer > 0)
        {
            popUpTimer -= Time.deltaTime;
            yield return null;
        }
        pickupAlert.SetActive(false);
        PopUpCoroutine = null;
    }

    private void EscapeOptions()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isEscapeMenuOpen)
        {
            escapeMenu.SetActive(true);
            CraftingSystem.Instance.craftingScreenUI.SetActive(false);
            CraftingSystem.Instance.weaponsCategoryScreenUI.SetActive(false);
            CraftingSystem.Instance.toolsCategoryScreenUI.SetActive(false);
            CraftingSystem.Instance.refineryCategoryScreenUI.SetActive(false);
            inventoryScreenUI.SetActive(false);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            isEscapeMenuOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isEscapeMenuOpen)
        {
            escapeMenu.SetActive(false);
            isEscapeMenuOpen = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}

[Serializable]

public class KeyValuePair
{

    public GameObject key;

    public int val;

}
