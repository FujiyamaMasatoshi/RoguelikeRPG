using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetInfoFromGM : MonoBehaviour
{
    public string charaName = "";
    public bool isHP = false;
    public int infoIndex = -1;
    private TextMeshProUGUI infoText = null;
    
    // Start is called before the first frame update
    void Start()
    {
        infoText = GetComponent<TextMeshProUGUI>();

        // infoIndexの設定
        for (int i=0; i<GameManager.instance.alies.Count; i++)
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
            infoText.text = $"{GameManager.instance.nowAliesStatus[infoIndex]["hp"]}/{GameManager.instance.nowAliesStatus[infoIndex]["max_hp"]}";
        }
        else
        {
            infoText.text = $"{GameManager.instance.nowAliesStatus[infoIndex]["mp"]}/{GameManager.instance.nowAliesStatus[infoIndex]["max_mp"]}";
        }

        //if (charaName == "Thief")
        //{
        //    //
        //    if (isHP)
        //    {
        //        infoText.text = $"{GameManager.instance.nowAliesStatus[infoIndex]["max_hp"]}/{GameManager.instance.nowAliesStatus[infoIndex]["max_hp"]}";
        //    }
        //    else
        //    {
        //        infoText.text = $"{GameManager.instance.nowAliesStatus[infoIndex]["max_mp"]}/{GameManager.instance.nowAliesStatus[infoIndex]["max_mp"]}";
        //    }
        //}
        //else if (charaName == "Archer")
        //{
        //    if (isHP)
        //    {
        //        infoText.text = $"{GameManager.instance.NpcsHP[0]}/{GameManager.instance.NpcsMaxHP[0]}";
        //    }
        //    else
        //    {

        //        infoText.text = $"{GameManager.instance.NpcsMP[0]}/{GameManager.instance.NpcsMaxMP[0]}";
        //    }
        //}
        //else if (charaName == "Warrior")
        //{
        //    if (isHP)
        //    {
        //        infoText.text = $"{GameManager.instance.NpcsHP[1]}/{GameManager.instance.NpcsMaxHP[1]}";
        //    }
        //    else
        //    {

        //        infoText.text = $"{GameManager.instance.NpcsMP[1]}/{GameManager.instance.NpcsMaxMP[1]}";
        //    }
        //}
    }
}
