using System;
using Cinemachine;
using Core;
using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;

namespace DefaultNamespace
{
    public class CameraManager : MonoBehaviour, MMEventListener<RunGameEvent>
    {
        [SerializeField] private float OrthoSize_Run = 4.5f;
        [SerializeField] private float OrthoSize_Focus = 3.0f;

        private CinemachineVirtualCamera _camera;
        private float _orthoSize;

        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Start()
        {
            _orthoSize = OrthoSize_Run;
            _camera.m_Lens.OrthographicSize = _orthoSize;
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
            DOTween.To(() => _camera.m_Lens.OrthographicSize,
                x => _camera.m_Lens.OrthographicSize = x,
                OrthoSize_Run,
                0.8f);
        }

        private void DoEncounter()
        {
            DOTween.To(() => _camera.m_Lens.OrthographicSize,
                x => _camera.m_Lens.OrthographicSize = x,
                OrthoSize_Focus,
                0.8f);
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