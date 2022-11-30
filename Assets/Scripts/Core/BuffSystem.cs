using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    public static class BuffSystem
    {
        public static Buff AddBuff<T>(this Character target, BuffType buffType, float duration = -1) where T : Buff
        {
            bool hasBuff = false;
            if (target.BuffSocket.TryGetComponent(out T buff))
            {
                if (buff.IsBuffActivated)
                {
                    hasBuff = true;
                    buff.OnOverride(duration);
                }
            }

            if (!hasBuff)
            {
                buff = target.BuffSocket.AddComponent<T>();
            }

            if (buff != null)
            {
                buff.BuffType = buffType;
                buff.OnAddBuff(target, duration);
            }

            return buff;
        }
    }
}