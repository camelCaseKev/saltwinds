using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject ammoPrefab;
    List<GameObject> ammoPool;
    //static List<GameObject> ammoPool;
    public int poolSize;
    public float weaponVelocity;

    [HideInInspector] public AudioManager audioManager; // CHECK


    void Awake() // Called once, when the script is being loaded
    {
        if(ammoPool == null)
        {
            ammoPool = new List<GameObject>();
        }

        for(int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.GetComponent<Ammo>().character = GetComponent<Character>();
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>(); // CHECK
    }

    public GameObject SpawnAmmo(Vector3 location)
    {
        foreach(GameObject ammo in ammoPool)
        {
            if(ammo.activeSelf == false)
            {
                ammo.SetActive(true);
                ammo.transform.position = location;
                return ammo;
            }
        }
        return null;
    }

    void OnDestroy()
    {
        ammoPool = null;
    }
}

/*public class Weapon : MonoBehaviour
{
    public GameObject ammoPrefab;
    List<GameObject> ammoPool;
    //static List<GameObject> ammoPool;
    public int poolSize;
    public float weaponVelocity;

    void Awake() // Called once, when the script is being loaded
    {
        if (ammoPool == null)
        {
            ammoPool = new List<GameObject>();
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.GetComponent<Ammo>().character = GetComponent<Character>(); // CHECK
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireAmmo();
        }
    }

    public GameObject SpawnAmmo(Vector3 location)
    {
        foreach (GameObject ammo in ammoPool)
        {
            if (ammo.activeSelf == false)
            {
                ammo.SetActive(true);
                ammo.transform.position = location;
                return ammo;
            }
        }
        return null;
    }

    void FireAmmo()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject ammo = SpawnAmmo(transform.position);

        if (ammo != null)
        {
            Arc arcScript = ammo.GetComponent<Arc>();
            float travelDuration = 1.0f / weaponVelocity;

            StartCoroutine(arcScript.TravelArc(mousePosition, travelDuration));
        }
    }

    void OnDestroy()
    {
        ammoPool = null;
    }
}*/
