using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(menuName = "Create Enemy")]
public class EnemyBase : ScriptableObject
{
    [SerializeField] Sprite sprite;
    [SerializeField] int maxHealth;
    [SerializeField] int damage;
    [SerializeField] float reloadTime;

    [SerializeField] float speed; 
    [SerializeField] float evasion; //not one to 100. Hit rate calculated by player accuracy divided by enemy evasion. If result is >1, it has same effect as = to 1

    public Sprite Sprite
    {
        get { return sprite; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }

    }

    public int Damage
    {
        get { return damage; }
    }

    public float ReloadTime
    {
        get { return reloadTime; }
    }
    public float Speed
    {
        get { return speed; }
    }

    public float Evasion
    {
        get { return evasion; }
    }

}
public enum Stat
    {
        Attack,
        ReloadTime,
        Damage,
        Speed
    }
