using UnityEngine;

namespace PatternMVC
{
    public interface IGameplayService
    {
        void Play();
        void Stop();
        void ChangeModel();
        void ChangeView();
    }


    public class GameplayService : MonoBehaviour, IGameplayService
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private View _playerView;
        [SerializeField] private Transform _viewHolder;

        private IController _controller;
        private IModel _model;
        private IView _view;


        public void Play()
        {
            _controller = CreatePlayer();
            _controller.Play();
        }

        public void Stop()
        {
            _controller.Complete();
            _model = null;
            _controller = null;
        }

        public void ChangeModel()
        {
            if (_controller != null)
            {
                var model = new Model(_playerData);
                _controller.ChangeModel(model);
            }
        }

        public void ChangeView()
        {
            if (_controller != null)
            {
                var oldView = _view;
                _view = Instantiate(_playerView, _viewHolder);
                _controller.ChangeView(_view);
                oldView.Release();
            }
        }

        private IController CreatePlayer()
        {
            _model = new Model(_playerData);
            _view = Instantiate(_playerView, _viewHolder);
            var controller = new Controller();
            controller.Init(_model, _view);
            return controller;
        }
    }
}

