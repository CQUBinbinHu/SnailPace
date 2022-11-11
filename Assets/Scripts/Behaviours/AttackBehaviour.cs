﻿using UnityEngine;

namespace Core
{
    public class AttackBehaviour : BaseBehaviour
    {
        public int Atk;

        public override void Perform()
        {
            Controller.Target.Health.TakeDamage(Atk);
            Controller.CoolDownTimer = CoolDown;
            BattleManager.Instance.OnPlayerContinue(Character.CharacterType);
        }
    }
}