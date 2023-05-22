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
        void StartGameplay();
        void CompleteTurn();
        void SetDead(Player player);
    }


    public class GameplayService : MonoBehaviour, IGameplayService
    {
        private const string kWinFormat = "{0} win the battle in {1} turns!";
        private const string kRoundFormat = "Round: {0}";

        [SerializeField] private PlayerData _leftPlayerData;
        [SerializeField] private PlayerData _rightPlayerData;
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
            _turn = 1;
            _playersQueue = new Queue<IController>();
            _isGameEnded = false;
            _startGameButton.gameObject.SetActive(false);

            _leftPlayer = GetPlayer(Player.Left);
            _rightPlayer = GetPlayer(Player.Right);

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
                    _winText.text = string.Format(kWinFormat, _rightPlayerData.Name, _turn);
                    break;
                case Player.Right:
                    _winText.text = string.Format(kWinFormat, _leftPlayerData.Name, _turn);
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
            DisplayRound(_turn);
            _playersQueue = new(2);
            _playersQueue.Enqueue(_leftPlayer);
            _leftPlayer.Wait();
            _playersQueue.Enqueue(_rightPlayer);
            _rightPlayer.Wait();
            _turn++;
        }

        private void DisplayRound(int round)
        {
            _turnText.text = string.Format(kRoundFormat, _turn);
        }

        private void StartTurn()
        {
            var player = _playersQueue.Dequeue();
            player.Play();
        }

        private void EndGame()
        {
            _leftPlayer.Complete();
            _rightPlayer.Complete();
            _startGameButton.gameObject.SetActive(true);
        }

        private IController GetPlayer(Player player)
        {
            IController controller = null;
            switch (player)
            {
                case Player.Left:
                    _leftModel = new Model(_leftPlayerData);
                    _leftView = _leftPlayerView;
                    controller = new Controller(Player.Left, this);
                    controller.Init(_leftModel, _leftView);
                    return controller;

                case Player.Right:
                    _rightModel = new Model(_rightPlayerData);
                    _rightView = _rightPlayerView;
                    controller = new Controller(Player.Right, this);
                    controller.Init(_rightModel, _rightView);
                    return controller;
            }

            throw new System.NotImplementedException(
                string.Format("Case for player: {0} is not implemented", player)
                );
        }


        [ContextMenu("Update left player Model")]
        private void ChangeLeftModel()
        {
            var model = new Model(_leftPlayerData);
            _leftPlayer.ChangeModel(model);
            Debug.Log("Left player model changed!");
        }

        [ContextMenu("Update right player Model")]
        private void ChangeRightModel()
        {
            var model = new Model(_rightPlayerData);
            _rightPlayer.ChangeModel(model);
            Debug.Log("Right player model changed!");
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

