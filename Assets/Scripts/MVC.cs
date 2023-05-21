using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace PatternMVC
{
    public interface IView
    {
        void SetName(string name);
        void SetMaxHealth(int value);
        void SetCurrentHealth(int value);
        void SetPoints(int value);
        void ActivateSkillButtons()
    }

    public class View: MonoBehaviour, IView
    {
        private const string kHealthFormat = "{0}/{1}";

        [SerializeField] private Text _name;
        [SerializeField] private Text _health;
        [SerializeField] private Text _power;
        [SerializeField] private Text _points;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Button _healButton;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _plusHealthButton;
        [SerializeField] private Button _minusHealthButton;
        [SerializeField] private Button _plusPowerButton;
        [SerializeField] private Button _minusPowerButton;

        public void SetName(string name)
        {
            _name.text = name;
        }


    }
}

