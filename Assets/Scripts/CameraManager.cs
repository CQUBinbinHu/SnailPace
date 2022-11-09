using System;
using Cinemachine;
using Core;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace DefaultNamespace
{
    public class CameraManager : MonoBehaviour, MMEventListener<RunGameEvent>
    {
        [SerializeField] private int assetsPPU_Run = 120;
        [SerializeField] private int assetsPPU_Foc = 150;

        private const float FocusDuration = 0.8f;

        // private CinemachineVirtualCamera _camera;
        private PixelPerfectCamera _pixel;

        private void Awake()
        {
            // _camera = GetComponent<CinemachineVirtualCamera>();
            _pixel = GetComponent<PixelPerfectCamera>();
        }

        private void Start()
        {
            // _camera.m_Lens.OrthographicSize = _orthoSize;
            _pixel.assetsPPU = assetsPPU_Run;
        }

        public void OnMMEvent(CoreGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case CoreGameEventTypes.Start:
                    break;
                case CoreGameEventTypes.GameOver:
                    break;
                case CoreGameEventTypes.Pause:
                    break;
            }
        }

        public void OnMMEvent(RunGameEvent eventType)
        {
            switch (eventType.EventType)
            {
                case RunEventTypes.RunStart:
                    break;
                case RunEventTypes.Encounter:
                    DoEncounter();
                    break;
                case RunEventTypes.Continue:
                    DoContinue();
                    break;
            }
        }

        private void DoContinue()
        {
            DOTween.To(() => _pixel.assetsPPU,
                (x) => _pixel.assetsPPU = x,
                assetsPPU_Run,
                FocusDuration);
        }

        private void DoEncounter()
        {
            DOTween.To(() => _pixel.assetsPPU,
                (x) => _pixel.assetsPPU = x,
                assetsPPU_Foc,
                FocusDuration);
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
    }
}