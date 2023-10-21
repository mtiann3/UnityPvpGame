using UnityEngine;

public class WeaponInteraction : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab of the small sphere to be shot
    public Vector3 barrelEndOffset; // Offset from the weapon's origin to the barrel end
    private Transform barrelEnd; // The position of the barrel's end from where the bullets will be shot
    private WeaponPickup weaponPickup; // Reference to the WeaponPickup component

    private void Start()
    {
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
                barrelEnd.position += weaponPickup.weapon.transform.TransformDirection(barrelEndOffset);
            }
            else
            {
                Debug.LogError("Barrel end not found on the weapon.");
            }
        }
    }

    // Method to drop the weapon
    public void Drop()
    {
        if (weaponPickup != null && weaponPickup.weapon != null)
        {
            weaponPickup.weapon.transform.SetParent(null); // Set the weapon's parent to null to drop it
            weaponPickup.hasPickedUp = false; // Set hasPickedUp to false to allow picking up the weapon again
        }
    }

    // Method to shoot the small sphere from the gun barrel
    public void Shoot()
    {
        if (weaponPickup != null && weaponPickup.hasPickedUp)
        {
            if (bulletPrefab != null && barrelEnd != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, barrelEnd.position, barrelEnd.rotation);
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
            }
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
