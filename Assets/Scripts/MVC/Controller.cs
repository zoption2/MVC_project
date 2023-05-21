namespace PatternMVC
{
    public interface IController
    {
        void Init();
        void Play();
        void Wait();
        void Reset();
        void DoDamage(int value);
        void DoHeal(int value);
        void ChangeModel(IModel model);
        void ChangeView(IView view);
    }


    public class Controller : IController
    {
        private IModel _model;
        private IView _view;
        private readonly IGameplayService _gameplayService;
        private readonly Player _player;

        public Controller(IModel model, IView view, Player player, IGameplayService gameplayService)
        {
            _gameplayService = gameplayService;
            _player = player;
            _model = model;
            _view = view;
        }

        public void Init()
        {
            _model.Init();
            InitView();
        }

        public void Play()
        {
            _view.SetActive(true);
        }

        public void Wait()
        {
            _view.SetActive(false);
        }

        public void Reset()
        {
            _model.DoReset();
        }

        public void DoDamage(int value)
        {
            var health = _model.CurrentHealth;
            health -= value;
            _model.ChangeCurrentHealth(health);
            if (health <= 0)
            {
                _view.SetAlive(false);
                _gameplayService.SetDead(_player);
            }
        }

        public void DoHeal(int value)
        {
            var health = _model.CurrentHealth;
            health += value;
            _model.ChangeCurrentHealth(health);
        }

        public void ChangeModel(IModel model)
        {
            _model.Unsubscribe(_view.StatsObserver);
            _view.ON_ATTACK_CLICK -= DoOnAttack;
            _view.ON_HEAL_CLICK -= DoOnHeal;
            _model = model;
            _model.Init();
            InitView();
        }

        public void ChangeView(IView view)
        {
            _model.Unsubscribe(_view.StatsObserver);
            _view.ON_ATTACK_CLICK -= DoOnAttack;
            _view.ON_HEAL_CLICK -= DoOnHeal;
            _view = view;
            InitView();
        }

        private void InitView()
        {
            _view.SetName(_model.Name);
            _view.SetMaxHealth(_model.MaxHealth);
            _view.SetCurrentHealth(_model.CurrentHealth);
            _view.SetPower(_model.Power);

            _model.Subscribe(_view.StatsObserver);
            _view.ON_ATTACK_CLICK += DoOnAttack;
            _view.ON_HEAL_CLICK += DoOnHeal;

            _view.Show();
        }

        private void DoOnAttack()
        {
            _gameplayService.Attack(_player);
            _view.SetActive(false);
            _gameplayService.CompleteTurn();
        }

        private void DoOnHeal()
        {
            _gameplayService.Heal(_player);
            _view.SetActive(false);
            _gameplayService.CompleteTurn();
        }
    }
}

