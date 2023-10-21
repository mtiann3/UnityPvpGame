using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weapon;
    public float pickupRange = 3f;
    public Transform player;
    public TextMeshProUGUI pickupText;
    private Camera mainCamera;

    public bool isInRange = false;
    public bool hasPickedUp = false;

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
        }
    }

    private void Update()
    {
        if (!hasPickedUp)
        {
            float distance = Vector3.Distance(player.position, weapon.transform.position);
            if (distance <= pickupRange)
            {
                isInRange = true;
                pickupText.text = "Press F to pickup";
            }
            else
            {
                isInRange = false;
                pickupText.text = "";
            }

            // Check for the F key press to initiate the pickup
            if (isInRange && Keyboard.current.fKey.wasPressedThisFrame)
            {
                Pickup();
            }
        }
        else
        {
            pickupText.text = ""; // Clear the pickup text if the weapon has been picked up
        }
    }

    public void Pickup()
    {
        if (!hasPickedUp)
        {
            weapon.transform.SetParent(mainCamera.transform);
            weapon.transform.localPosition = new Vector3(0.35f, -0.2f, 1f); // Adjust the local position values
            weapon.transform.localRotation = Quaternion.identity;
            hasPickedUp = true;
        }
    }
}
