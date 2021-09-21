using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer musicMixer, effectsMixer;
    public AudioSource bgMusic, hit, shoot, gem, enemyDead, playerDead, levelUp, mainMenu, gameOver;

    [Range(-80, 10)]
    public float masterVol, effectsVol;
    public Slider masterSlider, effectSlider;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //PlayAudio(bgMusic);
        //masterSlider.value = masterVol;
        //effectSlider.value = effectsVol;

        masterSlider.minValue = -80;
        masterSlider.maxValue = 10;

        effectSlider.minValue = -80;
        effectSlider.maxValue = 10;

        masterSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0);
        effectSlider.value = PlayerPrefs.GetFloat("SfxVolume", 0);
    }

    // Update is called once per frame
    void Update()
    {
        //MasterVolume();
        //EffectsVolume();
    }

    public void MasterVolume()
    {
        DataManager.instance.MusicData(masterSlider.value);
        musicMixer.SetFloat("Master Volume", PlayerPrefs.GetFloat("MusicVolume"));
    }

    public void SfxVolume()
    {
        DataManager.instance.SfxData(effectSlider.value);
        effectsMixer.SetFloat("Effects Volume", PlayerPrefs.GetFloat("SfxVolume"));
    }

    public void PlayAudio(AudioSource audio) 
    {
        audio.Play();
    }
}
