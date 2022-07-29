using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    public GameObject highscoreList20;
    public GameObject highscoreList30;
    public HighscoreListItem highscoreListItemPrefab;
    

    void Start()
    {
        //Requests savemanager to load highscores from firebase asyncronously and runs the callback method PopulateHighscoreLists20/30 on completion
        SaveManager.Instance.LoadHighscores("Highscores20", PopulateHighscoreLists20);
        SaveManager.Instance.LoadHighscores("Highscores30", PopulateHighscoreLists30);
    }

    void PopulateHighscoreLists20(List<HighscoreEntry> highscoreEntries)
    {
        // Sorts highscore entries by field score using iComparer syntax
        highscoreEntries.Sort((p1, p2) => p1.score.CompareTo(p2.score));
        for (int i = 0; i < 3; i++)
        {
            var highscoreEntry = highscoreEntries[i];
            var listItem = Instantiate<HighscoreListItem>(highscoreListItemPrefab, highscoreList20.transform);
            listItem.Initialize(i+1, highscoreEntry);
        }
    }
    void PopulateHighscoreLists30(List<HighscoreEntry> highscoreEntries)
    {
        // Sorts highscore entries by field score using iComparer syntax
        highscoreEntries.Sort((p1, p2) => p1.score.CompareTo(p2.score));
        for (int i = 0; i < 3; i++)
        {
            var highscoreEntry = highscoreEntries[i];
            var listItem = Instantiate<HighscoreListItem>(highscoreListItemPrefab, highscoreList30.transform);
            listItem.Initialize(i + 1, highscoreEntry);
        }
    }
}
