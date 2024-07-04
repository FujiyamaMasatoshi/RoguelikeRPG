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

}
