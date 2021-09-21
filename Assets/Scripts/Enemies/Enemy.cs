using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float health;
    public float defense;
    public float speed;
    public float stunTime;
    public float knockbackForceX;
    public float knockbackForceY;
    public float forceDamage;
    public float exp;
    public bool shouldRespaw;
}
