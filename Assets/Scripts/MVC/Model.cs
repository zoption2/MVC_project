using System.Collections.Generic;


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
        private string _name;
        private int _maxHealth;
        private int _currentHealth;
        private int _power;

        public string Name => _name;
        public int MaxHealth => _maxHealth;
        public int CurrentHealth => _currentHealth;
        public int Power => _power;

        public Model(PlayerData playerData)
        {
            _name = playerData.Name;
            _maxHealth = playerData.MaxHealth;
            _currentHealth = playerData.CurrentHealth;
            _power = playerData.Power;

            _statListeners = new List<IStatsObserver>();
        }

        public void ChangeMaxHealth(int value)
        {
            _maxHealth = value;
            for (int i = 0, j = _statListeners.Count; i < j; i++)
            {
                _statListeners[i].SetMaxHealth(value);
            }
        }

        public void ChangeCurrentHealth(int value)
        {
            _currentHealth = value;
            for (int i = 0, j = _statListeners.Count; i < j; i++)
            {
                _statListeners[i].SetCurrentHealth(value);
            }
        }

        public void ChangePower(int value)
        {
            _power = value;
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

