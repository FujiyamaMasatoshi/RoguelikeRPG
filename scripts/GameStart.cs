using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameStart : MonoBehaviour
{
    //[Header("ゲームレベル")] public int game_level = 1;

    public void StartGame()
    {


        //Debug.Log("ボタンがクリックされました");
        SceneManager.LoadScene("level1_0");
    }
}
