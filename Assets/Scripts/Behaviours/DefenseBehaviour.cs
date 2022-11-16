﻿using UnityEngine;

namespace Core
{
    public class DefenseBehaviour : BaseBehaviour
    {
        public int Armor;

        public override void Perform()
        {
            Controller.CoolDownTimer = CoolDown;
            BattleManager.Instance.OnPlayerContinue(Character.CharacterType);
        }
    }
}