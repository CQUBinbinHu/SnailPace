using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
    public abstract class Item : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private GameObject Options;

        private void OnEnable()
        {
            Options.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Options.SetActive(true);
            Debug.Log("OnPointerDown");
        }

        public virtual void OnDrop()
        {
            Destroy(gameObject);
            Debug.Log("OnDrop");
        }

        public virtual void OnUse()
        {
            Debug.Log("OnUse");
        }
    }
}