using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using GeneralData;


namespace PatternMVP
{
    public interface IView
    {
        void Init(IPresenter presenter);
        void Show(Action callback = null);
        void Hide(Action callback = null);
        void OnModelChanged(IStatsProvider stats);
    }

    public class View: MonoBehaviour, IView
    {
        private const string kHealthFormat = "{0}/{1}";

        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _power;
        [SerializeField] private TMP_Text _points;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Button _healButton;
        [SerializeField] private Button _attackButton;

        [SerializeField] private Button _plusHealthButton;
        [SerializeField] private Button _minusHealthButton;
        [SerializeField] private Button _plusPowerButton;
        [SerializeField] private Button _minusPowerButton;

        private IPresenter _presenter;

        public void Init(IPresenter presenter)
        {
            _presenter = presenter;
        }

        private void SetName(string name)
        {
            _name.text = name;
        }
    }

    public interface IPresenter
    {
        void Init(IModel model, IView view);
        void Play();
        void Complete();
        void ChangeModel(IModel model);
        void ChangeView(IView view);

        void PowerUp();
        void PowerDown();
        void MaxHealthUp();
        void MaxHealthDown();
        void ApplyStats();
        void CancelStats();
    }

    public class Presenter
    {

    }


    public interface IStatsProvider
    {
        public string Name { get; }
        public int MaxHealth { get; }
        public int CurrentHealth { get; }
        public int Power { get; }
    }

    public interface IModel : IStatsProvider
    {
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

