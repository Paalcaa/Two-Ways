using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    

    [Header("Movement")]
    private float moveInput;
    [SerializeField] private float moveSpeed = 7.5f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpTime = 0.5f;

    [Header("Turn Check")]
    [SerializeField] private GameObject Right;
    [SerializeField] private GameObject Left;
    [HideInInspector] public bool isFacingRight;

    [Header("Camera Stuff")]
    [SerializeField] private GameObject _cameraFollowGO;
    private CameraFollowObject _cameraFollowObject;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isJumping;
    private bool isFalling;
    private float jumpTimeCounter;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        StartDirectionCheck();

        _cameraFollowObject = _cameraFollowGO.GetComponent<CameraFollowObject>();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    #region Movement Functions

    private void Move()
    {
       moveInput = UserInput.instance.moveInput.x;

        if(moveInput >0 || moveInput <0)
        {
            anim.SetBool("isWalking", true);
            TurnCheck();
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

       rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if(UserInput.instance.controls.Jumping.Jump.WasPressedThisFrame())
        {
           isJumping = true;
        }

        if (UserInput.instance.controls.Jumping.Jump.IsPressed())
        {

        }

        if (UserInput.instance.controls.Jumping.Jump.WasReleasedThisFrame())
        {
            isJumping = false;
        }
    }

    #endregion

    #region Turn Checks

    private void StartDirectionCheck()
    {
        if (Right.transform.position.x > Left.transform.position.x)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
    }   

    private void TurnCheck()
    {
        if(UserInput.instance.moveInput.x > 0 && !isFacingRight)
        {
            Turn();
        }
        else if(UserInput.instance.moveInput.x < 0 && isFacingRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            // turn the camera to follow object
            _cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            // turn the camera to follow object
            _cameraFollowObject.CallTurn();
        }
    }
    #endregion


}
