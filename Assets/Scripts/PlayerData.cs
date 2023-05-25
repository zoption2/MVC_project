using UnityEngine;


namespace GeneralData
{

    [CreateAssetMenu(fileName = "NewMVCModel", menuName = "ScriptableObjects/Models/MVC")]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; } = "Player";
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        [field: SerializeField] public int CurrentHealth { get; private set; } = 100;
        [field: SerializeField] public int Power { get; private set; } = 20;
        [field: SerializeField] public int FreePoints { get; private set; } = 0;

        public void UpdateMaxHealth(int value)
        {
            MaxHealth = value;
        }

        public void UpdatePower(int value)
        {
            Power = value;
        }

        public void UpdateFreePoints(int value)
        {
            FreePoints = value;
        }
    }
}

