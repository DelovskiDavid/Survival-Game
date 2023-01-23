using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FirstPersonController : MonoBehaviour
{
    public static FirstPersonController Instance;

    public CharacterController controller;
    public Animator animator;
    

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float walkSpeed;
    public float sprintSpeed;
    public float currentSpeed;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float idleThreshold = 0.1f;
    Vector3 velocity;
    bool isGrounded;
    public bool isSprinting;
    public bool isSwinging = false;
    public bool canSwing = true;
    public bool isMoving;

    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

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
        currentSpeed = walkSpeed;
        
    }
    private void Update()
    {
        Movement();
        Sprinting();
        Jumping();
    }
    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * x + transform.forward * z;
        controller.Move(direction * currentSpeed * Time.deltaTime);

        if (lastPosition != gameObject.transform.position)
        {
            isMoving = true;
            SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
        }
        else
        {
            isMoving = false;
            SoundManager.Instance.grassWalkSound.Stop();
        }
        lastPosition = gameObject.transform.position;
    }

    private void Jumping()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void Sprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            currentSpeed = walkSpeed;
            isSprinting = false;
        }
    }

    //private void HandleAnims()
    //{
    //    animator.SetFloat("vInput", Input.GetAxisRaw("Vertical"));
    //    animator.SetFloat("hzInput", Input.GetAxisRaw("Horizontal"));
    //    if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > idleThreshold || Mathf.Abs(Input.GetAxisRaw("Vertical")) > idleThreshold)
    //    {
    //        animator.SetBool("Walking", true);
    //    }
    //    else
    //    {
    //        animator.SetBool("Walking", false);
    //    }
    //}

   
}
