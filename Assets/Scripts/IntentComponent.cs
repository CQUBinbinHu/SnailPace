using System;
using System.Collections.Generic;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public enum Intent
    {
        None,
        Attack,
        Defence,
        UnKnown
    }

    public class IntentComponent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI AttackText;
        [SerializeField] private TextMeshProUGUI CountDownText;
        [SerializeField] private GameObject CountDown;
        [SerializeField] private Image CountDownImage;
        [SerializeField] private Image IntentAttack;
        [SerializeField] private Image IntentDefence;
        [SerializeField] private Image IntentUnKnown;
        private BehaviourController _behaviourController;
        private Dictionary<Intent, Image> _intents;

        private void Awake()
        {
            _behaviourController = GetComponentInParent<BehaviourController>();
            _intents = new Dictionary<Intent, Image>();
            _intents.Add(Intent.Attack, IntentAttack);
            _intents.Add(Intent.Defence, IntentDefence);
            _intents.Add(Intent.UnKnown, IntentUnKnown);
        }

        public void SetIntent(Intent intent, int attack = 0)
        {
            AttackText.text = String.Empty;
            foreach (var intentPair in _intents)
            {
                intentPair.Value.enabled = false;
            }

            if (_intents.ContainsKey(intent))
            {
                _intents[intent].enabled = true;
            }

            switch (intent)
            {
                case Intent.Attack:
                    AttackText.text = attack.ToString();
                    break;
            }
        }

        private void Update()
        {
            if (_behaviourController.IsOnCountDown)
            {
                CountDown.SetActive(true);
                CountDownImage.fillAmount = _behaviourController.CountDownRatio;
                CountDownText.text = _behaviourController.CountDown.ToString();
            }
            else
            {
                CountDown.SetActive(false);
            }
        }
    }
}