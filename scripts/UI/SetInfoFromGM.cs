using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetInfoFromGM : MonoBehaviour
{
    public string charaName = "";
    public bool isHP = false;
    private TextMeshProUGUI infoText = null;
    
    // Start is called before the first frame update
    void Start()
    {
        infoText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (charaName == "Thief")
        {
            //
            if (isHP)
            {
                infoText.text = $"{GameManager.instance.pHP}/{GameManager.instance.pMaxHP}";
            }
            else
            {
                infoText.text = $"{GameManager.instance.pMP}/{GameManager.instance.pMaxMP}";
            }
        }
        else if (charaName == "Archer")
        {
            if (isHP)
            {
                infoText.text = $"{GameManager.instance.NpcsHP[0]}/{GameManager.instance.NpcsMaxHP[0]}";
            }
            else
            {

                infoText.text = $"{GameManager.instance.NpcsMP[0]}/{GameManager.instance.NpcsMaxMP[0]}";
            }
        }
        else if (charaName == "Warrior")
        {
            if (isHP)
            {
                infoText.text = $"{GameManager.instance.NpcsHP[1]}/{GameManager.instance.NpcsMaxHP[1]}";
            }
            else
            {

                infoText.text = $"{GameManager.instance.NpcsMP[1]}/{GameManager.instance.NpcsMaxMP[1]}";
            }
        }
    }
}
