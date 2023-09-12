using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    InputControls controls;

    [SerializeField] float speed = 1;
    [SerializeField] float sprintSpeed = 2;
    [SerializeField] float characterRotationSmoothing = 0.03f;
    float playerHeight;

    [SerializeField] CharacterController charControl;
    Rigidbody rb;

    bool IsSprinting = false;

    Vector2 moveDirection;
    bool IsGrounded;

    public float gravity = -9.8f;

    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    bool canMove;

    [SerializeField] Animator anim;
    [SerializeField] GameObject playerMesh;

    private static readonly int Idle = Animator.StringToHash("Standing Idle");
    private static readonly int Walk = Animator.StringToHash("Walking");
    private static readonly int Run = Animator.StringToHash("Running");

    private void Awake()
    {
        canMove = true;
    }
    private void Start()
    {
        controls = gameObject.GetComponent<Player>().controls;
        controls.Player.Sprint.performed += sprinting => Sprinting();
        controls.Player.StopSprint.performed += sprintStop => StopSprinting();

        playerHeight = transform.position.y;
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        EventManager.OnInteractionStart.AddListener(DisableMovement);
        EventManager.OnInteractionEnd.AddListener(EnableMovement);
    }

    private void OnDisable()
    {
        EventManager.OnInteractionStart.RemoveListener(DisableMovement);
        EventManager.OnInteractionEnd.RemoveListener(EnableMovement);
    }

    void Update()
    {
        playerMesh.transform.position = transform.position;
        playerMesh.transform.rotation = transform.rotation;

        if (canMove)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);


            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2;
            }
            moveDirection = controls.Player.Move.ReadValue<Vector2>();

            charControl.Move(new Vector3(moveDirection.x, 0, moveDirection.y) * speed * Time.deltaTime);
            Rotation();


            velocity.y += gravity * Time.deltaTime;
            charControl.Move(velocity * speed);
        }

        if (moveDirection == new Vector2(0, 0))
        {
            anim.CrossFade(Idle, 0, 0);
        }
        else if (IsSprinting)
        {
            anim.CrossFade(Run, 0, 0);
        }
        else
        {
            anim.CrossFade(Walk, 0, 0);
        }

        //Ray ray = new Ray(waterCheck.transform.position, -transform.up);

        //if (Physics.Raycast(ray, out RaycastHit hit, 2))
        //{
        //    if (hit.collider.CompareTag(waterTag))
        //    {

        //    }
        //}
    }

    void Rotation()
    {
        float horizontalMovement = moveDirection.x;
        float verticalMovement = moveDirection.y;

        Vector3 rotationOfCharacter = new Vector3(horizontalMovement, 0.0f, verticalMovement);
        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationOfCharacter), characterRotationSmoothing);
        }
    }
    void Sprinting()
    {

        if (speed != speed + sprintSpeed)
        {
            speed += sprintSpeed;
            IsSprinting = !IsSprinting;
        }
    }
    void StopSprinting()
    {

        speed -= sprintSpeed;
        IsSprinting = !IsSprinting;

    }

    void EnableMovement()
    {
        canMove = true;
    }
 
    void DisableMovement()
    {
        canMove = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 3);

        Gizmos.DrawSphere(groundCheck.position, groundDistance);
    }


}
