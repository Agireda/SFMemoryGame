using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class HighScoreEntries
{

    public Dictionary<string, HighscoreEntry> highScoreDictionary { get; set; }
    public HighScoreEntries()
    {
        this.highScoreDictionary = new Dictionary<string, HighscoreEntry>();
    }

    public void Add(HighscoreEntry highScoreEntry)
    {
        highScoreDictionary.Add(System.Guid.NewGuid().ToString(), highScoreEntry);
    }
}


