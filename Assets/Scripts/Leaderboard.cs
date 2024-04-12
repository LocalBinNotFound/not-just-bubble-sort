using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class Leaderboard : MonoBehaviour
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
                if (PlayerPrefs.HasKey("username"))
                {
                    // listen for all events
                    FirebaseDatabase.DefaultInstance.GetReference("users").ValueChanged += HandleValueChanged;
                }
            }
            else
            {
                Debug.LogError(task.Result);
            }
        });
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        if (args.Snapshot != null && args.Snapshot.Value != null)
        {
            Debug.Log(args.Snapshot.Value);
            foreach (KeyValuePair<string, object> user in (Dictionary<string, object>) args.Snapshot.Value)
            {
                Debug.Log(user.Key);
                var u = (Dictionary<string, object>) user.Value;
                Debug.Log(u["username"]);
                Debug.Log(u["completeDuration"]);
            }
        }
        else
        {
            Debug.Log("null");
        }
    }
}
