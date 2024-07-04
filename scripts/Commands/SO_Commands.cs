using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class SO_Commands : ScriptableObject
{
    [Header("コマンドの名前")] public string Name; // コマンドの名前
    [Header("消費MP")] public int mp; // 必要MP
    [Header("アクション属性[attack, heal, buf, debuf, protect]")] public string att;
    [Header("1対のキャラクタに対してか")] public bool forOneCharacter;

    // サウンドエフェクト
    public AudioClip se;
    public List<GameObject> effect = new List<GameObject>();
    public List<string> forWho = new List<string>(); //{user or target}

    // コマンド実行
    public virtual void Execute(Character user, Character target)
    {

    }

    public void ReceiveDamage(Character target)
    {
        if (target.isEnemy)
        {
            Enemy enemy = target.GetComponent<Enemy>();

            //bool temp = enemy.AnimationOnePlay("damaged");
            enemy.TakeDamage();
        }
        // Player側のキャラクタがダメージを受けた場合
        else
        {
            Debug.Log("call TakeDamage()");
            Player player = target.GetComponent<Player>();
            player.TakeDamage();
        }
    }




    // 行動が行われるたびにGMのステータス情報を更新する

    // Playerに関することは、GameManagerに逐一情報更新していく
    public void SyncCharacterStatusInGameManager(Character user, Character target)
    {
        // user
        if (user.CompareTag("Player") || user.CompareTag("PartyMember"))
        {
            for (int i=0; i<GameManager.instance.alies.Count; i++)
            {
                if (user.name == GameManager.instance.alies[i].name)
                {
                    GameManager.instance.nowAliesStatus[i]["max_hp"] = user.max_hp;
                    GameManager.instance.nowAliesStatus[i]["hp"] = user.hp;
                    GameManager.instance.nowAliesStatus[i]["max_mp"] = user.max_mp;
                    GameManager.instance.nowAliesStatus[i]["mp"] = user.mp;
                    GameManager.instance.nowAliesStatus[i]["atk"] = user.atk;
                    GameManager.instance.nowAliesStatus[i]["def"] = user.def;
                    GameManager.instance.nowAliesStatus[i]["spd"] = user.spd;
                    GameManager.instance.nowAliesStatus[i]["rec"] = user.rec;
                    if (user.hp <= 0) GameManager.instance.isAliesDead[i] = true;
                }
            }
        }
        // target
        if (target.CompareTag("Player") || target.CompareTag("PartyMember"))
        {
            for (int i = 0; i < GameManager.instance.alies.Count; i++)
            {
                if (target.name == GameManager.instance.alies[i].name)
                {
                    GameManager.instance.nowAliesStatus[i]["max_hp"] = target.max_hp;
                    GameManager.instance.nowAliesStatus[i]["hp"] = target.hp;
                    GameManager.instance.nowAliesStatus[i]["max_mp"] = target.max_mp;
                    GameManager.instance.nowAliesStatus[i]["mp"] = target.mp;
                    GameManager.instance.nowAliesStatus[i]["atk"] = target.atk;
                    GameManager.instance.nowAliesStatus[i]["def"] = target.def;
                    GameManager.instance.nowAliesStatus[i]["spd"] = target.spd;
                    GameManager.instance.nowAliesStatus[i]["rec"] = target.rec;
                    if (target.hp <= 0) GameManager.instance.isAliesDead[i] = true;
                }
            }
        }

        //// userがPlayerの時
        //if (user.CompareTag("Player"))
        //{
        //    //
        //    GameManager.instance.pMaxHP = user.max_hp;
        //    GameManager.instance.pHP = user.hp;
        //    GameManager.instance.pMaxMP = user.max_mp;
        //    GameManager.instance.pMP = user.mp;
        //    GameManager.instance.pATK = user.atk;
        //    GameManager.instance.pDEF = user.def;
        //    GameManager.instance.pSPD = user.spd;
        //    GameManager.instance.pREC = user.rec;
        //}
        //// 味方NPCの時
        //else if (user.CompareTag("PartyMember"))
        //{
        //    // Archerの時
        //    if (user.Name == "Archer")
        //    {
        //        GameManager.instance.NpcsMaxHP[0] = user.max_hp;
        //        GameManager.instance.NpcsHP[0] = user.hp;
        //        GameManager.instance.NpcsMaxMP[0] = user.max_mp;
        //        GameManager.instance.NpcsMP[0] = user.mp;
        //        GameManager.instance.NpcsATK[0] = user.atk;
        //        GameManager.instance.NpcsDEF[0] = user.def;
        //        GameManager.instance.NpcsSPD[0] = user.spd;
        //        GameManager.instance.NpcsREC[0] = user.rec;
        //        if (user.hp <= 0)
        //        {
        //            GameManager.instance.isNpcsDie[0] = true;
        //        }
        //    }
        //    // Warriorの時
        //    else if (user.Name == "Warrior")
        //    {
        //        GameManager.instance.NpcsMaxHP[1] = user.max_hp;
        //        GameManager.instance.NpcsHP[1] = user.hp;
        //        GameManager.instance.NpcsMaxMP[1] = user.max_mp;
        //        GameManager.instance.NpcsMP[1] = user.mp;
        //        GameManager.instance.NpcsATK[1] = user.atk;
        //        GameManager.instance.NpcsDEF[1] = user.def;
        //        GameManager.instance.NpcsSPD[1] = user.spd;
        //        GameManager.instance.NpcsREC[1] = user.rec;
        //        if (user.hp <= 0)
        //        {
        //            GameManager.instance.isNpcsDie[1] = true;
        //        }
        //    }
        //}

        //// targetがPlayerの時
        //if (target.CompareTag("Player"))
        //{
        //    //
        //    GameManager.instance.pMaxHP = target.max_hp;
        //    GameManager.instance.pHP = target.hp;
        //    GameManager.instance.pMaxMP = target.max_mp;
        //    GameManager.instance.pMP = target.mp;
        //    GameManager.instance.pATK = target.atk;
        //    GameManager.instance.pDEF = target.def;
        //    GameManager.instance.pSPD = target.spd;
        //    GameManager.instance.pREC = target.rec;
        //}
        //else if (target.CompareTag("PartyMember"))
        //{
        //    // Archerの時
        //    if (target.Name == "Archer")
        //    {
        //        GameManager.instance.NpcsMaxHP[0] = target.max_hp;
        //        GameManager.instance.NpcsHP[0] = target.hp;
        //        GameManager.instance.NpcsMaxMP[0] = target.max_mp;
        //        GameManager.instance.NpcsMP[0] = target.mp;
        //        GameManager.instance.NpcsATK[0] = target.atk;
        //        GameManager.instance.NpcsDEF[0] = target.def;
        //        GameManager.instance.NpcsSPD[0] = target.spd;
        //        GameManager.instance.NpcsREC[0] = target.rec;
        //        if (target.hp <= 0)
        //        {
        //            GameManager.instance.isNpcsDie[0] = true;
        //        }
        //    }
        //    // Warriorの時
        //    else if (target.Name == "Warrior")
        //    {
        //        GameManager.instance.NpcsMaxHP[1] = target.max_hp;
        //        GameManager.instance.NpcsHP[1] = target.hp;
        //        GameManager.instance.NpcsMaxMP[1] = target.max_mp;
        //        GameManager.instance.NpcsMP[1] = target.mp;
        //        GameManager.instance.NpcsATK[1] = target.atk;
        //        GameManager.instance.NpcsDEF[1] = target.def;
        //        GameManager.instance.NpcsSPD[1] = target.spd;
        //        GameManager.instance.NpcsREC[1] = target.rec;
        //        if (target.hp <= 0)
        //        {
        //            GameManager.instance.isNpcsDie[1] = true;
        //        }
        //    }
        //}

    
    }
}
