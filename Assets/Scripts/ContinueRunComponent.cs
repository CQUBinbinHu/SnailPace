using System;
using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class ContinueRunComponent : MonoBehaviour
    {
        public void OnContinue()
        {
            RunGameEvent.Trigger(RunEventTypes.Continue);
        }
    }
}