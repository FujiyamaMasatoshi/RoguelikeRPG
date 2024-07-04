using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleWindow : MonoBehaviour
{
    public ST_CommandText[] actionTexts;
    //private List<string> commands_text = new List<string>();
    //private List<ST_CommandText> activeText = new List<ST_CommandText>();

    //private List<string> commands = new List<string>();
    //public List<SelectableText> selectableTexts = new List<SelectableText>();
    //public List<bool> isSelects = new List<bool>(); // 選択したテキストのindexのみtrueにする
    //public int selectIndex = -1;

    // 1. actionTextsのtext内容を""にする
    // 2. 選択状態を解除
    // 3. アクションテキストを全て非アクティブにして、
    public void InitActionText()
    {
        // 全て非アクティブにする
        foreach (ST_CommandText at in actionTexts)
        {
            at.SetInitText(""); // 1. initTextを""に設定
            at.SetIsSelect(false); //2. 選択解除
            at.gameObject.SetActive(false); // 3. 非アクティブ
        }
    }

    // 0. キャラクタの利用可能なコマンドをセットする
    // 1. actionTexts[i]をアクティブにする
    // 2. nowCharacterの持っているコマンドをセットする
    public void SetActionTexts(Character nowCharacter)
    {
        // 0. 利用可能コマンドのセット
        nowCharacter.SetUserableCommands();
        //nowCharacterのコマンドの名前をactionTextsにセットしていく
        //for(int i=0; i<nowCharacter.commands.Count; i++)
        for (int i = 0; i < nowCharacter.userableCommands.Count; i++)
        {
            // 1. actionTexts[i]をアクティブにする
            actionTexts[i].gameObject.SetActive(true);
            // 2. コマンドセット
            // 利用可能なコマンドのみ、InitTextをセット
            //actionTexts[i].SetInitText(nowCharacter.commands[i].Name);
            actionTexts[i].SetInitText(nowCharacter.userableCommands[i].Name);
            // TMProにinitTextをセットする
            TextMeshProUGUI text = actionTexts[i].GetComponent<TextMeshProUGUI>();
            text.text = actionTexts[i].GetInitText();
        }
    }



    public int GetSelectTextIndex()
    {
        int index = -1;
        for (int i = 0; i < actionTexts.Length; i++)
        {
            if (actionTexts[i].IsSelect())
            {
                index = i;
                return index;
            }
        }
        return index;
    }


    public void SelectActiveText_0()
    {
        actionTexts[0].Select();
    }



    public bool IsAnySelected()
    {
        foreach(ST_CommandText actionText in actionTexts)
        {
            bool selected = actionText.IsSelect();
            if (selected) 
            {

                return true;
            }
        }
        return false;
    }

    //public void SetAllText_isSelect(bool b)
    //{
    //    // 全てのactiveTextのisSelectをfalseにする
    //    foreach (ST_CommandText activetext in activeText)
    //    {
    //        activetext.SetIsSelect(b);
    //    }
    //}





    //// 表示されているアクティブな文字から「>」を取り除く
    //public void InitPanel()
    //{
    //    foreach (ST_CommandText selectableText in activeText)
    //    {
    //        TextMeshProUGUI text = selectableText.GetComponent<TextMeshProUGUI>();
    //        text.text = selectableText.GetInitText();

    //        //isSelectをすべてfalseに戻す
    //        selectableText.SetIsSelect(false);
    //    }

    //}


    //public void ClearActiveText()
    //{
    //    activeText.Clear();
    //}

    //// now_characterを受け取りコマンドにあるテキストのみactiveにする
    //public void SetCommands(Character now_character)
    //{
    //    // 前回用意したコマンドテキストをからにする
    //    commands_text.Clear();

    //    // コマンドテキストを用意
    //    for(int i=0; i<now_character.commands.Count; i++)
    //    {
    //        commands_text.Add(now_character.commands[i].Name);
    //    }
    //    Debug.Log("commnda_text.Count: " + commands_text.Count);
    //}



    //public void SetPanel()
    //{
    //    // 全てのテキストパネルを非activeにする
    //    foreach (GameObject text in actionText)
    //    {
    //        //text.SetActive(true);
    //        text.SetActive(false);
    //    }


    //    foreach (GameObject textPanel in actionText)
    //    {
    //        GameObject text = textPanel;
    //        //Debug.Log($"textPanel.name: {textPanel.name}");
    //        foreach (string cmdtxt in commands_text)
    //        {
    //            string txt = cmdtxt + "Text";
    //            //Debug.Log($"txt: {txt}, textPanel.name: {text.name}");
    //            if (txt == (string)text.name)
    //            {
    //                ST_CommandText selectableText = text.GetComponent<ST_CommandText>();
    //                activeText.Add(selectableText);
    //                text.SetActive(true);
    //                break;
    //            }
    //        }
    //    }

    //    Debug.Log("activeText.Count:" + activeText.Count);
    //}


}

