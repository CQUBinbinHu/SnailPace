using UnityEngine;

namespace Core
{
    public class SleepBehaviour : BaseBehaviour
    {
        public override void Perform()
        {
            BattleManager.Instance.BattleCallBack(Character.CharacterType);
        }
    }
}