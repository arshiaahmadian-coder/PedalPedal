using ArabicTextSupport;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private GameObject darkBG;
    [Header("small window")]
    [SerializeField] private GameObject smallNotifWindow;
    [SerializeField] private ArabicUIText messageText;
    [SerializeField] private ArabicUIText buttonText;
    [Header("coin window")]
    [SerializeField] private GameObject coinNotifWindow;
    [SerializeField] private ArabicUIText coinMessageText;

    private static NotificationManager instance;

    public static NotificationManager GetInstance()
    {
        if (instance == null) instance = FindFirstObjectByType<NotificationManager>();

        return instance;
    }

    public void ShowSmallNotification(string message = "پیام خالی", string buttonMessage = "باشه")
    {
        darkBG.SetActive(true);
        smallNotifWindow.SetActive(true);
        messageText.Text = message;
        buttonText.Text = buttonMessage;
    }

    public void CloseWindw()
    {
        darkBG.SetActive(false);
        smallNotifWindow.SetActive(false);
        coinNotifWindow.SetActive(false);
    }

    public void ShowCoinNotification(int amount)
    {
        darkBG.SetActive(true);
        coinNotifWindow.SetActive(true);
        coinMessageText.Text = amount.ToString();
    }

}
