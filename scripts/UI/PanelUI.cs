using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelUI : MonoBehaviour
{
    public TextMeshProUGUI floorText = null;
    public TextMeshProUGUI stageText = null;

    private string floorTextInitText = "Floor: ";
    private string stageTextInitText = "Stage: ";



    // フロア ステージの更新
    private void Update_Stage_Floor()
    {
        floorText.text = floorTextInitText + $"{GameManager.instance.now_floor}/{GameManager.instance.n_floors}";
        stageText.text = stageTextInitText + $"{GameManager.instance.n_stage - GameManager.instance.FloorList.Count}/{GameManager.instance.n_stage}";
    }



    // Update is called once per frame
    void Update()
    {
        // フロア、ステージの更新
        Update_Stage_Floor();





    }
}
