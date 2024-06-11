using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singletonObj : MonoBehaviour
{
    static bool existsInstance = false;

    void Awake()
    {

        if (existsInstance)
        {

            Destroy(gameObject);

            return;

        }



        existsInstance = true;

        DontDestroyOnLoad(gameObject);

    }
}
