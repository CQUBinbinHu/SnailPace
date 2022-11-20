using Core;
using DefaultNamespace;
using UnityEngine;

namespace HeroPerform
{
    public class Attack : SkillComponent, IUsePerform
    {
        public int Atk;

        public override void OnUse()
        {
            Target.Health.TakeDamage(Atk);
            BattleManager.Instance.BattleCallBack(Owner.CharacterType);
        }

        public override void OnCancel()
        {
        }
    }
}