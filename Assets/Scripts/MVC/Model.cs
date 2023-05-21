using System.Collections.Generic;
using UnityEngine;


namespace PatternMVC
{
    public interface IModel
    {
        public string Name { get; }
        public int MaxHealth { get; }
        public int CurrentHealth { get; }
        public int Power { get; }
        void Init();
        void DoReset();
        void ChangeMaxHealth(int value);
        void ChangeCurrentHealth(int value);
        void ChangePower(int value);
        void Subscribe(IStatsObserver listener);
        void Unsubscribe(IStatsObserver listener);
    }

    [CreateAssetMenu(fileName = "NewMVCModel", menuName = "ScriptableObjects/Models/MVC")]
    public class Model : ScriptableObject, IModel
    {
        [SerializeField] private string _name = "Player";
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private int _currentHealth = 100;
        [SerializeField] private int _power = 20;

        private List<IStatsObserver> _statListeners;
        private string _tempName;
        private int _tempMaxHealth;
        private int _tempCurrentHealth;
        private int _tempPower;

        public string Name => _tempName;
        public int MaxHealth => _tempMaxHealth;
        public int CurrentHealth => _tempCurrentHealth;
        public int Power => _tempPower;

        public void Init()
        {
            _statListeners = new();
            _tempName = _name;
            _tempMaxHealth = _maxHealth;
            _tempCurrentHealth = _currentHealth;
            _tempPower = _power;
        }

        public void DoReset()
        {
            Init();
        }

        public void ChangeMaxHealth(int value)
        {
            _tempMaxHealth = value;
            for (int i = 0, j = _statListeners.Count; i < j; i++)
            {
                _statListeners[i].SetMaxHealth(value);
            }
        }

        public void ChangeCurrentHealth(int value)
        {
            _tempCurrentHealth = value;
            for (int i = 0, j = _statListeners.Count; i < j; i++)
            {
                _statListeners[i].SetCurrentHealth(value);
            }
        }

        public void ChangePower(int value)
        {
            _tempPower = value;
            for (int i = 0, j = _statListeners.Count; i < j; i++)
            {
                _statListeners[i].SetPower(value);
            }
        }

        public void Subscribe(IStatsObserver listener)
        {
            if (!_statListeners.Contains(listener))
            {
                _statListeners.Add(listener);
            }
        }

        public void Unsubscribe(IStatsObserver listener)
        {
            if (_statListeners.Contains(listener))
            {
                _statListeners.Remove(listener);
            }
        }
    }
}

