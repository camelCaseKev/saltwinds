using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;
    public HealthBar healthBarPrefab;
    HealthBar healthBar;
    public Inventory inventoryPrefab;
    Inventory inventory;


    private void Start()
    {
        ResetCharacter();

        //hitPoints.value = startingHitPoints; // CONSIDER REINSTATING

        /*inventory = Instantiate(inventoryPrefab);
        hitPoints.value = startingHitPoints;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            if(hitObject != null)
            {
                bool shouldDisappear = false;
                
                switch(hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        shouldDisappear = inventory.AddItem(hitObject);
                        break;
                    case Item.ItemType.CHEST:
                        shouldDisappear = inventory.AddItem(hitObject);
                        break;
                    case Item.ItemType.HEALTH:
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;
                    default:
                        break;
                }

                if(shouldDisappear)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    public bool AdjustHitPoints(int amount)
    {
        if(hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            print("Adjusted HP by: " + amount + ". New value: " + hitPoints.value);

            return true;
        }
        return false;
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while(true)
        {
            StartCoroutine(FlickerCharacter());
            hitPoints.value -= damage;

            audioManager.PlaySFX(audioManager.cannonHit); // CHECK

            if (hitPoints.value <= float.Epsilon)
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
        //base.KillCharacter();
        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);

        ResetCharacter(); // CHECK
        transform.position = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position; // CHECK
    }

    public override void ResetCharacter()
    {
        inventory = Instantiate(inventoryPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;

        hitPoints.value = startingHitPoints;
    }
}
