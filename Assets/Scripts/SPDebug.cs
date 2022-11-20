using System;
using Core;
using MoreMountains.Tools;
using UnityEngine;

namespace DefaultNamespace
{
    public class SPDebug : MMSingleton<SPDebug>
    {
        private GameObject _encounterGo;

        public void EncounterGo(GameObject go)
        {
            _encounterGo = go;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (BattleManager.Instance.EncounterEnemy)
                {
                    BattleManager.Instance.EncounterEnemy.Health.TakeDamage(1000);
                }
            }
        }
    }
}