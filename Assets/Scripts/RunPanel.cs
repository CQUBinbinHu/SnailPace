using System;
using Core;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class RunPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI TextMeshPro;

        private void Update()
        {
            switch (GameManager.Instance.CurrentRun)
            {
                case MoveStatus.Idle:
                    TextMeshPro.text = GameManager.Instance.CountDown.ToString();
                    break;
                case MoveStatus.Run:
                case MoveStatus.Encounter:
                    TextMeshPro.text = GameManager.Instance.RunClock.ToString("0.00");
                    break;
            }
        }
    }
}