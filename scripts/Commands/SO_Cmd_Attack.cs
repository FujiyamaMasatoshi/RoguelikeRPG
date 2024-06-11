using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Cmd_Attack : SO_Commands
{
    //[SerializeField] int attackPoint;


    public override void Execute(Character user, Character target)
    {
        // damageを計算
        // damage = atk/2- def/4
        int damage = Mathf.Max(0, (int)(user.atk / 2 - target.def / 4));

        // ダメージ処理
        target.hp = Mathf.Max(0, target.hp - damage);

        // ストレス値の設定
        // ストレス値の加算
        target.frustration += damage;
        // もしフラスト状態ではなくて、ストレス値が溜まっていたら、
        if(!target.isFullFrustrated && target.frustration >= target.max_frustration)
        {
            // フラスト_フラグを立てる
            target.isFullFrustrated = true;
        }


        // result actionに結果を挿入
        //string result_action = $"{user.Name}: {this.Name}\n{target.Name} is {damage} points damage";
        string result_action = $"{user.Name}: {this.Name}\n{target.Name} は {damage} ポイント ダメージを受けた\n";

        // 具体的な数値をメッセージに表示させる時は、味方キャラクタの場合のみ
        if (user.isEnemy || target.isEnemy)
        {
            // 何もしない
        }
        else
        {
            result_action += $"{target.Name}  HP: {target.hp}/{target.max_hp}";
            
        }
        user.SetResultAction(result_action);

        // アニメーションプレイ
        ReceiveDamage(target);
        //// ダメージが〇より大きく、targetが敵ならば、hit2アニメを再生する
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
