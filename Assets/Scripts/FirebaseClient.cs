using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseClient
{
    private IFirebaseListener listener;
    public FirebaseClient(IFirebaseListener listener)
    {
        this.listener = listener;
    }

    public void RegisterOrLogin(User user)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.username).Child("username").SetValueAsync(user.username);
    }

    public void UpdateCompleteDuration(User user)
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.username).Child("completeDuration").SetValueAsync(user.completeDuration);
    }

    public void RetrieveLeaderboard()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users").OrderByChild("completeDuration").StartAt(0)
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                    Debug.LogError(task.Result);
                }
                else if (task.IsCompleted) {
                    List<User> users = new();
                    DataSnapshot snapshot = task.Result;
                    if (snapshot != null && snapshot.Value != null)
                    {
                        Debug.Log(((Dictionary<string, object>) snapshot.Value).Count);
                        foreach (KeyValuePair<string, object> user in (Dictionary<string, object>) snapshot.Value)
                        {
                            var u = (Dictionary<string, object>) user.Value;
                            Debug.Log($"{(string) u["username"]}: {Convert.ToInt32(u["completeDuration"])}");
                            users.Add(new((string) u["username"], Convert.ToInt32(u["completeDuration"])));
                        }
                    }
                    listener?.OnLeaderboardRetrieveCompleted(users);
                }
            });
    }
}
