using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;
    private bool isSprinting = false; // Add a flag to track sprinting state

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => motor.Jump();

        onFoot.Crouch.performed += ctx => motor.Crouch();
        // Handle Sprint based on "hold to sprint" logic
        onFoot.Sprint.started += ctx => StartSprinting();
        onFoot.Sprint.canceled += ctx => StopSprinting();
    }
    private void StartSprinting()
    {
        isSprinting = true;
        motor.Sprint();
    }

    private void StopSprinting()
    {
        isSprinting = false;
        motor.StopSprinting();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //tell the player to move using the value from our movement action.
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());

    }
    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());

    }
    private void OnEnable()
    {
        onFoot.Enable();

    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
