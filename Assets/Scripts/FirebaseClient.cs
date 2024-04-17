using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseClient
{
    private IFirebaseListener listener;
    private int totalLevels = 15;
    public FirebaseClient(IFirebaseListener listener)
    {
        this.listener = listener;
    }

    public void RegisterOrLogin(User user)
    {
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.GetReference("users").Child(user.username);
        userRef.Child("username").SetValueAsync(user.username).ContinueWithOnMainThread(regTask => {
            if (regTask.IsFaulted)
            {
                Debug.LogError("Error registering/logging in user: " + regTask.Exception);
            }
            else if (regTask.IsCompleted)
            {
                userRef.Child("levelMenu").GetValueAsync().ContinueWithOnMainThread(levelTask => {
                    if (!levelTask.Result.Exists)
                    {
                        Debug.Log("Initializing levels for this user...");
                        DatabaseReference levelsRef = FirebaseDatabase.DefaultInstance.GetReference($"users/{user.username}/levelMenu");
                        for (int i = 1; i <= totalLevels; i++)
                        {
                            levelsRef.Child($"Level_{i}").Child("starsEarned").SetValueAsync(0);
                        }
                    }
                });
            }
        });
        RetrieveLevelsData(user.username);
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

    public void RetrieveLevelsData(string username)
    {
        string path = $"users/{username}/levelMenu";
        DatabaseReference levelsRef = FirebaseDatabase.DefaultInstance.GetReference(path);

        levelsRef.GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving levels data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    foreach (DataSnapshot levelSnapshot in snapshot.Children)
                    {
                        int levelIndex = int.Parse(levelSnapshot.Key.Replace("Level_", ""));
                        int starsEarned = int.Parse(levelSnapshot.Child("starsEarned").Value.ToString());
                        PlayerPrefs.SetInt($"Level_{levelIndex}", starsEarned);
                    }
                }
            }
        });
    }

    public void SaveAllLevelsData(string username)
    {
        Dictionary<string, object> levelsDataToUpdate = new Dictionary<string, object>();

        for (int i = 1; i <= totalLevels; i++)
        {
            string key = $"Level_{i}";
            int starsEarned = PlayerPrefs.GetInt(key, 0);
            string levelPath = $"levelMenu/{key}/starsEarned";
            levelsDataToUpdate[levelPath] = starsEarned;
        }

        string userPath = $"users/{username}";
        DatabaseReference userRef = FirebaseDatabase.DefaultInstance.GetReference(userPath);

        userRef.UpdateChildrenAsync(levelsDataToUpdate)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to save all level data: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    Debug.Log("Successfully saved all level data for user: " + username);
                }
            });
    }


}
