using UnityEngine;

namespace Core
{
    public class HeroBehaviourController : BehaviourController
    {
        public override void Initialize()
        {
            base.Initialize();
            SetTarget(BattleManager.Instance.EncounterEnemy);
        }
    }
}