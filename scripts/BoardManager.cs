//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Cinemachine;

//public class BoardManager : MonoBehaviour
//{
//    [Header("迷路の大きさwidth")]public static int MAP_WIDTH = 40; // 迷路の横の大きさ
//    [Header("迷路の大きさheight")] public static int MAP_HEIGHT = 40; // 迷路の縦の大きさ
//    [Header("宝箱の数")] public int n_treasures = 10;
//    [Header("敵の数")] public int n_enemies = 10;
//    [Header("生成するオブジェクトのスケール")] public int objScale = 3;
//    [Header("パーティメンバ")] public Player[] players = null;

//    private int startX; // スタート地点x
//    private int startY; // スタート地点y
//    private int goalX;
//    private int goalY;
//    private Vector3 PlayerStartPos; // プレイヤがスタートするポジション
//    //部屋の最大数
//    public int maxRooms = 10;
//    public int roomMaxSize = 7;
//    public int roomMinSize = 6;


//    private string WALL = "#";
//    private string PATH = ".";
//    private string START = "s";
//    private string GOAL = "g";


//    //public List<List<string>> map = new List<List<string>>();
//    public static string[,] map = new string[MAP_HEIGHT,MAP_WIDTH];

//    //private void Awake()
//    //{
        

//    //}


//    // Start is called before the first frame update
//    void Start()
//    {
//        Debug.Log("call BoardManager");
//        // create map
//        // GenerateMapないで敵をセットする or GMのmapのeをセットしなおす
//        LoadMap();

//        Debug.Log("created map");

//        // setting map
//        SettingMap();

//        // パーティメンバ playersをアクティブにする
//        foreach (Player p in players)
//        {
//            p.gameObject.transform.position = GameManager.instance.mapStartPos;
//        }
//        GameManager.instance.isNewMap = false;
//    }

//    /// <summary>
//    /// 壁でマップを初期化
//    /// </summary>
//    public void InitializeMap()
//    {
//        // mapを壁で埋める
//        for(int i=0; i<MAP_HEIGHT; i++)
//        {
//            for(int j=0; j<MAP_WIDTH; j++)
//            {
//                map[i,j] = "#";
//            }
//        }
//    }

//    // マップを用意する
//    public void LoadMap()
//    {
//        //if (GameManager.instance.isNewMap)
//        //{
//        //    // マップを再生成する時
//        //    Debug.Log("GenerateMapが呼び出された");
//        //    GenerateMap();
//        //    //GameManager.instance.nowScene = "";
//        //}
//        //else
//        //{
//        //    Debug.Log("GMからマップをロードした");
//        //    // GameManagerからfloorMapをmapにロードする
//        //    map = GameManager.instance.floorMap;
//        //    SetEnemy();


//        //}
//        GenerateMap();

//        // isNewMapの初期化 --> Playerスクリプト.Start()で行う
//        //GameManager.instance.isNewMap = false;

//    }



//    // ######################
//    // 穴掘り法
//    // ######################
//    private class Room
//    {
//        public int x1, y1, x2, y2;
//        public Room(int x, int y, int w, int h)
//        {
//            x1 = x;
//            y1 = y;
//            x2 = x + w;
//            y2 = y + h;
//        }

//        public Vector2Int Center()
//        {
//            int centerX = (x1 + x2) / 2;
//            int centerY = (y1 + y2) / 2;
//            return new Vector2Int(centerX, centerY);
//        }

//        public bool Intersects(Room other)
//        {
//            return x1 <= other.x2 && x2 >= other.x1 && y1 <= other.y2 && y2 >= other.y1;
//        }
//    }


//    void GenerateMap()
//    {

//        // Initialize dungeon with walls
//        for (int x = 0; x < MAP_WIDTH; x++)
//        {
//            for (int y = 0; y < MAP_HEIGHT; y++)
//            {
//                map[x, y] = "#";
//            }
//        }

//        List<Room> rooms = new List<Room>();
//        int numRooms = 0;

//        //int r = 0;
//        while(numRooms<maxRooms)
//        {
//            int w = Random.Range(roomMinSize, roomMaxSize);
//            int h = Random.Range(roomMinSize, roomMaxSize);
//            int x = Random.Range(1, MAP_WIDTH - w - 1);
//            int y = Random.Range(1, MAP_HEIGHT - h - 1);

//            Room newRoom = new Room(x, y, w, h);

//            bool intersects = false;
//            foreach (Room otherRoom in rooms)
//            {
//                if (newRoom.Intersects(otherRoom))
//                {
//                    intersects = true;
//                    break;
//                }
//            }

//            if (intersects) continue;

//            CreateRoom(newRoom);
//            Vector2Int newCenter = newRoom.Center();

//            if (numRooms != 0)
//            {
//                Vector2Int prevCenter = rooms[numRooms - 1].Center();

//                if (Random.Range(0, 2) == 0)
//                {
//                    CreateHTunnel(prevCenter.x, newCenter.x, prevCenter.y);
//                    CreateVTunnel(prevCenter.y, newCenter.y, newCenter.x);
//                }
//                else
//                {
//                    CreateVTunnel(prevCenter.y, newCenter.y, prevCenter.x);
//                    CreateHTunnel(prevCenter.x, newCenter.x, newCenter.y);
//                }
//            }

