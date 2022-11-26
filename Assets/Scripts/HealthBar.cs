using System;
using System.Collections;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image HealthBarImage;
        [SerializeField] private Image DamageBarImage;
        [SerializeField] private Image WhiteBarImage;
        [SerializeField] private Image ArmorImage;
        [SerializeField] private Image ArmorBackGround;
        [SerializeField] private Image ArmorCountDown;
        [SerializeField] private TextMeshProUGUI HealthAmount;
        [SerializeField] private TextMeshProUGUI ArmorAmount;
        [Range(0, 1)] public float CurrentRatio;

        private const float Duration = 0.4f;
        private const float Delay = 0.1f;
        private HealthComponent _healthComponent;

        private void Awake()
        {
            _healthComponent = GetComponentInParent<HealthComponent>();
        }

        public void Initialize()
        {
            ArmorImage.enabled = false;
            WhiteBarImage.fillAmount = _healthComponent.GetArmorRatio();
            CurrentRatio = _healthComponent.GetHpRatio();
            HealthBarImage.fillAmount = CurrentRatio;
            DamageBarImage.fillAmount = CurrentRatio;
            HealthAmount.text = _healthComponent.CurrentHp
                                + "/"
                                + _healthComponent.MaxHp;
            int armors = _healthComponent.Armors;
            ArmorAmount.text = armors == 0 ? string.Empty : armors.ToString();
        }

        private void Update()
        {
// debug
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.C))
            {
                CurrentRatio -= Random.Range(0.2f, 0.8f);
                CurrentRatio = Mathf.Clamp01(CurrentRatio);
                UpdateDamageBar();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                CurrentRatio = 1;
                UpdateDamageBar();
            }
#endif
        }

        public void UpdateDamageBar()
        {
            if (_healthComponent.IsDead)
            {
                return;
            }

            CurrentRatio = _healthComponent.GetHpRatio();
            HealthBarImage.fillAmount = CurrentRatio;
            WhiteBarImage.fillAmount = _healthComponent.GetArmorRatio();
            HealthAmount.text = _healthComponent.CurrentHp
                                + "/"
                                + _healthComponent.MaxHp;
            int armors = _healthComponent.Armors;
            ArmorAmount.text = armors == 0 ? string.Empty : armors.ToString();

            StartCoroutine(UpdateDamageDelay_Cro(Delay));
        }

        public void UpdateArmorPresentation()
        {
            ArmorImage.enabled = _healthComponent.IsWithArmor;
            ArmorCountDown.enabled = _healthComponent.IsWithArmor;
            ArmorBackGround.enabled = _healthComponent.IsWithArmor;
            ArmorCountDown.fillAmount = _healthComponent.ArmorCountDown;
        }

        IEnumerator UpdateDamageDelay_Cro(float delay)
        {
            yield return new WaitForSeconds(delay);
            DOTween.To(() => DamageBarImage.fillAmount,
                x => DamageBarImage.fillAmount = x,
                CurrentRatio,
                Duration);
        }
    }
}