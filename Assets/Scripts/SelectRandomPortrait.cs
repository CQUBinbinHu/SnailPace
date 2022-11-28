using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class SelectRandomPortrait : MonoBehaviour
    {
        [SerializeField] private GameObject[] ShowObjects;

        private void Awake()
        {
            foreach (var go in ShowObjects)
            {
                go.SetActive(false);
            }
        }

        private void OnEnable()
        {
            ShowObjects[Random.Range(0, ShowObjects.Length)].SetActive(true);
        }

        private void OnDisable()
        {
            foreach (var go in ShowObjects)
            {
                go.SetActive(false);
            }
        }
    }
}