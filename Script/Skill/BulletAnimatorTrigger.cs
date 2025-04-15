using UnityEngine;

public class BulletAnimatorTrigger : MonoBehaviour
{
    private CrystalSkillController skillController => GetComponentInParent<CrystalSkillController>();

    private void SelfDestory()
    {
        skillController.SelfDestory();
    }
    private void AnimationExplodeEvent()
    {
        skillController.AnimationExplodeEvent();
    }

}
