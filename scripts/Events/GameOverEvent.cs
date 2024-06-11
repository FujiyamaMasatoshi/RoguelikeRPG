using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverEvent : MonoBehaviour
{
    [SerializeField] FadeLoader fadeLoader;
    [Header("ゲームオーバーイベント")] public GameObject GOWindow = null;

    private ClearWindow gameOverWindow = null;

    // 初めてキーボードを叩いたかどうか
    private bool firstPress = false;


    // Start is called before the first frame update
    void Start()
    {
        gameOverWindow = GOWindow.GetComponent<ClearWindow>();
        GOWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (firstPress == false)
            {
                firstPress = true; // はじめてのキーボードを叩くflagを立てる
                GOWindow.SetActive(true);
            }
            else
            {
                // 選択したテキストの内容を実行する
                // タイトルテキストが選択された -> タイトル画面へ遷移
                if (gameOverWindow.titlePanel.IsSelect())
                {
                    // ゲーム情報のリセット
                    //GameManager.instance.InitGame();
                    //SceneManager.LoadScene("Title");
                    fadeLoader.FadeLoadScene("Title");
                }
                // level1_0へ遷移
                else if (gameOverWindow.restartPanel.IsSelect())
                {
                    // ゲーム情報のリセット
                    GameManager.instance.InitGame();
                    //SceneManager.LoadScene("level1_0");
                    fadeLoader.FadeLoadScene("level1_0");
                }
            }

        }
    }
}
