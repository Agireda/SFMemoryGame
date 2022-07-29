using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreListItem : MonoBehaviour
{

    public TextMeshProUGUI score;
    public TextMeshProUGUI position;
    public TextMeshProUGUI fighter;
    public TextMeshProUGUI date;
    public TextMeshProUGUI playerName;

    public void Initialize(int i, HighscoreEntry highscoreEntry)
    {
        position.text = i.ToString();
        score.text = ((int)highscoreEntry.score).ToString();
        fighter.text = highscoreEntry.fighter;
        date.text = highscoreEntry.date;
        playerName.text = highscoreEntry.name;
    }

}
