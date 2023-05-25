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

        void ChangeCurrentHealth(int value);
        void Subscribe(IHealthObserver listener);
        void Unsubscribe(IHealthObserver listener);
    }


    public class Model : IModel
    {
        private List<IHealthObserver> _healthObservers;
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

            _healthObservers = new List<IHealthObserver>();
        }

        public void ChangeCurrentHealth(int value)
        {
            _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
            for (int i = 0, j = _healthObservers.Count; i < j; i++)
            {
                _healthObservers[i].SetHealth(_currentHealth);
            }
        }

        public void Subscribe(IHealthObserver listener)
        {
            if (!_healthObservers.Contains(listener))
            {
                _healthObservers.Add(listener);
            }
        }

        public void Unsubscribe(IHealthObserver listener)
        {
            if (_healthObservers.Contains(listener))
            {
                _healthObservers.Remove(listener);
            }
        }
    }
}

