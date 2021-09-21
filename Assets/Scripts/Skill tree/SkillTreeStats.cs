using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeStats : MonoBehaviour
{
    public string skillName;
    public Sprite skillImage;

     [TextArea(1, 3)]
     public string descrition;
     public bool isUpgraded;
}
