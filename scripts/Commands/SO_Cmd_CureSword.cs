using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Cmd_CureSword : SO_Commands
{
    public override void Execute(Character user, Character target)
    {
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

        // ダメージ処理
        int damage = Mathf.Max(0, (int)((user.atk / 2 - target.def / 4)*2f));
        int curePoint = (int)((float)damage/1.2f);
        target.hp = Mathf.Max(0, target.hp - damage);

        // ストレス値の設定
        // ストレス値の加算
        target.frustration += damage;
        // もしフラスト状態ではなくて、ストレス値が溜まっていたら、
        if (!target.isFullFrustrated && target.frustration >= target.max_frustration)
        {
            // フラスト_フラグを立てる
            target.isFullFrustrated = true;
        }

        // 回復処理
        user.hp = Mathf.Min(user.hp+curePoint, user.get_max_hp());


        // result actionに結果を挿入
        //string result_action = $"{user.Name}: {this.Name}\n{target.Name} is {damage} points damage\n";
        //result_action += $"{user.Name} is healed\n";
        string result_action = $"{user.Name}: {this.Name}\n{target.Name} は {damage} ポイント ダメージを受けた\n";
        result_action += $"{user.Name} は 回復した\n";


        if (target.isEnemy || user.isEnemy)
        {
            // 何もしない
        }
        else
        {
            result_action += $"{user.Name} HP: {user.hp}/{user.max_hp}";
        }

        user.SetResultAction(result_action);
        Debug.Log(result_action);

        // GMにキャラクタステータス情報を更新する
        SyncCharacterStatusInGameManager(user, target);

    }
}
