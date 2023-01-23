using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{
    [HideInInspector]
    public Animator axeAnimator;
    [HideInInspector]
    public Animator spearAnimator;

    public float staminaSpentDoingAction;

    private void Start()
    {
        axeAnimator = GetComponent<Animator>();
        spearAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        AxeAttack();
        SpearAttack();
    }

    private void AxeAttack()
    {
        if (Input.GetMouseButtonDown(0) && !InventorySystem.Instance.isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            axeAnimator.SetTrigger("hit");
        }

    }

    private void SpearAttack()
    {
        if (Input.GetMouseButtonDown(0) && !InventorySystem.Instance.isOpen && !ConstructionManager.Instance.inConstructionMode)
        {
            spearAnimator.SetTrigger("SpearAttack");

        }

    }

    public void GetAttacked()
    {
        GameObject selectedAnimal = SelectionManager.Instance.selectedAnimal;

        if (selectedAnimal != null)
        {
            selectedAnimal.GetComponent<AttackableAnimal>().GetHit();
        }
    }

    public void ChopTree()
    {
        GameObject selectedTree = SelectionManager.Instance.selectedTree;

        if (selectedTree != null)
        {
            selectedTree.GetComponent<ChoppableTree>().GetChopped();
        }
    }

    public void AxeHitSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.choppingWoodSound);
    }


    public void SwooshSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.swooshSound);
    }

    public void staminaWhenDoingAction()
    {
        PlayerState.Instance.currentStamina -= 10;
    }

}
