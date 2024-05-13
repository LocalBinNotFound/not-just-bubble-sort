using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

using FirebaseWebGL.Scripts.FirebaseBridge;


public class FirebaseDataManager : MonoBehaviour
{
    public static FirebaseDataManager Instance;

    public Action<int> OnTotalStarsRetrieved;
    public Action<int[]> OnDataRetrieved;
    private string currentUsername;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterOrLogin(string username)
    {
        int totalLevels = SceneManager.sceneCountInBuildSettings - 3;
        FirebaseDatabase.RegisterOrLogin(username, totalLevels);
    }

}