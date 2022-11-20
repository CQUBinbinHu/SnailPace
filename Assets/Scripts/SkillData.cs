using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "SnailPace/SkillData", order = 0)]
    public class SkillData : ScriptableObject
    {
        public SkillReward[] SkillRewards;
    }
}