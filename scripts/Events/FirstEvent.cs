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
    //string[] messages = {
    //    "The monsters in the MisteriTower have been troubling the residents on this island.",
    //    "You came to the island to calm the monsters' anger",
    //    "Inside the tower, you can find allies and skills to help in your quest",
    //    "Now, enter the MistericTower ahead and begin your quest!!"

    //};

    // Start is called before the first frame update
    void Start()
    {
        messageWindow = MWindow.GetComponent<MessageWindow>();
        player = playableCharacter.GetComponent<Player>();
        player.isSelectingMode = true;


        //messageWindow.SetMessage("spaceキーを押してください");
        messageWindow.SetMessage("press space key");
        // セットするメッセージ
        // きみは、不思議な塔を代々攻略していく使命を持った一族だ。
        // ここには、魔物が現れたりするが、不思議の塔のクリアのために、
        // 塔の中でパワーアップや仲間を増やして攻略しなければならない
        // まっすぐ進んで塔の中に入って攻略を始めるのだ！！
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space) && press && i <= 3)
        //{
        //    if(i == 0)
        //    {
        //        messageWindow.SetMessage(messages[0]);
        //    }
        //    else
        //    {
        //        messageWindow.AddMessage(messages[i]);
        //    }
        //    press = false;
            
        //}
        //else if(Input.GetKeyDown(KeyCode.Space) && !press && i<=3)
        //{
        //    i += 1;
        //    press = true;
        //}
        if (Input.GetKeyDown(KeyCode.Space) && i <= 3)
        {
            i += 1;
            press = true;
        }
        if (press && i<= 3)
        {
            //if (i == 0)
            //{
            //    messageWindow.SetMessage(messages[0]);
            //    press = false;
            //}
            //else
            //{
            //    messageWindow.AddMessage(messages[i]);
            //    press = false;
            //}
            messageWindow.SetMessage(messages[i]);
            press = false;
        }
        else if(i>3)
        {
            player.isSelectingMode = false;
            transform.gameObject.SetActive(false);
        }
    }
}
