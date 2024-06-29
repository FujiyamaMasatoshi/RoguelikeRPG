using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Cmd_Heal : SO_Commands
{
    [SerializeField] int healPoint;

    // 継承したSO_Cmmands.Executeの上書きを行い独自の処理にする
    public override void Execute(Character user, Character target)
    {
        // 上書き処理
        //int currentMP = user.mp;

        // mp消費
        // ストレス状態ならば
        if (user.isFullFrustrated)
        {
            // mp処理を行わない
        }
        //通常時は
        else
        {
            user.mp -= mp;
        }

        


        // HP回復処理
        int targetHP = target.hp;
        //int healingPoint = (int)((1 + user.rec / 100) * healPoint);
        int healingPoint = healPoint + user.rec / 10;
        Debug.Log("healingPoint:" + healingPoint);
        targetHP += healingPoint;
        target.hp = Mathf.Min(targetHP, target.get_max_hp());




        // english
        //string result_action = $"{user.Name}: {this.Name}\n{target.Name} is healed {healingPoint} points.\n";
        // japanese
        string result_action = $"{user.Name}: {this.Name}\n{target.Name} は {healingPoint} ポイント 回復した\n";

        if (target.isEnemy || user.isEnemy)
        {
            // 何もしない
        }
        else
        {
            //result_action += $"{target.Name}'s HP: { target.hp}/{ target.max_hp}";
            result_action += $"{target.Name} HP: { target.hp}/{ target.max_hp}";

        }

        user.SetResultAction(result_action);
        Debug.Log(result_action);

        // GMにキャラクタステータス情報を更新する
        SyncCharacterStatusInGameManager(user, target);
    }
}
