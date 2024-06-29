using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 選択したコマンドをCharacterのバトラーのコマンドにセットする
public class CommandWindow : MonoBehaviour
{
    
    // selectable text
    [Header("Battle")] public ST_CommandText BattlePanel;
    [Header("Tool")] public ST_CommandText ToolPanel;
    [Header("Run")] public ST_CommandText RunPanel;


    public bool isBattle = false;
    public bool isTool = false;
    public bool isRun = false;


    public void InitPanel()
    {
        // BattlePanelの文字を初期化
        TextMeshProUGUI battletext = BattlePanel.GetComponent<TextMeshProUGUI>();
        BattlePanel.SetInitText("Battle"); //初期テキストをセット
        battletext.text = BattlePanel.GetInitText();
        Debug.Log($"battletext.text: {battletext.text}, {BattlePanel.GetInitText()}");

        // ToolPanelの文字を初期化
        TextMeshProUGUI tooltext = ToolPanel.GetComponent<TextMeshProUGUI>();
        ToolPanel.SetInitText("Tool"); //初期テキストをセット
        tooltext.text = ToolPanel.GetInitText();
        Debug.Log($"tooltext.text: {tooltext.text}");

        // RunPanelの文字を初期化
        TextMeshProUGUI runtext = RunPanel.GetComponent<TextMeshProUGUI>();
        RunPanel.SetInitText("Run"); //初期テキストをセット
        runtext.text = RunPanel.GetInitText();
        Debug.Log($"runtext.text: {runtext.text}");
    }

    public bool IsAnySelected()
    {
        if(IsBattle() || IsTool() || IsRun())
        {
            // 全てのパネルのisSelectをfalseに戻す
            BattlePanel.SetIsSelect(false);
            ToolPanel.SetIsSelect(false);
            RunPanel.SetIsSelect(false);

            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsBattle()
    {
        //if (BattlePanel.IsSelect())
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        return isBattle;
    }
    public bool IsTool()
    {
        //if (ToolPanel.IsSelect()) { 
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        return isTool;

    }
    public bool IsRun()
    {
        //if (RunPanel.IsSelect())
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        return isRun;
    }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    //
    //}

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Return))
    //    {
    //        if (BattlePanel.IsSelect())
    //        {
    //            isBattle = true;
    //            isTool = false;
    //            isRun = false;
    //        }
    //        else if (ToolPanel.IsSelect())
    //        {
    //            isBattle = false;
    //            isTool = true;
    //            isRun = false;
    //        }
    //        else if (RunPanel.IsSelect())
    //        {
    //            isBattle = false;
    //            isTool = false;
    //            isRun = true;
    //        }
    //    }
    //}


}
