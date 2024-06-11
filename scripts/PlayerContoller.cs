using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoller : MonoBehaviour
{
    // インスタンスで変更する
    [Header("キャラクタ名前")] public string charaName = "Thief";
    [Header("移動spd")] public int moveSpeed = 5;
    [Header("Battler")] public GameObject battler;

    // private val
    private Animator anim = null; // 移動時のアニメーター
    private BoxCollider2D col = null;
    private Dictionary<string, float> status; // ステータス



    // Start is called before the first frame update
    void Start()
    {
        // get component
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        battler = this.transform.Find("Battler").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        moveCharacter();
    }

    private void moveCharacter()
    {
        // 入力キーを受け取る
        float horizontalKey = Input.GetAxis("Horizontal");
        float verticalKey = Input.GetAxis("Vertical");

        // 動く方向を獲得
        Vector3 direction = getMoveDirection(horizontalKey, verticalKey);

        // アニメーション
        updateAnimation(direction);

        // 水平方向への移動       
        transform.position += new Vector3(horizontalKey, 0f, 0f) * moveSpeed * Time.deltaTime;

        // 垂直方向への移動
        transform.position += new Vector3(0f, verticalKey, 0f) * moveSpeed * Time.deltaTime;

        // 
    }

    private Vector3 getMoveDirection(float xKey, float yKey)
    {
        Vector3 directioin = new Vector3(xKey, yKey, 0);
        //Debug.Log(directioin.x + directioin.y);
        return directioin;
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
            }else if(direction.x < 0)
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
}
