using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerTxt;
    public int seconds; // for data saving and record

    private bool isStoped = true;
    private float timer;
    private int min;
    private int sec;

    private string secStr;
    private string minStr;
    private string timeStr;

    private void FixedUpdate() 
    {
        if (isStoped) return;

        timer += Time.deltaTime;

        if (timer >= 1) { // one second passed
            sec += 1;
            timer = 0;
            seconds += 1;
        }

        if (sec >= 60) { // one minute passed
            sec = 0;
            min += 1;
        }

        UpdateUI();
    }

    public void ResetTimer() 
    {
        seconds = 0;
        min = 0;
        sec = 0;
        
        UpdateUI();
    }

    public void StartTimer() 
    {
        isStoped = false;
    }

    public void StopTimer() 
    {
        isStoped = true;
    }

    private void UpdateUI()
    {
        secStr = (sec + "").Length == 1 ? "0" + sec : sec + "";
        minStr = (min + "").Length == 1 ? "0" + min : min + "";

        timeStr =  minStr + ":" + secStr;

        timerTxt.text = timeStr;
    }
}
