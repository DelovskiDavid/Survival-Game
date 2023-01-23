using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Movement : MonoBehaviour
{
    Animator animator;

    Vector3 stopPosition;

    public float movementSpeed = 0.2f;
    float walkTime;
    public float walkCounter;
    float waitTime;
    public float waitCounter;

    int walkDirection;

    public bool isWalking;
    public bool isInControl = true;

    private void Start()
    {
        animator = GetComponent<Animator>();

        walkTime = Random.Range(4, 8);
        waitTime = Random.Range(6, 9);

        walkCounter = walkTime;
        waitCounter = waitTime;
        ChooseDirection();
    }
    private void Update()
    {
        if (isInControl)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (isWalking)
        {
            animator.SetBool("isRunning", true);

            walkCounter -= Time.deltaTime;

            switch (walkDirection)
            {
                case 0:
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    transform.position += transform.forward * movementSpeed * Time.deltaTime;
                    break;
                case 1:
                    transform.localRotation = Quaternion.Euler(0, 45, 0);
                    transform.position += transform.forward * movementSpeed * Time.deltaTime;
                    break;
                case 2:
                    transform.localRotation = Quaternion.Euler(0, -45, 0);
                    transform.position += transform.forward * movementSpeed * Time.deltaTime;
                    break;
                case 3:
                    transform.localRotation = Quaternion.Euler(0, 90, 0);
                    transform.position += transform.forward * movementSpeed * Time.deltaTime;
                    break;
                case 4:
                    transform.localRotation = Quaternion.Euler(0, -90, 0);
                    transform.position += transform.forward * movementSpeed * Time.deltaTime;
                    break;
                case 5:
                    transform.localRotation = Quaternion.Euler(0, 135, 0);
                    transform.position += transform.forward * movementSpeed * Time.deltaTime;
                    break;
                case 6:
                    transform.localRotation = Quaternion.Euler(0, -135, 0);
                    transform.position += transform.forward * movementSpeed * Time.deltaTime;
                    break;
                case 7:
                    transform.localRotation = Quaternion.Euler(0, 180, 0);
                    transform.position += transform.forward * movementSpeed * Time.deltaTime;
                    break;
            }
            if (walkCounter <= 0)
            {
                stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                isWalking = false;
                this.transform.position = stopPosition;
                animator.SetBool("isRunning", false);
                waitCounter = waitTime;
            }

        }
        else
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }

    public void ChooseDirection()
    {
        walkDirection = Random.Range(0, 8);

        isWalking = true;
        walkCounter = walkTime;
    }
}
