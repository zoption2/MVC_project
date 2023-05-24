using System.Collections.Generic;
using UnityEngine;
using GeneralData;


namespace PatternMVC
{
    public interface IModel
    {
        public string Name { get; }
        public int MaxHealth { get; }
        public int CurrentHealth { get; }
        public int Power { get; }
        void ChangeMaxHealth(int value);
        void ChangeCurrentHealth(int value);
        void ChangePower(int value);
        void Subscribe(IStatsObserver listener);
        void Unsubscribe(IStatsObserver listener);
    }


    public class Model : IModel
    {
        private List<IStatsObserver> _statListeners;
        private PlayerData _data;
        private string _name;
        private int _maxHealth;
        private int _currentHealth;
        private int _power;

        public string Name => _name;
        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        public int Power => _power;

        public Model(PlayerData data)
        {
            _data = data;
            _name = data.Name;
            _maxHealth = data.MaxHealth;
            _currentHealth = data.CurrentHealth;
            _power = data.Power;

            _statListeners = new List<IStatsObserver>();
        }

        public void ChangeMaxHealth(int value)
        {
            _maxHealth = value;
            _data.UpdateMaxHealth(value);
            for (int i = 0, j = _statListeners.Count; i < j; i++)
            {
                _statListeners[i].SetMaxHealth(value);
            }
        }

        public void ChangeCurrentHealth(int value)
        {
            _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
            for (int i = 0, j = _statListeners.Count; i < j; i++)
            {
                _statListeners[i].SetCurrentHealth(_currentHealth);
            }
        }

        public void ChangePower(int value)
        {
            _power = value;
            _data.UpdatePower(value);
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

