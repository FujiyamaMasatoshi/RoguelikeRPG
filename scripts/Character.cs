using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//public class CharacterData : MonoBehaviour
//{
//    public int unique_value;
//}

public class Character : MonoBehaviour
{
    // info
    [Header("名前")] public string Name;
    [Header("移動spd")] public float moveSpeed = 5;
    [Header("step size")] public float stepSize = 0.1f;

    // ステータス
    [Header("スタータス(HP)")] public int hp = 100;
    [Header("スタータス(MaxHP)")] public int max_hp = 100;
    [Header("スタータス(MP)")] public int mp = 10;
    [Header("スタータス(MaxMP)")] public int max_mp = 10;
    [Header("スタータス(ATK)")] public int atk = 100;
    [Header("スタータス(DEF)")] public int def = 100;
    [Header("スタータス(SPD)")] public int spd = 50;
    [Header("スタータス(REC)")] public int rec = 80;

    // ##############
    // BurstMode
    // ###############
    // frustration値が100を超えるとMP消費せずに技を打てるようになる
    public int frustration = 0;
    public int max_frustration = 30;
    // frustrationモードかどうか
    public bool isFullFrustrated = false;
    // 一回だけ表示させるためのflag, burstMode終了したらフラグを下げる
    public bool isSetMessageBurstMode = false;
    public int max_turn = 2; //最大継続ターン
    public int n_turn = 0; //現在の継続ターン

    // ここは固定

    [Header("敵or味方")] public bool isEnemy;

    //選択可能なコマンド
    [Header("選択可能コマンド")] public List<SO_Commands> commands;

    public List<SO_Commands> userableCommands = new List<SO_Commands>();

    // コマンド選択、ターゲット選択
    public SO_Commands selectCmd;
    public Character target;

    //キャラクタデータ
    //CharacterData data = null;

    // 行動結果を保持するstring result_action
    private string result_action = "";




    private void Start()
    {
        //SetNowStatus();
    }


    // 現在のmp値から使用可能なコマンドのみをセットする
    public void SetUserableCommands()
    {
        userableCommands.Clear();
        foreach(SO_Commands cmd in commands)
        {
            // isFullFrustratedならば、全て追加する
            if (isFullFrustrated)
            {
                userableCommands.Add(cmd);
            }
            else
            {
                // 現在のMPがコマンドに必要なMPよりも少ないならば、userableCommandsに追加しない
                if (mp < cmd.mp)
                {
                    // 追加しない
                }
                // そうでなければ
                else
                {
                    userableCommands.Add(cmd);
                }
            }
            
        }
    }

    

    public string GetResultAction()
    {
        return result_action;
    }

    public void SetResultAction(string result)
    {
        result_action = result;
    }

    // 最大HPを返す
    public int get_max_hp()
    {
        return max_hp;
    }

    // 最大MPを返す
    public int get_max_mp()
    {
        return max_mp;
    }

    //public void SetNowStatus()
    //{
    //    Dictionary<string, int> characterDict = GameManager.instance.characterData[Name];
    //    hp = characterDict["hp"];
    //    mp = characterDict["mp"];
    //    atk = characterDict["atk"];
    //    def = characterDict["def"];
    //    spd = characterDict["spd"];
    //    rec = characterDict["rec"];
    //}

    public void SetInitStatus()
    {
        hp = max_hp;
        mp = max_mp;
    }

    public void SelectCommand()
    {
        // userableコマンドをセットする
        SetUserableCommands();

        // ランダムにコマンドを選択
        int randomIndex = Random.Range(0, userableCommands.Count);

        selectCmd = commands[randomIndex];
        //selectCmd = userableCommands[0]; //Attack

    }

    /// <summary>
    /// 引数はバトルに含まれている全てのキャラクタを保持しているList
    /// </summary>
    /// <param name="character_list"></param>
    public void SelectTarget(List<Character> character_list)
    {
        // 相手を選択する時
        int randomIndex = Random.Range(0, character_list.Count);
        target = character_list[randomIndex];
    }
}
