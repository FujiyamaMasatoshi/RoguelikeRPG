using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // インスタンス
    public static GameManager instance = null;

    //[Header("バトルマネージャー")] public BattleManager battleManager = null;



    public string touchEnemy = ""; // 接触したエネミー
    //public bool isInFloor = false;
    public bool isBattle = false; //バトルをしていたかどうか(enemyに触れたらtrue, フィールドに戻ってプレイヤをspawnさせたらfalseにする)
    public int init_n_enemy = 10; // GenerateMapした時の初期の敵数
    private int n_now_enemy = 10; // 現在のエネミーの数--倒したら減らしていく

    public List<string> battleEnemies = new List<string>(); // バトルで使用するエネミーのstring Nameを保持する
    //public List<Player> NPCMembers = new List<Player>(); //NPC味方キャラクタ全て
    public List<string> partyMember = new List<string>(); // パーティーメンバ

    // 選択可能なスキルコマンド
    public List<SO_Commands> AllSkillSet = new List<SO_Commands>();

    // ################
    // 初期ステータス
    // ################
    public int initHP = 100;
    public int initMaxHP = 100;
    public int initMaxMP = 10;
    public int initMP = 10;
    public int initATK = 100;
    public int initDEF = 100;
    public int initSPD = 100;
    public int initREC = 80;
    public List<SO_Commands> initCommands = new List<SO_Commands>();
    // ################
    // プレイヤ情報
    // ################
    public int pHP = 100; // HP
    public int pMaxHP = 100; // Max HP
    public int pMP = 10; // MP
    public int pMaxMP = 10; //Max MP
    public int pDEF = 100; // DEF
    public int pATK = 100; // ATK
    public int pSPD = 100; // SPD
    public int pREC = 80; // Rec
    // 持っているスキル
    public List<SO_Commands> pCommands = new List<SO_Commands>();

    // NPC初期ステータス
    public int[] initNpcsHP = { 100, 150 };
    public int[] initNpcsMaxHP = { 100, 150 };
    public int[] initNpcsMP = { 20, 10 };
    public int[] initNpcsMaxMP = { 20, 10 };
    public int[] initNpcsATK = { 100, 150 };
    public int[] initNpcsDEF = { 100, 100 };
    public int[] initNpcsSPD = { 90, 30 };
    public int[] initNpcsREC = { 100, 40 };
    public bool[] isInitNpcsDie = { false, false };
    // NPCの現在のステータス
    public int[] NpcsHP = { 100, 150 };
    public int[] NpcsMaxHP = { 100, 150 };
    public int[] NpcsMP = { 20, 10 };
    public int[] NpcsMaxMP = { 20, 10 };
    public int[] NpcsATK = { 100, 150 };
    public int[] NpcsDEF = { 100, 100 };
    public int[] NpcsSPD = { 90, 30 };
    public int[] NpcsREC = { 100, 40 };
    public bool[] isNpcsDie = { false, false };


    // バトル中のダメージやコマンドエフェクトなどのアニメーションが実行されているかどうか
    public bool isBattleAnimation = false;


    // バトルウィンドウ
    //[Header("バトルマネージャー")] public BattleManager battleManager;


    // ゲーム内に登場する全てのエネミーの名前を格納する
    private string[] enemies = { "AngryPig", "Bee", "BlueBird", "Bunny" };
    private string[] bossEnemies = { "BossPig", "Bossny" };
    public string[] party = { "Thief", "Archer", "Warrior" };
    public bool[] isParty = { true, false, false };

    // Battle0意外の現在のシーンを保持する
    public string nowScene = "";

    public bool isFloorStart = false; // FloorStartでゲートを通ったかどうか
    public bool isNextScene = false; //シーンを切り替えたかどうか
    public string fromCardinalDirection = ""; // 前のフロアのどの位置から移動したか(四方)
    public List<string> FloorList = new List<string>(); // 作成されたフロアを管理
    public int n_stage = 0;



    // FloorGoalからFloorStartに戻る
    public bool isGoBackFloorStart = false;

    // 操作キャラクタ
    public Player playableCharacter = null;
    // バトル前のシーンでの位置情報
    public Vector3 beforeBattleScenePos;

    //ゲーム難易度
    public string gameMode = "";

    // 塔の高さ
    public int n_floors = 3; // 塔の高さ
    public int now_floor = 0; // 現在のフロア
    public int floorIndex = 0; // フロアの添字 (フロアの雰囲気変更)

    // フロアの最初に生成された敵
    public List<string> FloorEnemy = new List<string>();
    public List<bool> isBattledFloorEnemy = new List<bool>();
    // 接触した敵
    public GameObject battleEnemy = null;
    public bool isBossBattle = false;


    // フロアに最初に置かれたTreasure
    public List<bool> isOpenedTreasure = new List<bool>();


    // バトル中のエフェクト
    public bool isEffecting = false; // エフェクト実行中かどうかのフラグ
    public GameObject effect = null; // 実行しているエフェクト


    // ゲームのリスタートの時ゲーム状態全てをリセットする
    public void InitGame() 
    {
        // キャラクタステータスのリセット
        SetInitStatus();

        touchEnemy = "";

        // ゲーム状態
        now_floor = 0;

        // フラグ等
        nowScene = "";

        isFloorStart = false; // FloorStartでゲートを通ったかどうか
        isNextScene = false; //シーンを切り替えたかどうか
        isBattle = false;
        fromCardinalDirection = ""; // 前のフロアのどの位置から移動したか(四方)
        FloorList.Clear(); // 作成されたフロアを管理
        n_stage = 0;

        // Listの初期化
        isBattledFloorEnemy.Clear();
        FloorEnemy.Clear();
        isOpenedTreasure.Clear();
        FloorList.Clear();
        battleEnemies.Clear();
        partyMember.Clear();

        // FloorGoalからFloorStartに戻る
        isGoBackFloorStart = false;

        // isParyのセットする
        isParty[0] = true;
        isParty[1] = false;
        isParty[2] = false;

    }




    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            //DontDestroyOnLoad(this.gameObject);
        }


    }

    // enemiesに含まれているenemyをランダムにその名前を返す
    public string GetRandomEnemy()
    {
        // enemiesからランダムに1つの文字列(enemy.name)を返す
        int randomInt = Random.Range(0, enemies.Length);
        return enemies[randomInt];
    }

    public string GetRandomBossEnemy()
    {
        // enemiesからランダムに1つの文字列(enemy.name)を返す
        int randomInt = Random.Range(0, bossEnemies.Length);
        return bossEnemies[randomInt];
    }

    // プレイヤの初期ステータスをセットする関数(Awakeで呼び出す)
    public void SetInitStatus()
    {
        // プレイアブル
        pHP = initHP;
        pMaxHP = initMaxHP;
        pMP = initMP;
        pMaxMP = initMaxMP;
        pATK = initATK;
        pDEF = initDEF;
        pSPD = initSPD;
        pREC = initREC;
        pCommands.Clear();
        foreach(SO_Commands cmd in initCommands)
        {
            pCommands.Add(cmd);
        }

        // NPCs
        NpcsMaxHP = initNpcsMaxHP;
        NpcsHP = initNpcsHP;
        NpcsMaxMP = initNpcsMaxMP;
        NpcsMP = initNpcsMP;
        NpcsATK = initNpcsATK;
        NpcsDEF = initNpcsDEF;
        NpcsSPD = initNpcsDEF;
        NpcsREC = initNpcsREC;
        isNpcsDie = isInitNpcsDie;

    }
    // execute after Awake()
    private void Start()
    {
        // フレームレート設定
        Application.targetFrameRate = 60; // ここに希望のフレームレートを設定
        QualitySettings.vSyncCount = 0; // vSyncを無効にしてフレームレートを手動で制御

        ////バトルマネージャーを取得
        //battleManager = GetComponent<BattleManager>();
        //battleManager.gameObject.SetActive(false);


    }

    public void MakeFloorList()
    {
        //リストをクリアする
        FloorList.Clear();

        //floorIndexgはlevel1_0で決定される

        //floorIndex = Random.Range(0, 2);
        // リストにFloorを3つ追加していく
        string floorName = $"Floor{floorIndex}";
        FloorList.Add($"{floorName}_0");
        FloorList.Add($"{floorName}_1");
        FloorList.Add($"{floorName}_2");

        // flagをリセットする
        isFloorStart = false;

        // stage数を設定する
        n_stage = FloorList.Count;

    }

    private void Update()
    {
        

        //if (isFloorStart && now_floor <= n_floors)
        //{
        //    MakeFloorList();

        //    // 最初だけGMを使ってシーン切り替えを行う
        //    string firstFloor = string.Copy(FloorList[0]);
        //    FloorList.Remove(FloorList[0]);
        //    isNextScene = true;
        //    fromCardinalDirection = "north";
        //    SceneManager.LoadScene(firstFloor);
        //}
        
    }

    // 敵の数をリセットする
    public void InitNumEnemy()
    {
        n_now_enemy = init_n_enemy;
    }


    // 現在の敵の数を返す
    public int GetNumEnemy()
    {
        return n_now_enemy　　　;
    }

    // 現在の敵を増減させる
    public void AddEnemy(int n)
    {
        // 増やす場合
        if (n > 0)
        {
            // 初期値より多くしない
            n_now_enemy = (int)Mathf.Min(init_n_enemy, n_now_enemy + n);
        }
        // 減らす場合
        else if (n < 0)
        {
            n_now_enemy = (int)Mathf.Max(0, n_now_enemy - n);
        }
        
        
    }

    public string[] GetParty()
    {
        return party;
    }

    public void SetPartyMember()
    {
        int n_party = party.Length;
        for (int i = 0; i < n_party; i++)
        {
            if (isParty[i])
            {
                partyMember.Add(party[i]);
            }
            
        }
    }


    public void SetBattleEnemies()
    {
        int n_enemy = Random.Range(1, 3);
        for (int i=0; i<n_enemy; i++)
        {
            battleEnemies.Add(string.Copy(touchEnemy));
        }
    }
    
}
