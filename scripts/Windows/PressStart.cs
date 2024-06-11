using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressStart : MonoBehaviour
{
    [SerializeField] FadeLoader fadeLoader;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ゲーム情報のリセット
            GameManager.instance.InitGame();
            //SceneManager.LoadScene("level1_0");
            //Initiate.Fade("level1_0", Color.black, 1.0f);
            fadeLoader.FadeLoadScene("level1_0");
        }
    }
}
