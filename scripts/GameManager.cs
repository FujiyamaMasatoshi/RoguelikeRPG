using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // インスタンス
    public static GameManager instance = null;

    // ゲーム開始時のメッセージの表示
    public bool isFirstMessage = false; // FirstEventでtrueにする
    // 最初のイベントが行われているかどうか
    public bool isEventDoing = true; // Event中かどうか 

    public string touchEnemy = ""; // 接触したエネミー
    public bool isBattle = false; //バトルをしていたかどうか(enemyに触れたらtrue, フィールドに戻ってプレイヤをspawnさせたらfalseにする)
    public int init_n_enemy = 10; // GenerateMapした時の初期の敵数
    private int n_now_enemy = 10; // 現在のエネミーの数--倒したら減らしていく

    public List<string> battleEnemies = new List<string>(); // バトルで使用するエネミーのstring Nameを保持する
    public int n_battleEnemys = 2; // バトルシーンで登場させるエネミーの数

    //public List<string> partyMember = new List<string>(); // パーティーメンバ
    public List<GameObject> partyMember = new List<GameObject>(); // 味方キャラクタ

    // 選択可能なスキルコマンド
    public List<SO_Commands> AllSkillSet = new List<SO_Commands>();



    // バトル中のダメージやコマンドエフェクトなどのアニメーションが実行されているかどうか
    public bool isBattleAnimation = false;


    // バトルウィンドウ
    //[Header("バトルマネージャー")] public BattleManager battleManager;

    // ###########################################
    // 敵キャラクタEnemysの情報を保持する
    // ###########################################
    //private string[] enemies = { "AngryPig", "Bee", "BlueBird", "Bunny" };
    //private string[] bossEnemies = { "BossPig", "Bossny" };
    //public string[] party = { "Thief", "Archer", "Warrior" };

    public List<GameObject> enemys = new List<GameObject>(); // 敵キャラクタ
    public List<GameObject> bossEnemys = new List<GameObject>(); // ボスキャラクタ

    // #######################################
    // * 味方キャラクタaliesのステータスなどの情報
    // * alies, nowAliesStatus, initAliesStatusの
    //   添え字は同キャラクタと一致
    // #######################################
    // プレイアブルキャラクタの保持スキル
    public List<SO_Commands> initCommands = new List<SO_Commands>(); // 初期スキル
    public List<SO_Commands> pCommands = new List<SO_Commands>(); // 使用可能スキル
    public List<GameObject> pItems = new List<GameObject>(); // プレイヤの保持するitem
    public int needBurstMode = 1; // 必殺技を打つのに必要なburstモードのキャラクタの数
    public SO_Cmd_SpecialSkill specialSkill = null; // 必殺技スキル
    public int max_item = 6; // itemの最大数
    public List<GameObject> alies = new List<GameObject>(); // 味方キャラクター
    public List<Dictionary<string, int>> nowAliesStatus = new List<Dictionary<string, int>>(); // aliesの現在のステータス -> InitGameで初期化常に更新し続ける
    public List<Dictionary<string, int>> initAliesStatus = new List<Dictionary<string, int>>(); //aliesのステータスの初期値
    public List<bool> isAliesDead = new List<bool>(); // 味方キャラクタが死んでいるかどうか

    public List<bool> isParty = new List<bool>(); // InitGameでThiefをtrue, 他をfalseに初期化する
    //public bool[] isParty = { true, false, false };  

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
    public float statusUpRate = 1.2f; // バトル終了後各ステータスがstatusUpRate倍になる


    // ゲームのリスタートの時ゲーム状態全てをリセットする
    public void InitGame() 
    {
        // Titleシーンならば 最初のイベントをもう一回実行させる
        if (SceneManager.GetActiveScene().name == "Title")
        {
            isFirstMessage = false;
            isEventDoing = true;
        }
        else
        {
            isEventDoing = false;
        }
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
        //isParty[0] = true;
        //isParty[1] = false;
        //isParty[2] = false;
        for (int i=0; i<alies.Count; i++)
        {
            // Thiefのみtrueにして他はfalseにする
            if (alies[i].name == "Thief") isParty[i] = true;
            else isParty[i] = false;
        }

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

        InitGame();
    }

    // enemiesに含まれているenemyをランダムにその名前を返す
    public GameObject GetRandomEnemy()
    {
        // enemiesからランダムに1つの文字列(enemy.name)を返す
        //int randomInt = Random.Range(0, enemies.Length);
        int randomInt = Random.Range(0, enemys.Count);
        //return enemies[randomInt];
        return enemys[randomInt];
    }

    public GameObject GetRandomBossEnemy()
    {
        // enemiesからランダムに1つの文字列(enemy.name)を返す
        //int randomInt = Random.Range(0, bossEnemies.Length);
        int randomInt = Random.Range(0, bossEnemys.Count);

        //return bossEnemies[randomInt];
        return bossEnemys[randomInt];
    }

    // キャラクタの添え字を受け取り、nowAliesStatusの値をinitAliesStatusに一致させる
    public void SyncStatus(int i, bool hp=true, bool mp=true, bool atk=false, bool def=false, bool spd=false, bool rec=false)
    {
        if (hp)
        {
            nowAliesStatus[i]["max_hp"] = initAliesStatus[i]["max_hp"];
            nowAliesStatus[i]["hp"] = initAliesStatus[i]["max_hp"];
        }
        if (mp)
        {
            nowAliesStatus[i]["max_mp"] = initAliesStatus[i]["max_mp"];
            nowAliesStatus[i]["mp"] = initAliesStatus[i]["max_mp"];
        }
        if (atk) nowAliesStatus[i]["atk"] = initAliesStatus[i]["atk"];
        if (def) nowAliesStatus[i]["def"] = initAliesStatus[i]["def"];
        if (spd) nowAliesStatus[i]["spd"] = initAliesStatus[i]["spd"];
        if (rec) nowAliesStatus[i]["rec"] = initAliesStatus[i]["rec"];
    }

    // プレイヤの初期ステータスをセットする関数(Awakeで呼び出す)
    public void SetInitStatus()
    {
        // PlayableCharacterのpCommandsをクリアする
        pCommands.Clear();

        // これまでのnowAliesStatus, initAliesStatus, isAliesDeadをクリアする
        nowAliesStatus.Clear();
        initAliesStatus.Clear();
        isAliesDead.Clear();

        // 1. 初期値の設定
        // 2. 現在のステータスに反映
        // 3. isAliesDeadを設定
        foreach (GameObject obj in alies)
        {
            Character c = obj.GetComponent<Character>();
            Dictionary<string, int> data = new Dictionary<string, int>();
            data["max_hp"] = c.max_hp;
            data["hp"] = c.max_hp;
            data["max_mp"] = c.max_mp;
            data["mp"] = c.max_mp;
            data["atk"] = c.atk;
            data["def"] = c.def;
            data["spd"] = c.spd;
            data["rec"] = c.rec;

            //1. 初期値の設定
            nowAliesStatus.Add(data);
            // 2. 現在のステータスに設定
            initAliesStatus.Add(data);
            // 3. isAliesDeadを設定
            isAliesDead.Add(false);
        }

       

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
        InitGame();


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

    // バトル終了後、nowAliesStatusを、更新し、パワーアップした分の情報(string)で返す
    // Characterの添え字(aliesでの)を受け取る
    public string StatusUp(int i)
    {
        // hp
        int dMaxHP = (int)(statusUpRate * (float)nowAliesStatus[i]["max_hp"] - (float)nowAliesStatus[i]["max_hp"]);
        nowAliesStatus[i]["max_hp"] += dMaxHP;
        nowAliesStatus[i]["hp"] += dMaxHP;
        // mp
        int dMaxMP = (int)(statusUpRate * (float)nowAliesStatus[i]["max_mp"] - (float)nowAliesStatus[i]["max_mp"]);
        nowAliesStatus[i]["max_mp"] += dMaxMP;
        nowAliesStatus[i]["mp"] += dMaxMP;
        // atk
        int dAtk = (int)(statusUpRate * (float)nowAliesStatus[i]["atk"] - (float)nowAliesStatus[i]["atk"]);
        nowAliesStatus[i]["atk"] += dAtk;
        // def
        int dDef = (int)(statusUpRate * (float)nowAliesStatus[i]["def"] - (float)nowAliesStatus[i]["def"]);
        nowAliesStatus[i]["def"] += dDef;
        // spd
        int dSpd = (int)(statusUpRate * (float)nowAliesStatus[i]["spd"] - (float)nowAliesStatus[i]["spd"]);
        nowAliesStatus[i]["spd"] += dSpd;
        // rec
        int dRec = (int)(statusUpRate * (float)nowAliesStatus[i]["rec"] - (float)nowAliesStatus[i]["rec"]);
        nowAliesStatus[i]["rec"] += dRec;

        string message = $"HP: +{dMaxHP}, MP: +{dMaxMP}, 攻撃力: +{dAtk}, \n守備力: +{dDef}, 素早さ: +{dSpd}, 回復量: +{dRec}";
        return message;
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

    //public string[] GetParty()
    //{
    //    return party;
    //}

    public void SetPartyMember()
    {
        //int n_party = party.Length;
        //for (int i = 0; i < n_party; i++)
        //{
        //    if (isParty[i])
        //    {
        //        partyMember.Add(party[i]);
        //    }

        //}
        for (int i=0; i<alies.Count; i++)
        {
            if (isParty[i]) partyMember.Add(alies[i]);
        }
    }


    public void SetBattleEnemies()
    {
        // n_battleEnemys以下の敵キャラクタ数を設定
        int n_enemy = Random.Range(1, n_battleEnemys+1);
        for (int i=0; i<n_enemy; i++)
        {
            battleEnemies.Add(string.Copy(touchEnemy));
        }
    }
    
}
