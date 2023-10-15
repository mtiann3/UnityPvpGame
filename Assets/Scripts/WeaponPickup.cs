using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weapon;
    public float pickupRange = 3f;
    public TextMeshProUGUI pickupText; // Assigned in the inspector
    private bool isPickedUp = false;

    public void OnPickup()
    {
        if (!isPickedUp)
        {
            PickUp();
        }
    }

    public void OnShoot()
    {
        if (isPickedUp)
        {
            Shoot();
        }
    }

    public void OnDrop()
    {
        if (isPickedUp && weapon != null)
        {
            DropWeapon();
        }
    }

    private void Update()
    {
        if (pickupText != null)
        {
            float distance = Vector3.Distance(transform.position, weapon.transform.position);

            if (!isPickedUp && distance < pickupRange)
            {
                pickupText.text = "Press F to Pick Up";
                if (Keyboard.current.fKey.wasPressedThisFrame)
                {
                    OnPickup();
                }
            }
            else
            {
                pickupText.text = "";
            }

            if (isPickedUp && Keyboard.current.tKey.wasPressedThisFrame)
            {
                OnDrop();
            }
        }
    }

    private void PickUp()
    {
        weapon.transform.SetParent(transform);
        weapon.transform.localPosition = new Vector3(0.5f, 0.5f, 1f);
        weapon.transform.localRotation = Quaternion.identity;
        isPickedUp = true;
        pickupText.text = "";
    }

    private void DropWeapon()
    {
        weapon.transform.SetParent(null);
        weapon.transform.position = transform.position + transform.forward; // Put the weapon in front of the player
        weapon.transform.rotation = Quaternion.Euler(0f, 90f, 0f); // Rotate the weapon sideways
        isPickedUp = false;
    }

    private void Shoot()
    {
        // Implement the shooting logic here, instantiate a bullet from the weapon's muzzle.
    }
}
