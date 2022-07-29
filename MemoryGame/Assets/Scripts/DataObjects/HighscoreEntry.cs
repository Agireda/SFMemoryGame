using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreEntry : IComparer<HighscoreEntry>
{

    public string name;
    public string date;
    public string fighter;
    public float score;


    public HighscoreEntry(string name, string fighter, string date, float score)
    {
        this.name = name;
        this.date = date;
        this.fighter = fighter;
        this.score = score;
    }

    public int Compare(HighscoreEntry x, HighscoreEntry y)
    {
        return x.score.CompareTo(y.score);
    }
}

