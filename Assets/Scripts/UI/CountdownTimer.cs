using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public int startMinutes = 1;
    public int startSeconds = 30;

    private float remainingTime;

    void Start()
    {
        remainingTime = startMinutes * 60 + startSeconds;
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                SceneManager.LoadScene("GameOver");
            }
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}