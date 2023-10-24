using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackgroundMusic  : MonoBehaviour
{
    private static BackgroundMusic instance;

    public AudioSource audioSource;

    public static BackgroundMusic Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BackgroundMusic>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "BackgroundMusic";
                    instance = obj.AddComponent<BackgroundMusic>();
                    instance.audioSource = obj.AddComponent<AudioSource>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    // Método para reproducir música de fondo en la escena actual
    public void PlayBackgroundMusic(AudioClip music)
    {
        audioSource.clip = music;
        audioSource.Play();
    }
}





