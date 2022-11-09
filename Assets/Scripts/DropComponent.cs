using UnityEngine;

namespace DefaultNamespace
{
    public class DropComponent : MonoBehaviour
    {
        private Item _item;

        private void Awake()
        {
            _item = GetComponent<Item>();
        }

        public void OnDrop()
        {
            _item.OnUse();
        }
    }
}