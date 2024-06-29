using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageWindow : MonoBehaviour
{
    [Header("メッセージウィンドウ")] public GameObject messageWindow = null;
    private void Start()
    {
        InitMessage();
    }
    public void InitMessage()
    {
        TextMeshProUGUI text = messageWindow.GetComponent<TextMeshProUGUI>();
        text.text = "";
    }

    public void SetMessage(string message)
    {
        TextMeshProUGUI text = messageWindow.GetComponent<TextMeshProUGUI>();
        text.text = message;
    }

    public void AddMessage(string message)
    {
        TextMeshProUGUI text = messageWindow.GetComponent<TextMeshProUGUI>();
        text.text += message;
    }
}
