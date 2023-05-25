using UnityEngine;
using GeneralData;
using System.Collections.Generic;

namespace PatternMVP
{
    public interface IObservableData
    {
        public string Name { get; }
        public int MaxHealth { get; }
        public int CurrentHealth { get; }
        public int Power { get; }
        public int FreePoints { get; }
    }

    public interface IModel : IObservableData
    {
        void ChangeMaxHealth(int value);
        void ChangeCurrentHealth(int value);
        void ChangePower(int value);
        void ChangeFreePoints(int value);
        void Subscribe(IDataObserver observer);
        void Unsubscribe(IDataObserver observer);
    }


    public class Model : IModel
    {
        private List<IDataObserver> _dataObservers;
        private PlayerData _data;
        private string _name;
        private int _maxHealth;
        private int _currentHealth;
        private int _power;
        private int _freePoints;

        public string Name => _name;
        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        public int Power => _power;
        public int FreePoints => _freePoints;

        public Model(PlayerData data)
        {
            _data = data;
            _name = data.Name;
            _maxHealth = data.MaxHealth;
            _currentHealth = data.CurrentHealth;
            _power = data.Power;
            _freePoints = data.FreePoints;

            _dataObservers = new List<IDataObserver>();
        }

        public void ChangeMaxHealth(int value)
        {
            _maxHealth = value;
            _data.UpdateMaxHealth(value);
            NotifyObservers();
        }

        public void ChangeCurrentHealth(int value)
        {
            _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
            NotifyObservers();
        }

        public void ChangePower(int value)
        {
            _power = value;
            _data.UpdatePower(value);
            NotifyObservers();
        }

        public void ChangeFreePoints(int value)
        {
            _freePoints = value;
            _data.UpdateFreePoints(value);
            NotifyObservers();
        }

        public void Subscribe(IDataObserver observer)
        {
            if (!_dataObservers.Contains(observer))
            {
                _dataObservers.Add(observer);
            }
        }

        public void Unsubscribe(IDataObserver observer)
        {
            if (_dataObservers.Contains(observer))
            {
                _dataObservers.Remove(observer);
            }
        }

        private void NotifyObservers()
        {
            for (int i = 0, j = _dataObservers.Count; i < j; i++)
            {
                _dataObservers[i].UpdateData(this);
            }
        }
    }
}

