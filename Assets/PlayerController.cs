using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _Camera;


    void Start()
    {
        Controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerMovemement();
        PlayerRotation();
    }

    #region Movement

    [Header("MOVEMENT")]
    [SerializeField] private float MoveSmoothTime;
    [SerializeField] private float GravityStrength;
    [SerializeField] private float JumpStrength;
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;
    [SerializeField] public bool isGrounded;
    private CharacterController Controller;
    [HideInInspector] public Vector3 CurrentForceVelocity;
    [HideInInspector] public Vector3 CurrentMoveVelocity;
    [HideInInspector] public Vector3 MoveDampVelocity;

    bool isJumping  = false;
    float jumpTimer = 10f;
    private void PlayerMovemement()
    {
        Vector3 PlayerInput = new Vector3
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0f,
            z = Input.GetAxisRaw("Vertical")
        };

        if (PlayerInput.magnitude > 1f)
        {
            PlayerInput.Normalize();
        }

        Vector3 MoveVector = transform.TransformDirection(PlayerInput);
        float CurrentSpeed = Input.GetKey(KeyCode.LeftControl) ? RunSpeed : WalkSpeed;

        CurrentMoveVelocity = Vector3.SmoothDamp(
            CurrentMoveVelocity,
            MoveVector * CurrentSpeed,
            ref MoveDampVelocity,
            MoveSmoothTime
        );

        Controller.Move(CurrentMoveVelocity * Time.deltaTime);

        isGrounded = false;
        Vector3 xOffset = new Vector3(0.39f, 0f, 0f);
        Vector3 zOffset = new Vector3(0f, 0f, 0.39f);

        Ray[] groundCheckRay = new Ray[9];
        groundCheckRay[0] = new Ray(transform.position, Vector3.down);
        groundCheckRay[1] = new Ray(transform.position - xOffset, Vector3.down);
        groundCheckRay[2] = new Ray(transform.position + xOffset, Vector3.down);
        groundCheckRay[3] = new Ray(transform.position - zOffset, Vector3.down);
        groundCheckRay[4] = new Ray(transform.position + zOffset, Vector3.down);
        groundCheckRay[5] = new Ray(transform.position - xOffset - zOffset, Vector3.down);
        groundCheckRay[6] = new Ray(transform.position + xOffset + zOffset, Vector3.down);
        groundCheckRay[7] = new Ray(transform.position - zOffset - xOffset, Vector3.down);
        groundCheckRay[8] = new Ray(transform.position + zOffset + xOffset, Vector3.down);

        for (int i = 0; i < groundCheckRay.Length; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(groundCheckRay[i], 1.1f))
            {
                isGrounded = true;
            }
        }

        if (isGrounded)
        {
            CurrentForceVelocity.y = -2f;

            if (Input.GetKey(KeyCode.Space))
            { 
                CurrentForceVelocity.y = JumpStrength;
            }
        }
        else
        {
            CurrentForceVelocity.y -= GravityStrength * Time.deltaTime;
        }

    
        //if (!Input.GetKey(KeyCode.Space))
        //{
        //    isJumping = false;
        //    jumpTimer = 10f;
        //}
        //else if (isJumping && jumpTimer >= 0)
        //{
        //    //CurrentForceVeloicty.y = 0f;
        //    CurrentForceVelocity.y += JumpStrength/2f;
        //    jumpTimer--;
        //}
        //Debug.Log(jumpTimer);
        
        Controller.Move(CurrentForceVelocity * Time.deltaTime);
    }

    #endregion

    #region Rotation

    [Header("ROTATION")]
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] Vector2 Sensitivity;

    private Vector2 Rotation;

    private void PlayerRotation()
    {
        Vector2 MouseInput = new Vector2
        {
            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        Rotation.x -= MouseInput.y * Sensitivity.y;
        Rotation.y += MouseInput.x * Sensitivity.x;

        Rotation.x = Mathf.Clamp(Rotation.x, -90f, 90f);

        transform.eulerAngles = new Vector3(0f, Rotation.y, 0f);
        PlayerCamera.localEulerAngles = new Vector3(Rotation.x, 0f, 0f);
    }

    private void OnDrawGizmos()
    {
        //Vector3 xOffset = new Vector3(0.5f, 0f, 0f);
        //Vector3 zOffset = new Vector3(0f, 0f, 0.5f);

        //Gizmos.color = Color.red;

        //Gizmos.DrawLine(transform.position + xOffset, Vector3.down);
        //Gizmos.DrawLine(transform.position - xOffset, Vector3.down);
        //Gizmos.DrawLine(transform.position + zOffset, Vector3.down);
        //Gizmos.DrawLine(transform.position - zOffset, Vector3.down);

        //Gizmos.DrawLine(transform.position - xOffset - zOffset, Vector3.down);
        //Gizmos.DrawLine(transform.position + xOffset + zOffset, Vector3.down);
        //Gizmos.DrawLine(transform.position - zOffset - xOffset, Vector3.down);
        //Gizmos.DrawLine(transform.position + zOffset + xOffset, Vector3.down);
    }


    #endregion
}
