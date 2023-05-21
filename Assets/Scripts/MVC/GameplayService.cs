using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace PatternMVC
{
    public interface IGameplayService
    {
        void Attack(Player player);
        void Heal(Player player);
        void CompleteTurn();
        void SetDead(Player player);
    }


    public class GameplayService : MonoBehaviour, IGameplayService
    {
        private const string kWinFormat = "{0} win the battle in {1} turns!";
        private const string kRoundFormat = "Round: {0}";

        [SerializeField] private Model _leftPlayerModel;
        [SerializeField] private Model _rightPlayerModel;
        [SerializeField] private MVC_View _leftPlayerView;
        [SerializeField] private MVC_View _rightPlayerView;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TMP_Text _winText;
        [SerializeField] private TMP_Text _turnText;

        private IController _leftPlayer;
        private IController _rightPlayer;
        private IModel _leftModel;
        private IModel _rightModel;
        private IView _leftView;
        private IView _rightView;

        private int _turn = 1;
        private bool _isGameEnded;
        private Queue<IController> _playersQueue;

        public void StartGameplay()
        {
            _isGameEnded = false;
            _startGameButton.gameObject.SetActive(false);
            _leftModel = _leftPlayerModel;
            _rightModel = _rightPlayerModel;
            _leftView = _leftPlayerView;
            _rightView = _rightPlayerView;

            _leftPlayer = GetPlayer(Player.Left);
            _leftPlayer.Init();
            _rightPlayer = GetPlayer(Player.Right);
            _rightPlayer.Init();

            PrepareRound();
            StartTurn();
        }

        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(StartGameplay);
        }

        private void OnDisable()
        {
            _startGameButton.onClick.RemoveListener(StartGameplay);
        }

        public void Attack(Player player)
        {
            switch (player)
            {
                case Player.Left:
                    _rightPlayer.DoDamage(_leftModel.Power);
                    break;
                case Player.Right:
                    _leftPlayer.DoDamage(_rightModel.Power);
                    break;
            }
        }

        public void Heal(Player player)
        {
            switch (player)
            {
                case Player.Left:
                    _leftPlayer.DoHeal(_leftModel.Power);
                    break;
                case Player.Right:
                    _rightPlayer.DoHeal(_rightModel.Power);
                    break;
            }
        }

        public void SetDead(Player player)
        {
            switch (player)
            {
                case Player.Left:
                    _winText.text = string.Format(kWinFormat, _rightPlayerModel.Name, _turn);
                    break;
                case Player.Right:
                    _winText.text = string.Format(kWinFormat, _leftPlayerModel.Name, _turn);
                    break;
            }
            _isGameEnded = true;
        }

        public void CompleteTurn()
        {
            if (_isGameEnded)
            {
                EndGame();
                return;
            }

            if (_playersQueue.Count > 0)
            {
                StartTurn();
            }
            else
            {
                PrepareRound();
                StartTurn();
            }
        }

        private void PrepareRound()
        {
            _turnText.text = string.Format(kRoundFormat, _turn);
            _playersQueue = new(2);
            _playersQueue.Enqueue(_leftPlayer);
            _leftPlayer.Wait();
            _playersQueue.Enqueue(_rightPlayer);
            _rightPlayer.Wait();
            _turn++;
        }

        private void StartTurn()
        {
            var player = _playersQueue.Dequeue();
            player.Play();
        }

        private void EndGame()
        {
            _startGameButton.gameObject.SetActive(true);
            _turn = 1;
            _leftPlayer.Reset();
            _rightPlayer.Reset();
        }

        private IController GetPlayer(Player player)
        {
            IModel model = null;
            IView view = null;
            IController controller = null;
            switch (player)
            {
                case Player.Left:
                    model = _leftPlayerModel;
                    view = _leftPlayerView;
                    controller = new Controller(model, view, Player.Left, this);
                    controller.Init();
                    return controller;

                case Player.Right:
                    model = _rightPlayerModel;
                    view = _rightPlayerView;
                    controller = new Controller(model, view, Player.Right, this);
                    controller.Init();
                    return controller;
            }

            throw new System.NotImplementedException(
                string.Format("Case for player: {0} is not implemented", player)
                );
        }


        [ContextMenu("Update left player Model")]
        private void ChangeLeftModel()
        {
            if (!_leftModel.Equals(_leftPlayerModel))
            {
                _leftPlayer.ChangeModel(_leftPlayerModel);
                Debug.Log("Left player model changed!");
            }
        }

        [ContextMenu("Update right player Model")]
        private void ChangeRightModel()
        {
            if (!_rightModel.Equals(_rightPlayerModel))
            {
                _rightPlayer.ChangeModel(_rightPlayerModel);
                Debug.Log("Right player model changed!");
            }
        }

        [ContextMenu("Update left player View")]
        private void ChangeLeftView()
        {
            if (!_leftView.Equals(_leftPlayerView))
            {
                _leftPlayer.ChangeView(_leftPlayerView);
                Debug.Log("Left player view changed!");
            }
        }

        [ContextMenu("Update right player View")]
        private void ChangeRightView()
        {
            if (!_rightView.Equals(_rightPlayerView))
            {
                _rightPlayer.ChangeView(_rightPlayerView);
                Debug.Log("Right player view changed!");
            }
        }
    }
}

