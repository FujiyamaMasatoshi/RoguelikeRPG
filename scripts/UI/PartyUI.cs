using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyUI : MonoBehaviour
{
    public GameObject thiefUI = null;
    public GameObject archerUI = null;
    public GameObject warriorUI = null;
    // Start is called before the first frame update
    void Start()
    {
        //thiefUI = GetComponent<GameObject>();
        thiefUI.SetActive(false);
        //archerUI = GetComponent<GameObject>();
        archerUI.SetActive(false);
        //warriorUI = GetComponent<GameObject>();
        warriorUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isParty[0])
        {
            thiefUI.SetActive(true);
        }
        else
        {
            thiefUI.SetActive(false);
        }
        if (GameManager.instance.isParty[1])
        {
            archerUI.SetActive(true);
        }
        else
        {
            archerUI.SetActive(false);
        }

        if (GameManager.instance.isParty[2])
        {
            warriorUI.SetActive(true);
        }
        else
        {
            warriorUI.SetActive(false);
        }
    }
}
