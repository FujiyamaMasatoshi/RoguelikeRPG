using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetWindow : MonoBehaviour
{
    //public GameObject[] targetText = null;
    //private List<ST_CommandText> activeTexts = new List<ST_CommandText>();

    public ST_CommandText[] targetTexts;

    // 1. targetTextsのtext内容を""にする
    // 2. 選択状態を解除
    // 3. アクションテキストを全て非アクティブにして、
    public void InitTargetText()
    {
        // 全て非アクティブにする
        foreach (ST_CommandText tt in targetTexts)
        {
            tt.SetInitText(""); // 1. initTextを""に設定
            tt.SetIsSelect(false); //2. 選択解除
            tt.gameObject.SetActive(false); // 3. 非アクティブ
        }
    }


    // 1. targetTexts[i]をアクティブにする
    // 2. character_listのNameをtargetTexts[i]にセットしていく
    public void SetTargetText(List<Character> character_list)
    {
        for (int i=0; i<character_list.Count; i++)
        {
            // 1. アクティブ
            targetTexts[i].gameObject.SetActive(true);
            // 2. ターゲットセット
            targetTexts[i].SetInitText(character_list[i].Name);
            // TMProUGUI.textにテキストをセットする
            TextMeshProUGUI text = targetTexts[i].GetComponent<TextMeshProUGUI>();
            text.text = targetTexts[i].GetInitText();
        }
    }


    public int GetSelectTargetIndex()
    {
        int index = -1;
        for(int i=0; i<targetTexts.Length; i++)
        {
            if (targetTexts[i].IsSelect())
            {
                index = i;
                return index;
            }
        }
        return index;
    }


    // 0番目を選択状態にする
    public void SelectActiveText_0()
    {
        targetTexts[0].Select();
    }

    // targetTextsのどれか一つでも選択状態ならばtrue, else flase
    public bool IsAnySelected()
    {
        foreach (ST_CommandText selectableText in targetTexts)
        {
            bool selected = selectableText.IsSelect();
            if (selected)
            {

                return true;
            }
        }
        return false;
    }


    //public void SetTargetText(List<Character> character_list)
    //{
    //    // 前回のactiveTextsをクリアする
    //    activeTexts.Clear();

    //    // 全てのtextを非アクティブにする
    //    foreach (GameObject obj in targetText)
    //    {
    //        obj.SetActive(false);
    //    }

    //    // 選択可能なテキストオブジェクトをアクティブに敷いて名前を設定する
    //    Debug.Log($"enemy_list.Count: {enemy_list.Count}");
    //    for (int i = 0; i < enemy_list.Count; i++)
    //    {
    //        // 現在いる敵
    //        Character enemy = enemy_list[i];
    //        //TextMeshProUGUI textMeshPro = targetText[i].GetComponent<TextMeshProUGUI>();
    //        //textMeshPro.text = enemy.Name;

    //        // テキストをセットする
    //        ST_CommandText selectableText = targetText[i].GetComponent<ST_CommandText>();
    //        selectableText.SetInitText(enemy.Name);
    //        TextMeshProUGUI text = selectableText.GetComponent<TextMeshProUGUI>();
    //        text.text = enemy.Name;

    //        //アクティブにする
    //        targetText[i].SetActive(true);

    //        // selectableTextに追加する
    //        activeTexts.Add(selectableText);
    //    }
    //}



    //public void SelectActiveText_0()
    //{
    //    activeTexts[0].Select();
    //}

    //public void SetAllText_isSelect(bool b)
    //{
    //    foreach(ST_CommandText selectableText in activeTexts)
    //    {
    //        selectableText.SetIsSelect(b);
    //    }
    //}

    //public bool IsAnySelected()
    //{
    //    foreach (ST_CommandText selectableText in activeTexts)
    //    {
    //        bool selected = selectableText.IsSelect();
    //        if (selected)
    //        {

    //            return true;
    //        }
    //    }
    //    return false;
    //}



    //public void InitPanel()
    //{
    //    foreach(ST_CommandText selectableText in activeTexts)
    //    {
    //        TextMeshProUGUI text = selectableText.GetComponent<TextMeshProUGUI>();
    //        text.text = selectableText.GetInitText();
    //    }
    //}

    //public int GetSelectTargetIndex()
    //{
    //    int index = -1;
    //    for(int i=0; i<activeTexts.Count; i++)
    //    {
    //        if (activeTexts[i].IsSelect())
    //        {
    //            index = i;
    //            return index;
    //        }
    //    }
    //    return index;
    //}



}
