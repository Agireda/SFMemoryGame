using UnityEngine;
using Firebase; //We need to include all firebase stuff
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;

public class FirebaseTest : MonoBehaviour
{
    FirebaseDatabase db;

    void Start()
    {
        //Setup for talking to firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            //Log if we get any errors from the opperation
            if (task.Exception != null)
                Debug.LogError(task.Exception);

            //the database
            db = FirebaseDatabase.DefaultInstance;

            //Set the value World to the key Hello in the database
            var uuid = System.Guid.NewGuid().ToString();
            var highscoreEntry = new HighscoreEntry("hej", "på", "dej", 10f);

            string jsonString = JsonUtility.ToJson(highscoreEntry); 
            Debug.Log(jsonString);
            db.RootReference.Child("Highscores").Child(uuid).SetValueAsync(jsonString);

        });
    }
}
