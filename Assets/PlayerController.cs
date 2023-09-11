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
        InitializeHeadBob();
    }
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PlayerMovemement();
        PlayerRotation();
        HeadBob();
    }

    #region Movement

    [Header("MOVEMENT")]
    [SerializeField] private float MoveSmoothTime;
    [SerializeField] private float GravityStrength;
    [SerializeField] private float JumpStrength;
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;
    private CharacterController Controller;
    private Vector3 CurrentForceVelocity;
    private Vector3 CurrentMoveVelocity;
    private Vector3 MoveDampVelocity;
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

    #region Head-Bobbing
    [Header("HEAD-BOBBING")]
    [SerializeField] private bool EnableHeadBobbing = true;
    [SerializeField, Range(0f, 0.1f)] private float HeadBobAmplitude = 0.015f;
    [SerializeField, Range(0f, 30f)] private float HeadBobFrequency = 10f;
    [SerializeField] private Camera HeadBobCamera;
    [SerializeField] private Transform HeadBobHolder;

    private float HeadBobToggleSpeed = 3.0f;
    private Vector3 HeadBobStartPos;
    private void InitializeHeadBob()
    {
        HeadBobStartPos = HeadBobCamera.transform.localPosition;
    }
    private void ResetHeadBobPosition()
    {
        if (HeadBobCamera.transform.localPosition == HeadBobStartPos) return;
        HeadBobCamera.transform.localPosition = Vector3.Lerp(HeadBobCamera.transform.localPosition, HeadBobStartPos, 1 * Time.deltaTime);
    }
    private void HeadBob()
    {
        if (!EnableHeadBobbing)
            return;

        /* EnableCheck */
        float speed = new Vector3(CurrentMoveVelocity.x, 0, CurrentMoveVelocity.z).magnitude;
        if (speed < HeadBobToggleSpeed | !Controller.isGrounded) 
            return;

        /* Head Up-Down-Left-Right Movement */
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * HeadBobFrequency) * HeadBobAmplitude * 2.5f;
        pos.x += Mathf.Cos(Time.time * HeadBobFrequency / 2) * HeadBobAmplitude * 2;
        HeadBobCamera.transform.localPosition += pos;

        /* Go back to normal Position */
        ResetHeadBobPosition();

        /* Look at target point */
        //vector3 targetpoint = new vector3(transform.position.x, transform.position.x + headbobholder.localposition.y, transform.position.z);
        //targetpoint += headbobholder.forward * 15f;
        //headbobcamera.transform.lookat(targetpoint);
    }



    #endregion
}
