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
        private int _freePoints;
        private int _maxHealth;
        private int _power;

        public int FreePoints { get { return _freePoints; }}
        public int MaxHealthPoints { get { return _maxHealth; } }
        public int PowerPoints { get { return _power; } }

        public Presenter(IModel model, IView view)
        {
            _model = model;
            _view = view;

            UpdateData(_model);
            _model.Subscribe(this);
            InitView();
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
            }
        }

        public void ApplyStats()
        {
            _model.ChangeFreePoints(_freePoints);
            _model.ChangePower(_power);
            _model.ChangeMaxHealth(_maxHealth);
        }

        public void CancelStats()
        {
            UpdateData(_model);
        }

        public void MaxHealthDown()
        {
            _maxHealth--;
            _freePoints++;
        }

        public void MaxHealthUp()
        {
            _maxHealth++;
            _freePoints--;
        }

        public void PowerDown()
        {
            _power--;
            _freePoints++;
        }

        public void PowerUp()
        {
            _power++;
            _freePoints--;
        }

        public void UpdateData(IObservableData data)
        {
            _power = data.Power;
            _freePoints = data.FreePoints;
            _maxHealth = data.MaxHealth;

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
        }

        public void ChangeView(IView view)
        {
            _model.Unsubscribe(_view);
            _view = view;
            InitView();
        }

        private void InitView()
        {
            _view.UpdateData(_model);
            _model.Subscribe(_view);
            _view.Init(this);
        }
    }
}

