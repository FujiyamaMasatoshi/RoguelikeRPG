using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    [Header("ヒーリングエフェクト")] public GameObject[] healingEffects;
    [Header("メッセージウィンドウ")] public GameObject MWindow = null;
    [Header("NPCs")] public GameObject[] NPCs = null;
    //// Start is called before the first frame update
    void Start()
    {
        // NPCsの設定

        foreach(GameObject healEffect in healingEffects)
        {
            healEffect.SetActive(false);
            MWindow.SetActive(false);
        }
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (collision.CompareTag("Player"))
        {
            //Player player = obj.GetComponent<Player>();
            //GameManager.instance.pHP = GameManager.instance.pMaxHP;
            //GameManager.instance.pMP = GameManager.instance.pMaxMP;

            //player.hp = GameManager.instance.pMaxHP;
            //player.mp = GameManager.instance.pMaxMP;

            //// 味方キャラクタを回復させる
            //// GMのパーティメンバの中身に死んだキャラクタがいるならば、
            //// isDieをfalseにして回復させる
            //for (int i = 1; i < GameManager.instance.isParty.Count; i++)
            //{
            //    // 味方キャラクターが追加されていて、かつ、死んでいたら、回復させる
            //    if (GameManager.instance.isParty[i])
            //    {
            //        // HP, MPを回復
            //        GameManager.instance.NpcsHP[i-1] = GameManager.instance.NpcsMaxHP[i-1];
            //        GameManager.instance.NpcsMP[i-1] = GameManager.instance.NpcsMaxMP[i-1];

            //        // 死亡フラグをリセット
            //        GameManager.instance.isNpcsDie[i-1] = false;

            //        // gameObjectをactive trueにする
            //        NPCs[i-1].SetActive(true);

            //    }
            //}

            // isPartyの場合に、HPとMPを回復させる
            for (int i=0; i<GameManager.instance.alies.Count; i++)
            {
                // 味方キャラクタならば、HPとMPを回復させる
                if (GameManager.instance.isParty[i])
                {
                    // GM.initAliesStatusをGM.nowAliesStatusに反映させる
                    GameManager.instance.SyncStatus(i); // ステータスの回復
                    // 死亡フラグをfalseに戻す
                    GameManager.instance.isAliesDead[i] = false;

                    if (i>0) NPCs[i - 1].SetActive(true);
                }
            }

        }
        //else if (collision.CompareTag("PartyMember"))
        //{
        //    Player player = obj.GetComponent<Player>();
        //    int index = 0;
        //    if(player.Name == "Archer")
        //    {
        //        index = 0;
        //    }else if(player.Name == "Warrior")
        //    {
        //        index = 1;
        //    }
        //    GameManager.instance.NpcsHP[index] = GameManager.instance.NpcsMaxHP[index];
        //    GameManager.instance.NpcsMP[index] = GameManager.instance.NpcsMaxMP[index];

        //    player.hp = GameManager.instance.NpcsMaxHP[index];
        //    player.mp = GameManager.instance.NpcsMaxMP[index];
        //}

        foreach(GameObject healEffect in healingEffects)
        {
            healEffect.SetActive(true);
        }

        // メッセージウィンドウを表示させて、回復したメッセージを表示させる
        MWindow.SetActive(true);
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    //
    //    GameObject obj = collision.gameObject;
    //    if (collision.CompareTag("Player"))
    //    {
    //        Player player = obj.GetComponent<Player>();
    //        player.hp = GameManager.instance.pMaxHP;
    //        player.mp = GameManager.instance.pMaxMP;
    //    }
    //}


    private void OnTriggerExit2D(Collider2D collision)
    {
        // 
        foreach (GameObject healEffect in healingEffects)
        {
            healEffect.SetActive(false);
        }

        // messageWindow active false
        MWindow.SetActive(false);
    }
}
