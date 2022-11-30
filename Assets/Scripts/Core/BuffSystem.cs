using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    public static class BuffSystem
    {
        public static Buff AddBuff(this Character target, BuffType buffType, float duration = -1)
        {
            Buff buff = null;
            switch (buffType)
            {
                case BuffType.Cure:
                    if (target.BuffSocket.TryGetComponent(out CureBuff cureBuff))
                    {
                        if (cureBuff.IsBuffActivated)
                        {
                            cureBuff.OnOverride(duration);
                            break;
                        }
                    }

                    buff = target.BuffSocket.AddComponent<CureBuff>();
                    break;
                case BuffType.Week:
                    if (target.BuffSocket.TryGetComponent(out WeekBuff weekBuff))
                    {
                        if (weekBuff.IsBuffActivated)
                        {
                            weekBuff.OnOverride(duration);
                            break;
                        }
                    }

                    buff = target.BuffSocket.AddComponent<WeekBuff>();
                    break;
                case BuffType.Enhancement:
                    if (target.BuffSocket.TryGetComponent(out EnhancementBuff enhancementBuff))
                    {
                        if (enhancementBuff.IsBuffActivated)
                        {
                            enhancementBuff.OnOverride(duration);
                            break;
                        }
                    }

                    buff = target.BuffSocket.AddComponent<EnhancementBuff>();
                    break;
                case BuffType.Vulnerable:
                    if (target.BuffSocket.TryGetComponent(out VulnerableBuff vulnerableBuff))
                    {
                        if (vulnerableBuff)
                        {
                            vulnerableBuff.OnOverride(duration);
                            break;
                        }
                    }


                    buff = target.BuffSocket.AddComponent<VulnerableBuff>();
                    break;
                default:
                    Debug.LogWarning("Not find Buff " + buffType);
                    return null;
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