using System;
using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class HeroTriggerBox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            BattleManager.Instance.SetEncounter(other.attachedRigidbody.GetComponent<Character>());
            RunGameEvent.Trigger(RunEventTypes.Encounter);
        }
    }
}