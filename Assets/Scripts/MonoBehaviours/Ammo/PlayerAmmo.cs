using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAmmo : Ammo
{

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision is BoxCollider2D)
        {
            if(collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                StartCoroutine(enemy.DamageCharacter(damageInflicted, 0.0f));
                gameObject.SetActive(false);
            }
            else if(collision.gameObject.CompareTag("Merchant"))
            {
                Merchant merchant = collision.gameObject.GetComponent<Merchant>();

                StartCoroutine(merchant.DamageCharacter(damageInflicted, 0.0f));
                gameObject.SetActive(false);
            }
        }
    }
}
