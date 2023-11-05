using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponInteraction : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of the small sphere to be shot
    public Vector3 barrelEndOffset; // Offset from the weapon's origin to the barrel end
    private Transform barrelEnd; // The position of the barrel's end from where the bullets will be shot
    private WeaponPickup weaponPickup; // Reference to the WeaponPickup component
    private Camera mainCamera;
    private float originalFOV;
    public float zoomedFOV = 30f; // Adjust the value to change the zoom level

    // HUD variables
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI bulletCountText;
    public string weaponName = "Sniper Rifle";
    public int maxBulletCount = 30;
    private int currentBulletCount;

    private void Start()
    {
         // Initialize bullet count
        currentBulletCount = maxBulletCount;

        // Set the weapon name text in the HUD
        if (weaponNameText != null)
        {
            weaponNameText.text = weaponName;
            weaponNameText.gameObject.SetActive(false);
        }

        // Set the bullet count text in the HUD
        if (bulletCountText != null)
        {
            bulletCountText.text = "Bullets: " + currentBulletCount;
            bulletCountText.gameObject.SetActive(false);
        }

        // Get the reference to the WeaponPickup component
        weaponPickup = GetComponent<WeaponPickup>();
        if (weaponPickup == null)
        {
            Debug.LogError("WeaponPickup component not found.");
        }

        // Dynamically set the barrelEnd position using an offset from the weapon's origin
        if (weaponPickup.weapon != null)
        {
            barrelEnd = weaponPickup.weapon.transform;
            if (barrelEnd != null)
            {
                barrelEnd.position += weaponPickup.weapon.transform.TransformDirection(
                    barrelEndOffset
                );
            }
            else
            {
                Debug.LogError("Barrel end not found on the weapon.");
            }
        }

        // Get the main camera component
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
        }
        originalFOV = mainCamera.fieldOfView; // Store the original FOV
    }

    // Method to drop the weapon
    public void Drop()
    {
                    Debug.Log("Weapon dropped."); // Check if this message appears in the console

        if (weaponPickup != null && weaponPickup.weapon != null)
        {
            weaponPickup.weapon.transform.SetParent(null); // Set the weapon's parent to null to drop it
            weaponPickup.hasPickedUp = false; // Set hasPickedUp to false to allow picking up the weapon again
            // Reset the FOV when dropping the weapon
            if (mainCamera != null)
            {
                mainCamera.fieldOfView = originalFOV;
            }

             // Disable the HUD elements when dropping the weapon
            if (weaponNameText != null)
            {
                weaponNameText.gameObject.SetActive(false);
            }
            if (bulletCountText != null)
            {
                bulletCountText.gameObject.SetActive(false);
            }
        }
    }

    // Method to shoot the small sphere from the gun barrel
    public void Shoot()
    {
        if (weaponPickup != null && weaponPickup.hasPickedUp && currentBulletCount > 0)
        {
            if (bulletPrefab != null && barrelEnd != null)
            {
                GameObject bullet = Instantiate(
                    bulletPrefab,
                    barrelEnd.position,
                    barrelEnd.rotation
                );
                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
                if (bulletRigidbody != null)
                {
                    bulletRigidbody.AddForce(barrelEnd.forward * 20f, ForceMode.Impulse); // Adjust the force as needed
                }

                // Adding the script for destroying the bullet on collision
                BulletCollision bulletCollision = bullet.GetComponent<BulletCollision>();
                if (bulletCollision == null)
                {
                    bulletCollision = bullet.AddComponent<BulletCollision>();
                }

                // Decrease bullet count and update HUD
                currentBulletCount--;
                if (bulletCountText != null)
                {
                    bulletCountText.text = "Bullets: " + currentBulletCount;
                }
                if (currentBulletCount == 0)
                {
                    // Disable shooting if there are no bullets left
                    Debug.Log("Out of bullets!");
                }
            }
        }
    }

    // Method to toggle ADS
    public void ToggleADS(bool isAiming)
    {
        if (mainCamera != null)
        {
            if (isAiming)
            {
                mainCamera.fieldOfView = zoomedFOV; // Set the zoomed-in FOV
            }
            else
            {
                mainCamera.fieldOfView = originalFOV; // Reset the FOV to the original value
            }
        }
    }

    // Method to handle ADS input action
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleADS(true);
        }
        else if (context.canceled)
        {
            ToggleADS(false);
        }
    }

    // Method to handle weapon pickup
    public void OnPickup()
    {
            Debug.Log("Weapon picked up."); // Check if this message appears in the console

        // Enable the HUD elements when picking up the weapon
        if (weaponNameText != null)
        {
            weaponNameText.gameObject.SetActive(true);
            SetWeaponNameTextPosition();
        }
        if (bulletCountText != null && currentBulletCount > 0) // Only display if there are bullets left
        {
            bulletCountText.gameObject.SetActive(true);
            SetBulletCountTextPosition();
            bulletCountText.text = "Bullets: " + currentBulletCount; // Update bullet count text when picking up the weapon
        }
    }

    // Method to set the position of the bullet count text
    private void SetBulletCountTextPosition()
    {
        if (bulletCountText != null)
        {
            // Set the anchor position to the bottom right corner
            bulletCountText.rectTransform.anchorMin = new Vector2(1, 0);
            bulletCountText.rectTransform.anchorMax = new Vector2(1, 0);
            bulletCountText.rectTransform.pivot = new Vector2(1, 0);
            bulletCountText.rectTransform.anchoredPosition = new Vector2(-10, 10); // Adjust the position as needed
        }
    }

    // Method to set the position of the weapon name text
    private void SetWeaponNameTextPosition()
    {
        if (weaponNameText != null)
        {
            // Set the anchor position to the bottom left corner
            weaponNameText.rectTransform.anchorMin = new Vector2(0, 0);
            weaponNameText.rectTransform.anchorMax = new Vector2(0, 0);
            weaponNameText.rectTransform.pivot = new Vector2(0, 0);
            weaponNameText.rectTransform.anchoredPosition = new Vector2(10, 10); // Adjust the position as needed
        }
    }
}

public class BulletCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject); // Destroy the bullet on collision
    }
}
