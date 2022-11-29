using System;
using Core;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class UpdateItemNum : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private void Update()
        {
            _textMeshProUGUI.text = BattleManager.Instance.CurrentRewardNum.ToString();
        }
    }
}