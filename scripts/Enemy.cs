using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Enemy : Character
{
    [Header("移動範囲")] public float moveDist = 5;
    [Header("経験値")] public int exp = 10;
    //[Header("アイテム")] 

    [Header("生成されたインデックス")] public int generatedIndex = -1;

    // private val
    private SpriteRenderer spriteRenderer; //スプライト
    public bool isDiscovered = false; // プレイヤを見つけたかどうか
    private bool isDamaged = false;
    private bool isBattle = false;
    private int beforeHP = 0;
    private int currentHP = 0;

    private Animator anim = null;
    private Vector3 initPos; // 初期位置を保持
    public Vector3 moveVector; // 動く方向
    private float xMin, xMax; //移動可能範囲のx座標
    private float yMin, yMax; //移動可能範囲のy座標
    private string targetTag = "Player";
    private Scene currentScene;

    // timer
    private float timer = 0f;
    // 立ち止まるtimer
    private float stopTimer = 0f;
    // ジャンプようのタイマー
    private float jumpTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // スプライト取得
        spriteRenderer = GetComponent<SpriteRenderer>();

        // アニメーション
        anim = GetComponent<Animator>();


        // Sceneのセット
        currentScene = SceneManager.GetActiveScene();

        // hpのセット
        beforeHP = hp;
        currentHP = hp;

        // 初期位置のセット, 移動範囲の設定
        initPos = transform.position;
        xMin = initPos.x - moveDist / 2;
        xMax = initPos.x + moveDist / 2;
        yMin = initPos.y - moveDist / 2;
        yMax = initPos.y + moveDist / 2;
        //Debug.Log("move range: " + "(" + xMin + yMin + ")" + "(" + xMax + yMax + ")");
        SetRandomVector(); // 初期moveベクトルをセットする
    }

    // Update is called once per frame
    void Update()
    {
        // Sceneのセット
        currentScene = SceneManager.GetActiveScene();

        // バトルシーンなら
        if (currentScene.name == $"Battle{GameManager.instance.floorIndex}")
        {
            
        }
        // フィールドなら
        else
        {
            if (transform.gameObject.CompareTag("Boss"))
            {
                //何もしない
            }
            else
            {
                

                if(Name == "AngryPig")
                {
                    // 追いかけ始めて3秒経ったら、
                    if (timer >= 3f)
                    {
                        // 2秒立ち止まる
                        stopTimer += Time.deltaTime;
                        if(stopTimer >= 2f)
                        {
                            // ストップタイマーとタイマーのリセット
                            stopTimer = 0f;
                            timer = 0f;
                        }
                    }
                    else
                    {
                        Moving();
                    }
                }
                // Bunnyの時
                else if(Name == "Bunny")
                {
                    // 動いている時、縦方向にジャンプ表現を入れる
                    JumpMoving();

                }
                else if(Name == "Bee")
                {
                    Debug.Log("bee moving");
                    BeeMoving();
                }
                // 他のキャラクタの時
                else
                {
                    // 動かす
                    Moving();
                }

            }
        }

        

        // アニメーション
        SetAnimation();


    }

    // 1回だけアニメーションを再生する
    public bool AnimationOnePlay(string flag_name)
    {
        // Triggerをセット
        anim.SetTrigger(flag_name);

        // 1回必ずどうさせる
        //AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        //while (state.normalizedTime < 1.0f)
        //{
        //    state = anim.GetCurrentAnimatorStateInfo(0);
        //}
        return true;
    }

    public Animator GetAnim()
    {
        return anim;
    }


    /// <summary>
    /// ランダム行動を行う
    /// </summary>
    private bool isBackToInitPos = false; // 最初の場所に戻っているMovingかどうか
    private void RandomMoving()
    {
        // 現在のポジション
        Vector3 currentPos = transform.position;

        
        
        // 最初の場所の近くに戻ったら、
        if (isBackToInitPos)
        {
            if (Mathf.Abs((transform.position - initPos).magnitude) < 0.1)
            {
                isBackToInitPos = false;
                SetRandomVector();
                transform.position += moveVector * Mathf.Abs(moveSpeed);
            }
            else
            {

                // そうでなければ最初の場所に戻る
                moveVector = initPos - transform.position;
                moveVector.Normalize();
                transform.position += moveVector * Mathf.Abs(moveSpeed);
            }
        }
        // ランダムに動いている時
        else
        {
            // 現在のポジションが行動範囲内ならその方向のまま動く
            if (xMin < currentPos.x && currentPos.x < xMax && yMin < currentPos.y && currentPos.y < yMax)
            {
                transform.position += moveVector * Mathf.Abs(moveSpeed);
            }
            else
            {
                isBackToInitPos = true;
                // そうでなければ最初の場所に戻る
                moveVector = initPos - transform.position;
                moveVector.Normalize();
                transform.position += moveVector * Mathf.Abs(moveSpeed);
            }
        }
    }


    private void TargetMoving()
    {
        transform.position += moveVector * Mathf.Abs(moveSpeed * 2);
    }

    private void Moving()
    {
        // 動いている向きを設定
        // get scale
        float xScale = Mathf.Abs(transform.localScale.x);
        float yScale = Mathf.Abs(transform.localScale.y);
        if (moveVector.x < 0) // 左向きの時 -> そのまま
        {
            transform.localScale = new Vector3(xScale, yScale, 1);
        }
        else // 右向きの時 -> xスケールを反転させる
        {
            transform.localScale = new Vector3(-xScale, yScale, 1);
        }

        Debug.Log("isDiscovered: " + isDiscovered);
        //Debug.Log("target: " + target);
        //if (isDiscovered && target != null)
        //{
        //    TargetMoving();
        //}
        //else
        //{
        //    RandomMoving();
        //}
        if (isDiscovered)
        {
            TargetMoving();
        }
        else
        {
            RandomMoving();
        }
    }


    // Bee用
    private void BeeMoving()
    {
        //8の字を描いて動く
        // 動いている向きを設定
        // get scale
        float xScale = Mathf.Abs(transform.localScale.x);
        float yScale = Mathf.Abs(transform.localScale.y);
        if (moveVector.x < 0) // 左向きの時 -> そのまま
        {
            transform.localScale = new Vector3(xScale, yScale, 1);
        }
        else // 右向きの時 -> xスケールを反転させる
        {
            transform.localScale = new Vector3(-xScale, yScale, 1);
        }

        Debug.Log("isDiscovered: " + isDiscovered);
        //Debug.Log("target: " + target);

        

        if (isDiscovered)
        {
            //moveVectorの傾きが1より大きいならば、
            if (moveVector.y / moveVector.x > 1 && moveVector.x != 0f)
            {
                transform.position += (moveVector  + new Vector3(2*Mathf.Sin(4*jumpTimer), 0, 0)) * Mathf.Abs(moveSpeed * 2);
            }
            // 傾きが1より小さいならば、
            else
            {
                transform.position += (moveVector+ new Vector3(0, 2 * Mathf.Sin(4*jumpTimer),  0)) * Mathf.Abs(moveSpeed * 2);
            }
            jumpTimer += Time.deltaTime;

        }
        else
        {
            RandomMoving();
            jumpTimer = 0f;
        }


    }

    // Bunny用
    private void JumpMoving()
    {
        // 動いている向きを設定
        // get scale
        float xScale = Mathf.Abs(transform.localScale.x);
        float yScale = Mathf.Abs(transform.localScale.y);
        if (moveVector.x < 0) // 左向きの時 -> そのまま
        {
            transform.localScale = new Vector3(xScale, yScale, 1);
        }
        else // 右向きの時 -> xスケールを反転させる
        {
            transform.localScale = new Vector3(-xScale, yScale, 1);
        }

        Debug.Log("isDiscovered: " + isDiscovered);
        //Debug.Log("target: " + target);

        if (isDiscovered)
        {
            //TargetMoving():
            //transform.position += moveVector * Mathf.Abs(moveSpeed * 2);

            //上方向にジャンプ表下を入れる
            // deltaTimeでカウントで間隔をあけて、y方向ジャンプをsin波で表現する
            // 並行方向に移動している時は、ジャンプ要素を入れる
            if(moveVector.y/moveVector.x < 0.3f)
            {
                float jump_y = Mathf.Abs(1.5f* Mathf.Sin(6*jumpTimer));
                transform.position += (moveVector + new Vector3(0, jump_y, 0)) * Mathf.Abs(moveSpeed * 2);
                jumpTimer += Time.deltaTime;
            }
            else
            {
                transform.position += moveVector * Mathf.Abs(moveSpeed * 2);
            }
            

        }
        else
        {
            jumpTimer = 0f;
            RandomMoving();
        }
    }

    /// <summary>
    /// update内で呼び出しアニメーションをセットする
    /// </summary>
    public void SetAnimation()
    {
        // バトルシーンでは
        
        if (currentScene.name == $"Battle{GameManager.instance.floorIndex}")
        {
            isBattle = true;
            anim.SetBool("isbattle", isBattle);
            //currentHP = hp;
            //if (beforeHP != currentHP)
            //{
            //    Debug.Log($"beforeHP, currentHP: {beforeHP}, {currentHP} in if Statement");
            //    anim.Play("hit2");
            //    anim.Play("idle");

            //}
            Debug.Log($"beforeHP, currentHP: {beforeHP}, {currentHP} out of if Statement");
            beforeHP = currentHP;
            isDamaged = false;
        }
        // ゲームフィールドだけ
        else
        {
            isBattle = false;
            if (isDiscovered)
            {
                anim.SetBool("discover", true);
            }
            else
            {
                anim.SetBool("discover", false);
            }
        }
        
    }

    /// <summary>
    /// ランダムな方向ベクトルをセットする
    /// </summary>
    private void SetRandomVector()
    {
        // 動く方向を取得
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        direction.Normalize();
        moveVector = direction;
    }

    /// <summary>
    /// ターゲットへの向きをセットする
    /// </summary>
    private void SetTargetVector(Collider2D collision)
    {
        //GameObject colObj = collision.gameObject;

        if (collision.CompareTag(targetTag))
        {
            Vector3 targetPos = collision.bounds.center;
            if (Mathf.Abs((targetPos - transform.position).magnitude) < 0.1)
            {
                moveVector = Vector3.zero;
            }
            else
            {
                moveVector = (targetPos - transform.position);
                moveVector.Normalize();
            }


            //target distanceを設定する
            //UnityEngine.GameObject targetObj = collision.gameObject;
            //targetDistance = (targetObj.transform.position - transform.position).magnitude;
        }
    }

    //private void SetTargetVector(Player target)
    //{

    //    if(target == null)
    //    {
    //        //
    //    }
    //    else
    //    {
    //        Vector3 targetPos = target.transform.position;
    //        if(Mathf.Abs((targetPos - transform.position).magnitude) < 0.1)
    //        {
    //            moveVector = Vector3.zero;
    //        }
    //        else
    //        {
    //            moveVector = (targetPos - transform.position);
    //            moveVector.Normalize();
    //        }
    //    }

    //    //GameObject colObj = collision.gameObject;

    //    //if (collision.CompareTag(targetTag))
    //    //{
    //    //    Vector3 targetPos = collision.bounds.center;
    //    //    if (Mathf.Abs((targetPos - transform.position).magnitude) < 0.1)
    //    //    {
    //    //        moveVector = Vector3.zero;
    //    //    }
    //    //    else
    //    //    {
    //    //        moveVector = (targetPos - transform.position);
    //    //        moveVector.Normalize();
    //    //    }


    //        //target distanceを設定する
    //        //UnityEngine.GameObject targetObj = collision.gameObject;
    //        //targetDistance = (targetObj.transform.position - transform.position).magnitude;
    //    //}
    //}




    // 接触判定
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag(targetTag))
        {
            GameObject colObj = collision.gameObject;
            Player colPlayer = colObj.GetComponent<Player>();

            if (colPlayer.isSelectingMode)
            {
                isDiscovered = false;
                // timerのリセット
                timer = 0f;

                // ランダム方向に設定し直す
                SetRandomVector();
            }
            else
            {

                isDiscovered = true;

                // timerスタート
                timer = 0f;
                timer += Time.deltaTime;
                Debug.Log("Playerが侵入しました");

                // targetへの方向をmoveVectorにセットする
                SetTargetVector(collision);

            }
        }


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //GameObject colObj = collision.gameObject;
        if (collision.CompareTag(targetTag) && isDiscovered)
        {
            //isDiscovered = true;
            SetTargetVector(collision);

            timer += Time.deltaTime;

            //// プレイヤのゲームobjをとってくる
            //Player colPlayer = colObj.GetComponent<Player>();

            //// プレイヤが何かを選択中ならば、isDiscoverd=false
            //if (colPlayer.isSelectingMode)
            //{
            //    isDiscovered = false;
            //}
            //else
            //{

            //    //isDiscovered = true;
            //    Debug.Log("Playerが侵入しています");


            //}
            


        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.tag == playerTag)
        //{
        //    isDiscovered = false;
        //    Debug.Log("Playerを見失いました");
        //}
        if (collision.CompareTag(targetTag))
        {
            isDiscovered = false;
            Debug.Log("Playerを見失いました");
            //SetRandomVector();
        }

    }

    // エネミーのダメージ表現
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
        for (int i = 0; i < blinkCount; i++)
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
