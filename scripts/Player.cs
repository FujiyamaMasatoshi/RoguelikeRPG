using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : Character
{
    // private val
    private Animator anim = null; // 移動時のアニメーター
    private SpriteRenderer spriteRenderer; //スプライト

    private int experience = 0;
    public bool isPlayable = false;

    public Vector3 moveVector;
    public Vector3 beforeVector = new Vector3(0f, 0f, 0f);
    private int frameCount = 240;
    private int count = 0;

    public bool isIdle = false;
    public string keyPress = "";
    public bool isSelectingMode = false; // ゲームモード選択中かどうか


    [Header("ストレスobj")]public GameObject stressObj = null; // ストレスobj
    public bool isGeneratedStressObj = false;


    // Start is called before the first frame update
    void Start()
    {
        // スプライト取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        // シーンによるセット
        string activeScene = SceneManager.GetActiveScene().name;
        if(activeScene == "level1_0")
        {
            // level1_0に戻ってきた時はステータスをリセット
            GameManager.instance.SetInitStatus();
        }
        // プレイアブルなら
        if (transform.gameObject.CompareTag("Player"))
        {
            
            Debug.Log("thisObj name is " + Name);
            // ステータスのセット
            // GameManagerから値を変更する
            max_hp = GameManager.instance.pMaxHP;
            hp = GameManager.instance.pHP;
            max_mp = GameManager.instance.pMaxMP;
            mp = GameManager.instance.pMP;
            atk = GameManager.instance.pATK;
            def = GameManager.instance.pDEF;
            spd = GameManager.instance.pSPD;
            rec = GameManager.instance.pREC;
            commands.Clear();
            foreach(SO_Commands cmd in GameManager.instance.pCommands)
            {
                commands.Add(cmd);
            }
        }
        // NPCなら
        if (transform.gameObject.CompareTag("PartyMember"))
        {
            // GMからステータスをセットする
            if(Name == "Archer")
            {
                max_hp = GameManager.instance.NpcsMaxHP[0];
                hp = GameManager.instance.NpcsHP[0];
                max_mp = GameManager.instance.NpcsMaxMP[0];
                mp = GameManager.instance.NpcsMP[0];
                atk = GameManager.instance.NpcsATK[0];
                def = GameManager.instance.NpcsDEF[0];
                spd = GameManager.instance.NpcsSPD[0];
                rec = GameManager.instance.NpcsREC[0];
                if (hp <= 0)
                {
                    GameManager.instance.isNpcsDie[0] = true;
                }
            }
            else if(Name == "Warior")
            {
                max_hp = GameManager.instance.NpcsMaxHP[1];
                hp = GameManager.instance.NpcsHP[1];
                max_mp = GameManager.instance.NpcsMaxMP[1];
                mp = GameManager.instance.NpcsMP[1];
                atk = GameManager.instance.NpcsATK[1];
                def = GameManager.instance.NpcsDEF[1];
                spd = GameManager.instance.NpcsSPD[1];
                rec = GameManager.instance.NpcsREC[1];
                if (hp <= 0)
                {
                    GameManager.instance.isNpcsDie[1] = true;
                }
            }

            // GameManager.isPartyから引数を受け取り、自身がfalseの時、active falseにする
            // 死んでいる場合は、active falseにする
            if (Name == "Archer")
            {
                // 死んでいた場合 false
                if (GameManager.instance.isNpcsDie[0])
                {
                    transform.gameObject.SetActive(false);
                }
                //　死んでいない場合 -> GM.isParty trueならばactive trueにする
                else
                {
                    for (int i = 0; i < GameManager.instance.party.Length; i++)
                    {
                        if (transform.gameObject.name == GameManager.instance.party[i])
                        {
                            if (GameManager.instance.isParty[i])
                            {
                                transform.gameObject.SetActive(true);
                            }
                            else
                            {
                                transform.gameObject.SetActive(false);
                            }
                        }
                    }
                }
                
            }
            else if(Name == "Warrior")
            {
                // 死んでいた場合 false
                if (GameManager.instance.isNpcsDie[1])
                {
                    transform.gameObject.SetActive(false);
                }
                //　死んでいない場合 -> GM.isParty trueならばactive trueにする
                else
                {
                    for (int i = 0; i < GameManager.instance.party.Length; i++)
                    {
                        if (transform.gameObject.name == GameManager.instance.party[i])
                        {
                            if (GameManager.instance.isParty[i])
                            {
                                transform.gameObject.SetActive(true);
                            }
                            else
                            {
                                transform.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
        


        // get component
        anim = GetComponent<Animator>();

        // GameManagerのplayableCharacterにセットする
        if (isPlayable)
        {
            GameManager.instance.playableCharacter = this;
        }

        // スポーンさせる場所
        // バトル中
        if (activeScene == $"Battle{GameManager.instance.floorIndex}")
        {
            // バトル用のポジションにセット
        }
        // level1_0の時
        else if(activeScene == "level1_0")
        {
            transform.position = Vector3.zero;
        }
        // それ以外
        else
        {
            // シーン切り替えの時の初期の設定
            if (GameManager.instance.isNextScene || GameManager.instance.isGoBackFloorStart)
            {
                //transform.position = new Vector3(0f, 0f, 0f);
                //GameManager.instance.isNextScene = false; // flagのリセット
            }
            //// FloorStartに戻る場合
            //else if (GameManager.instance.isGoBackFloorStart)
            //{
            //    GameManager.instance.isGoBackFloorStart = false;
            //}
            // GameManagerからバトルシーン前の状態にポジションを戻す
            else if (GameManager.instance.isBattle)
            {
                if (!GameManager.instance.isNextScene || !GameManager.instance.isFloorStart)
                {
                    Debug.Log("call this");
                    transform.position = GameManager.instance.beforeBattleScenePos;
                }
                //GameManager.instance.isBattle = false;
                
            }
        }

        // GameManagerにバトル以外のシーンを記憶させる
        if (activeScene != $"Battle{GameManager.instance.floorIndex}")
        {
            GameManager.instance.nowScene = activeScene;
        }


    }

    // Update is called once per frame
    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"isPlayable: {isPlayable}");
        // バトルシーン
        if (currentScene == $"Battle{GameManager.instance.floorIndex}")
        {
            // ストレス状態ならば、ストレスマークを表示させる
            if (isFullFrustrated && !isGeneratedStressObj)
            {
                // ストレスマークを動的に生成する
                GameObject ef = (GameObject)Resources.Load("BurstEffect");
                stressObj = Instantiate<GameObject>(ef, transform);
                stressObj.transform.position = transform.position;
                stressObj.transform.localRotation = Quaternion.identity;
                //stressObj.transform.SetParent();

                stressObj.SetActive(true);

                //フラグを立てる
                isGeneratedStressObj = true;

            }
            if(n_turn >= max_turn)
            {

                //フラグを立てる
                isGeneratedStressObj = false;
                Destroy(stressObj);
                //stressObj.SetActive(false);
            }
            //else
            //{
            //    //フラグをfalseに戻す
            //    isGeneratedStressObj = false;

            //    //Destroy(stressObj, 0f);
            //    stressObj.SetActive(false);
            //}
        }
        // バトルシーン以外では、
        else
        {
            // プレイアブルキャラクタのバトル以外のポジションをつねに保持する
            if (isPlayable)
            {
                // ポジションセット
                Debug.Log("save before position");
                GameManager.instance.beforeBattleScenePos = transform.position;
                
                // 何かを選択中でないならば、
                if (!isSelectingMode)
                {
                    moveCharacter();
                }
                // 何かを選択中 or メニュー画面を開いている時
                else
                {
                    isIdle = true;
                    // ストップアニメーション
                    moveVector = Vector3.zero;
                    updateAnimation(moveVector);
                }
                
            }
            else
            {
                moveFollowPlayer();
            }
        }
        
        
    }

    private void SetBeforeVector()
    {
        
        count++;
        if(frameCount != 0)
        {
            if(count % frameCount == 0)
            {
                //前回の動きの方向をセット
                beforeVector = moveVector;
                count = 0; // カウントのリセット
            }
        }
        
    }

    private void moveCharacter()
    {
        // 入力キーを受け取る
        //float horizontalKey = Input.GetAxis("Horizontal");
        //float verticalKey = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.UpArrow))
        {
            isIdle = false;
            moveVector = Vector3.up * stepSize;
            keyPress = "up";
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            isIdle = false;
            moveVector = Vector3.down * stepSize;
            keyPress = "down";
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            isIdle = false;
            moveVector = Vector3.left * stepSize;
            keyPress = "left";
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            isIdle = false;
            moveVector = Vector3.right * stepSize;
            keyPress = "right";
        }
        else
        {
            moveVector = Vector3.zero;
            isIdle = true;
        }
        // 動く方向を獲得
        
        //moveVector = new Vector3(horizontalKey, verticalKey, 0);

        // アニメーション
        updateAnimation(moveVector);

        //移動
        transform.position += moveVector * moveSpeed * Time.deltaTime;
        //Debug.Log($"moveVector (x, y): {moveVector.x}, {moveVector.y}");
    }

    // Playableキャラクタに追従して動く
    private void moveFollowPlayer()
    {
        //GameManagerからプレイアブルのpositionをゲットしてくる
        if (GameManager.instance.playableCharacter != null)
        {
            // プレイアブルが動いているなら
            if (GameManager.instance.playableCharacter.isIdle == false)
            {
                Vector3 playablePos = GameManager.instance.playableCharacter.transform.position;
                // playableの動く方向


                // 動く方向を獲得
                Vector3 movector = Vector3.zero;
                // 自分のキャラクタpartyの何番目に設定されているかを表す設定
                int index_partycharacter = 0;
                if(this.Name == "Warrior")
                {
                    index_partycharacter = 2;
                }else if(this.Name == "Archer")
                {
                    index_partycharacter = 1;
                }
                if(GameManager.instance.playableCharacter.keyPress == "up")
                {
                    playablePos.y -= 1f * (float)index_partycharacter;
                    movector = (playablePos - transform.position);
                }else if (GameManager.instance.playableCharacter.keyPress == "down")
                {
                    playablePos.y += 1f * (float)index_partycharacter;
                    movector = (playablePos - transform.position);
                }
                else if (GameManager.instance.playableCharacter.keyPress == "left")
                {
                    playablePos.x += 1f * (float)index_partycharacter;
                    movector = (playablePos - transform.position);
                }
                else if (GameManager.instance.playableCharacter.keyPress == "right")
                {
                    playablePos.x -= 1f * (float)index_partycharacter;
                    movector = (playablePos - transform.position);
                }
                // アニメーション
                updateAnimation(GameManager.instance.playableCharacter.moveVector);

                //Debug.Log($"moveVector: {moveVector}");
                transform.position += movector * moveSpeed * Time.deltaTime;
            }
            // プレイアブルがidle状態なら
            else
            {
                if(this.Name == "Archer")
                {
                    Vector3 playableScale = GameManager.instance.playableCharacter.transform.localScale;

                    // 左向いているなら
                    if (playableScale.x > 0)
                    {
                        transform.position = new Vector3(GameManager.instance.playableCharacter.transform.position.x + 1.0f, GameManager.instance.playableCharacter.transform.position.y, 0);
                        // アニメーション
                        updateAnimation(GameManager.instance.playableCharacter.moveVector);
                    }
                    // 右を向いているなら
                    else
                    {
                        transform.position = new Vector3(GameManager.instance.playableCharacter.transform.position.x - 1.0f, GameManager.instance.playableCharacter.transform.position.y, 0);
                        // アニメーション
                        updateAnimation(GameManager.instance.playableCharacter.moveVector);
                    }
                }else if(this.Name == "Warrior")
                {
                    Vector3 playableScale = GameManager.instance.playableCharacter.transform.localScale;

                    // 左向いているなら
                    if (playableScale.x > 0)
                    {
                        transform.position = new Vector3(GameManager.instance.playableCharacter.transform.position.x + 2.0f, GameManager.instance.playableCharacter.transform.position.y, 0);
                        // アニメーション
                        updateAnimation(GameManager.instance.playableCharacter.moveVector);
                    }
                    // 右を向いているなら
                    else
                    {
                        transform.position = new Vector3(GameManager.instance.playableCharacter.transform.position.x - 2.0f, GameManager.instance.playableCharacter.transform.position.y, 0);
                        // アニメーション
                        updateAnimation(GameManager.instance.playableCharacter.moveVector);
                    }
                }
                
            }
            
        }
        else
        {
            transform.position += new Vector3(0f, 0f, 0f);
        }
    }


    private void updateAnimation(Vector3 direction)
    {
        if (anim)
        {
            // character表示
            if (direction.x > 0)
            {
                // get player scale
                float xScale = Mathf.Abs(transform.localScale.x);
                float yScale = Mathf.Abs(transform.localScale.y);

                transform.localScale = new Vector3(-xScale, yScale, 1);
            }
            else if (direction.x < 0)
            {
                // get player scale
                float xScale = Mathf.Abs(transform.localScale.x);
                float yScale = Mathf.Abs(transform.localScale.y);

                transform.localScale = new Vector3(xScale, yScale, 1);
            }
            // animation set
            anim.SetInteger("WalkX", direction.x < 0 ? -1 : direction.x > 0 ? 1 : 0);
            anim.SetInteger("WalkY", direction.y < 0 ? 1 : direction.y > 0 ? -1 : 0);

        }
    }


    public void AddExperience(int exp)
    {
        experience += exp;
    }

    public int GetExperience()
    {
        return experience;
    }

    // playableを設定する
    public void SetIsPlayable(bool b)
    {
        isPlayable = b;
    }


    // プレイヤのダメージ表現
    public void TakeDamage()
    {
        GameManager.instance.isBattleAnimation = true;
        StartCoroutine(Blink());
        // falseに戻すのは、Blink関数の最後
        
    }
    // スプライトを点滅させる
    public float blinkDuration = 0.1f;
    public int blinkCount = 4;

    public IEnumerator Blink()
    {
        for(int i=0; i<blinkCount; i++)
        {
            // スプライトの透明度を0にする
            SetSpriteAlpha(0f);
            // blinkDuration秒待機
            yield return new WaitForSeconds(blinkDuration);
            // スプライトの透明度を1に戻す
            SetSpriteAlpha(1f);
            // blinkDuration秒待機
            yield return new WaitForSeconds(blinkDuration);
        }
        GameManager.instance.isBattleAnimation = false;
    }

    // スプライトの透明度を設定するメソッド
    private void SetSpriteAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
