using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class BuffIconData
    {
        public BuffType BuffType;
        public ShowBuffComponent ShowBuffComponent;
    }

    [CreateAssetMenu(fileName = "BuffShowData", menuName = "SnailPace/BuffShowData", order = 0)]
    public class BuffShowData : ScriptableObject
    {
        public BuffIconData[] BuffIconDatas;
        public Dictionary<BuffType, ShowBuffComponent> ShowBuffs;

        public void Initialize()
        {
            ShowBuffs = new Dictionary<BuffType, ShowBuffComponent>();
            foreach (var buffIconData in BuffIconDatas)
            {
                ShowBuffs.Add(buffIconData.BuffType, buffIconData.ShowBuffComponent);
            }
        }
    }
}