using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DontDestroygameObject : MonoBehaviour
{
    private void Awake()
    {
        var objs = FindObjectsOfType<DontDestroygameObject>();
        if (objs.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
