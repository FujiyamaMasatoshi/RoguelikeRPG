using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;



public class ST_CommandText : Selectable
{

    // 最初に洗濯している状態にするか
    [Header("最初の選択の有無")] public bool isFirstSelect;

    // 選択されているかどうか
    public bool isSelect = false;
    // TMPオブジェクト
    private TextMeshProUGUI textMeshPro = null;
    private string initText;
    public string att;
    public int IndexParentSetPanel = -1;

    //// 親オブジェクト
    public GameObject parentObj = null;
    //public BattleWindow parentWindow = null;
    //public int index = -1;

    public void SetInitText(string text)
    {
        initText = text;
    }

    public string GetInitText()
    {
        return initText;
    }

    public void SetIsSelect(bool b)
    {
        isSelect = b;
    }

    // start
    protected override void Start()
    {
        
        textMeshPro = GetComponent<TextMeshProUGUI>();
        initText = textMeshPro.text;

        //// 親オブジェクトを指定
        parentObj = transform.parent.gameObject;
        Debug.Log($"parentObj: {parentObj}");

        if (isFirstSelect)
        {
            Select();
            
            //textMeshPro.text = "> " + initText;
        }

    }


    // 選択状態になった時に勝手に実行される
    public override void OnSelect(BaseEventData eventData)
    {
        //base.OnSelect(eventData);
        isSelect = true;
        textMeshPro.text = "> " + initText;

        //// 親オブジェクトを操作
        // CommandWindowの時
        if (parentObj.name == "CommandWindow")
        {
            CommandWindow commandWindow = parentObj.GetComponent<CommandWindow>();
            if(att == "Battle")
            {
                commandWindow.isBattle = true;
                commandWindow.isSpecialSkill = false;
                commandWindow.isRun = false;
            }
            else if(att == "SpecialSkill")
            {
                commandWindow.isBattle = false;
                commandWindow.isSpecialSkill = true;
                commandWindow.isRun = false;
            }else if(att == "Run")
            {
                commandWindow.isBattle = false;
                commandWindow.isSpecialSkill = false;
                commandWindow.isRun = true;
            }
        }

        Debug.Log($"{gameObject.name}が選択された, {isSelect}");


        // selectModeWindowの時
        if(parentObj.name == "selectModeWindow")
        {
            SelectModeWindow SMWindow = parentObj.GetComponent<SelectModeWindow>();
            if(att == "Easy")
            {
                SMWindow.iseasy = true;
                SMWindow.isnormal = false;
                SMWindow.ishard = false;
                SMWindow.isback = false;
            }
            else if (att == "Normal")
            {
                SMWindow.iseasy = false;
                SMWindow.isnormal = true;
                SMWindow.ishard = false;
                SMWindow.isback = false;
            }
            else if(att == "Hard")
            {
                SMWindow.iseasy = false;
                SMWindow.isnormal = false;
                SMWindow.ishard = true;
                SMWindow.isback = false;
            }else if(att == "Back")
            {
                SMWindow.iseasy = false;
                SMWindow.isnormal = false;
                SMWindow.ishard = false;
                SMWindow.isback = true;
            }
        }

        // EventWindowの時
        if(parentObj.name == "EventWindow")
        {
            EventWindow EWindow = parentObj.GetComponent<EventWindow>();
            if(initText == "Yes")
            {
                //
                EWindow.isyes = true;
                EWindow.isno = false;
            }
            else if(initText == "No")
            {
                //
                EWindow.isyes = false;
                EWindow.isno = true;
            }
        }


        // ClearWindowの時
        if(parentObj.name == "ClearWindow")
        {
            ClearWindow ClrWindow = parentObj.GetComponent<ClearWindow>();
            if(initText == "Title")
            {
                ClrWindow.istitle = true;
                ClrWindow.isrestart = false;
            }else if(initText == "Restart")
            {
                ClrWindow.istitle = false;
                ClrWindow.isrestart = true;
            }
        }

    }


    // 選択解除されたら勝手に実行される
    public override void OnDeselect(BaseEventData eventData)
    {
        //base.OnDeselect(eventData);
        isSelect = false;
        textMeshPro.text = initText;
        Debug.Log($"{gameObject.name}が選択解除された, {isSelect}");

        //// 親オブジェクトを操作
        //parentWindow.isSelects[index] = false;


    }

    public bool IsSelect()
    {
        return isSelect;
    }
}
