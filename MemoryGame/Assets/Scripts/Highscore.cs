using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Highscore : MonoBehaviour
{
    public Text[] scoresText_20Cards;
    public Text[] dateText_20Cards;
    public Text[] scoresText_30Cards;
    public Text[] dateText_30Cards;
    void Start()
    {
        UpdateHighscore();
    }
    public void UpdateHighscore()
    {
        Config.UpdateScoreList();
        DisplayCardScoreData(Config.ScoreTimeList20Cards, Config.CardNumberList20Cards, scoresText_20Cards, dateText_20Cards);
        DisplayCardScoreData(Config.ScoreTimeList30Cards, Config.CardNumberList30Cards, scoresText_30Cards, dateText_30Cards);
    }

    private void DisplayCardScoreData(float[] scoreTimeList, string[] cardNumberList, Text[] scoresText, Text[] dateText)
    {
        for (var index = 0; index < 3; index++)
        {
            if (scoreTimeList[index] > 0)
            {
                var dataTime = Regex.Split(cardNumberList[index], "T");

                var minutes = Mathf.Floor(scoreTimeList[index] / 60);
                float seconds = Mathf.RoundToInt(scoreTimeList[index] % 60);

                scoresText[index].text = minutes.ToString("00") + ":" + seconds.ToString("00");
                dateText[index].text = dataTime[0] + " " + dataTime[1];
            }
            else
            {
                scoresText[index].text = " ";
                dateText[index].text = " ";
            }
        }
    }
}
