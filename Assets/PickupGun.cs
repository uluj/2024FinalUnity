using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PickupGun : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a relevant tag
        if (collision.gameObject.CompareTag("Revolver") || collision.gameObject.CompareTag("Rifle") || collision.gameObject.CompareTag("Shotgun"))
        {
            // Pass the tag of the collided object to TaskGunPickup
            TaskGunPickup(collision.gameObject.tag);
        }
    }

    void TaskGunPickup(string gunTag)
    {
        // Step 1: Get the active gun
        (GameObject activeGun, var activeChildren) = GetActiveGun();

        // Step 2: Deactivate the currently active gun (if any)
        if (activeGun != null)
        {
            DeactivateActiveGun(activeGun.transform);
        }

        // Step 3: Throw the pickable gun (based on the active gun)
        if (activeGun != null)
        {
            GameObject weaponToThrow = WhichOneToThrow(activeGun);
            ThrowGun(weaponToThrow);
        }

        // Step 4: Activate the collided gun
        GameObject gunToActivate = FindGunByName(gunTag);
        if (gunToActivate != null)
        {
            ActivateGun(gunToActivate);
        }
        else
        {
            Debug.LogWarning("No gun found with the name: " + gunTag);
        }
    }

    GameObject FindGunByName(string gunName)
    {
        Transform cubeTransform = GameObject.Find("Cube")?.transform;

        if (cubeTransform == null)
        {
            Debug.LogWarning("Cube object not found.");
            return null;
        }

        // Find the child with the matching name
        Transform gunTransform = cubeTransform.Cast<Transform>()
            .FirstOrDefault(child => child.gameObject.name.Equals(gunName, StringComparison.OrdinalIgnoreCase));

        return gunTransform?.gameObject;
    }

    void ActivateGun(GameObject gun)
    {
        if (gun != null)
        {
            gun.SetActive(true);
            Debug.Log(gun.name + " activated.");
        }
        else
        {
            Debug.LogWarning("No gun to activate.");
        }
    }

    (GameObject, List<Transform>) GetActiveGun()
    {
        // Find all child GameObjects of Cube
        var allChildren = GameObject.Find("Cube").transform.Cast<Transform>().ToList();

        // Get the active children
        var activeChildren = allChildren.Where(child => child.gameObject.activeSelf).ToList();

        if (activeChildren.Count == 1)
        {
            return (activeChildren[0].gameObject, activeChildren);
        }

        if (activeChildren.Count > 1)
        {
            Debug.LogWarning("Multiple active guns found.");
        }

        return (null, activeChildren);
    }

    GameObject WhichOneToThrow(GameObject activeGun)
    {
        if (activeGun == null)
        {
            Debug.LogWarning("No active gun found to throw.");
            return null;
        }

        switch (activeGun.name)
        {
            case "Rifle": return Resources.Load<GameObject>("PickupGunRifle");
            case "Revolver": return Resources.Load<GameObject>("PickupGunRevolver");
            case "Shotgun": return Resources.Load<GameObject>("PickupGunShotgun");
            default:
                Debug.LogWarning("Unknown gun: " + activeGun.name);
                return null;
        }
    }

    void DeactivateActiveGun(Transform activeChild)
    {
        activeChild.gameObject.SetActive(false);
        Debug.Log(activeChild.gameObject.name + " deactivated.");
    }

    void ThrowGun(GameObject weaponToThrow)
    {
        if (weaponToThrow == null)
        {
            Debug.LogWarning("No weapon to throw.");
            return;
        }

        GameObject thrownWeapon = Instantiate(weaponToThrow, transform.position, Quaternion.identity);
        Rigidbody rb = thrownWeapon.GetComponent<Rigidbody>();
        Collider colliderComponent = thrownWeapon.GetComponent<Collider>();

        colliderComponent.enabled = false;
        rb.AddForce(new Vector3(-50f, 0f, 0f), ForceMode.Impulse);

        StartCoroutine(ReenableCollider(colliderComponent));
        Debug.Log("Thrown " + weaponToThrow.name);
    }

    System.Collections.IEnumerator ReenableCollider(Collider colliderComponent)
    {
        yield return new WaitForSeconds(1f);
        colliderComponent.enabled = true;
    }
}
