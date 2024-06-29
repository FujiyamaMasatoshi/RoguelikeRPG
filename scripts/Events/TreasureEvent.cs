using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureEvent : MonoBehaviour
{
    [Header("イベント画面")] public GameObject EWindow = null;
    [Header("メッセージ画面")] public GameObject MWindow = null;
    [Header("プレイアブルキャラクタ")] public GameObject playableCharacter;
    [Header("味方NPC")] public GameObject [] NPCs;
    [Header("イベントで入手可能なスキル")] public SO_Commands[] eventCommands;

    public GameObject parentObj = null;

    private EventWindow eventWindow = null;
    private MessageWindow messageWindow = null;
    private Player player = null;
    public bool isSelectingMode = false;

    private bool isOnePush = false;

    // フロアに置かれたトレジャーのうち、何番目かを表すインデックス
    public int floorIndex = -1;

    // イベントの内容
    public void treasureEvent()
    {
        player = playableCharacter.GetComponent<Player>();
        // ########################
        // イベントの割合
        // * 仲間0人の時
        // 仲間ゲット 60%
        // スキルゲット 20%
        // ステータスアップ 20% (スキル数が6になっていたら40%とする)
        // * 仲間を1人ゲットしていたら
        // 仲間ゲット 40%
        // スキルゲット 30%
        // ステータスアップ 30% (スキル数が6になっていたら60%とする)
        // * 仲間全員ゲット
        // スキルゲット 60%
        // ステータスアップ 40% (スキル数が6になっていたら100%とする)
        // ##########################
        int n_patymember = 0;
        foreach(bool b in GameManager.instance.isParty)
        {
            if (b)
            {
                n_patymember += 1;
            }
        }
        // partymember数によって切り替え
        int randomInt = Random.Range(0, 100);

        // Thiefだけの時
        // * 仲間0人の時
        // 仲間ゲット 60%
        // スキルゲット 20%
        // ステータスアップ 20% (スキル数が6になっていたら40%とする)
        if (n_patymember == 1)
        {
            // コマンドを6個以上持っていた時
            if(player.commands.Count >= 6)
            {
                // 仲間ゲット確率
                if (0 <= randomInt && randomInt < 60)
                {
                    GetPartyMember();
                }
                // スキルゲット確率は0で残りをステータスアップ40%
                else
                {
                    StatusUp();
                }
            }
            // コマンドが6個以下の時
            else
            {
                // 仲間ゲット確率
                if (0 <= randomInt && randomInt < 60)
                {
                    GetPartyMember();
                }
                else if (60 <= randomInt && randomInt < 80)
                {
                    GetSkill();
                }
                // スキルゲット確率は0で残りをステータスアップ40%
                else
                {
                    StatusUp();
                }
            }
        }
        // Thief +1の時
        // * 仲間を1人ゲットしていたら
        // 仲間ゲット 50%
        // スキルゲット 25%
        // ステータスアップ 25% (スキル数が6になっていたら50%とする)
        else if (n_patymember == 2)
        {
            // コマンドを6個以上持っていた時
            if (player.commands.Count >= 6)
            {
                // 仲間ゲット確率
                if (0 <= randomInt && randomInt < 50)
                {
                    GetPartyMember();
                }
                // スキルゲット確率は0で残りをステータスアップ40%
                else
                {
                    StatusUp();
                }
            }
            // コマンドが6個以下の時
            else
            {
                // 仲間ゲット確率
                if (0 <= randomInt && randomInt < 50)
                {
                    GetPartyMember();
                }
                else if (50 <= randomInt && randomInt < 75)
                {
                    GetSkill();
                }
                // スキルゲット確率は0で残りをステータスアップ40%
                else
                {
                    StatusUp();
                }
            }
        }
        // Thief +2の時
        // * 仲間全員ゲット
        // スキルゲット 60%
        // ステータスアップ 40% (スキル数が6になっていたら100%とする)
        else if (n_patymember == 3)
        {
            // コマンドを6個以上持っていた時
            if (player.commands.Count >= 6)
            {
                StatusUp();
            }
            // コマンドが6個以下の時
            else
            {
                if(0 <= randomInt && randomInt < 60)
                {
                    GetSkill();
                }
                else
                {
                    StatusUp();
                }
            }
        }
        else
        {
            StatusUp();
        }
    }


    // ##############################
    // 各種イベント
    // ##############################

    public void GetPartyMember()
    {
        // ##########################
        // 味方を増やす
        // ##########################
        // Archerを加える
        if (!GameManager.instance.isParty[1])
        {
            GameManager.instance.isParty[1] = true;
            NPCs[0].SetActive(true); // 0番目がArcher

            // ステータスセット
            GameManager.instance.NpcsMaxHP[0] = GameManager.instance.initNpcsMaxHP[0];
            GameManager.instance.NpcsHP[0] = GameManager.instance.initNpcsMaxHP[0];
            GameManager.instance.NpcsMaxMP[0] = GameManager.instance.initNpcsMaxMP[0];
            GameManager.instance.NpcsMP[0] = GameManager.instance.initNpcsMaxMP[0];
            GameManager.instance.NpcsATK[0] = GameManager.instance.initNpcsATK[0];
            GameManager.instance.NpcsDEF[0] = GameManager.instance.initNpcsDEF[0];
            GameManager.instance.NpcsSPD[0] = GameManager.instance.initNpcsSPD[0];
            GameManager.instance.NpcsREC[0] = GameManager.instance.initNpcsREC[0];

            Character p = NPCs[0].GetComponent<Character>();
            p.max_hp = GameManager.instance.NpcsMaxHP[0];
            p.hp = p.max_hp;
            p.max_mp = GameManager.instance.NpcsMaxMP[0];
            p.mp = p.max_mp;
            p.atk = GameManager.instance.NpcsATK[0];
            p.def = GameManager.instance.NpcsDEF[0];
            p.spd = GameManager.instance.NpcsSPD[0];
            p.rec = GameManager.instance.NpcsREC[0];
            GameManager.instance.isNpcsDie[0] = false;


            // EvetnWindowをactive falseにする
            EWindow.SetActive(false);

            // MessageWindowにテキスト設置
            MWindow.SetActive(true);
            //messageWindow.SetMessage($"{NPCs[0].name} is joined !!");
            messageWindow.SetMessage($"{NPCs[0].name} が 仲間に加わった!!");

        }
        // Warriorを加える
        else if (!GameManager.instance.isParty[2])
        {
            GameManager.instance.isParty[2] = true;
            NPCs[1].SetActive(true); // 1番目がWarrior

            // ステータスセット
            GameManager.instance.NpcsMaxHP[1] = GameManager.instance.initNpcsMaxHP[1];
            GameManager.instance.NpcsHP[1] = GameManager.instance.initNpcsMaxHP[1];
            GameManager.instance.NpcsMaxMP[1] = GameManager.instance.initNpcsMaxMP[1];
            GameManager.instance.NpcsMP[1] = GameManager.instance.initNpcsMaxMP[1];
            GameManager.instance.NpcsATK[1] = GameManager.instance.initNpcsATK[1];
            GameManager.instance.NpcsDEF[1] = GameManager.instance.initNpcsDEF[1];
            GameManager.instance.NpcsSPD[1] = GameManager.instance.initNpcsSPD[1];
            GameManager.instance.NpcsREC[1] = GameManager.instance.initNpcsREC[1];
            Character p = NPCs[1].GetComponent<Character>();
            p.max_hp = GameManager.instance.initNpcsMaxHP[1];
            p.hp = p.max_hp;
            p.max_mp = GameManager.instance.initNpcsMaxMP[1];
            p.mp = p.max_mp;
            p.atk = GameManager.instance.initNpcsATK[1];
            p.def = GameManager.instance.initNpcsDEF[1];
            p.spd = GameManager.instance.initNpcsSPD[1];
            p.rec = GameManager.instance.initNpcsREC[1];
            GameManager.instance.isNpcsDie[1] = false;


            // EvetnWindowをactive falseにする
            EWindow.SetActive(false);

            // MessageWindowにテキスト設置
            MWindow.SetActive(true);
            //messageWindow.SetMessage($"{NPCs[1].name} is joined !!");
            messageWindow.SetMessage($"{NPCs[1].name}  が 仲間に加わった!!");
        }
    }

    // プレイヤ(Thief)のスキル数が6以上の時は呼び出されない
    // ランダムにスキルをゲットし、スキルがThiefがすでに持っていたものならば、
    // ステータスアップとする
    public void GetSkill()
    {
        SO_Commands getSkill = null;
        bool isAlradyHas = false;

        // ランダムなスキル
        int randomInt = Random.Range(0, eventCommands.Length);
        getSkill = eventCommands[randomInt];

        // Thiefのスキルを確認する
        foreach(SO_Commands cmd in player.commands)
        {
            if (getSkill.Name == cmd.Name)
            {
                isAlradyHas = true;
                break;
            }
        }

        // すでに持っていたら、ステータスアップを呼び出す
        if (isAlradyHas)
        {
            Debug.Log("Player already has the Skill. got status up event");
            StatusUp();
        }
        else
        {
            GameManager.instance.pCommands.Add(getSkill);
            // EvetnWindowをactive falseにする
            EWindow.SetActive(false);

            // MessageWindowにテキスト設置
            MWindow.SetActive(true);
            //messageWindow.SetMessage($"Skill Get: {getSkill.Name}");
            messageWindow.SetMessage($"スキルゲット: {getSkill.Name}");

        }



        

        

        
    }


    public void StatusUp()
    {
        int randomInt = Random.Range(0, 3); //0: HPup, 1: MPup, 2; Otherup
        // #######################################################
        // ステータス向上させる isParty == trueのキャラクタ全てに適応
        // #######################################################
        if(randomInt == 0)
        {
            // HP上限を上げて、回復
            for (int i = 0; i < GameManager.instance.isParty.Length; i++)
            {
                if (GameManager.instance.isParty[i])
                {
                    // Thiefの時
                    if (i == 0)
                    {
                        GameManager.instance.pMaxHP += 50;
                        GameManager.instance.pHP = GameManager.instance.pMaxHP;
                    }
                    // Archerの時
                    else if (i == 1)
                    {
                        GameManager.instance.NpcsMaxHP[0] += 50;
                        GameManager.instance.NpcsHP[0] = GameManager.instance.NpcsMaxHP[0];
                    }
                    // Warriorの時
                    else if (i == 2)
                    {
                        GameManager.instance.NpcsMaxHP[1] += 50;
                        GameManager.instance.NpcsHP[1] = GameManager.instance.NpcsMaxHP[1];
                    }
                }
            }
            // EvetnWindowをactive falseにする
            EWindow.SetActive(false);


            // MessageWindowにテキスト設置
            MWindow.SetActive(true);
            //messageWindow.SetMessage("All Characters'HP UP !!");
            messageWindow.SetMessage("全員の HP が上がった !!");
        }
        else if(randomInt == 1)
        {
            // MP上限を上げて、回復
            for (int i = 0; i < GameManager.instance.isParty.Length; i++)
            {
                if (GameManager.instance.isParty[i])
                {
                    // Thiefの時
                    if (i == 0)
                    {
                        GameManager.instance.pMaxMP += 10;
                        GameManager.instance.pMP = GameManager.instance.pMaxMP;
                    }
                    // Archerの時
                    else if (i == 1)
                    {
                        GameManager.instance.NpcsMaxMP[0] += 15;
                        GameManager.instance.NpcsMP[0] = GameManager.instance.NpcsMaxMP[0];
                    }
                    // Warriorの時
                    else if (i == 2)
                    {
                        GameManager.instance.NpcsMaxMP[1] += 5;
                        GameManager.instance.NpcsMP[1] = GameManager.instance.NpcsMaxMP[1];
                    }
                }
            }
            // EvetnWindowをactive falseにする
            EWindow.SetActive(false);

            // MessageWindowにテキスト設置
            MWindow.SetActive(true);
            //messageWindow.SetMessage("All Characters'MP UP !!");
            messageWindow.SetMessage("全員の MP が上がった !!");
        }
        else if(randomInt == 2)
        {
            // HP, MP以外のステータスを上昇させる
            for (int i = 0; i < GameManager.instance.isParty.Length; i++)
            {
                if (GameManager.instance.isParty[i])
                {
                    // Thiefの時
                    if (i == 0)
                    {
                        GameManager.instance.pATK += 30;
                        GameManager.instance.pDEF += 40;
                        GameManager.instance.pSPD += 15;
                        GameManager.instance.pREC += 40;
                    }
                    // Archerの時
                    else if (i == 1)
                    {
                        GameManager.instance.NpcsATK[0] += 20;
                        GameManager.instance.NpcsDEF[0] += 50;
                        GameManager.instance.NpcsSPD[0] += 20;
                        GameManager.instance.NpcsREC[0] += 30;

                    }
                    // Warriorの時
                    else if (i == 2)
                    {
                        GameManager.instance.NpcsATK[1] += 50;
                        GameManager.instance.NpcsDEF[1] += 50;
                        GameManager.instance.NpcsSPD[1] += 5;
                        GameManager.instance.NpcsREC[1] += 0;
                    }
                }
            }
            // EvetnWindowをactive falseにする
            EWindow.SetActive(false);

            // MessageWindowにテキスト設置
            MWindow.SetActive(true);
            //messageWindow.SetMessage("All Characters' {ATK, DEF, SPD, REC} UP !!");
            messageWindow.SetMessage("全員の 攻撃力 守備力 などが上がった!!");
        }

        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // envetWindow ゲット
        eventWindow = EWindow.GetComponent<EventWindow>();
        EWindow.SetActive(false);

        // messageWindow ゲット
        messageWindow = MWindow.GetComponent<MessageWindow>();
        MWindow.SetActive(false);

        // プレイアブルキャラクタ
        player = playableCharacter.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // Treasureイベントが開始した時
        if (isSelectingMode)
        {
            EWindow.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space) && !isOnePush)
            {
                if (eventWindow.isyes)
                {
                    //　イベント開始
                    Debug.Log("Event Start");
                    treasureEvent();

                    // イベント終了後、GMのisOpenedTreasureとインデックスが等しい場所をtrueにする
                    GameManager.instance.isOpenedTreasure[floorIndex] = true;

                    isOnePush = true;
                    

                    ////親オブジェクトを非アクティブにする
                    //parentObj.SetActive(false);

                    //// プレイヤが動き出せるようにする
                    //player.isSelectingMode = false;
                }
                else if (eventWindow.isno)
                {
                    // プレイヤが動き出せるようにする
                    player.isSelectingMode = false;

                    // SMWindowを非アクティブにする
                    isSelectingMode = false;
                    EWindow.SetActive(false);
                }
            }

            // すでに1回 return key を推していて、さらにもう一回押した時
            else if (Input.GetKeyDown(KeyCode.Space) && isOnePush)
            {
                //親オブジェクトを非アクティブにする
                parentObj.SetActive(false);

                // プレイヤが動き出せるようにする
                player.isSelectingMode = false;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("call treasure scripts");
            eventWindow.YesPanel.Select();

            // 宝箱を開くか確認するパネルを表示
            isSelectingMode = true;

            // プレイヤの動きを止める
            player.isSelectingMode = true;
        }
    }
}
