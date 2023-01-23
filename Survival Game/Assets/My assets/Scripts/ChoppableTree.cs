using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeChopped;

    public float treeMaxHealth, treeHealth;

    [HideInInspector]
    public Animator animator;
    private void Start()
    {
        treeHealth = treeMaxHealth;
        animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    private void Update()
    {
        TreeHealthUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void GetChopped()
    {
        animator.SetTrigger("Shake");
        animator.SetTrigger("ShakeTall");

        treeHealth -= 1;

        if (treeHealth <= 0)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallingDownSound);
            DestroyTree();
        }
    }
    private void DestroyTree()
    {
        Vector3 treePosition = this.transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.treeHealth.SetActive(false);

        GameObject log = Instantiate(Resources.Load<GameObject>("ChoppedTree"), new Vector3(treePosition.x, treePosition.y + 4, treePosition.z), Quaternion.Euler(89, 0, 0));
    }

    private void TreeHealthUpdate()
    {
        if (canBeChopped)
        {
            GlobalState.Instance.resourceHealth = treeHealth;
            GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
        }
    }
}
