using UnityEngine;

public class fengbaotouAnimationTrigger : MonoBehaviour

{
    private EnemyRobort0 enemyRobort0 => GetComponentInParent<EnemyRobort0>();



    private void OpenCounterWindow()
    {
        enemyRobort0.OpenCounterAttackWindow();
        //enemyRobort0.CounterAttackImage.SetActive(true);
    }
    private void CloseCounterWindow()
    {
        enemyRobort0.CloseCounterAttackWindow();
        //enemyRobort0.CounterAttackImage.SetActive(false);
    }

    private void OpenFirstCollider()
    {
        enemyRobort0.OpenFanwei(0);
    }

    private void OpenSecondCollider()
    {
        enemyRobort0.OpenFanwei(1);
    }
    private void OpenThirdColloder()
    {
        enemyRobort0.OpenFanwei(2);
    }


    private void UseSkill_boss1Skill1()
    {
        enemyRobort0.Boss1Skill1();

    }



}





