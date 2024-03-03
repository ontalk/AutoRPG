using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePortal : MonoBehaviour
{

    public string transferMapName;
    private CharacterScripts thePlayer;

    private void Start()
    {
        if(thePlayer == null)
            thePlayer = GetComponent<CharacterScripts>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LoadingScene.LoadScene(transferMapName);
        }
    }
}
