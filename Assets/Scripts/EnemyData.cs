using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "SnailPace/EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public GameObject[] Enemys;
    }
}