using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class Config
{
#if UNITY_EDITOR
    static readonly string Dir = Directory.GetCurrentDirectory();
#elif UNITY_ANDROID
    static readonly string Dir = Application.persistentDataPath;
#else
private static readonly string Dir = Directory.GetcurrentDirectory();
#endif

    static readonly string File = @"\CardMatching.ini";
    static readonly string Path = Dir + File;

    private const int NumberOfScoreRecords = 3;

    public static float[] ScoreTimeList20Cards = new float[NumberOfScoreRecords];
    public static string[] CardNumberList20Cards = new string[NumberOfScoreRecords];

    public static float[] ScoreTimeList30Cards = new float[NumberOfScoreRecords];
    public static string[] CardNumberList30Cards = new string[NumberOfScoreRecords];

    private static bool bestScore = false;

    public static void CreateScoreFile()
    {
        if(System.IO.File.Exists(Path) == false)
        {
            CreateFile();
        }

        UpdateScoreList();
    }

    public static void UpdateScoreList()
    {
        var file = new StreamReader(Path);
        UpdateScoreList(file, ScoreTimeList20Cards, CardNumberList20Cards);
        UpdateScoreList(file, ScoreTimeList30Cards, CardNumberList30Cards);
        file.Close();
    }

    private static void UpdateScoreList(StreamReader file, float[] scoreTimeList, string[] cardNumberList)
    {
        if (file == null) return;

        var line = file.ReadLine();

        while(line != null && line[0] == '(')
        {
            line = file.ReadLine();
        }

        for (int i = 1; i <= NumberOfScoreRecords; i++)
        {
            var word = line.Split('#');

            if(word[0] == i.ToString())
            {
                string[] substring = Regex.Split(word[1], "D");
                if(float.TryParse(substring[0], out var scoreOnPosition))
                {
                    scoreTimeList[i - 1] = scoreOnPosition;
                    if(scoreTimeList[i-1] > 0)
                    {
                        var dataTime = Regex.Split(substring[1], "T");
                        cardNumberList[i - 1] = dataTime[0] + "T" + dataTime[1];
                    }
                    else
                    {
                        cardNumberList[i - 1] = " ";
                    }
                }
                else
                {
                    scoreTimeList[i - 1] = 0;
                    cardNumberList[i - 1] = " ";
                }
            }

            line = file.ReadLine();
        }
    }

    public static void PlaceScoreOnBoard(float time)
    {
        UpdateScoreList();
        bestScore = false;

        switch(GameSettings.Instance.GetCardNumber())
        {
            case GameSettings.ECardNumber.E20Cards:
                PlaceScoreOnBoard(time, ScoreTimeList20Cards, CardNumberList20Cards);
                break;
            case GameSettings.ECardNumber.E30Cards:
                PlaceScoreOnBoard(time, ScoreTimeList30Cards, CardNumberList30Cards);
                break;
        }

        SaveScoreList();
    }

    private static void PlaceScoreOnBoard(float time, float[] scoreTimeList, string[] cardNumberList)
    {
        var theTime = System.DateTime.Now.ToString("hh:mm");
        var theDate = System.DateTime.Now.ToString("dd/MM/yyyy");
        var currentDate = theDate + "T" + theTime;

        for(int i = 0; i < NumberOfScoreRecords; i++)
        {
            if(scoreTimeList[i] > time || scoreTimeList[i] == 0.0f)
            {
                if (i == 0)
                    bestScore = true;

                for (var moveScoreDown = (NumberOfScoreRecords -1); moveScoreDown > i; moveScoreDown --)
                {
                    scoreTimeList[moveScoreDown] = scoreTimeList[moveScoreDown - 1];
                    cardNumberList[moveScoreDown] = cardNumberList[moveScoreDown - 1];
                }

                scoreTimeList[i] = time;
                cardNumberList[i] = currentDate;
                break;
            }
        }
    }


    public static bool IsBestScore()
    {
        return bestScore;
    }

    public static void CreateFile()
    {
        SaveScoreList();
    }

    public static void SaveScoreList()
    {
        System.IO.File.WriteAllText(Path, string.Empty);

        var writer = new StreamWriter(Path, false);

        writer.WriteLine("(20CARDS)");
        for (var i = 1; i <= NumberOfScoreRecords; i++)
        {
            var x = ScoreTimeList20Cards[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + CardNumberList20Cards[i - 1]);
        }

        writer.WriteLine("(30CARDS)");
        for (var i = 1; i <= NumberOfScoreRecords; i++)
        {
            var x = ScoreTimeList30Cards[i - 1].ToString();
            writer.WriteLine(i.ToString() + "#" + x + "D" + CardNumberList30Cards[i - 1]);
        }

        writer.Close();
    }
}
