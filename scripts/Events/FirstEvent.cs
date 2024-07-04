using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEvent : MonoBehaviour
{
    [Header("メッセージウィンドウ")] public GameObject MWindow = null;
    [Header("プレイアブルキャラクタ")] public GameObject playableCharacter = null;

    private MessageWindow messageWindow = null;
    private Player player = null;

    private bool press = false;
    private int i = -1;
    string[] messages = {
        "きみは、不思議な塔を代々攻略していく使命を持った一族だ。\n",
        "前方の塔からは、凶暴な魔物が出現し、毎年その怒りを納めなければならない\n",
        "塔の中にはきみの仲間となるキャラクターやスキルが用意されているので、集めることでパワーアップできる。\n",
        "さあ、まっすぐ進んで塔の中に入って攻略を始めるのだ！！\n"
    };

    // Start is called before the first frame update
    void Start()
    {
        messageWindow = MWindow.GetComponent<MessageWindow>();
        player = playableCharacter.GetComponent<Player>();
        if (!GameManager.instance.isFirstMessage)
        {
            
            player.isSelectingMode = true;


            //messageWindow.SetMessage("spaceキーを押してください");
            messageWindow.SetMessage("press space key");
        }
        else
        {
            messageWindow.gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isFirstMessage)
        {
            if (Input.GetKeyDown(KeyCode.Space) && i <= 3)
            {
                i += 1;
                press = true;
            }
            if (press && i <= 3)
            {
                messageWindow.SetMessage(messages[i]);
                press = false;
            }
            else if (i > 3)
            {
                player.isSelectingMode = false;
                transform.gameObject.SetActive(false);

                GameManager.instance.isEventDoing = false;
                GameManager.instance.isFirstMessage = true;
            }
        }
        
    }
}
