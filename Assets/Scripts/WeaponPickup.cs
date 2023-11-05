using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponPickup : MonoBehaviour
{
    public GameObject weapon;
    public float pickupRange = 3f;
    public Transform player;
    public TextMeshProUGUI pickupText;

    public bool isInRange = false;
    public bool hasPickedUp = false;

    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI bulletCountText;
    public string weaponName = "Sniper Rifle";
    public int maxBulletCount = 30;
    private int currentBulletCount;

    private void Start()
    {
        // Initialize bullet count
        currentBulletCount = maxBulletCount;
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
            ShowHUD();
        }
    }

    public void Pickup()
    {
        if (!hasPickedUp && isInRange)
        {
            weapon.transform.SetParent(player);
            weapon.transform.localPosition = new Vector3(0.5f, 0.5f, 1f);
            weapon.transform.localRotation = Quaternion.identity;
            hasPickedUp = true;
            pickupText.text = ""; // Clear the pickup text after picking up the weapon
        }
    }

    private void ShowHUD()
    {
        if (weaponNameText != null)
        {
            weaponNameText.gameObject.SetActive(true);
            weaponNameText.text = weaponName;
            // Position the weaponNameText on the bottom left of the screen
            weaponNameText.rectTransform.anchorMin = new Vector2(0, 0);
            weaponNameText.rectTransform.anchorMax = new Vector2(0, 0);
            weaponNameText.rectTransform.pivot = new Vector2(0, 0);
            weaponNameText.rectTransform.anchoredPosition = new Vector2(10, 10); // Adjust the position as needed
        }

        if (bulletCountText != null && currentBulletCount > 0)
        {
            bulletCountText.gameObject.SetActive(true);
            bulletCountText.text = "Bullets: " + currentBulletCount; // Update the bullet count text
            // Position the bulletCountText on the bottom right of the screen
            bulletCountText.rectTransform.anchorMin = new Vector2(1, 0);
            bulletCountText.rectTransform.anchorMax = new Vector2(1, 0);
            bulletCountText.rectTransform.pivot = new Vector2(1, 0);
            bulletCountText.rectTransform.anchoredPosition = new Vector2(-10, 10); // Adjust the position as needed
        }
    }
}
