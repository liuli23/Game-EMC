using TMPro;
using UnityEngine;

public class UISkillToolTip : UIToolTip
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;

    public void ShowToolTip(string _skillDescription , string _skillName ,int cost)
    {
        skillText.text = _skillDescription;
        skillName.text = _skillName;
        skillCost.text = "Cost:" + cost.ToString();
        //AdjustPosition();技能我不想做动态窗
        gameObject.SetActive(true);

    }
    public void HideToolTip() =>gameObject.SetActive(false);

}
