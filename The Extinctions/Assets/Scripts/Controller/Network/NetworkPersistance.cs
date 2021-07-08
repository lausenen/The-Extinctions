using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkPersistance : MonoBehaviour
{
    
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Networking");

        foreach (GameObject obj in objs)
        {
            DontDestroyOnLoad(obj);
        }
    }
}
