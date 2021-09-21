using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    void Awake()
    {
        if(instance ==null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();
        DontDestroyOnLoad(gameObject);
    }

    public void MusicData(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SfxData(float value)
    {
        PlayerPrefs.SetFloat("SfxVolume", value);
    }

    public void ExpData(float value)
    {
        PlayerPrefs.SetFloat("currentExp", value);
    }

    public void LevelData(int value)
    {
        PlayerPrefs.SetInt("level", value);
    }

    public void ExpToNextLevelData(float value)
    {
        PlayerPrefs.SetFloat("expTNL", value);
    }

    public void AmmoAmountData(int value)
    {
        PlayerPrefs.SetInt("ammoAmount", value);
    }

    public void ExpPointData(int value)
    {
        PlayerPrefs.SetInt("expPoint", value);
    }

    public void MaxHealthData(float value)
    {
        PlayerPrefs.SetFloat("maxHealth", value);
    }

    public void GoldData(int value)
    {
        PlayerPrefs.SetInt("gold", value);
    }
}
