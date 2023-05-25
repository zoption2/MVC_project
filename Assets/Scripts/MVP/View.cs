using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

namespace PatternMVP
{
    public interface IDataObserver
    {
        //void SetName(string value);
        //void SetMaxHealth(int value);
        //void SetCurrentHealth(int value);
        //void SetPower(int value);
        //void SetFreePoints(int value);

        void UpdateData(IObservableData data);
    }

    public interface IView : IDataObserver
    {
        void Init(IPresenter presenter);
        void Show(Action callback = null);
        void Hide(Action callback = null);
        void SetActive(bool isActive);
        void ActivateStatsPanel(bool isActive);
        void ShowImmediately();
        void HideImmediately();
        void Release();
    }

    public class View: MonoBehaviour, IView
    {
        private const int kMinPoints = 0;
        private const float kAnimationDuration = 1;
        private const float kShowedScale = 2;
        private const float kHidedScale = 0;
        private const string kHealthFormat = "{0}/{1}";

        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _power;
        [SerializeField] private TMP_Text _points;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Button _healButton;
        [SerializeField] private Button _attackButton;

        [SerializeField] private GameObject _statsPanel;
        [SerializeField] private TMP_Text _freePoints;
        [SerializeField] private Button _plusHealthButton;
        [SerializeField] private Button _minusHealthButton;
        [SerializeField] private Button _plusPowerButton;
        [SerializeField] private Button _minusPowerButton;
        [SerializeField] private Button _applyStatsButton;
        [SerializeField] private Button _cancelStatsButton;

        private int _maxValue;
        private int _currentValue;
        private IPresenter _presenter;


        public void Init(IPresenter presenter)
        {
            _presenter = presenter;
        }

        public void Show(Action callback = null)
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            AnimateShowing(() =>
            {
                SubscribeButtons();
                callback?.Invoke();
            });
        }

        public void Hide(Action callback = null)
        {
            UnsubscribeButtons();
            AnimateHiding(() =>
            {
                gameObject.SetActive(false);
                callback?.Invoke();
            });
        }

        public void ShowImmediately()
        {

        }

        public void HideImmediately()
        {
            
        }

        public void Release()
        {
            Destroy(gameObject);
        }

        public void SetActive(bool isActive)
        {
            _healButton.interactable = isActive;
            _attackButton.interactable = isActive;
            ActivateStatsPanel(isActive);
        }

        public void ActivateStatsPanel(bool isActive)
        {
            _statsPanel.SetActive(isActive);
        }

        public void UpdateData(IObservableData data)
        {
            SetMaxHealth(data.MaxHealth);
            SetCurrentHealth(data.CurrentHealth);
            SetFreePoints(data.FreePoints);
            SetPower(data.Power);
            SetName(data.Name);
        }

        private void SetCurrentHealth(int value)
        {
            _currentValue = value;
            _healthBar.value = value;
            _health.text = string.Format(kHealthFormat, _currentValue, _maxValue);
        }

        private void SetFreePoints(int value)
        {
            _freePoints.text = value.ToString();
        }

        private void SetMaxHealth(int value)
        {
            _maxValue = value;
            _healthBar.maxValue = value;
        }

        private void SetPower(int value)
        {
            _power.text = value.ToString();
        }

        private void SetName(string name)
        {
            _name.text = name;
        }

        private void SubscribeButtons()
        {
            _plusHealthButton.onClick.AddListener(MaxHealthUp);
            _minusHealthButton.onClick.AddListener(MaxHealthDown);
            _plusPowerButton.onClick.AddListener(PowerUp);
            _minusPowerButton.onClick.AddListener(PowerDown);
            _applyStatsButton.onClick.AddListener(ApplyStatsChanges);
            _cancelStatsButton.onClick.AddListener(CancelStatsChanges);
        }

        private void UnsubscribeButtons()
        {
            _plusHealthButton.onClick.RemoveListener(MaxHealthUp);
            _minusHealthButton.onClick.RemoveListener(MaxHealthDown);
            _plusPowerButton.onClick.RemoveListener(PowerUp);
            _minusPowerButton.onClick.RemoveListener(PowerDown);
            _applyStatsButton.onClick.RemoveListener(ApplyStatsChanges);
            _cancelStatsButton.onClick.RemoveListener(CancelStatsChanges);
        }

        private void ApplyStatsChanges()
        {
            _presenter.ApplyStats();
        }

        private void CancelStatsChanges()
        {
            _presenter.CancelStats();
        }

        private void MaxHealthUp()
        {
            _presenter.MaxHealthUp();
            ValidateStatsInputs();
        }

        private void MaxHealthDown()
        {
            _presenter.MaxHealthDown();
            ValidateStatsInputs();
        }

        private void PowerUp()
        {
            _presenter.PowerUp();
            ValidateStatsInputs();
        }

        private void PowerDown()
        {
            _presenter.PowerDown();
            ValidateStatsInputs();
        }

        private void ValidateStatsInputs()
        {
            bool isFreePointsAvailable = _presenter.PowerPoints > kMinPoints;
            _plusHealthButton.interactable = isFreePointsAvailable;
            _plusPowerButton.interactable = isFreePointsAvailable;

            bool isHealthPointsExist = _presenter.MaxHealthPoints > kMinPoints;
            _minusHealthButton.interactable = isHealthPointsExist;

            bool isPowerPointsExist = _presenter.PowerPoints > kMinPoints;
            _minusPowerButton.interactable = isPowerPointsExist;
        }

        private void AnimateShowing(Action callback)
        {
            transform.DOScale(kShowedScale, kAnimationDuration).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }

        private void AnimateHiding(Action callback)
        {
            transform.DOScale(kHidedScale, kAnimationDuration).SetEase(Ease.Linear).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }
    }
}

