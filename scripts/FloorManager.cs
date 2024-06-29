using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloorManager : MonoBehaviour
{
    [Header("NorthSpawnPoint")] public GameObject northSpawnPoint;
    [Header("SouthSpawnPoint")] public GameObject southSpawnPoint;
    [Header("WestSpawnPoint")] public GameObject westSpawnPoint;
    [Header("EastSpawnPoint")] public GameObject eastSpawnPoint;
    [Header("プレイアブルキャラクタ")] public GameObject playableCharacter;

    [Header("Enemyスポーンポイント")] public List<GameObject> enemySpawnPoints = new List<GameObject>();
    [Header("Treasureスポーンポイント")] public List<GameObject> treasures = new List<GameObject>();

    [Header("ボス登場Pos")] public GameObject bossSpawnPoint = null;



    // Start is called before the first frame update
    void Start()
    {
        string activeScene = SceneManager.GetActiveScene().name;

        if(activeScene == $"FloorGoal{GameManager.instance.floorIndex}")
        {
            if (GameManager.instance.isBattle)
            {
                // 何も召喚しない

                // プレイアブルを元の位置に戻す
                playableCharacter.transform.position = GameManager.instance.beforeBattleScenePos;

                // Battleフラグを下ろす
                GameManager.instance.isBattle = false;
            }
            else
            {
                // プレイヤを召喚する
                if (GameManager.instance.fromCardinalDirection == "north")
                {
                    //southSpawnPointにキャラクタをspawnさせる
                    playableCharacter.transform.position = southSpawnPoint.transform.position;
                }
                else if (GameManager.instance.fromCardinalDirection == "south")
                {
                    //northSpawnPointにキャラクタをspawnさせる
                    playableCharacter.transform.position = northSpawnPoint.transform.position;
                }
                else if (GameManager.instance.fromCardinalDirection == "east")
                {
                    //westSpawnPointにキャラクタをspawnさせる
                    playableCharacter.transform.position = westSpawnPoint.transform.position;
                }
                else if (GameManager.instance.fromCardinalDirection == "west")
                {
                    //eastSpawnPointにキャラクタをspawnさせる
                    playableCharacter.transform.position = eastSpawnPoint.transform.position;
                }
                // ボスを召喚する
                string enemy_name = GameManager.instance.GetRandomBossEnemy();
                GameObject enemy_prefab = Resources.Load<GameObject>(enemy_name);

                // 動的に召喚
                GameObject enemy = Instantiate(enemy_prefab, bossSpawnPoint.transform.position, Quaternion.identity);
                // Enemy コンポーネントを取得
                Enemy e = enemy.GetComponent<Enemy>();
                e.generatedIndex = 0;
                Debug.Log($"memroyAdress: {e.gameObject.GetInstanceID()}, {e.Name}: generatedIndex={e.generatedIndex}");
            }
            
        }
        else
        {

            if (GameManager.instance.isFloorStart)
            {
                // southPointにplayerを配置する
                //southSpawnPointにキャラクタをspawnさせる
                playableCharacter.transform.position = southSpawnPoint.transform.position;
                GameManager.instance.isFloorStart = false;
            }
            else
            {
                // バトルから戻ってきたら
                if (GameManager.instance.isBattle)
                {
                    playableCharacter.transform.position = GameManager.instance.beforeBattleScenePos;

                    // ########################################################
                    // GameManagerからFloorInitEnemyを操作してエネミーを召喚する
                    // ########################################################
                    //GameManager.instance.FloorEnemy.Clear();
                    //GameManager.instance.isBattledFloorEnemy.Clear();
                    if (enemySpawnPoints.Count > 0)
                    {

                        //for (int i = 0; i < enemySpawnPoints.Count; i++)
                        //{
                        //    // GM.FloorEnemyに名前だけ追加していく
                        //    GameManager.instance.FloorEnemy.Add(string.Copy(GameManager.instance.GetRandomEnemy()));
                        //    // バトルをしたかどうかを判定する
                        //    //GameManager.instance.isBattledFloorEnemy.Add(true);
                        //}
                        for (int i = 0; i < enemySpawnPoints.Count; i++)
                        {
                            if (GameManager.instance.isBattledFloorEnemy[i])
                            {
                                string enemy_name = GameManager.instance.FloorEnemy[i];
                                GameObject enemy_prefab = Resources.Load<GameObject>(enemy_name);

                                // 動的に召喚
                                GameObject enemy = Instantiate(enemy_prefab, enemySpawnPoints[i].transform.position, Quaternion.identity);
                                // Enemy コンポーネントを取得
                                Enemy e = enemy.GetComponent<Enemy>();
                                e.generatedIndex = i;
                                Debug.Log($"memroyAdress: {e.gameObject.GetInstanceID()}, {e.Name}: generatedIndex={e.generatedIndex}, i={i}");
                            }
                            else
                            {
                                continue;
                            }

                        }
                    }

                    //########################################
                    //Treasureをactive true or falseで設定する
                    // #######################################

                    for (int i = 0; i < treasures.Count; i++)
                    {
                        if (GameManager.instance.isOpenedTreasure[i])
                        {
                            treasures[i].SetActive(false);
                        }
                        else
                        {
                            treasures[i].SetActive(true);
                        }
                    }

                    // バトルフラグを下ろす
                    GameManager.instance.isBattle = false;
                }
                // 初めてインスタンティエイトするならば、
                else
                {
                    // ##############
                    // 敵を召喚する
                    // ###############
                    // enemySpawnPointsがからでなければenemyを召喚する
                    GameManager.instance.FloorEnemy.Clear();
                    GameManager.instance.isBattledFloorEnemy.Clear();
                    if (enemySpawnPoints.Count > 0)
                    {

                        for (int i = 0; i < enemySpawnPoints.Count; i++)
                        {
                            // GM.FloorEnemyに名前だけ追加していく
                            GameManager.instance.FloorEnemy.Add(string.Copy(GameManager.instance.GetRandomEnemy()));
                            // バトルをしたかどうかを判定する
                            GameManager.instance.isBattledFloorEnemy.Add(true);
                        }
                        for (int i = 0; i < enemySpawnPoints.Count; i++)
                        {
                            string enemy_name = GameManager.instance.FloorEnemy[i];
                            GameObject enemy_prefab = Resources.Load<GameObject>(enemy_name);

                            // 動的に召喚
                            GameObject enemy = Instantiate(enemy_prefab, enemySpawnPoints[i].transform.position, Quaternion.identity);
                            // Enemy コンポーネントを取得
                            Enemy e = enemy.GetComponent<Enemy>();
                            e.generatedIndex = i;
                            Debug.Log($"memroyAdress: {e.gameObject.GetInstanceID()}, {e.Name}: generatedIndex={e.generatedIndex}, i={i}");
                        }
                    }
                    // ####################
                    // Treasureを設置する
                    // ####################
                    GameManager.instance.isOpenedTreasure.Clear();
                    for (int i = 0; i < treasures.Count; i++)
                    {
                        //未開封なのでfalseで設定
                        // 開けられたタイミングでtrueにする (TreasureEventの中でtrueにする)
                        GameManager.instance.isOpenedTreasure.Add(false);
                        treasures[i].SetActive(true);
                    }

                    // ##################
                    // 味方の場所を決定する
                    // ##################
                    if (GameManager.instance.fromCardinalDirection == "north")
                    {
                        //southSpawnPointにキャラクタをspawnさせる
                        playableCharacter.transform.position = southSpawnPoint.transform.position;
                    }
                    else if (GameManager.instance.fromCardinalDirection == "south")
                    {
                        //northSpawnPointにキャラクタをspawnさせる
                        playableCharacter.transform.position = northSpawnPoint.transform.position;
                    }
                    else if (GameManager.instance.fromCardinalDirection == "east")
                    {
                        //westSpawnPointにキャラクタをspawnさせる
                        playableCharacter.transform.position = westSpawnPoint.transform.position;
                    }
                    else if (GameManager.instance.fromCardinalDirection == "west")
                    {
                        //eastSpawnPointにキャラクタをspawnさせる
                        playableCharacter.transform.position = eastSpawnPoint.transform.position;
                    }
                    GameManager.instance.isNextScene = false;

                }
            }
        }
        

    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
