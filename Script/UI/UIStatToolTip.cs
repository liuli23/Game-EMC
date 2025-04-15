using TMPro;
using UnityEngine;

public class UIStatToolTip : UIToolTip
{
    [SerializeField] private TextMeshProUGUI description;

    public void ShowStartToolTip(string _text)
    {
        description.text = _text;
        AdjustPosition();


        gameObject.SetActive(true);

    }


    public void HidStatToolTip()
    {
        description.text = "";
        gameObject.SetActive(false);


    }


}
