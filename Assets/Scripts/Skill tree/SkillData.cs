using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Skill Stats", fileName = "Skill")]
public class SkillData : ScriptableObject
{
    public int skillId;
    public Sprite skillSprite;
    public string skillName;
    public int skillLevel;
    public int maxLevel;
    public int xpCost;

    [TextArea(1, 8)]
    public string skillDesc;

    public bool isUnlocked;
    public SkillData[] preSkills;
}
