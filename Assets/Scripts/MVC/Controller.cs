namespace PatternMVC
{
    public interface IController
    {
        void Init(IModel model, IView view);
        void Play();
        void Complete();
        void ChangeModel(IModel model);
        void ChangeView(IView view);
    }


    public class Controller : IController
    {
        private IModel _model;
        private IView _view;

        public void Init(IModel model, IView view)
        {
            _model = model;
            _view = view;
            InitView();
        }

        public void Play()
        {
            _view.SetActive(false);
            _view.Show(()=>
            {
                _view.SetActive(true);
            });
        }

        public void Complete()
        {
            _view.Hide(()=>
            {
                _view.Release();
            });
        }

        public void DoDamage()
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

        public void DoHeal()
        {
            var health = _model.CurrentHealth;
            var healValue = _model.Power;
            health += healValue;
            _model.ChangeCurrentHealth(health);
        }

        public void ChangeModel(IModel model)
        {
            _model.Unsubscribe(_view.StatsObserver);
            _view.ON_ATTACK_CLICK -= DoDamage;
            _view.ON_HEAL_CLICK -= DoHeal;
            _model = model;
            InitView();
        }

        public void ChangeView(IView view)
        {
            _model.Unsubscribe(_view.StatsObserver);
            _view.ON_ATTACK_CLICK -= DoDamage;
            _view.ON_HEAL_CLICK -= DoHeal;
            _view.HideImmediately();
            _view = view;
            InitView();
            _view.ShowImmediately();
        }

        private void InitView()
        {
            _view.SetName(_model.Name);
            _view.SetMaxHealth(_model.MaxHealth);
            _view.SetCurrentHealth(_model.CurrentHealth);
            _view.SetPower(_model.Power);

            _model.Subscribe(_view.StatsObserver);
            _view.ON_ATTACK_CLICK += DoDamage;
            _view.ON_HEAL_CLICK += DoHeal;
        }
    }
}

