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

        Ray groundCheckRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundCheckRay, 1.1f))
        {
            isGrounded = true;
            CurrentForceVelocity.y = -2f;

            if (Input.GetKey(KeyCode.Space))
            {
                CurrentForceVelocity.y = JumpStrength;
            }
        }
        else
        {
            isGrounded = false;
            CurrentForceVelocity.y -= GravityStrength * Time.deltaTime;
        }

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

    #endregion
}
