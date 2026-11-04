using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCannons : Weapon
{
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            FireAmmo();
        }
    }

    void FireAmmo()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject ammo = SpawnAmmo(transform.position);

        if(ammo != null)
        {
            Arc arcScript = ammo.GetComponent<Arc>();
            float travelDuration = 1.0f / weaponVelocity;

            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
            audioManager.PlaySFX(audioManager.cannonfire); // CHECK
        }
    }
}
