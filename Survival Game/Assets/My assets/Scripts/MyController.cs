using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float speed = 6f;
    public float sprintSpeed = 6f;
    public float currentSpeed;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    Vector3 velocity;
    bool isGrounded;
    bool isSprinting;
    //bool isJumping;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    Animator animator;
    public float idleThreshold;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        Movement();
        Sprinting();
        Jumping();
        HandleAnims();
    }

    private void Movement()
    {
        //input za movement, levo desno napred nazad
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            //kalkuliranje na agolot spored inputot
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //za da ne snapnuva od eden agol vo drug(da se vrti smoothly)
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //rotacijata
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //kade sakame da se dvizimi spored toa kade e kamerata
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //davanje na sila za da se dvizi
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }

    }

    private void Jumping()
    {
        //pravime topka i proveruvame dali se collidame so nesto
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //ako sme na zemja ja resetirame velocityto
        if (isGrounded && velocity.y < 0)
        {
            //isGrounded moze da se registrira pred da sme skroz na zemja zatoa stavame -2 a ne 0
            velocity.y = -2f;
        }
        //ako stiskame space i sme grounded da ripneme
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //v = kvadraten koren od h * -2 * gravity
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        //davame gravity na variabla velocity
        velocity.y += gravity * Time.deltaTime;
        //go dodavame velocityto na igracot
        controller.Move(velocity * Time.deltaTime);
    }

    private void Sprinting()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (isSprinting == true)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = speed;
        }
    }

    private void HandleAnims()
    {
        animator.SetFloat("vInput", Input.GetAxisRaw("Vertical"));
        animator.SetFloat("hzInput", Input.GetAxisRaw("Horizontal"));
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > idleThreshold || Mathf.Abs(Input.GetAxisRaw("Vertical")) > idleThreshold)
        {
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Idle", true);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        if (Input.GetButton("Fire1"))
        {
            animator.SetTrigger("Attacking");
        }     
    }


}
