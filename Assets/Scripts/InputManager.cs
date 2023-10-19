using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //action maps
    private PlayerInput playerInput; // Define the PlayerInput variable
    private PlayerInput.OnFootActions onFoot;
    private PlayerInput.OnWeaponActions onWeapon;

    //components
    private PlayerMotor motor;
    private PlayerLook look;
    private WeaponPickup pickupWeapon;
    private WeaponInteraction  weaponInteraction;



    private bool isSprinting = false; // Add a flag to track sprinting state

    // Start is called before the first frame update
    void Awake()
    {

        playerInput = new PlayerInput();

        //action maps
        onFoot = playerInput.OnFoot;
        onWeapon = playerInput.OnWeapon;
        //components
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        pickupWeapon = GetComponent<WeaponPickup>();
        weaponInteraction = GetComponent<WeaponInteraction>();


        //methods
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.started += ctx => motor.StartSprinting();
        onFoot.Sprint.canceled += ctx => motor.StopSprinting();
        onWeapon.Pickup.performed += ctx => pickupWeapon.Pickup();
        onWeapon.Drop.performed += ctx => weaponInteraction.Drop();
        onWeapon.Shoot.performed += ctx => weaponInteraction.Shoot();

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
        onWeapon.Enable();

    }

    private void OnDisable()
    {
        onFoot.Disable();
        onWeapon.Disable();


    }
}
