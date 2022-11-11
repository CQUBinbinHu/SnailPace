using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class CoolDownBar : MonoBehaviour
    {
        [SerializeField] private Image EnergyBarImage;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetLength(float len)
        {
            var size = _rectTransform.sizeDelta;
            size.x = len;
            _rectTransform.sizeDelta = size;
        }

        public void SetFillAmount(float fill)
        {
            EnergyBarImage.fillAmount = Mathf.Clamp01(fill);
        }
    }
}