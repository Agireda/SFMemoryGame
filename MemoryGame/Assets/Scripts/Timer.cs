using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GUIStyle timerStyle;
    private float timer;
    private float seconds;
    private float minutes;
    private const float virtualWidth = 480f;
    private const float virtualHeigth = 854f;
    private bool stopTimer;
    private Matrix4x4 matrix;
    private Matrix4x4 oldMatrix;
    void Start()
    {
        stopTimer = false;
        matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / virtualWidth, Screen.height / virtualHeigth, 1f));
        oldMatrix = GUI.matrix;
    }
    void Update()
    {
        if (!stopTimer)
        {
            timer += Time.deltaTime;
        }
    }

    private void OnGUI()
    {
        GUI.matrix = matrix;
        minutes = Mathf.Floor(timer / 60);
        seconds = Mathf.RoundToInt(timer % 60);

        GUI.Label(new Rect(Camera.main.rect.x + 20, 10, 120, 50), "" + minutes.ToString("00") + ":" + seconds.ToString("00"), timerStyle);

        GUI.matrix = oldMatrix;
    }

    public float GetCurrentTime()
    {
        return timer;
    }

    public void StopTimer()
    {
        stopTimer = true;
    }
}
