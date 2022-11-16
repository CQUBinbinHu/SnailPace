using UnityEngine;

namespace Core
{
    public class DefenseBehaviour : BaseBehaviour
    {
        public int Armor;

        public override void Perform()
        {
            BattleManager.Instance.BattleCallBack(Character.CharacterType);
        }
    }
}