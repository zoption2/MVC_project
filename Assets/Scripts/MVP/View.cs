using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace PatternMVP
{
    public class View: MonoBehaviour
    {
        private const string kHealthFormat = "{0}/{1}";

        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _health;
        [SerializeField] private TMP_Text _power;
        [SerializeField] private TMP_Text _points;
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

