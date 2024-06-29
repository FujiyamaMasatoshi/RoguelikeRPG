using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearEvent : MonoBehaviour
{
    [SerializeField] FadeLoader fadeLoader;
    [Header("クリアウィンドウ")] public GameObject ClrWindow = null;

    private ClearWindow clearWindow = null;

    // 初めてキーボードを叩いたか
    private bool firstPress = false;


    // Start is called before the first frame update
    void Start()
    {
        clearWindow = ClrWindow.GetComponent<ClearWindow>();
        ClrWindow.SetActive(false);

        //clearWindow.titlePanel.Select(); // タイトルテキストを選択する
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(firstPress == false)
            {
                firstPress = true; // はじめてのキーボードを叩くflagを立てる
                ClrWindow.SetActive(true);
            }
            else
            {
                // 選択したテキストの内容を実行する
                // タイトルテキストが選択された -> タイトル画面へ遷移
                if (clearWindow.titlePanel.IsSelect())
                {
                    // ゲーム情報のリセット
                    //GameManager.instance.InitGame();
                    //SceneManager.LoadScene("Title");
                    fadeLoader.FadeLoadScene("Title");
                }
                // level1_0へ遷移
                else if (clearWindow.restartPanel.IsSelect())
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
