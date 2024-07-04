using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToFloors : MonoBehaviour
{
    [SerializeField] FadeLoader fadeLoader;
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.isFloorStart = true;
        if (GameManager.instance.isFloorStart && GameManager.instance.now_floor <= GameManager.instance.n_floors)
        {
            GameManager.instance.MakeFloorList();

            // 最初だけGMを使ってシーン切り替えを行う
            string firstFloor = string.Copy(GameManager.instance.FloorList[0]);
            GameManager.instance.FloorList.Remove(GameManager.instance.FloorList[0]);
            GameManager.instance.isNextScene = true;
            GameManager.instance.fromCardinalDirection = "north";
            //SceneManager.LoadScene(firstFloor);
            fadeLoader.FadeLoadScene(firstFloor);
        }

    }
}
