using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; set; }

    public GameObject interaction_Info_UI;
    Text interaction_text;

    public bool onTarget;

    [HideInInspector]
    public GameObject selectedObject;

    public Image centerDot;
    public Image grabHand;

    [HideInInspector]
    public GameObject selectedTree;
    public GameObject treeHealth;

    [HideInInspector]
    public GameObject selectedAnimal;
    //public GameObject animalHealthBar;
    public GameObject animalHealthHolder;
    private Text animalHealthName;

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<Text>();
        onTarget = false;
        animalHealthName = animalHealthHolder.transform.Find("Text").GetComponent<Text>();
    }

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
    void Update()
    {
        RayCastHit();
    }

    private void RayCastHit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
            ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();
            AttackableAnimal attackableAnimal = selectionTransform.GetComponent<AttackableAnimal>();

            if (attackableAnimal && attackableAnimal.playerInRange && QuickSlotSystem.Instance.selectedItemName == "Spear")
            {
                attackableAnimal.canBeAttacked = true;
                selectedAnimal = attackableAnimal.gameObject;
                animalHealthHolder.SetActive(true);
                animalHealthName.text = (attackableAnimal.animalName);
            }
            else
            {
                if (selectedAnimal != null)
                {
                    selectedAnimal.gameObject.GetComponent<AttackableAnimal>().canBeAttacked = false;
                    selectedAnimal = null;
                    animalHealthHolder.SetActive(false);
                }
            }

            if (choppableTree && choppableTree.playerInRange && QuickSlotSystem.Instance.selectedItemName == "Axe")
            {
                choppableTree.canBeChopped = true;
                selectedTree = choppableTree.gameObject;
                treeHealth.SetActive(true);
            }
            else
            {
                if (selectedTree != null)
                {
                    selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
                    selectedTree = null;
                    treeHealth.SetActive(false);
                }
            }

            if (interactable && interactable.inRange)
            {
                selectedObject = interactable.gameObject;

                onTarget = true;

                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);

                if (interactable.CompareTag("Pickable"))
                {
                    centerDot.gameObject.SetActive(false);
                    grabHand.gameObject.SetActive(true);
                }
                else
                {
                    centerDot.gameObject.SetActive(true);
                    grabHand.gameObject.SetActive(false); 
                }

            }
            else // if there is a hit but without an interactable script
            {
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                centerDot.gameObject.SetActive(true);
                grabHand.gameObject.SetActive(false);
            }

        }
        else // if there is no hit at all
        {
            onTarget = false;
            interaction_Info_UI.SetActive(false);
            centerDot.gameObject.SetActive(true);
            grabHand.gameObject.SetActive(false);
            treeHealth.SetActive(false);
            animalHealthHolder.SetActive(false);
        }
    }

    public void EnableSelection()
    {
        grabHand.enabled = true;
        centerDot.enabled = true;
        interaction_Info_UI.SetActive(true);
    }

    public void DisableSelection()
    {
        grabHand.enabled = false;
        centerDot.enabled = false;
        interaction_Info_UI.SetActive(false);

        selectedObject = null;
    }
}
