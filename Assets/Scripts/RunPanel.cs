using System;
using Core;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class RunPanel : MonoBehaviour, MMEventListener<RunGameEvent>
    {
        [SerializeField] private TextMeshProUGUI TextMeshPro;

        private int _count;
        private float _timer;

        private void Start()
        {
            _count = 3;
            _timer = 1;
            TextMeshPro.text = _count.ToString();
        }

        private void Update()
        {
            switch (GameManager.Instance.CurrentRun)
            {
                case MoveStatus.Idle:
                    if (_timer < 0)
                    {
                        _timer = 1;
                        _count -= 1;
                        TextMeshPro.text = _count.ToString();
                    }
                    else
                    {
                        _timer -= Time.deltaTime;
                    }

                    break;
                case MoveStatus.Run:
                    TextMeshPro.text = GameManager.Instance.RunClock.ToString("0.00");
                    break;
            }
        }

        /// <summary>
        /// OnDisable, we start listening to events.
        /// </summary>
        protected virtual void OnEnable()
        {
            // this.MMEventStartListening<CoreGameEvent>();
            this.MMEventStartListening<RunGameEvent>();
        }

        /// <summary>
        /// OnDisable, we stop listening to events.
        /// </summary>
        protected virtual void OnDisable()
        {
            // this.MMEventStopListening<CoreGameEvent>();
            this.MMEventStopListening<RunGameEvent>();
        }

        public void OnMMEvent(RunGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case RunEventTypes.RunStart:
                    break;
                case RunEventTypes.Encounter:
                    break;
                case RunEventTypes.Continue:
                    break;
            }
        }
    }
}