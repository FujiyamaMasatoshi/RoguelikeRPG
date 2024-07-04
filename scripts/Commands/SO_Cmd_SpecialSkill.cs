using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Cmd_SpecialSkill : SO_Commands
{
    // 味方プレイヤの二人以上バーストモードになっていた場合、
    // * それらを消費して、
    // * 味方キャラクタの行動全てを消費して
    // 使用可能

    public void ExecuteSpecialSkill(List<Character> users, List<Character> targets)
    {
        // ダメージ計算
        // userのatkの値を全て足し、x2した値のダメージを与える
        int sum_atk = 0;
        foreach(Character c in users)
        {
            sum_atk += c.atk;
        }

        // ターゲットのdefの値によってダメージを決定していく
        foreach(Character t in targets)
        {
            int damage = (int)((float)sum_atk * 2 - (float)t.def / 2f);

            // ダメージ処理
            t.hp = Mathf.Max(0, t.hp - damage);
        }

        // ダメージ処理
        for (int i=0; i<targets.Count; i++)
        {
            ReceiveDamage(targets[i]);
            SyncCharacterStatusInGameManager(users[0], targets[i]);
        }
    }
}
