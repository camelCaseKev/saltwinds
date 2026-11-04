using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : Character
{
    float hitPoints;
    public GameObject itemToDrop1;
    public GameObject itemToDrop2;
    public GameObject itemToDrop3;
    public int lootMultiplyer = 2;

    private void OnEnable()
    {
        ResetCharacter();
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            StartCoroutine(FlickerCharacter());
            hitPoints -= damage;

            audioManager.PlaySFX(audioManager.cannonHit); // CHECK

            if (hitPoints <= float.Epsilon)
            {
                KillCharacter();
                break;
            }

            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    public override void KillCharacter()
    {
        Vector3 dropPosition = transform.position;

        for(int i = 0; i < lootMultiplyer; i++)
        {
            if (itemToDrop1 != null && Random.value < 0.5f) // 50% chance
            {
                    Instantiate(itemToDrop1, dropPosition + new Vector3(0.5f, 0.5f), Quaternion.identity);
            }

            if (itemToDrop2 != null && Random.value < 0.3f) // 30% chance
            {
                Instantiate(itemToDrop2, dropPosition + new Vector3(-0.5f, -0.5f), Quaternion.identity);
            }

            if (itemToDrop3 != null && Random.value < 0.1f) // 10% chance
            {
                Instantiate(itemToDrop3, dropPosition, Quaternion.identity);
            }
        }
        

        Destroy(gameObject);
    }

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }
}