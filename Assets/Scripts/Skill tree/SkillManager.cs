using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
   public SkillData activeSkill;

   [Header("UI")]
   public Image skillImage;
   public TextMeshProUGUI skillNameTxt, skillLvlTxt, skillDescTxt;

    [Header("Skill Point")]
    [SerializeField] private int skillPoint;
    [SerializeField] private int pointsPot;
    public TextMeshProUGUI pointText;
    public Color lockedColor;

    public SkillButton[] skillButtons;
    public Sprite maxLevelFrame;
    public Sprite normalFrame;

   void Awake()
   {
        if(instance == null)
            instance = this;
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
   }

   void Start()
   {
       UpdateSkillInfo();
       UnlockedSkills();
   }

   public void UpgradeButton()
   {
       if(activeSkill == null)
            return;

       if(skillPoint >= activeSkill.xpCost && activeSkill.preSkills.Length == 0 && activeSkill.skillLevel < activeSkill.maxLevel)
       {
           UpdateSkill();
       }

       if(skillPoint >= activeSkill.xpCost && activeSkill.skillLevel < activeSkill.maxLevel)
       {
           for (int i = 0; i < activeSkill.preSkills.Length; i++)
           {
               if(activeSkill.preSkills[i].isUnlocked == true)
               {
                   UpdateSkill();
                   break;
               }
           }
       }
   }

   private void UpdateSkill()
   {
       skillButtons[activeSkill.skillId].GetComponent<Image>().color = Color.white;
        skillButtons[activeSkill.skillId].transform.GetChild(1).gameObject.SetActive(true);
        skillButtons[activeSkill.skillId].GetComponent<Animator>().SetTrigger("pressed");
        activeSkill.skillLevel++;
        skillButtons[activeSkill.skillId].transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = activeSkill.skillLevel.ToString();

        DisplaySkillInfo();
        ReachMaxLevel();

        skillPoint -= activeSkill.xpCost;
        pointsPot += activeSkill.xpCost;
        UpdateSkillInfo();
        activeSkill.isUnlocked = true;
   }

   private void ReachMaxLevel()
   {
       if(activeSkill == null)
            return;

       if(activeSkill.skillLevel == activeSkill.maxLevel)
       {
           skillButtons[activeSkill.skillId].transform.GetChild(0).GetComponent<Image>().sprite = maxLevelFrame;
       }
   }

   private void UnlockedSkills()
   {
       for (int i = 0; i < skillButtons.Length; i++)
       {
           if(skillButtons[i].skillData.isUnlocked)
           {
                skillButtons[i].GetComponent<Image>().color = Color.white;
                skillButtons[i].transform.GetChild(1).gameObject.SetActive(true);
                skillButtons[i].transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = skillButtons[i].skillData.skillLevel.ToString();
           }

           if(skillButtons[i].skillData.skillLevel == skillButtons[i].skillData.maxLevel)
           {
               skillButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = maxLevelFrame;
           }
       }
   }

   public void DisplaySkillInfo()
   {
       skillImage.sprite = activeSkill.skillSprite;
       skillNameTxt.text = activeSkill.skillName;
       skillLvlTxt.text = "skill lvel: lvl " + activeSkill.skillLevel;
       skillDescTxt.text = "description: \n" + activeSkill.skillDesc;
   }

   public void UpdateSkillInfo()
   {
       if(activeSkill != null)
            pointText.text = "points: " + skillPoint.ToString() + "/" + activeSkill.xpCost.ToString();
        else
            pointText.text = "points: " + skillPoint.ToString();
   }

   public void ResetSkills()
   {
       for (int i = 0; i < skillButtons.Length; i++)
       {
           skillButtons[i].skillData.skillLevel = 0;
           skillButtons[i].skillData.isUnlocked = false;
           skillButtons[i].transform.GetChild(1).gameObject.SetActive(false);
           skillButtons[i].GetComponent<Image>().color = lockedColor;
           skillButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = normalFrame;
       }
   }

}
