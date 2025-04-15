using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;


    public DashSkill dash { get; private set;}
    public CloneSkill clone { get; private set;}
    public FireSkill fire { get; private set;}
    public BlackholeSkill blackhole { get; private set;}
    public CrystalSkill crystal { get; private set;}
    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        fire = GetComponent<FireSkill>();
        blackhole = GetComponent<BlackholeSkill>();
        crystal = GetComponent<CrystalSkill>();
    }

    private void Start()
    {
    }
}
