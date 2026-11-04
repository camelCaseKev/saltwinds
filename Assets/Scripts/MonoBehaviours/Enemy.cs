using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    float hitPoints;
    public GameObject itemToDrop1;
    public GameObject itemToDrop2;
    public GameObject itemToDrop3;
    //public int damageStrength;
    //Coroutine damageCoroutine;

    private void OnEnable()
    {
        ResetCharacter();
    }

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            print("Collision detected");

            Player player = collision.gameObject.GetComponent<Player>();

            if(damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 1.0f));
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }*/

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while(true)
        {
            StartCoroutine(FlickerCharacter());
            hitPoints -= damage;

            audioManager.PlaySFX(audioManager.cannonHit); // CHECK

            if (hitPoints <= float.Epsilon)
            {
                KillCharacter();
                break;
            }

            if(interval > float.Epsilon)
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

        if(itemToDrop1 != null && Random.value < 0.5f) // 50% chance
        {
            Instantiate(itemToDrop1, dropPosition + new Vector3(0.5f, 0.5f), Quaternion.identity);
        }

        if(itemToDrop2 != null && Random.value < 0.3f) // 30% chance
        {
            Instantiate(itemToDrop2, dropPosition + new Vector3(-0.5f, -0.5f), Quaternion.identity);
        }

        if(itemToDrop3 != null && Random.value < 0.1f) // 10% chance
        {
            Instantiate(itemToDrop3, dropPosition, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }
}
