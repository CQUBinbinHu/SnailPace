using System;
using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class HeroTriggerBox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            var encounter = other.attachedRigidbody.GetComponent<Character>();
            switch (encounter.CharacterType)
            {
                case CharacterType.Enemy:
                    GameEventManager.Instance.OnRunEncounter?.Invoke(encounter);
                    break;
                case CharacterType.Winning:
                    GameEventManager.Instance.OnGameWinning?.Invoke();
                    break;
            }
        }
    }
}