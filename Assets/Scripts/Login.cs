using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Net.Security;

public class Login : MonoBehaviour
{
    private bool isFirebaseInitialized = false;
    // Start is called before the first frame update
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                isFirebaseInitialized = true;
                LogUserIn("test1");
            }
            else
            {
                Debug.LogError(task.Result);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LogUserIn(string username)
    {
        User user = new(username);
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.username).SetRawJsonValueAsync(JsonUtility.ToJson(user));
        PlayerPrefs.SetString("username", user.username);
    }
}
