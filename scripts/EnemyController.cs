using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    // inspector
    [Header("移動範囲")] public float moveDist = 5;
    [Header("移動スピード")] public float speed = 4;
    [Header("Battler")] public GameObject battler;



    // private val
    private bool isDiscovered = false; // プレイヤを見つけたかどうか
    private Animator anim = null;
    private Vector3 initPos; // 初期位置を保持
    private Vector3 moveVector; // 動く方向
    private float xMin, xMax; //移動可能範囲のx座標
    private float yMin, yMax; //移動可能範囲のy座標
    private string targetTag = "Player";
    private Scene currentScene;
    

    private float targetDistance = -1f;

    private Dictionary<string, float> status; // ステータス



    // Start is called before the first frame update
    void Start()
    {
        // アニメーション
        anim = GetComponent<Animator>();
        battler = this.transform.Find("Battler").gameObject;
        // Sceneのセット
        currentScene = SceneManager.GetActiveScene();


        // 初期位置のセット, 移動範囲の設定
        initPos = transform.position;
        xMin = initPos.x - moveDist/2;
        xMax = initPos.x + moveDist/2;
        yMin = initPos.y - moveDist/2;
        yMax = initPos.y + moveDist/2;
        //Debug.Log("move range: " + "(" + xMin + yMin + ")" + "(" + xMax + yMax + ")");
        SetRandomVector(); // 初期moveベクトルをセットする
    }

    // Update is called once per frame
    void Update()
    {
        if (!(currentScene.name == "Battle0"))
        {
            Moving();
        }
        
    }


    

    /// <summary>
    /// ランダム行動を行う
    /// </summary>
    private void RandomMoving()
    {
        // 現在のポジション
        Vector3 currentPos = transform.position;


        // 現在のポジションが行動範囲内ならその方向のまま動く
        if (xMin < currentPos.x && currentPos.x < xMax && yMin < currentPos.y && currentPos.y < yMax)
        {
            transform.position += moveVector * Mathf.Abs(speed);

        }
        // そうでなければ新しく方向を決定する
        else
        {
            SetRandomVector();
            transform.position += moveVector * Mathf.Abs(speed);
        }

    }


    private void TargetMoving()
    {
        transform.position += moveVector * Mathf.Abs(speed);
    }

    private void Moving()
    {
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
        if (collision.CompareTag(targetTag))
        {
            Vector3 targetPos = collision.bounds.center;

            moveVector = (targetPos - transform.position);
            moveVector.Normalize();

            //target distanceを設定する
            UnityEngine.GameObject targetObj = collision.gameObject;
            targetDistance = (targetObj.transform.position - transform.position).magnitude;
        }
    }

    


    // 接触判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == playerTag)
        //{
        //    isDiscovered = true;
        //    Debug.Log("Playerが侵入しました");
        //}
        if (collision.CompareTag(targetTag))
        {
            isDiscovered = true;
            Debug.Log("Playerが侵入しました");

            // targetへの方向をmoveVectorにセットする
            SetTargetVector(collision);
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
            SetRandomVector();
        }
        
    }

}
