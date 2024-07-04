using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SO_cmd_DefUp : SO_Commands
{
    // 味方の防御力をあげる
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

        int beforeDEF = target.def;

        // targetへの効果付与
        // 最大値は儲ける max=250
        target.def= Mathf.Min(300, (int)(target.def * 1.2));

        // result actionに結果を挿入
        //string result_action = $"{user.Name}: {this.Name}\n=> {target.Name}'DEF Up\n";
        string result_action = $"{user.Name}: {this.Name}\n{target.Name} は 守備力が上がった\n";

        // 具体的な数値をメッセージに表示させる時は、味方キャラクタの場合のみ
        if (user.isEnemy || target.isEnemy)
        {
            // 何もしない
        }
        else
        {
            //result_action += $"{target.Name}'s ATK is {beforeDEF} -> {target.def}";
            result_action += $"{target.Name} 守備力 {beforeDEF} -> {target.def}";

        }

        user.SetResultAction(result_action);


        // GMにキャラクタステータス情報を更新する
        SyncCharacterStatusInGameManager(user, target);
    }
}
