﻿using System;
using System.Collections;
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

        [Range(0, 1)] public float CurrentRatio;

        private const float Duration = 0.5f;
        private const float Delay = 0.1f;

        private void Start()
        {
            CurrentRatio = 1;
            UpdateDamageBar();
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

        private void UpdateDamageBar()
        {
            HealthBarImage.fillAmount = CurrentRatio;
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