using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    public SkillData skillData;
    public void OnPointerClick(PointerEventData eventData)
    {
        SkillManager.instance.activeSkill = skillData;
        SkillManager.instance.DisplaySkillInfo();
        SkillManager.instance.UpdateSkillInfo();
    }
}
