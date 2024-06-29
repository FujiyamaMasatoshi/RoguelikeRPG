using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectModeWindow : MonoBehaviour
{
    public ST_CommandText easyPanel;
    public ST_CommandText normalPanel;
    public ST_CommandText hardPanel;
    public ST_CommandText backPanel;

    public bool iseasy = false;
    public bool isnormal = false;
    public bool ishard = false;
    public bool isback = false;

    private void OnEnable()
    {
        iseasy = true;
        isnormal = false;
        ishard = false;
        isback = false;
    }

    private void Start()
    {
        //初期値をセットする
        //TextMeshProUGUI easytext = easyPanel.GetComponent<TextMeshProUGUI>();
        //easytext.text = easyPanel.GetInitText();

        //TextMeshProUGUI normaltext = easyPanel.GetComponent<TextMeshProUGUI>();
        //normaltext.text = normalPanel.GetInitText();

        //TextMeshProUGUI hardtext = hardPanel.GetComponent<TextMeshProUGUI>();
        //hardtext.text = hardPanel.GetInitText();

    }

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
