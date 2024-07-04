using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBackFloorStart : MonoBehaviour
{
    [SerializeField] FadeLoader fadeLoader;

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.instance.now_floor == GameManager.instance.n_floors)
            {
                // ゲームクリア画面へ
                //SceneManager.LoadScene("GameClear");
                fadeLoader.FadeLoadScene("GameClear");
            }
            else
            {
                //SceneManager.LoadScene("FloorStart");

                // 新しくGM.floorIndexを決める
                GameManager.instance.floorIndex = Random.Range(0, 2);
                Debug.Log($"GM.floorIndex in GoBackFloorStart: {GameManager.instance.floorIndex}");

                fadeLoader.FadeLoadScene($"FloorStart{GameManager.instance.floorIndex}");
                //fadeLoader.FadeLoadScene("FloorStart");
            }
        }
        
        
    }
}
