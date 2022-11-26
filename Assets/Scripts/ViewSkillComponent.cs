using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class ViewSkillComponent : MonoBehaviour
    {
        private Scrollbar[] _scrollbars;

        private void Awake()
        {
            _scrollbars = GetComponentsInChildren<Scrollbar>();
        }

        private void OnEnable()
        {
            foreach (var scrollbar in _scrollbars)
            {
                scrollbar.value = 1;
            }
        }
    }
}