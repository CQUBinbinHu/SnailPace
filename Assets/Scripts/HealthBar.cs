using System;
using System.Collections;
using Core;
using DG.Tweening;
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
        [SerializeField] private SpriteRenderer ArmorIcon;
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
            var color = Color.white;
            color.a = 0;
            ArmorIcon.color = color;
            WhiteBarImage.fillAmount = _healthComponent.GetArmorRatio();
            CurrentRatio = _healthComponent.GetHpRatio();
            HealthBarImage.fillAmount = CurrentRatio;
            DamageBarImage.fillAmount = CurrentRatio;
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
            CurrentRatio = _healthComponent.GetHpRatio();
            ArmorIcon.DOFade(_healthComponent.IsWithArmor ? 1 : 0, 0.1f);
            HealthBarImage.fillAmount = CurrentRatio;
            WhiteBarImage.fillAmount = _healthComponent.GetArmorRatio();
            StartCoroutine(UpdateDamageDelay_Cro(Delay));
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