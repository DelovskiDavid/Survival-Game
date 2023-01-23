using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableAnimal : MonoBehaviour
{
    public GameObject meatOnKill;
    public bool playerInRange;
    public bool canBeAttacked;
    public string animalName;

    public float animalMaxHealth, animalHealth;
    private void Start()
    {
        animalHealth = animalMaxHealth;
    }

    private void Update()
    {
        AnimalHealthUpdate();
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

    public void GetHit()
    {
        animalHealth -= 2;

        if (animalHealth <= 0)
        {
            KillAnimal();
        }
    }
    private void KillAnimal()
    {
        Vector3 animalPosition = this.transform.position;

        Destroy(gameObject);
        canBeAttacked = false;
        SelectionManager.Instance.selectedAnimal = null;
        SelectionManager.Instance.animalHealthHolder.SetActive(false);

        GameObject animalMeat = Instantiate(meatOnKill, new Vector3(animalPosition.x, animalPosition.y + 0.5f, animalPosition.z), Quaternion.Euler(0, 0, 0));
    }

    private void AnimalHealthUpdate()
    {
        if (canBeAttacked)
        {
            GlobalState.Instance.resourceHealth = animalHealth;
            GlobalState.Instance.resourceMaxHealth = animalMaxHealth;
        }
    }
}
