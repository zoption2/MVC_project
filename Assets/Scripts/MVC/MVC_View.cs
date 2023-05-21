using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

namespace PatternMVC
{
    public interface IView
    {
        public event Action ON_ATTACK_CLICK;
        public event Action ON_HEAL_CLICK;
        IStatsObserver StatsObserver { get; }
        void Show(Action callback = null);
        void Hide(Action callback = null);
        void SetName(string value);
        void SetMaxHealth(int value);
        void SetCurrentHealth(int value);
        void SetPower(int value);
        void SetActive(bool isActive);
        void SetAlive(bool isAlive);
    }

    public interface IStatsObserver
    {
        void SetMaxHealth(int value);
        void SetCurrentHealth(int value);
        void SetPower(int value);
    }


    public class MVC_View : MonoBehaviour, IView, IStatsObserver
    {
        public event Action ON_ATTACK_CLICK;
        public event Action ON_HEAL_CLICK;

        private const string kHealthFormat = "{0}/{1}";
        private const int kMinHealthLimit = 0;

        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _power;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Button _healButton;
        [SerializeField] private Button _attackButton;

        private int _maxValue;
        private int _currentValue;

        public IStatsObserver StatsObserver => this;

        public void Show(Action callback = null)
        {
            gameObject.SetActive(true);
            _attackButton.onClick.AddListener(OnAttackButtonClick);
            _healButton.onClick.AddListener(OnHealButtonClick);
            callback?.Invoke();
        }

        public void Hide(Action callback = null)
        {
            gameObject.SetActive(false);
            _attackButton.onClick.RemoveListener(OnAttackButtonClick);
            _healButton.onClick.RemoveListener(OnHealButtonClick);
            callback?.Invoke();
        }

        public void SetActive(bool isActive)
        {
            _healButton.interactable = isActive;
            _attackButton.interactable = isActive;
        }

        public void SetMaxHealth(int value)
        {
            _maxValue = value;
            _healthBar.maxValue = value;
        }

        public void SetCurrentHealth(int value)
        {
            value = Mathf.Clamp(value, kMinHealthLimit, _maxValue);
            _currentValue = value;
            _healthBar.value = value;
            _health.text = string.Format(kHealthFormat, _currentValue, _maxValue);
        }

        public void SetName(string name)
        {
            _name.text = name;
        }

        public void SetPower(int value)
        {
            _power.text = value.ToString();
        }

        public void SetAlive(bool isAlive)
        {

        }

        private void OnAttackButtonClick()
        {
            ON_ATTACK_CLICK?.Invoke();
        }

        private void OnHealButtonClick()
        {
            ON_HEAL_CLICK?.Invoke();
        }
    }

    public enum Player
    {
        Left,
        Right
    }
}

