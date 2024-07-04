using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeLoader : MonoBehaviour
{
    [SerializeField] Fade fade;
    // あるフラグが立ち上がったら、1秒間演出があって、シーン移動
    public void FadeLoadScene(string scene_name)
    {
        // 1秒間掛けて演出が入る
        // fade.FadeIn(時間(seconds), () => シーン切り替え);
        fade.FadeIn(1f, () =>SceneManager.LoadScene(scene_name));
    }
}
