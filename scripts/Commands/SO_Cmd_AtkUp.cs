using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_Cmd_AtkUp : SO_Commands
{
    // 味方の攻撃力をあげる
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

        int beforeATK = target.atk;

        // targetへの効果付与
        // 最大値は儲ける max=250
        target.atk = Mathf.Min(500, (int)(target.atk * 1.2));

        // result actionに結果を挿入
        //string result_action = $"{user.Name}: {this.Name}\n=> {target.Name}'ATK Up\n";
        string result_action = $"{user.Name}: {this.Name}\n{target.Name} は 攻撃力が上がった\n";

        // 具体的な数値をメッセージに表示させる時は、味方キャラクタの場合のみ
        if (user.isEnemy || target.isEnemy)
        {
            // 何もしない
        }
        else
        {
            result_action += $"{target.Name} の攻撃力: {beforeATK} -> {target.atk}";
        }
        
        user.SetResultAction(result_action);


        // GMにキャラクタステータス情報を更新する
        SyncCharacterStatusInGameManager(user, target);
    }


}
