using DefaultNamespace;
using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    public static class BuffSystem
    {
        public static void AddBuff(this Character target, BuffType buffType)
        {
            switch (buffType)
            {
                case BuffType.Week:
                    target.gameObject.AddComponent<WeekBuff>();
                    break;
                case BuffType.Enhancement:
                    target.gameObject.AddComponent<EnhancementBuff>();
                    break;
                case BuffType.Vulnerable:
                    target.gameObject.AddComponent<VulnerableBuff>();
                    break;
            }
        }
    }
}