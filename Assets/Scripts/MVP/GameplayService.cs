using GeneralData;
using UnityEngine;

namespace PatternMVP
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

        private IPresenter _presenter;
        private IModel _model;
        private IView _view;


        public void Play()
        {
            _presenter = CreatePlayer();
            _presenter.Play();
        }

        public void Stop()
        {
            _presenter.Complete();
            _model = null;
            _presenter = null;
        }

        public void ChangeModel()
        {
            if (_presenter != null)
            {
                var model = new Model(_playerData);
                _presenter.ChangeModel(model);
            }
        }

        public void ChangeView()
        {
            if (_presenter != null)
            {
                var oldView = _view;
                _view = Instantiate(_playerView, _viewHolder);
                _presenter.ChangeView(_view);
                oldView.Release();
            }
        }

        private IPresenter CreatePlayer()
        {
            _model = new Model(_playerData);
            _view = Instantiate(_playerView, _viewHolder);
            var presenter = new Presenter(_model, _view);
            return presenter;
        }
    }
}

