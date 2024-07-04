using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ReloadMap : MonoBehaviour
{
    // Fade
    [SerializeField] FadeLoader fadeLoader;
    [Header("難易度選択画面")] public GameObject SMWindow = null;
    [Header("プレイアブルキャラクタ")] public GameObject playableCharacter;

    private SelectModeWindow selectModeWindow = null;
    private Player player = null;
    private bool isSelectingGameMode = false;
    private string selectedMode = ""; //{Easy, Normal, Hard}

    private void Start()
    {
        // モード選択画面
        selectModeWindow = SMWindow.GetComponent<SelectModeWindow>();
        SMWindow.SetActive(false);

        // プレイアブルキャラクタ
        player = playableCharacter.GetComponent<Player>();
    }

    private void Update()
    {
        // ドアの前にいる時、
        if (isSelectingGameMode)
        {

            // #####################
            // ゲーム難易度設定を行う
            // #####################
            SMWindow.SetActive(true);

            // ↓ EventSystemで解決したかも

            //ウィンドウがアクティブになったら、Easyテキストパネルを選択する
            //selectModeWindow.easyPanel.Select();

            ////Input.GetKeyDown(KeyCode.up or donw)で選択をしていく
            //// 上矢印を押した時
            //if (Input.GetKeyDown(KeyCode.UpArrow))
            //{
            //    if (selectModeWindow.easyPanel.isSelect)
            //    {
            //        // 何もしない
            //    }
            //    else if(selectModeWindow.normalPanel.isSelect)
            //    {

            //    }
            //}

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // フロアインデックスを決定する
                GameManager.instance.floorIndex = Random.Range(0, 2);
                Debug.Log("GM.floorIndex: " + GameManager.instance.floorIndex);

                if (selectModeWindow.iseasy)
                {
                    selectedMode = selectModeWindow.easyPanel.GetInitText();
                    GameManager.instance.gameMode = selectedMode;
                    GameManager.instance.n_floors = 1; // フロアの数
                    //SceneManager.LoadScene("FloorStart");
                    fadeLoader.FadeLoadScene($"FloorStart{GameManager.instance.floorIndex}");
                }
                else if (selectModeWindow.isnormal)
                {
                    selectedMode = selectModeWindow.normalPanel.GetInitText();
                    GameManager.instance.gameMode = selectedMode;
                    GameManager.instance.n_floors = 2; // フロアの数
                    //SceneManager.LoadScene("FloorStart");
                    fadeLoader.FadeLoadScene($"FloorStart{GameManager.instance.floorIndex}");
                }
                else if (selectModeWindow.ishard)
                {
                    selectedMode = selectModeWindow.hardPanel.GetInitText();
                    GameManager.instance.gameMode = selectedMode;
                    GameManager.instance.n_floors = 3; // フロアの数
                    //SceneManager.LoadScene("FloorStart");
                    fadeLoader.FadeLoadScene($"FloorStart{GameManager.instance.floorIndex}");
                }
                else if (selectModeWindow.isback)
                {
                    selectedMode = selectModeWindow.backPanel.GetInitText();
                    GameManager.instance.gameMode = selectedMode;
                    player.isSelectingMode = false;

                    // SMWindowを非アクティブにする
                    isSelectingGameMode = false;
                    SMWindow.SetActive(false);

                    GameManager.instance.isEventDoing = false;
                }
                Debug.Log("selectedMode:" + GameManager.instance.gameMode);
                Debug.Log("GM.n_floors:" + GameManager.instance.n_floors);
            }
        }
        else
        {
            selectModeWindow.iseasy = false;
            selectModeWindow.isnormal = false;
            selectModeWindow.ishard = false;
            selectModeWindow.isback = false;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        // ゲームモードを選択中
        if (collision.CompareTag("Player"))
        {
            // テキストの初期選択
            selectModeWindow.easyPanel.Select(); 

            //GMでセレクティングモードを設定する
            GameManager.instance.isEventDoing = true;

            isSelectingGameMode = true;
            // プレイアブルキャラクタの移動を止める
            player.isSelectingMode = true;
        }
        

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            GameManager.instance.isEventDoing = true;
            // 選択中
            isSelectingGameMode = true;
            Debug.Log($"selectedMode: {selectedMode}");
            // プレイアブルキャラクタの移動を止める
            
            if (selectedMode != "Back")
            {
                player.isSelectingMode = true;
            }
            else
            {
                player.isSelectingMode = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.isEventDoing = false;
            // テキストを初期テキストに戻す
            // easyPanel
            TextMeshProUGUI easyText = selectModeWindow.easyPanel.GetComponent<TextMeshProUGUI>();
            easyText.text = selectModeWindow.easyPanel.GetInitText();
            // normalPanel
            TextMeshProUGUI normalText = selectModeWindow.normalPanel.GetComponent<TextMeshProUGUI>();
            normalText.text = selectModeWindow.normalPanel.GetInitText();
            // hardPanel
            TextMeshProUGUI hardText = selectModeWindow.hardPanel.GetComponent<TextMeshProUGUI>();
            hardText.text = selectModeWindow.hardPanel.GetInitText();
            // backPanel
            TextMeshProUGUI backText = selectModeWindow.backPanel.GetComponent<TextMeshProUGUI>();
            backText.text = selectModeWindow.backPanel.GetInitText();


            isSelectingGameMode = false;
            SMWindow.SetActive(false);

            // プレイアブルキャラクタの移動を止める
            player.isSelectingMode = false;
            selectedMode = "";
        }
    }
}
