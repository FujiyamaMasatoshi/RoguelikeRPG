using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartManager : MonoBehaviour
{
    [Header("PlayableCharacter")] public GameObject playableCharacter;
    [Header("PlayerSpawnPos")] public GameObject playerSpawnPos;
    // Start is called before the first frame update
    void Start()
    {
        playableCharacter.transform.position = playerSpawnPos.transform.position;
        GameManager.instance.isGoBackFloorStart = false;

        // GM.now_floorsを1すすめす
        GameManager.instance.now_floor += 1;
        Debug.Log("GM.now_floors " + GameManager.instance.now_floor);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
