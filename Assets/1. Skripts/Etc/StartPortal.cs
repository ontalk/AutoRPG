using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPortal : MonoBehaviour
{
    public string startPoint;
    private CharacterScripts thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (thePlayer == null)
            thePlayer = FindObjectOfType<CharacterScripts>();
        thePlayer.transform.position = transform.position;

    }
}
