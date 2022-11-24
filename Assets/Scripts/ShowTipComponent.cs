﻿using System.Collections;
using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ShowTipComponent : MonoBehaviour, IPoolable
    {
        [SerializeField] private Vector3 JumpVec;
        [SerializeField] private float JumpPower;
        [SerializeField] private TextMeshProUGUI TipsText;
        [SerializeField] private GameObject HandleChild;
        [SerializeField] private Color InitColor;
        private float _duration;
        private const float FixedDelay = 1f;

        public void SetTips(string tips, Color color, float duration = 1f)
        {
            TipsText.text = tips;
            TipsText.color = color;
            _duration = duration;
            TipsText.DOFade(0, _duration);
            HandleChild.transform.DOLocalJump(JumpVec, JumpPower, 1, _duration);
            StartCoroutine(DespawnDelay(_duration + FixedDelay));
        }

        public void OnSpawn()
        {
            HandleChild.transform.position = Vector3.zero;
        }

        IEnumerator DespawnDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            LeanPool.Despawn(this);
        }

        public void OnDespawn()
        {
        }
    }
}