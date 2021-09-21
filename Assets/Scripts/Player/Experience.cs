using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Experience : MonoBehaviour
{
    public static Experience instance;
    public Image expImg;
    public float currentExp, expTNL; // Experience To Next level
    public int level = 1;
    public int expPoints;

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
        currentExp = PlayerPrefs.GetFloat("currentExp", 0);
        level = PlayerPrefs.GetInt("level", 1);
        expTNL = PlayerPrefs.GetFloat("expTNL", expTNL);
        expPoints = PlayerPrefs.GetInt("expPoint", expPoints);
        expImg.fillAmount = currentExp / expTNL;
        UiManager.instance.LevelUiUpdate(level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  ExpModifier(float exp)
    {
        //currentExp = PlayerPrefs.GetFloat("currentExp", 0);
        currentExp += exp;

        expTNL = PlayerPrefs.GetFloat("expTNL", expTNL);

        if(currentExp >= expTNL)
        {
            expTNL *= 2;
            currentExp = 0;
            level++;
            expPoints += 1;
            AudioManager.instance.PlayAudio(AudioManager.instance.levelUp);
            
            //DataManager.instance.ExpPointData(expPoints);

            PlayerHealth.instance.maxHealth += 10;
            UiManager.instance.LevelUiUpdate(level);
        }

        expImg.fillAmount = currentExp / expTNL;
    }

    public void DataToSave()
    {
        DataManager.instance.ExpData(currentExp);
        DataManager.instance.ExpToNextLevelData(expTNL);
        DataManager.instance.LevelData(level);
        DataManager.instance.ExpPointData(expPoints);

        currentExp = PlayerPrefs.GetFloat("currentExp");
        expTNL = PlayerPrefs.GetFloat("expTNL");
        level = PlayerPrefs.GetInt("level");
        expPoints = PlayerPrefs.GetInt("expPoint");
    }
}
