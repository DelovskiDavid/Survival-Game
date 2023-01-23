using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TrashSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject deleteAlertUI;

    private Text deleteText;

    private Image imageSpriteComponent;

    private Button YesBTN;
    private Button NoBTN;

    public Sprite TrashCanClosed;
    public Sprite TrashCanOpen;

    GameObject itemToBeDeleted;
    GameObject draggedItem
    {
        get
        {
            return DragDrop.itemBeingDragged;
        }
    }

    public string itemName
    {
        get
        {
            string name = itemToBeDeleted.name;
            string toRemove = "(Clone)";
            string result = name.Replace(toRemove, "");
            return result;
        }
    }

    private void Start()
    {
        imageSpriteComponent = transform.Find("TrashCan").GetComponent<Image>();

        deleteText = deleteAlertUI.transform.Find("Text").GetComponent<Text>();

        YesBTN = deleteAlertUI.transform.Find("Yes").GetComponent<Button>();
        YesBTN.onClick.AddListener(delegate { DeleteItem(); });

        NoBTN = deleteAlertUI.transform.Find("No").GetComponent<Button>();
        NoBTN.onClick.AddListener(delegate { CancelDeletion(); });
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            itemToBeDeleted = draggedItem.gameObject;

            StartCoroutine(NotifyBeforeDeletion());
        }
    }

    private IEnumerator NotifyBeforeDeletion()
    {
        deleteAlertUI.SetActive(true);
        deleteText.text = "Do you want to delete this " + itemName + " from your inventory ?";
        yield return new WaitForSeconds(1);
    }

    private void CancelDeletion()
    {
        imageSpriteComponent.sprite = TrashCanClosed;
        deleteAlertUI.SetActive(false);
    }

    private void DeleteItem()
    {
        imageSpriteComponent.sprite = TrashCanClosed;
        DestroyImmediate(itemToBeDeleted.gameObject);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
        deleteAlertUI.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageSpriteComponent.sprite = TrashCanOpen;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
        {
            imageSpriteComponent.sprite = TrashCanClosed;
        }
    }
}
