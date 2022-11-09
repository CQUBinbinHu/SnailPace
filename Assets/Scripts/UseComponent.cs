using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class UseComponent : MonoBehaviour
    {
        private Item _item;

        private void Awake()
        {
            _item = GetComponent<Item>();
        }

        public void OnUse()
        {
            _item.OnUse();
        }
    }
}