using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAmmo : Ammo
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision is BoxCollider2D && collision.gameObject.CompareTag("Player")) // May have to change BoxCollider2D to another kind 
        {
            Player player = collision.gameObject.GetComponent<Player>();

            StartCoroutine(player.DamageCharacter(damageInflicted, 0.0f));
            gameObject.SetActive(false);
        }
    }
}
