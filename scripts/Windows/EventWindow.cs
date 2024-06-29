using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWindow : MonoBehaviour
{
    [Header("YesPanel")] public ST_CommandText YesPanel;
    [Header("NoPanel")] public ST_CommandText NoPanel;

    public bool isyes = false;
    public bool isno = false;

}
