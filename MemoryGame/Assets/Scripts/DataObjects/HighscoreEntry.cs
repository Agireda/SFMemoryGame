using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreEntry
{
    
    public string name;
    public string date;
    public string fighter;
    public float score;


    public HighscoreEntry(string name, string fighter, string date, float score)
    {
        this.name = name;
        this.fighter = fighter;
        this.date = date;
        this.score = score;
    }
}
