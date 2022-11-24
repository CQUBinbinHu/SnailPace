using System.Collections;
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
        [SerializeField] private Image BackGround;
        [SerializeField] private TextMeshProUGUI TipsText;
        [SerializeField] private GameObject HandleChild;
        [SerializeField] private Color InitColor;
        private float _duration;
        private const float FixedDelay = 1f;

        public void SetTips(string tips, float duration)
        {
            TipsText.text = tips;
            _duration = duration;
        }

        public void OnSpawn()
        {
            HandleChild.transform.position = Vector3.zero;
            BackGround.color = InitColor;
            TipsText.color = Color.white;
            BackGround.DOFade(0, _duration);
            TipsText.DOFade(0, _duration);
            HandleChild.transform.DOLocalJump(JumpVec, JumpPower, 1, _duration);
            StartCoroutine(DespawnDelay(_duration + FixedDelay));
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