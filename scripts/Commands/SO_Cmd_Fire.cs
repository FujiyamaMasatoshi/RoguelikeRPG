using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Cmd_Fire : SO_Commands
{
    [SerializeField] int attackPoint;

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

        // target hp処理
        int targetHP = target.hp;
        int damage = Mathf.Max(0, (int)(attackPoint * (1+user.atk/500)));
        targetHP -= damage;

        // ダメージ処理
        target.hp = Mathf.Max(0, target.hp - damage);

        // #####################
        // BurstMode
        // #####################
        // ストレス値の加算
        target.frustration += damage;
        // もしフラスト状態ではなくて、ストレス値が溜まっていたら、
        if (!target.isFullFrustrated && target.frustration >= target.max_frustration)
        {
            // フラスト_フラグを立てる
            target.isFullFrustrated = true;
        }

        // result actionに結果を挿入
        //string result_action = $"{user.Name}: {this.Name}\n{target.Name} is {damage} points damage\n";
        string result_action = $"{user.Name}: {this.Name}\n{target.Name} は {damage} ポイント ダメージを受けた\n";

        // 具体的な数値をメッセージに表示させる時は、味方キャラクタの場合のみ
        if (user.isEnemy || target.isEnemy)
        {
            // 何もしない
        }
        else
        {
            result_action += $"{target.Name} HP: {target.hp}/{target.max_hp}";
            
        }
        user.SetResultAction(result_action);

        // アニメーション
        ReceiveDamage(target);
        ////ダメージが〇より大きく、targetが敵ならば、hit2アニメを再生する
        //if (damage > 0 && target.CompareTag("Enemy"))
        //{
        //    Enemy e = target.GetComponent<Enemy>();
        //    e.GetAnim().Play("hit2");
        //    e.GetAnim().Play("idle");
        //}

        Debug.Log(result_action);

        // GMにキャラクタステータス情報を更新する
        SyncCharacterStatusInGameManager(user, target);
    }
}
