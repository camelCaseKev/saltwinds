using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ammo : MonoBehaviour
{
    public int damageInflicted;
    [HideInInspector]public Character character;

    public abstract void OnTriggerEnter2D(Collider2D collision);

    private void Update()
    {
        if(character == null) // CHECK
        {
            gameObject.SetActive(false);
        }
    }
}
