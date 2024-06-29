using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battler : MonoBehaviour
{
    // ステータス
    [Header("初期スタータス(HP)")] public int hp = 100;
    [Header("初期スタータス(HP)")] public int mp = 10;
    [Header("初期スタータス(ATK)")] public int atk = 100;
    [Header("初期スタータス(DEF)")] public int def = 100;
    [Header("初期スタータス(SPD)")] public int spd = 50;
    [Header("初期スタータス(REC)")] public int rec = 80;

    private int max_hp;
    private int max_mp;

    private void Start()
    {
        max_hp = hp;
        max_mp = mp;
    }

    //コマンド
    public SO_Commands[] commands;

    // 最大HPを返す
    public int get_max_hp()
    {
        return max_hp;
    }

    // 最大MPを返す
    public int get_max_mp()
    {
        return max_mp;
    }

}


