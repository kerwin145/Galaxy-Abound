using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyBase _base;
    [SerializeField] int level;

    public Dictionary<Stat, int> Stats { get; private set; }

    void Init()
    {
        
    }

    void Update()
    {
        
    }
}
