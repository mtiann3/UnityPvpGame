using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weapon;
    public float pickupRange = 3f;
    public Transform player;
    public TextMeshProUGUI pickupText;

    public bool isInRange = false;
    public bool hasPickedUp = false;

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
        weapon.transform.SetParent(transform);
        weapon.transform.localPosition = new Vector3(0.5f, 0.5f, 1f);
        weapon.transform.localRotation = Quaternion.identity;
        // Add any additional logic you need for weapon pickup

        // Set the hasPickedUp boolean to true after picking up the weapon
        hasPickedUp = true;
    }
}
