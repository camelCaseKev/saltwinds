using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RPGGameManager : MonoBehaviour
{
    public SpawnPoint playerSpawnPoint;
    public RPGCameraManager cameraManager;
    public static RPGGameManager sharedInstance = null;
    GameObject map; // CHECK

    void Awake()
    {
        if(sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }

        map = GameObject.FindGameObjectWithTag("Map"); // CHECK
        map.SetActive(false);
    }

    void Start()
    {
        SetupScene();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            if(map.activeInHierarchy)
            {
                map.SetActive(false);
            }
            else
            {
                map.SetActive(true);
            }
        }
    }

    public void SetupScene()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if(playerSpawnPoint != null)
        {
            GameObject player = playerSpawnPoint.SpawnObject();
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }
}
