﻿using UnityEngine;


namespace PatternMVC
{
    [CreateAssetMenu(fileName = "NewMVCModel", menuName = "ScriptableObjects/Models/MVC")]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; } = "Player"; 
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        [field: SerializeField] public int CurrentHealth { get; private set; } = 100;
        [field: SerializeField] public int Power { get; private set; } = 20;
    }
}

