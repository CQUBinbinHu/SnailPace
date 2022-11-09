using System;
using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class ContinueRunComponent : MonoBehaviour
    {
        private PackSystem _packSystem;

        private void Awake()
        {
            _packSystem = GetComponent<PackSystem>();
        }

        public void OnContinue()
        {
            RunGameEvent.Trigger(RunEventTypes.Continue);
            _packSystem.OnContinue();
        }
    }
}