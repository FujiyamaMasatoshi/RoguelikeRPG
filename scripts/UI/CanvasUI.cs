using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// ゲームメニュー画面

public class CanvasUI : MonoBehaviour
{

    // プレイアブルキャラクタ
    public Player playableCharacter = null;


    // 表示させるパネルUI
    public PanelUI panelUI = null;
    public PartyUI partyUI = null;

    // menu画面を開いているかどうか
    private bool isOpenMenu = false;


    // Start is called before the first frame update
    void Start()
    {
        //panelUI = GetComponent<PanelUI>();
        //partyUI = GetComponent<PartyUI>();
    }



    // Update is called once per frame
    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Battle" + $"{GameManager.instance.floorIndex}")
        {
            panelUI.gameObject.SetActive(false);
        }
        else
        {
            if (!isOpenMenu && Input.GetKeyDown(KeyCode.M))
            {
                isOpenMenu = true;
            }
            else if (isOpenMenu && Input.GetKeyDown(KeyCode.M))
            {
                isOpenMenu = false;
            }

            // Menu画面開いている状態なら
            if (isOpenMenu)
            {
                // プレイヤ~の動きを止める
                playableCharacter.isSelectingMode = true;
                panelUI.gameObject.SetActive(true);
                partyUI.gameObject.SetActive(true);
            }
            else if (GameManager.instance.isEventDoing)
            {
                playableCharacter.isSelectingMode = true;
            }
            else
            {

                // プレイヤを動かす
                playableCharacter.isSelectingMode = false;

                panelUI.gameObject.SetActive(false);
                partyUI.gameObject.SetActive(false);
            }
        }
        
        
    }
}