//            rooms.Add(newRoom);
//            numRooms++;
//        }

//        Debug.Log($"numRooms: {numRooms}");

//        // start positionをセット
//        SetStartPos(rooms[0]);

//        // Goal positionをセット
//        SetGoalPos(rooms[rooms.Count - 1]);

//        // 敵を設置する場所を決定
//        // Gamemanagerから敵の数を決定するが、GenerateMapはマップを新しくリロードする時しか呼ばないので、エネミーの数は初期値にセットする
//        GameManager.instance.InitNumEnemy();

//        // 敵をセットする
//        for (int i = 0; i < GameManager.instance.GetNumEnemy();)
//        {
//            int x = Random.Range(0, MAP_WIDTH);
//            int y = Random.Range(0, MAP_HEIGHT);
//            if (map[x, y] == ".")
//            {
//                map[x, y] = "e";
//                i++;
//            }
//        }


//        //GameManagerのfloorMapにmapの情報をコピーする
//        GameManager.instance.floorMap = map;



//    }

//    // GMのmapからロードした時のみ呼ぶ
//    void SetEnemy()
//    {
//        // mapの全ての"e"を"."に戻す
//        for (int x = 0; x < MAP_WIDTH; x++)
//        {
//            for (int y = 0; y < MAP_HEIGHT; y++)
//            {
//                if (map[x, y] == "e")
//                {
//                    map[x, y] = ".";
//                }
//            }
//        }

//        // 敵をセットする
//        for (int i = 0; i < GameManager.instance.GetNumEnemy();)
//        {
//            int x = Random.Range(0, MAP_WIDTH);
//            int y = Random.Range(0, MAP_HEIGHT);
//            if (map[x, y] == ".")
//            {
//                map[x, y] = "e";
//                i++;
//            }
//        }
//    }

//    void CreateRoom(Room room)
//    {
//        for (int x = room.x1; x < room.x2; x++)
//        {
//            for (int y = room.y1; y < room.y2; y++)
//            {
//                map[x, y] = ".";
//            }
//        }

//    }

//    void CreateHTunnel(int x1, int x2, int y)
//    {
//        for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++)
//        {
//            if (x > 0 && x < MAP_WIDTH - 1)
//            {
//                map[x, y] = ".";
//            }
//        }
//    }

//    void CreateVTunnel(int y1, int y2, int x)
//    {
//        for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
//        {
//            if (y > 0 && y < MAP_HEIGHT - 1)
//            {
//                map[x, y] = ".";
//            }
//        }
//    }


//    void SetStartPos(Room room)
//    {
//        Vector2Int startPos = room.Center();
//        startX = startPos[0];
//        startY = startPos[1];
//        map[startX, startY] = "s";
//        //Debug.Log($"start pos: {startX}, {startY}");

//        // gamemanagerにセットする
//        GameManager.instance.mapStartPos = new Vector3(startX, startY, 0);
//    }

//    void SetGoalPos(Room room)
//    {
//        Vector2Int startPos = room.Center();
//        goalX = startPos[0];
//        goalY = startPos[1];
//        map[goalX, goalY] = "g";
//        //Debug.Log($"goal pos: {goalX}, {goalY}");

//        // gamemanagerにセットする
//        GameManager.instance.mapGoalPos = new Vector3(goalX, goalY, 0);
//    }


//    /// <summary>
//    /// roguelikeのマップを初期化する
//    /// </summary>
//    public void SettingMap()
//    {
//        Debug.Log("call settingmap");
//        for (int i = 0; i < MAP_HEIGHT; i++)
//        {
//            for (int j = 0; j < MAP_WIDTH; j++)
//            {
//                if (map[i,j] == "#")
//                {
//                    GameObject wall = (GameObject)Resources.Load("Wall");
//                    Instantiate(wall, new Vector3(i * objScale, j * objScale, 0), Quaternion.identity);
                    
//                }
//                else if(map[i,j] == ".")
//                {
//                    //何もしない
//                }
//                else if(map[i,j] == "s")
//                {
//                    //Debug.Log("load start position");
//                    GameObject startPoint = (GameObject)Resources.Load("StartPoint");
//                    Instantiate(startPoint, new Vector3(i * objScale, j * objScale, 0), Quaternion.identity);
                    
//                    //プレイヤのスタートポジションを決定する
//                    PlayerStartPos = new Vector3(i * objScale, j * objScale, 0);
//                    GameManager.instance.mapStartPos = PlayerStartPos;
//                    Debug.Log($"GameManager.instance.mapStartPos in BoardManager: {GameManager.instance.mapStartPos}");

//                }
//                else if(map[i,j] == "g")
//                {
//                    GameObject goalPoint = (GameObject)Resources.Load("GoalPoint");
//                    Instantiate(goalPoint, new Vector3(i * objScale, j * objScale, 0), Quaternion.identity);
                    
//                }
//                else if (map[i, j] == "e")
//                {
//                    string e_name = string.Copy("AngryPig");
//                    GameObject enemy = (GameObject)Resources.Load(e_name);
//                    Instantiate(enemy, new Vector3(i * objScale, j * objScale, 0), Quaternion.identity);
//                }
//            }
//        }
//    }

//}
