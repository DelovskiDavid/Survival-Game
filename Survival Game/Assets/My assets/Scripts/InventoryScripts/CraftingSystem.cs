using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem Instance { get; set; }

    public UnityEvent wonGame;
    public UnityEvent resourcesCollected;

    public GameObject craftingScreenUI;
    public GameObject toolsCategoryScreenUI, weaponsCategoryScreenUI, refineryCategoryScreenUI, constructionScreenUI;

    public List<string> inventoryItemList = new List<string>();

    public bool isOpen;

    public int stoneCount = 0;
    public int stickCount = 0;
    public int logCount = 0;
    public int meatCount = 0;
    public int plankCount = 0;

    //Category Buttons
    Button toolsBTN, weaponsBTN, refineryBTN, constructionBTN;

    //Craft Buttons
    Button craftAxeBTN, craftSpearBTN, craftPlanksBTN, craftFoundationBTN, craftWallBTN;

    //Requirement text
    Text axeReq1, axeReq2;
    Text spearReq1, spearReq2;
    Text planksReq;
    Text foundationReq, wallReq;

    //Blueprints
    public Blueprint AxeBLP = new Blueprint(1, "Axe", 2, "Stone", 3, "Stick", 3);
    public Blueprint SpearBLP = new Blueprint(1, "Spear", 2, "Stone", 3, "Stick", 3);
    public Blueprint PlanksBLP = new Blueprint(2, "Plank", 1, "Log", 1, "", 0);
    public Blueprint FoundationBLP = new Blueprint(1, "Foundation", 1, "Planks", 4, "", 0);
    public Blueprint WallBLP = new Blueprint(1, "Wall", 1, "Planks", 2, "", 0);


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


        CategoryScreenButtons();

        //Axe
        axeReq1 = toolsCategoryScreenUI.transform.Find("Axe").transform.Find("AxeReq1").GetComponent<Text>();
        axeReq2 = toolsCategoryScreenUI.transform.Find("Axe").transform.Find("AxeReq2").GetComponent<Text>();

        craftAxeBTN = toolsCategoryScreenUI.transform.Find("Axe").transform.Find("CraftAxeButton").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftItem(AxeBLP); });

        //Spear
        spearReq1 = weaponsCategoryScreenUI.transform.Find("Spear").transform.Find("SpearReq1").GetComponent<Text>();
        spearReq2 = weaponsCategoryScreenUI.transform.Find("Spear").transform.Find("SpearReq2").GetComponent<Text>();

        craftSpearBTN = weaponsCategoryScreenUI.transform.Find("Spear").transform.Find("CraftSpearButton").GetComponent<Button>();
        craftSpearBTN.onClick.AddListener(delegate { CraftItem(SpearBLP); });

        //Planks
        planksReq = refineryCategoryScreenUI.transform.Find("Planks").transform.Find("planksReq").GetComponent<Text>();

        craftPlanksBTN = refineryCategoryScreenUI.transform.Find("Planks").transform.Find("CraftPlanksButton").GetComponent<Button>();
        craftPlanksBTN.onClick.AddListener(delegate { CraftItem(PlanksBLP); });

        //Buildings
        foundationReq = constructionScreenUI.transform.Find("Foundation").transform.Find("foundationReq").GetComponent<Text>();
        wallReq = constructionScreenUI.transform.Find("Wall").transform.Find("wallReq").GetComponent<Text>();

        craftFoundationBTN = constructionScreenUI.transform.Find("Foundation").transform.Find("CraftFoundationButton").GetComponent<Button>();
        craftFoundationBTN.onClick.AddListener(delegate { CraftItem(FoundationBLP); });

        craftWallBTN = constructionScreenUI.transform.Find("Wall").transform.Find("CraftWallButton").GetComponent<Button>();
        craftWallBTN.onClick.AddListener(delegate { CraftItem(WallBLP); });


    }

    private void Update()
    {
        OpenCraftingScreen();
    }
    private void OpenToolsCategoryScreen()
    {
        craftingScreenUI.SetActive(false);
        refineryCategoryScreenUI.SetActive(false);
        weaponsCategoryScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        toolsCategoryScreenUI.SetActive(true);
    }

    private void OpenWeaponsCategoryScreen()
    {
        craftingScreenUI.SetActive(false);
        toolsCategoryScreenUI.SetActive(false);
        refineryCategoryScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        weaponsCategoryScreenUI.SetActive(true);
    }

    private void OpenRefineryCategoryScreen()
    {
        craftingScreenUI.SetActive(false);
        toolsCategoryScreenUI.SetActive(false);
        weaponsCategoryScreenUI.SetActive(false);
        constructionScreenUI.SetActive(false);
        refineryCategoryScreenUI.SetActive(true);
    }

    private void OpenConstructionScreenUI()
    {
        craftingScreenUI.SetActive(false);
        toolsCategoryScreenUI.SetActive(false);
        weaponsCategoryScreenUI.SetActive(false);
        refineryCategoryScreenUI.SetActive(false);
        constructionScreenUI.SetActive(true);
    }
    private void OpenCraftingScreen()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

            isOpen = true;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolsCategoryScreenUI.SetActive(false);
            weaponsCategoryScreenUI.SetActive(false);
            refineryCategoryScreenUI.SetActive(false);
            constructionScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;

            isOpen = false;
        }
    }
    void CraftItem(Blueprint blueprintToCraft)
    {
        for (int i = 0; i < blueprintToCraft.amountToCraft; i++)
        {
            InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
        }


        if (blueprintToCraft.numOfReq == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req1, blueprintToCraft.req1Amount);
        }
        else if (blueprintToCraft.numOfReq == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req1, blueprintToCraft.req1Amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req2, blueprintToCraft.req2Amount);
        }

        SoundManager.Instance.PlaySound(SoundManager.Instance.craftingItemSound);

        StartCoroutine(Calculate());
    }

    public IEnumerator Calculate()
    {
        yield return 0;

        InventorySystem.Instance.ReCalculateList();

        RefreshNeededItems();
    }

    private void CategoryScreenButtons()
    {
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolsCategoryScreen(); });

        weaponsBTN = craftingScreenUI.transform.Find("WeaponsButton").GetComponent<Button>();
        weaponsBTN.onClick.AddListener(delegate { OpenWeaponsCategoryScreen(); });

        refineryBTN = craftingScreenUI.transform.Find("RefineryButton").GetComponent<Button>();
        refineryBTN.onClick.AddListener(delegate { OpenRefineryCategoryScreen(); });

        constructionBTN = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();
        constructionBTN.onClick.AddListener(delegate { OpenConstructionScreenUI(); });
    }

    public void RefreshNeededItems()
    {
        stoneCount = 0;
        stickCount = 0;
        logCount = 0;
        plankCount = 0;
        meatCount = 0;

        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stoneCount += 1;
                    break;
                case "Stick":
                    stickCount += 1;
                    break;
                case "Log":
                    logCount += 1;
                    break;
                case "RabbitMeat":
                    meatCount += 1;
                    break;
                case "DeerMeat":
                    meatCount += 1;
                    break;
                case "BearMeat":
                    meatCount += 1;
                    break;
                case "Plank":
                    plankCount += 1;
                    break;

            }
        }

        if (plankCount >= 10 && meatCount >= 5)
        {
            resourcesCollected?.Invoke();
        }

        //--Axe--//

        axeReq1.text = "3 Stone [" + stoneCount + "]";
        axeReq2.text = "3 Stick [" + stickCount + "]";

        if (stoneCount >= 3 && stickCount >= 3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }

        // --Spear-- //

        spearReq1.text = "3 Stone [" + stoneCount + "]";
        spearReq2.text = "3 Stick [" + stickCount + "]";

        if (stoneCount >= 3 && stickCount >= 3)
        {
            craftSpearBTN.gameObject.SetActive(true);
        }
        else
        {
            craftSpearBTN.gameObject.SetActive(false);
        }

        //--Planks--//

        planksReq.text = "1 Log [" + logCount + "]";

        if (logCount >= 1)
        {
            craftPlanksBTN.gameObject.SetActive(true);
        }
        else
        {
            craftPlanksBTN.gameObject.SetActive(false);
        }

        //--Buildings--//

        foundationReq.text = "4 Planks [" + plankCount + "]";

        if (plankCount >= 4)
        {
            craftFoundationBTN.gameObject.SetActive(true);
        }
        else
        {
            craftFoundationBTN.gameObject.SetActive(false);
        }

        wallReq.text = "3 Planks [" + plankCount + "]";

        if (plankCount >= 3)
        {
            craftWallBTN.gameObject.SetActive(true);
        }
        else
        {
            craftWallBTN.gameObject.SetActive(false);
        }

    }

    public void CheckWinCondition()
    {
        if (plankCount >= 10 && meatCount >= 5)
        {
            Debug.Log("Game Won");
            wonGame?.Invoke();
            UIManager.Instance.isWinPanelOpen = true;
        }
    }
}
