using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CheckTouching : MonoBehaviour
{
    [SerializeField] FadeLoader fadeLoader;

    private string targetTag = "Player";
    private bool isBattleStart = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colObj = collision.gameObject;

        if (collision.collider.tag == targetTag)
        {
            
            // GamaManagerのbattleEnemyに親オブジェクトの名前をセットする
            GameObject parent = transform.parent.gameObject;
            Enemy parent_enemy = parent.GetComponent<Enemy>();

            Player colPlayer = colObj.GetComponent<Player>();
            

            // プレイヤが何かを選択中でないならバトルスタート
            if (colPlayer.isSelectingMode)
            {
                parent_enemy.isDiscovered = false;
            }
            // 何も選択中とかでなければ
            else
            {
                //プレイヤの動きを止める
                colPlayer.isSelectingMode = true;

                string battleEnemyName = parent_enemy.Name; // 戦う敵の名前

                // ゲームマネージャーに接触した敵のセット
                // バトルかどうかのプレイヤがフィールドに戻ったらfalseにする
                GameManager.instance.touchEnemy = battleEnemyName;

                // Enemyの動きを止める
                parent_enemy.moveVector = Vector3.zero;

                // 接触した敵をGMにセット
                // 接触した敵のメモリアドレスが等しいゲームオブジェクトをFloorEnemyから削除する
                //foreach(GameObject obj in GameManager.instance.FloorEnemy)
                //{
                //    if(obj.GetInstanceID() == parent.gameObject.GetInstanceID())
                //    {
                //        Debug.Log("discover same memory adress!");
                //        obj.SetActive(false);
                //    }
                //}

                // バトル開始するので、flagを立てる
                // バトルが終わったらBattleManagerでfalseにする
                //GameManager.instance.isBattleStart = true;
                //Debug.Log($"battleEnemyName: {battleEnemyName}");

                // GameManagerを呼び出して各変数をセットする
                // 闘う敵をセット
                if (parent_enemy.CompareTag("Boss"))
                {
                    // ボスバトルフラグを立てる
                    GameManager.instance.isBossBattle = true;

                    // バトルエネミーをクリアする
                    GameManager.instance.battleEnemies.Clear();
                    GameManager.instance.battleEnemies.Add(string.Copy(GameManager.instance.touchEnemy));
                    Debug.Log("bossTouch" + GameManager.instance.touchEnemy);
                }
                else
                {
                    // ボスバトルフラグを下ろす
                    GameManager.instance.isBossBattle = false;

                    // バトルエネミーをクリアする
                    GameManager.instance.battleEnemies.Clear();
                    GameManager.instance.SetBattleEnemies();
                }
                

                //味方のセット
                GameManager.instance.partyMember.Clear();
                GameManager.instance.SetPartyMember();

                //バトルシーン移動前のシーンを保持する
                //GameManager.instance.nowScene = SceneManager.GetActiveScene().name;

                // GameManagerのFloorからバトルシーンになる場合のみflag isFloor2Battleをtrueにする
                //if (GameManager.instance.nowScene == "Floor")
                //{
                //    GameManager.instance.isFloor2Battle = true;
                //}



                //battle start
                // GMの再スポーンさせるかを管理するflagのリストを書き換える
                Debug.Log("parent_enemy.generatedIndex:" + parent_enemy.generatedIndex);
                GameManager.instance.isBattledFloorEnemy[parent_enemy.generatedIndex] = false;
                // バトル開始のフラグture
                GameManager.instance.isBattle = true;

                string battleScene = $"Battle{GameManager.instance.floorIndex}";
                fadeLoader.FadeLoadScene(battleScene);


                //parent.gameObject.SetActive(false);
                //SceneManager.LoadScene(battleScene); //シーンの切り替え
                

                //BattleManagerを呼び出す
                //GameObject battleManagerObj = (GameObject)Resources.Load("BattleManager");
                //Instantiate(battleManagerObj, Vector3.zero, Quaternion.identity);
                //BattleManager battleManager = battleManagerObj.GetComponent<BattleManager>();

            }


            
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject parent = transform.parent.gameObject;
        Enemy parent_enemy = parent.GetComponent<Enemy>();
        parent_enemy.moveVector = Vector3.zero;
    }

    public bool IsBattleStart()
    {
        return isBattleStart;
    }
}
