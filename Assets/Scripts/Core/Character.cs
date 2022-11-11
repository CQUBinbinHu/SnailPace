using UnityEngine;

namespace Core
{
    public enum CharacterType
    {
        Hero,
        Enemy
    }

    public class Character : MonoBehaviour
    {
        [SerializeField] public CharacterType CharacterType;
        [SerializeField] private string Name;
    }
}