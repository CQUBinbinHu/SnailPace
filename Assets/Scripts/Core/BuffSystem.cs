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
            Buff buff = null;
            switch (buffType)
            {
                case BuffType.Week:
                    buff = target.gameObject.AddComponent<WeekBuff>();
                    break;
                case BuffType.Enhancement:
                    buff = target.gameObject.AddComponent<EnhancementBuff>();
                    break;
                case BuffType.Vulnerable:
                    buff = target.gameObject.AddComponent<VulnerableBuff>();
                    break;
            }

            if (buff != null)
            {
                buff.OnAddBuff(target);
            }
        }
    }
}