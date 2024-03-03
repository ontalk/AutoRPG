using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Sound : MonoBehaviour
{
    public AudioMixer mixer;
    public AudioSource bgSound;
    public AudioClip[] bglist;
    public static Sound instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>(); // AudioSource 추가
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update 내용 추가
    }
    public void SfXPlay(string sfxName, AudioClip clip)
    {
        /* if (audioSource.isPlaying)
         {
             audioSource.Stop();
         }*/
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for(int i =0;i<bglist.Length; i++)
        {
            if (arg0.name == bglist[i].name)
                BgSoundPlay(bglist[i]);
        }
        
    }

    public void BGSoundVolume(float val)
    {
        mixer.SetFloat("BGSound", Mathf.Log10(val) *20);
    }

    public void SFXSound(float val)
    {
        mixer.SetFloat("SFX", Mathf.Log10(val) * 20);
    }
    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BGSound")[0];
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();
    }


}
