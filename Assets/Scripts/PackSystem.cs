using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace DefaultNamespace
{
    public class PackSystem : MonoBehaviour
    {
        [SerializeField] private GameObject ItemBase;
        [SerializeField] private GameObject HeroItems;
        [SerializeField] private GameObject RewardItems;
        [SerializeField] private GameObject RewardPanel;
        private HashSet<GameObject> _itemGOs;

        private void Awake()
        {
            _itemGOs = new HashSet<GameObject>();
        }

        private void Start()
        {
            RewardPanel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                RewardPanel.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                var item = Instantiate(ItemBase, HeroItems.transform);
                if (!_itemGOs.Contains(item))
                {
                    _itemGOs.Add(item);
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                foreach (var item in _itemGOs)
                {
                    Destroy(item);
                }
            }
        }

        public void OnContinue()
        {
            RewardPanel.SetActive(false);
        }
    }
}