using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannons : Weapon
{
    public float fireInterval;
    [HideInInspector] public Transform target;

    public IEnumerator FireAmmo()
    {
        while(true)
        {
            GameObject ammo = SpawnAmmo(transform.position);

            if(ammo != null)
            {
                Arc arcScript = ammo.GetComponent<Arc>();
                float travelDuration = 1.0f / weaponVelocity;

                StartCoroutine(arcScript.TravelArc(target.position, travelDuration));
                audioManager.PlaySFX(audioManager.cannonfire); // CHECK
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
