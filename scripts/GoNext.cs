using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoNext : MonoBehaviour
{
    // シーン切り替えfadeLoader
    [SerializeField] FadeLoader fadeLoader;

    [Header("eventWindow")] public GameObject EWindow = null;
    public GameObject playableCharacter;

    private EventWindow eventWindow = null;
    private Player player = null;
    private bool isSelectingMode = false;

    private void Start()
    {
        //MWindow = GetComponent<GameObject>();
        eventWindow = EWindow.GetComponent<EventWindow>();
        EWindow.SetActive(false);
        player = playableCharacter.GetComponent<Player>();
    }

    private void Update()
    {
        if (isSelectingMode)
        {
            EWindow.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                if (eventWindow.isyes)
                {
                    // プレイヤ情報の更新
                    // プレイヤが動き出せるようにする
                    player.isSelectingMode = false;
                    //MWindow.SetActive(false);

                    // シーン切り替え
                    // FloorListの中身が1以上ならば
                    if (GameManager.instance.FloorList.Count >= 1)
                    {
                        Debug.Log("GameManager.instance.FloorList.Count" + GameManager.instance.FloorList.Count);
                        // そのシーンに切り替える
                        string nextScene = string.Copy(GameManager.instance.FloorList[0]); //nextSceneを取り出す
                        GameManager.instance.isNextScene = true; // 切り替えflagを立てる
                        GameManager.instance.fromCardinalDirection = cardinalDirection; // spawn方位を決定

                        GameManager.instance.FloorList.RemoveAt(0);
                        //SceneManager.LoadScene(nextScene);
                        fadeLoader.FadeLoadScene(nextScene);
                    }
                    else
                    {
                        Debug.Log("GameManager.instance.FloorList.Count" + GameManager.instance.FloorList.Count);
                        GameManager.instance.isNextScene = true; // 切り替えflagを立てる
                        GameManager.instance.fromCardinalDirection = cardinalDirection; // spawn方位を決定
                        Debug.Log("cardinalDirection:" + cardinalDirection);
                        Debug.Log("GM.cardinalDirection:" + GameManager.instance.fromCardinalDirection);
                        // FloorGoalに移動させる
                        //SceneManager.LoadScene("FloorGoal");
                        fadeLoader.FadeLoadScene($"FloorGoal{GameManager.instance.floorIndex}");
                        //fadeLoader.FadeLoadScene("FloorGoal");
                    }

                }
                else if (eventWindow.isno)
                {
                    //プレイヤが動き出せるようにする
                    player.isSelectingMode = false;

                    isSelectingMode = false;
                    EWindow.SetActive(false);
                }
            }
        }
    }

    public string cardinalDirection = ""; // インスペクタで設定


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        eventWindow.YesPanel.Select();

    //        // 宝箱を開くか確認するパネルを表示
    //        isSelectingMode = true;

    //        // プレイヤの動きを止める
    //        player.isSelectingMode = true;


    //    }
        
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            eventWindow.YesPanel.Select();

            // 宝箱を開くか確認するパネルを表示
            isSelectingMode = true;

            // プレイヤの動きを止める
            player.isSelectingMode = true;


        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            eventWindow.isyes = false;
            eventWindow.isno = false;
            eventWindow.YesPanel.isFirstSelect = true;

        }
    }


    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        //eventWindow.YesPanel.Select();

    //        // 宝箱を開くか確認するパネルを表示
    //        isSelectingMode = true;

    //        // プレイヤの動きを止める
    //        player.isSelectingMode = true;


    //    }
    //}
}
