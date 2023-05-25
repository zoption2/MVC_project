using System;

namespace PatternMVP
{
    public interface IPresenter : IDataObserver
    {
        int FreePoints { get; }
        int MaxHealthPoints { get; }
        int PowerPoints { get; }
        void Play();
        void Complete();
        void ChangeModel(IModel model);
        void ChangeView(IView view);

        void Attack();
        void Heal();
        void PowerUp();
        void PowerDown();
        void MaxHealthUp();
        void MaxHealthDown();
        void ApplyStats();
        void CancelStats();
    }


    public class Presenter : IPresenter
    {
        private IModel _model;
        private IView _view;
        private int _maxHealth;
        private int _power;

        private int _freePoints;
        private int _maxHealthPoints;
        private int _powerPoints;

        public int FreePoints { get { return _freePoints; }}
        public int MaxHealthPoints { get { return _maxHealthPoints; } }
        public int PowerPoints { get { return _powerPoints; } }

        public Presenter(IModel model, IView view)
        {
            _model = model;
            _view = view;

            InitView();
            UpdateData(_model);
            _model.Subscribe(this);
        }


        public void Play()
        {
            _view.SetActive(false);
            _view.Show(() =>
            {
                _view.SetActive(true);
            });
        }

        public void Complete()
        {
            _view.Hide(() =>
            {
                _view.Release();
            });
        }

        public void Heal()
        {
            var health = _model.CurrentHealth;
            var healValue = _model.Power;
            health += healValue;
            _model.ChangeCurrentHealth(health);
        }

        public void Attack()
        {
            var health = _model.CurrentHealth;
            var damageValue = _model.Power;
            health -= damageValue;
            _model.ChangeCurrentHealth(health);
            if (health <= 0)
            {
                _view.SetActive(false);
                _view.ActivateStatsPanel(false);
            }
        }

        public void ApplyStats()
        {
            _model.ChangeFreePoints(_freePoints);
            _model.ChangePower(_power + _powerPoints);
            _model.ChangeMaxHealth(_maxHealth + _maxHealthPoints);
            _freePoints = 0;
            _powerPoints = 0;
            _maxHealthPoints = 0;
            _view.UpdateData(_model);
            UpdateData(_model);
        }

        public void CancelStats()
        {
            _freePoints = 0;
            _powerPoints = 0;
            _maxHealthPoints = 0;
            _view.UpdateData(_model);
            UpdateData(_model);
        }

        public void MaxHealthDown()
        {
            _maxHealthPoints--;
            _freePoints++;
        }

        public void MaxHealthUp()
        {
            _maxHealthPoints++;
            _freePoints--;
        }

        public void PowerDown()
        {
            _powerPoints--;
            _freePoints++;
        }

        public void PowerUp()
        {
            _powerPoints++;
            _freePoints--;
        }

        public void UpdateData(IObservableData data)
        {
            _power = data.Power;
            _maxHealth = data.MaxHealth;
            _freePoints = data.FreePoints;

            var needStatsPanel = _freePoints > 0;
            _view.ActivateStatsPanel(needStatsPanel);
        }

        public void ChangeModel(IModel model)
        {
            _model.Unsubscribe(_view);
            _model.Unsubscribe(this);
            _model = model;
            model.Subscribe(this);
            InitView();
            UpdateData(_model);
        }

        public void ChangeView(IView view)
        {
            _model.Unsubscribe(_view);
            _view = view;
            InitView();
            UpdateData(_model);
        }

        private void InitView()
        {
            _view.Init(this);
            _view.UpdateData(_model);
            _model.Subscribe(_view);
        }
    }
}

