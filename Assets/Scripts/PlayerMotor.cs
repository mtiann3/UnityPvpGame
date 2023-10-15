using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed;
    public float gravity = -9.8f;
    private float sprintSpeed = 6.75f; // Speed while sprinting
    private float regularSpeed = 5.0f; // Regular speed
    public float jumpHeight = 2.0f; // Adjusted jump height
    public bool lerpCrouch;
    public bool crouching;
    public bool sprinting;
    public float crouchTimer;
    private Camera mainCamera; // Reference to the main camera
    private float sprintFOV = 70f; // FOV when sprinting
    private float regularFOV = 60f; // Default FOV
    private float fovChangeSpeed = 5f; // Speed at which FOV changes
    private Coroutine fovCoroutine; // Coroutine for changing FOV smoothly
    public float crouchHeight = 1f; // Adjusted crouch height
public float crouchSpeed = 2.0f; // Speed while crouching
    private float originalSpeed; // Original speed before crouching
    // Start is called before the first frame update
    void Start()
    {
        // Get the reference to the main camera
        mainCamera = Camera.main;
        // Set the default FOV
        mainCamera.fieldOfView = regularFOV;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 0.5f; // Adjusted time for smoother transition
            p = Mathf.Sin(p * Mathf.PI * 0.5f); // Smooth out the transition curve

            if (crouching)
                controller.height = Mathf.Lerp(controller.height, crouchHeight, p);
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);

            if (p >= 0.99f) // Adjusted threshold for smoother transition completion
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    //receive the inputs for our InputManager.cs and apply them to our character controller.
     public void ProcessMove(Vector2 input)
    {
        float currentSpeed = crouching ? crouchSpeed : speed; // Adjust speed based on crouching state
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;

        // Prevent jumping while crouched
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        else if (isGrounded && crouching)
        {
            crouching = false; // Uncrouch when attempting to jump
            lerpCrouch = true;
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded && !crouching) // Only allow jumping if not crouching
        {
            playerVelocity.y = Mathf.Sqrt(2 * jumpHeight * -gravity); // Adjusted jump height formula
        }
        else if (crouching) // Uncrouch if trying to jump while crouched
        {
            crouching = false;
            lerpCrouch = true;
        }
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        sprinting = true;
        speed = sprintSpeed;
        if (fovCoroutine != null)
        {
            StopCoroutine(fovCoroutine);
        }
        fovCoroutine = StartCoroutine(LerpFOV(mainCamera.fieldOfView, sprintFOV));
    }

    public void StopSprinting()
    {
        sprinting = false;
        speed = regularSpeed;
        // Smoothly change the FOV back when not sprinting
        if (fovCoroutine != null)
        {
            StopCoroutine(fovCoroutine);
        }
        fovCoroutine = StartCoroutine(LerpFOV(mainCamera.fieldOfView, regularFOV));
    }
    
    private IEnumerator LerpFOV(float startFOV, float targetFOV)
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * fovChangeSpeed;
            mainCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime);
            yield return null;
        }
    }
    public void Slide()
{
    if (sprinting && isGrounded) // Only allow sliding while sprinting and grounded
    {
        Vector3 slideDirection = transform.forward;
        slideDirection.y = 0f; // Ensure sliding is parallel to the ground
        controller.Move(slideDirection * speed * 100f * Time.deltaTime); // Adjust the sliding distance as needed

        // You may need to handle the transition from sliding to crouching here
        crouching = true;
        lerpCrouch = true;
    }
    else if (!sprinting) // If not sprinting, just crouch
    {
        Crouch();
    }
}

//... (rest of the code)






}
