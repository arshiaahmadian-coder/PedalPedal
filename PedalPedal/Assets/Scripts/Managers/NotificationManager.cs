using ArabicTextSupport;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField] private GameObject darkBG;
    [SerializeField] private GameObject smallNotifWindow;
    [SerializeField] private ArabicUIText messageText;
    [SerializeField] private ArabicUIText buttonText;

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
    }
}
