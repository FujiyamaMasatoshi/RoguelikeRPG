using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// HPやMPバーを表示
public class NumericBar : MonoBehaviour
{
    public string charaName = "";
    public bool isHP = false; // trueの場合はhp, falseの場合はmp

    public int infoIndex = -1;
    private Image image;

    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        // infoIndexの設定
        for (int i = 0; i < GameManager.instance.alies.Count; i++)
        {
            if (GameManager.instance.alies[i].name == charaName)
            {
                infoIndex = i;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //
        if (isHP)
        {
            image.fillAmount = (float)GameManager.instance.nowAliesStatus[infoIndex]["hp"]/ (float)GameManager.instance.nowAliesStatus[infoIndex]["max_hp"];
        }
        else
        {
            image.fillAmount = (float)GameManager.instance.nowAliesStatus[infoIndex]["mp"]/ (float)GameManager.instance.nowAliesStatus[infoIndex]["max_mp"];
        }
        //Debug.Log("call fillAmout");
        //if (isHP)
        //{
        //    image.fillAmount = (float)GameManager.instance.pHP / (float)GameManager.instance.pMaxHP;
        //}
        //else
        //{
        //    image.fillAmount = (float)GameManager.instance.pMP / (float)GameManager.instance.pMaxMP;
        //}

        //if (charaName == "Thief")
        //{
        //    //
        //    Debug.Log("call fillAmout");
        //    if (isHP)
        //    {
        //        image.fillAmount = (float)GameManager.instance.pHP / (float)GameManager.instance.pMaxHP;
        //    }
        //    else
        //    {
        //        image.fillAmount = (float)GameManager.instance.pMP / (float)GameManager.instance.pMaxMP;
        //    }
        //}
        //else if (charaName == "Archer")
        //{
        //    //
        //    if (isHP)
        //    {
        //        image.fillAmount = (float)GameManager.instance.NpcsHP[0] / (float)GameManager.instance.NpcsMaxHP[0];
        //    }
        //    else
        //    {

        //        image.fillAmount = (float)GameManager.instance.NpcsMP[0] / (float)GameManager.instance.NpcsMaxMP[0];
        //    }
        //}
        //else if (charaName == "Warrior")
        //{
        //    //
        //    if (isHP)
        //    {

        //        image.fillAmount = (float)GameManager.instance.NpcsHP[1] / (float)GameManager.instance.NpcsMaxHP[1];
        //    }
        //    else
        //    {

        //        image.fillAmount = (float)GameManager.instance.NpcsMP[1] / (float)GameManager.instance.NpcsMaxMP[1];
        //    }
        //}
    }
}
