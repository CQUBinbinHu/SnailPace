using DefaultNamespace;
using MoreMountains.Tools;
using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    public static class BuffSystem
    {
        public static void AddBuff(this Character target, BuffType buffType, float duration = 1)
        {
            Buff buff = null;
            switch (buffType)
            {
                case BuffType.Week:
                    buff = target.BuffSocket.AddComponent<WeekBuff>();
                    break;
                case BuffType.Enhancement:
                    buff = target.BuffSocket.AddComponent<EnhancementBuff>();
                    break;
                case BuffType.Vulnerable:
                    buff = target.BuffSocket.AddComponent<VulnerableBuff>();
                    break;
                default:
                    Debug.LogWarning("Not find Buff " + buffType);
                    return;
            }

            if (buff != null)
            {
                buff.BuffType = buffType;
                buff.OnAddBuff(target, duration);
            }
        }
    }
}