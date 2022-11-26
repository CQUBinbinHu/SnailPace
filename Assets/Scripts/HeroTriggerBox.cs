using System;
using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class HeroTriggerBox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            GameEventManager.Instance.OnRunEncounter?.Invoke(other.attachedRigidbody.GetComponent<Character>());
        }
    }
}