using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [SerializeField] FadeLoader fadeLoaderGameOver;
    [SerializeField] FadeLoader fadeLoaderBackFloor;

    [Header("CommandWindow")] public GameObject CMDWindow = null; //コマンドウィンドウObj
    [Header("BattleWindow")] public GameObject BWindow = null; // バトルウィンドウobj
    [Header("TargetWindow")] public GameObject TWindow = null; // targerウィンドウobj
    [Header("MessageWindow")] public GameObject MWindow = null; // メッセージウィンドウobj

    [Header("味方キャラクタ")] public List<Character> partyMember = new List<Character>();
    [Header("BGM")] public AudioSource bgm;

    private CommandWindow commandWindow = null; // CommandWindow
    private BattleWindow battleWindow = null;
    private TargetWindow targetWindow = null;
    private MessageWindow messageWindow = null;
    private Character nowCharacter; // 行動キャラクタ
    private bool returnkeyPressed = false;
    
    private bool isbattle = false;
    private bool istool = false;
    private bool isrun = false;

    private int turn = 0;

    // バトル終了のflag
    private bool isFinish = false;

    // ゲームステート
    enum GameState
    {
        GAME_START, // バトル開始
        TURN_START, // ターン開始
        ORDER, // 行動順序決定
        POP, //行動キャラクタの決定
        SELECT_COMMAND, // コマンド決定
        EXE, // 実行
        RESULT, // 結果
        TURN_END, // ターンエンド
        FINISH // ゲームエンド
    }

    // スキルエフェクト
    public List<GameObject> skillEffects = new List<GameObject>();

    // ゲームステートを管理する変数
    GameState game_state;

    // 登場するキャラクタリスト
    List<Character> characters = new List<Character>();
    // エネミーを保持するリスト
    List<Character> enemy_list = new List<Character>();
    // 味方メンバーを保持するリスト
    List<Character> party_list = new List<Character>();
    // 生き残っているキャラクタを保持するリスト
    List<Character> alived_list = new List<Character>();
    // 死んでいるキャラクタを保持するリスト
    List<Character> dead_list = new List<Character>();

    //生き残っている味方
    List<Character> alived_party = new List<Character>();
    //死んでいる味方
    List<Character> dead_party = new List<Character>();

    //生き残っている敵
    List<Character> alived_enemy = new List<Character>();

    //死んでいる敵
    List<Character> dead_enemy = new List<Character>();

    // 行動順序順にキャラクタを保持するリスト
    List<Character> order_list = new List<Character>();


    //// キャラクタデータを保持するList
    //List<CharacterData> characterDatas = new List<CharacterData>();

    //// コマンド
    //SO_Commands selectedCommand = null;

    // SEとEffectを表現させる
    public void PlaySE(AudioClip se)
    {
        GameManager.instance.isBattleAnimation = true;
        bgm.PlayOneShot(se);
        GameManager.instance.isBattleAnimation = false;

        // エフェクト中のフラグを下ろす
        GameManager.instance.isEffecting = false;
        //GameManager.instance.effect = null;
        //Destroy(GameManager.instance.effect);

    }

    //public void PlaySEandEffect(SO_Commands cmd)
    //{
    //    GameManager.instance.isBattleAnimation = true;
    //    // エフェクトをactiveにする
    //    Instantiate(cmd.effect, nowCharacter.transform.position, Quaternion.identity);
    //    //cmd.effect.SetActive(true);
    //    //cmd.effect.transform.position = nowCharacter.transform.position;
    //    // seを鳴らす
    //    bgm.PlayOneShot(cmd.se);
    //    GameManager.instance.isBattleAnimation = false;

    //    // エフェクト中のフラグを下ろす
    //    GameManager.instance.isEffecting = false;

    //    //effectをactive falseにする
    //    //cmd.effect.SetActive(false);
    //}

    // Start is called before the first frame update
    void Start()
    {

        // CommandWindowを設定する
        commandWindow = CMDWindow.GetComponent<CommandWindow>();
        if(commandWindow == null)
        {
            Debug.Log("コマンドパネルが設定されていません");
        }
        CMDWindow.SetActive(false);

        // BattleWindowを設定する
        battleWindow = BWindow.GetComponent<BattleWindow>();
        BWindow.SetActive(false);

        // TargetWindowを設定する
        targetWindow = TWindow.GetComponent<TargetWindow>();
        TWindow.SetActive(false);

        //MessageWindowを設定
        messageWindow = MWindow.GetComponent<MessageWindow>();
        messageWindow.InitMessage();
        MWindow.SetActive(true);

        // ####################################
        // prefab化してあるcharacterを召喚する
        // ####################################

        //味方の用意
        //Debug.Log($"GameManager.instance.GetPartyMember(): {GameManager.instance.partyMember}, { GameManager.instance.partyMember.Count}");
        //for (int i = 0; i < GameManager.instance.partyMember.Count; i++)
        ////for (int i = 0; i < 1; i++)
        //{
        //    // キャラクタの名前
        //    string partyChara_name = GameManager.instance.partyMember[i];
        //    //Debug.Log($"{i}: partyChara_name: {partyChara_name}");

        //    // キャラクタ召喚
        //    GameObject partyChara = (GameObject)Resources.Load(partyChara_name);
        //    Instantiate(partyChara);
        //    // get component
        //    Character c = partyChara.GetComponent<Character>();
        //    // set position
        //    c.gameObject.transform.position = new Vector3(0.0f + (float)i * 2, -3.0f, 0.0f);
        //    //Debug.Log($"battlescene, player position: {c.transform.position}");

        //    // character Listに追加
        //    characters.Add(c);
        //    party_list.Add(c);
        //    alived_list.Add(c);
        //    alived_party.Add(c);
        //}

        // #################
        // 味方の用意
        // #################
        // キャラクタを登場させる前にactive falseにして、登場できるキャラクのみactive trueにする
        foreach(Character c in partyMember)
        {
            c.gameObject.SetActive(false);
        }
        // active trueに設定していく
        for(int i=0; i<GameManager.instance.partyMember.Count; i++)
        {
            // キャラクタの名前
            string partyChara_name = GameManager.instance.partyMember[i];
            // GM.partyMemberにいる場合のみacitve trueにする
            foreach(Character c in partyMember)
            {
                if(partyChara_name == c.Name)
                {
                    c.gameObject.SetActive(true);

                    // character Listに追加
                    characters.Add(c);
                    party_list.Add(c);
                    alived_list.Add(c);
                    alived_party.Add(c);

                    // キャラクタのステータスセット
                    // コマンドはselectCMDの時に呼ばれるのでセットしない
                    // 初期ステータスのみセットする
                    if(c.Name == "Thief")
                    {
                        // 
                        c.max_hp = GameManager.instance.pMaxHP;
                        c.hp = GameManager.instance.pHP;
                        c.max_mp = GameManager.instance.pMaxMP;
                        c.mp = GameManager.instance.pMP;
                        c.atk = GameManager.instance.pATK;
                        c.def = GameManager.instance.pDEF;
                        c.spd = GameManager.instance.pSPD;
                        c.rec = GameManager.instance.pREC;
                    }else if(c.Name == "Archer")
                    {
                        //
                        c.max_hp = GameManager.instance.NpcsMaxHP[0];
                        c.hp = GameManager.instance.NpcsHP[0];
                        c.max_mp = GameManager.instance.NpcsMaxMP[0];
                        c.mp = GameManager.instance.NpcsMP[0];
                        c.atk = GameManager.instance.NpcsATK[0];
                        c.def = GameManager.instance.NpcsDEF[0];
                        c.spd = GameManager.instance.NpcsSPD[0];
                        c.rec = GameManager.instance.NpcsREC[0];
                    }else if(c.Name == "Warrior")
                    {
                        //
                        c.max_hp = GameManager.instance.NpcsMaxHP[1];
                        c.hp = GameManager.instance.NpcsHP[1];
                        c.max_mp = GameManager.instance.NpcsMaxMP[1];
                        c.mp = GameManager.instance.NpcsMP[1];
                        c.atk = GameManager.instance.NpcsATK[1];
                        c.def = GameManager.instance.NpcsDEF[1];
                        c.spd = GameManager.instance.NpcsSPD[1];
                        c.rec = GameManager.instance.NpcsREC[1];
                    }
                }
            }
            
        }

        

        ////確認
        //for (int i = 0; i < party_list.Count; i++)
        //{
        //    //Debug.Log(i + "hashcode: " + party_list[i].gameObject.GetHashCode());
        //    //    CharacterData data = enemy_list[i].GetComponent<CharacterData>();
        //    //    Debug.Log($"{i}: {enemy_list[i].Name}, {data.unique_value}");
        //}


        //敵キャラクタの用意
        for (int i = 0; i < GameManager.instance.battleEnemies.Count; i++)
        {
            // ResourcesからプレファブをLoad
            //GameObject enemy_prefab = Resources.Load<GameObject>("AngryPig");

            string enemy_name = GameManager.instance.battleEnemies[i];
            GameObject enemy_prefab = Resources.Load<GameObject>(enemy_name);

            // 動的に召喚
            GameObject enemy = Instantiate(enemy_prefab, new Vector3(0.0f + (float)i * 10, 3f, 0.0f), Quaternion.identity);
            //enemy.transform.position = new Vector3(0.0f + (float)i * 100, 3f, 0.0f);
            //Instantiate(enemy, new Vector3(0.0f + 3f * (float)i, 0.0f, 0.0f), Quaternion.identity);


            Character c_enemy = enemy.GetComponent<Character>();
            c_enemy.gameObject.transform.position = new Vector3(0.0f + (float)i * 3, 3f, 0.0f);

            //Debug.Log($"battlescene, enemy position: {c_enemy.transform.position}");
            // CharacterDataを生成して子オブジェクトとして設定する
            // 
            //Debug.Log("data is " + data);
            //data.unique_value = 10 + i;

            //data.transform.SetParent(c_enemy.gameObject.transform);

            characters.Add(c_enemy);
            enemy_list.Add(c_enemy);
            alived_list.Add(c_enemy);
            alived_enemy.Add(c_enemy);
        }

        //確認
        //for (int i = 0; i < enemy_list.Count; i++)
        //{
        //    Debug.Log(i + "hashcode: " + enemy_list[i].gameObject.GetHashCode());
            
        //}






        //ゲームステートを初期化してバトルスタート
        game_state = GameState.GAME_START;
        StartCoroutine(Battle());

    }


    // コルーチンを使ってゲーム管理
    IEnumerator Battle()
    {

        // プレイヤー
        //Character player = party_list[0]; //0番目の見方
        //player.SetInitStatus();
        //Character enemy = characters[1];
        //enemy.SetInitStatus();

        yield return null;
        while (true)
        {
            Debug.Log(game_state);
            switch (game_state)
            {
                case GameState.GAME_START:
                    // {CMD, B}Wuindow -> nonActive
                    CMDWindow.SetActive(false);
                    BWindow.SetActive(false);
                    TWindow.SetActive(false);
                    game_state = GameState.TURN_START;
                    break;

                case GameState.TURN_START:
                    turn += 1;
                    //messageWindow.SetMessage($"TURN {turn}");
                    messageWindow.SetMessage($"ターン {turn}");

                    

                    yield return new WaitUntil(() => returnkeyPressed);
                    returnkeyPressed = false;

                    // ゲームステート遷移
                    game_state = GameState.ORDER;
                    break;

                case GameState.ORDER:
                    // 行動順序を決定する
                    // order_listを空にする
                    order_list.Clear();

                    //alived_listをorder_listにコピーする
                    order_list = new List<Character>(alived_list);

                    // spdで降順に設定
                    order_list.Sort((a, b) => b.spd - a.spd);

                    // ##################
                    // ストレス値の設定
                    // ##################
                    // 全てのキャラクタのisFullFrustratedがtrueのキャラクタのターンを進める
                    foreach(Character c in order_list)
                    {
                        if (c.isFullFrustrated)
                        {
                            if(c.n_turn <= c.max_turn)
                            {
                                c.n_turn += 1;
                            }
                            else
                            {
                                c.isFullFrustrated = false;
                                c.n_turn = 0;
                            }
                            
                        }
                    }

                    //確認
                    //Debug.Log($"order_list.Count: {order_list.Count}");
                    //for (int i=0; i<order_list.Count; i++)
                    //{
                    //    Character c = order_list[i];
                    //    Debug.Log($"{i}: {c.Name}: {c.spd}");
                    //}



                    game_state = GameState.POP;
                    break;

                case GameState.POP:
                    // now characterの設定を行う
                    // order_listの先頭の要素をnowCharacterに設定する

                    // order_listの先頭のキャラクタを行動キャラクタに設定
                    nowCharacter = order_list[0];

                    

                    //nowCharacter = player;

                    // 先頭のキャラクタを削除
                    order_list.RemoveAt(0);

                    //確認
                    //Debug.Log($"POP: order_list.Count: {order_list.Count}");
                    //for (int i = 0; i < order_list.Count; i++)
                    //{
                    //    Character c = order_list[i];
                    //    Debug.Log($"{i}: {c.Name}: {c.spd}");
                    //}

                    // 行動キャラクタをメッセージウィンドウに表示する
                    //messageWindow.SetMessage($"{nowCharacter.Name}'s turn\n");
                    messageWindow.SetMessage($"{nowCharacter.Name} の ターン\n");

                    yield return new WaitUntil(() => returnkeyPressed);
                    returnkeyPressed = false;

                    game_state = GameState.SELECT_COMMAND;
                    break;

                case GameState.SELECT_COMMAND:
                    // messageWindowに何したら良いかメッセージセットする
                    //messageWindow.AddMessage("select your action command\n");
                    messageWindow.AddMessage("スキルを選択してください\n");

                    // 行動キャラクタが味方キャラクタなら
                    if (!nowCharacter.isEnemy)
                    {
                        Debug.Log("now_character:" + nowCharacter);
                        // CMDWindowをアクティブか
                        CMDWindow.SetActive(true);
                        commandWindow.InitPanel();
                        commandWindow.BattlePanel.Select(); // 最初のセレクト
                        // コマンド選択

                        // コマンド決定
                        yield return new WaitUntil(() => returnkeyPressed);
                        returnkeyPressed = false;


                        if (commandWindow.IsBattle())
                        {
                            isbattle = true;
                            istool = false;
                            isrun = false;
                        }
                        else if (commandWindow.IsTool())
                        {
                            isbattle = false;
                            istool = true;
                            isrun = false;
                        }
                        else if (commandWindow.IsRun())
                        {

                            isbattle = false;
                            istool = false;
                            isrun = true;
                        }

                        if (isrun)
                        {
                            int randomInt = Random.Range(0, 100);
                            if(randomInt < 60 && GameManager.instance.isBossBattle == false)
                            {
                                Debug.Log("go finish state with isrun = true");
                                game_state = GameState.FINISH;
                                break;
                            }
                            else
                            {
                                CMDWindow.SetActive(false);
                                Debug.Log("you cannot run this boss battle");

                                //messageWindow.SetMessage("You could not run this Battle\n");
                                messageWindow.SetMessage("このバトルから逃げられなかった\n");

                                // order_listから味方キャラクタ全てを排除する
                                for (int i=0; i< order_list.Count; i++)
                                {
                                    // 味方キャラクタなら
                                    if (order_list[i].isEnemy == false)
                                    {
                                        order_list.RemoveAt(i);
                                    }
                                }

                                

                                yield return new WaitUntil(() => returnkeyPressed);
                                returnkeyPressed = false;
                                // 残りのorderlistの中身をみる
                                if (order_list.Count <= 0)
                                {
                                    game_state = GameState.TURN_END;
                                    break;
                                }
                                else
                                {
                                    game_state = GameState.POP;
                                    break;
                                }

                                
                            }
                            
                        }

                        // ウィンドウの切り替え
                        CMDWindow.SetActive(false);


                        // battleWindowの初期化
                        battleWindow.InitActionText();
                        battleWindow.SetActionTexts(nowCharacter);

                        // バトルウィンドウのアクティブ化
                        BWindow.SetActive(true);

                        // 最初の選択
                        battleWindow.SelectActiveText_0();


                        // 待機
                        yield return new WaitUntil(() => returnkeyPressed);
                        returnkeyPressed = false;

                        int action_num = battleWindow.GetSelectTextIndex();
                        Debug.Log($"action_num : {action_num}");


                        //// playerの行動
                        nowCharacter.selectCmd = nowCharacter.userableCommands[action_num];

                        //Debug.Log("after select action");

                        // ウィンドウ切り替え


                        // {CMD. B}Window非アクティブ
                        CMDWindow.SetActive(false);
                        BWindow.SetActive(false);

                        // #######################
                        // ターゲットウィンドウの設定
                        // #######################
                        // messageWindowに何をすべきか表示させる

                        // 選択したアクションattがattack or debufなら敵を選択
                        if((nowCharacter.selectCmd.att == "attack" || nowCharacter.selectCmd.att == "debuf") && nowCharacter.selectCmd.forOneCharacter)
                        {
                            // messageWindowに何をすべきか表示させる
                            //messageWindow.SetMessage("Select the enemy to execute the action.\n");
                            messageWindow.SetMessage("ターゲットを選択してください\n");

                            targetWindow.InitTargetText();
                            Debug.Log("alived_enemy.Count: " + alived_enemy.Count);
                            targetWindow.SetTargetText(alived_enemy);

                            // targetWindowをアクティブに
                            TWindow.SetActive(true);
                            //最初の選択
                            targetWindow.SelectActiveText_0();

                            yield return new WaitUntil(() => returnkeyPressed);
                            returnkeyPressed = false;

                            int target_num = targetWindow.GetSelectTargetIndex();
                            

                            //Debug.Log($"target_num: {target_num}");
                            nowCharacter.target = alived_enemy[target_num];


                            // ターゲットウィンドウを非アクティブ
                            TWindow.SetActive(false);
                        }
                        // アクションattがheal or bufならば味方を選択
                        else if ((nowCharacter.selectCmd.att == "heal" || nowCharacter.selectCmd.att == "buf") && nowCharacter.selectCmd.forOneCharacter)
                        {
                            // messageWindowに何をすべきか表示させる
                            //messageWindow.SetMessage("Select the ally to execute the action.\n");
                            messageWindow.SetMessage("ターゲットを選択してください\n");
                            targetWindow.InitTargetText();

                            Debug.Log("alived_party.Count: " + alived_party.Count);
                            targetWindow.SetTargetText(alived_party);

                            // targetWindowをアクティブに
                            TWindow.SetActive(true);
                            //最初の選択
                            targetWindow.SelectActiveText_0();

                            yield return new WaitUntil(() => returnkeyPressed);
                            returnkeyPressed = false;

                            int target_num = targetWindow.GetSelectTargetIndex();
                            

                            //Debug.Log($"target_num: {target_num}");
                            nowCharacter.target = alived_party[target_num];


                            // ターゲットウィンドウを非アクティブ
                            TWindow.SetActive(false);
                        }



                        

                    }
                    // #################
                    // 敵の行動なら
                    // ################
                    else
                    {
                        // 敵がThiefたちを選択する時
                        nowCharacter.SelectCommand();
                        if ((nowCharacter.selectCmd.att == "attack" || nowCharacter.selectCmd.att == "debuf") && nowCharacter.selectCmd.forOneCharacter)
                        {
                            nowCharacter.SelectTarget(alived_party);
                        }
                        // 敵が敵を選択する時
                        else if ((nowCharacter.selectCmd.att == "buf" || nowCharacter.selectCmd.att == "heal") && nowCharacter.selectCmd.forOneCharacter)
                        {
                            nowCharacter.SelectTarget(alived_enemy);
                        }

                        // english
                        //messageWindow.AddMessage($"{nowCharacter.Name} {nowCharacter.selectCmd.Name} to {nowCharacter.target.Name}\n");
                        // japanese
                        messageWindow.AddMessage($"{nowCharacter.Name} は{nowCharacter.selectCmd.Name}　を {nowCharacter.target.Name} にした\n");

                        //yield return new WaitUntil(() => returnkeyPressed);
                        //returnkeyPressed = false;
                    }


                    // ゲームステート遷移
                    game_state = GameState.EXE;
                    break;
                case GameState.EXE:

                    // 味方の行動なら
                    if (!nowCharacter.isEnemy)
                    {

                        // コマンド実行
                        nowCharacter.selectCmd.Execute(nowCharacter, nowCharacter.target);

                        // エフェクトを表示
                        skillEffects.Clear();
                        for (int i=0; i<nowCharacter.selectCmd.effect.Count; i++)
                        {
                            // userならば、userのposにインスタンティエイト
                            if(nowCharacter.selectCmd.forWho[i] == "user")
                            {
                                GameObject ef = Instantiate(nowCharacter.selectCmd.effect[i], nowCharacter.transform.position, Quaternion.identity);
                                ef.SetActive(true);
                                skillEffects.Add(ef);
                            }

                            // targetならば、targetのposにインスタンティエイト
                            else if(nowCharacter.selectCmd.forWho[i] == "target")
                            {
                                GameObject ef = Instantiate(nowCharacter.selectCmd.effect[i], nowCharacter.target.transform.position, Quaternion.identity);
                                ef.SetActive(true);
                                skillEffects.Add(ef);
                            }
                        }

                        //SEを鳴らす
                        PlaySE(nowCharacter.selectCmd.se);
                        //enemy.selectCmd.Execute(enemy, enemy.target);

                    }
                    // 敵の行動なら
                    else
                    {
                        // コマンド実行
                        nowCharacter.selectCmd.Execute(nowCharacter, nowCharacter.target);

                        //// エフェクトを表示
                        //skillEffect = Instantiate(nowCharacter.selectCmd.effect, nowCharacter.target.transform.position, Quaternion.identity);
                        //skillEffect.SetActive(true);

                        // エフェクトを表示
                        skillEffects.Clear();
                        for (int i = 0; i < nowCharacter.selectCmd.effect.Count; i++)
                        {
                            // userならば、userのposにインスタンティエイト
                            if (nowCharacter.selectCmd.forWho[i] == "user")
                            {
                                GameObject ef = Instantiate(nowCharacter.selectCmd.effect[i], nowCharacter.transform.position, Quaternion.identity);
                                ef.SetActive(true);
                                skillEffects.Add(ef);
                            }

                            // targetならば、targetのposにインスタンティエイト
                            else if (nowCharacter.selectCmd.forWho[i] == "target")
                            {
                                GameObject ef = Instantiate(nowCharacter.selectCmd.effect[i], nowCharacter.target.transform.position, Quaternion.identity);
                                ef.SetActive(true);
                                skillEffects.Add(ef);
                            }
                        }

                        //SEを鳴らす
                        PlaySE(nowCharacter.selectCmd.se);
                    }


                    yield return new WaitUntil(() => returnkeyPressed);
                    returnkeyPressed = false;

                    // スキルエフェクト active false
                    //skillEffect.SetActive(false);
                    foreach(GameObject ef in skillEffects)
                    {
                        Destroy(ef, 0f);
                    }


                    // ゲームステート遷移
                    game_state = GameState.RESULT;


                    break;

                case GameState.RESULT:
                    //yield return new WaitUntil(() => returnkeyPressed);
                    //returnkeyPressed = false;

                    messageWindow.SetMessage(nowCharacter.GetResultAction());

                    yield return new WaitUntil(() => returnkeyPressed);
                    returnkeyPressed = false;
                    // 行動の結果
                    // hpからalivedとdeadのリストを更新
                    for (int i = 0; i < characters.Count; i++)
                    {
                        Character c = characters[i];
                        if (c.hp <= 0)
                        {
                            if (c.isEnemy)
                            {
                                alived_enemy.Remove(c);
                                order_list.Remove(c);
                                
                                dead_enemy.Add(c);
                            }
                            else
                            {
                                alived_party.Remove(c);
                                order_list.Remove(c);
                                dead_party.Add(c);
                                // Thiefが死んだ場合は、すぐにゲームオーバーとする
                                if(c.Name == "Thief")
                                {
                                    // 将来的にGAMEOVER画面に遷移させる
                                    messageWindow.SetMessage("GAME OVER\n");
                                    yield return new WaitUntil(() => returnkeyPressed);
                                    returnkeyPressed = false;
                                    //SceneManager.LoadScene("GameOver");
                                    fadeLoaderGameOver.FadeLoadScene("GameOver");
                                }
                            }

                            // 画面から消す
                            // アニメーションの終了を待つ
                            c.gameObject.SetActive(false);
                            
                        }
                    }



                    yield return new WaitUntil(() => returnkeyPressed);
                    returnkeyPressed = false;

                    // ゲームステートの遷移
                    // alived_{party or enemy}が空ならFINISHに遷移
                    if (alived_enemy.Count == 0 || alived_party.Count == 0)
                    {
                        game_state = GameState.FINISH;
                    }
                    //　alived_{party or enemy} -> どちらもからではない
                    // 全てのキャラクタの行動が終わっていたら、TURN_ENDする
                    else if (order_list.Count == 0)
                    {
                        game_state = GameState.TURN_END;
                    }
                    else
                    {
                        game_state = GameState.POP;
                    }

                    break;

                case GameState.TURN_END:
                    // ゲームステートをターンスタートにする
                    game_state = GameState.TURN_START;
                    break;

                case GameState.FINISH:
                    if (isrun)
                    {
                        Debug.Log("clear1");
                        // GameMangerで管理している敵の数を1つ減らす
                        GameManager.instance.AddEnemy(-1);

                        //messageWindow.SetMessage("You Run the battle...\n");
                        messageWindow.SetMessage("バトルから逃げた...\n");

                        // 待機 
                        yield return new WaitUntil(() => returnkeyPressed);
                        returnkeyPressed = false;

                        Debug.Log("clear2");
                        isrun = false;


                        // シーン切り替え
                        fadeLoaderBackFloor.FadeLoadScene(string.Copy(GameManager.instance.nowScene));




                    }
                    else
                    {
                        // どちらかが全滅した
                        // 味方が全滅していたら、
                        if (alived_party.Count == 0)
                        {
                            // 将来的にGAMEOVER画面に遷移させる
                            messageWindow.SetMessage("GAME OVER\n");
                            yield return new WaitUntil(() => returnkeyPressed);
                            returnkeyPressed = false;
                            //SceneManager.LoadScene("GameOver");
                            fadeLoaderGameOver.FadeLoadScene("GameOver");
                        }
                        // 敵が全滅していたら
                        else if (alived_enemy.Count == 0)
                        {
                            // 敵の数を1つへらす
                            GameManager.instance.AddEnemy(-1);

                            //messageWindow.SetMessage("Finish Battle !\n");
                            messageWindow.SetMessage("バトル終了 !!\n");

                            yield return new WaitUntil(() => returnkeyPressed);
                            returnkeyPressed = false;
                            //messageWindow.SetMessage($"Your party members are power up!!\n");
                            //messageWindow.SetMessage($"全員パワーアップした !!\n");


                            //messageWindow.AddMessage($"All characters' status are up to ...\n");
                            //messageWindow.AddMessage($"HP: +50, MP: +5, ATK: +10\n DEF: +30, SPD: +15, REC: +10\n");

                            // GameManagerにステータスアップ後のステータスを登録する
                            for(int i=0; i<partyMember.Count; i++)
                            {
                                // パーティメンバーに加入しているキャラクタのみ反映
                                if (GameManager.instance.isParty[i])
                                {
                                    if (i==0)
                                    {
                                        // Thief
                                        GameManager.instance.pMaxHP += 50;
                                        GameManager.instance.pHP += 50;
                                        GameManager.instance.pMaxMP += 5;
                                        GameManager.instance.pMP += 5;
                                        GameManager.instance.pATK += 10;
                                        GameManager.instance.pDEF += 30;
                                        GameManager.instance.pSPD += 15;
                                        GameManager.instance.pREC += 10;

                                        messageWindow.SetMessage("Thief:\n");
                                        messageWindow.AddMessage($"HP: +50, MP: +5, ATK: +10\n DEF: +30, SPD: +15, REC: +10\n");

                                        yield return new WaitUntil(() => returnkeyPressed);
                                        returnkeyPressed = false;
                                    }
                                    else if (i==1)
                                    {
                                        //Archer
                                        GameManager.instance.NpcsMaxHP[0] += 30;
                                        GameManager.instance.NpcsHP[0] += 30;
                                        GameManager.instance.NpcsMaxMP[0] += 10;
                                        GameManager.instance.NpcsMP[0] += 10;
                                        GameManager.instance.NpcsATK[0] += 5;
                                        GameManager.instance.NpcsDEF[0] += 20;
                                        GameManager.instance.NpcsSPD[0] += 15;
                                        GameManager.instance.NpcsREC[0] += 15;

                                        messageWindow.SetMessage("Archer:\n");
                                        messageWindow.AddMessage($"HP: +30, MP: +10, ATK: +5\n DEF: +20, SPD: +15, REC: +15\n");

                                        yield return new WaitUntil(() => returnkeyPressed);
                                        returnkeyPressed = false;

                                    }
                                    else if (i==2)
                                    {
                                        //Warrior

                                        GameManager.instance.NpcsMaxHP[1] += 70;
                                        GameManager.instance.NpcsHP[1] += 70;
                                        GameManager.instance.NpcsMaxMP[1] += 5;
                                        GameManager.instance.NpcsMP[1] += 5;
                                        GameManager.instance.NpcsATK[1] += 15;
                                        GameManager.instance.NpcsDEF[1] += 35;
                                        GameManager.instance.NpcsSPD[1] += 1;
                                        GameManager.instance.NpcsREC[1] += 1;

                                        messageWindow.SetMessage("Warrior:\n");
                                        messageWindow.AddMessage($"HP: +70, MP: +5, ATK: +15\n DEF: +35, SPD: +1, REC: +1\n");

                                        yield return new WaitUntil(() => returnkeyPressed);
                                        returnkeyPressed = false;
                                    }
                                }
                            }


                            yield return new WaitUntil(() => returnkeyPressed);
                            returnkeyPressed = false;

                            // 敵を倒した時の処理を実行する
                            // 経験値の入手
                            // 経験値の計算
                            int exp = 0;
                            foreach (Character c in dead_enemy)
                            {
                                Enemy e = c.GetComponent<Enemy>();
                                exp += e.exp;
                            }
                            //Debug.Log($"exp: {exp}");
                            foreach (Character c in alived_party)
                            {
                                Player p = c.GetComponent<Player>();
                                p.AddExperience(exp);
                                //Debug.Log($"p.exp: {p.GetExperience()}");
                            }


                            // アイテムの獲得
                            //
                            //

                            // GameManagerの情報更新
                            GameManager.instance.battleEnemies.Clear();

                            // シーン切り替え
                            fadeLoaderBackFloor.FadeLoadScene(string.Copy(GameManager.instance.nowScene));



                        }

                    }

                    //バトル終了
                    //コルーチンから抜け出す
                    Debug.Log("clear3");
                    isFinish = true;
                    yield break;
                
            }

        }
    }

    private void Update()
    {

        // アニメーション実行中なら
        if (GameManager.instance.isBattleAnimation || GameManager.instance.isEffecting)
        {
            Debug.Log("you cannot touch keyboard");
            // 入力を受け付けない
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && commandWindow.IsAnySelected())
            {
                returnkeyPressed = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && battleWindow.IsAnySelected())
            {
                returnkeyPressed = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) && targetWindow.IsAnySelected())
            {
                returnkeyPressed = true;
            }

            if (game_state == GameState.SELECT_COMMAND && nowCharacter.isEnemy == true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    returnkeyPressed = true;
                }
            }

            if (game_state == GameState.SELECT_COMMAND && isbattle == true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    returnkeyPressed = true;
                }
            }


            if (game_state == GameState.FINISH || game_state == GameState.RESULT || game_state == GameState.TURN_START || game_state == GameState.EXE || game_state == GameState.POP)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    returnkeyPressed = true;
                }
            }

        }
    }


}
