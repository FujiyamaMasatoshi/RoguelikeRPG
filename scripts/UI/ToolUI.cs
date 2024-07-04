using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToolUI : MonoBehaviour
{
    public List<ST_CommandText> items = null;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(ST_CommandText t in items)
        {
            t.gameObject.SetActive(false);
        }
    }

    // GameManagerからアイテムの情報をitemsに反映させていく
    public void SyncItemFromGameManager()
    {
        for (int i=0; i<GameManager.instance.pItems.Count; i++)
        {
            items[i].gameObject.SetActive(true);
            TextMeshProUGUI item_text = items[i].GetComponent<TextMeshProUGUI>();
            item_text.text = GameManager.instance.pItems[i].name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SyncItemFromGameManager();
    }
}
