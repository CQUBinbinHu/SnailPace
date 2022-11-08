using System;
using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class HeroTriggerBox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            CoreGameEvent.Trigger(CoreGameEventTypes.EncounterBattle);
            Destroy(other.attachedRigidbody.gameObject);
        }
    }
}