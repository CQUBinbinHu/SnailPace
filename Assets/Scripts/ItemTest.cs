using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public class ItemTest : Item
    {
        public override void OnUse()
        {
            base.OnUse();
            Destroy(gameObject);
        }
    }
}