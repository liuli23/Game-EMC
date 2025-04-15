using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/// <summary>
/// ��Ʊ�д�����ɹ�
/// ����ʱ�䣺2020/04/14
/// �ű����ܣ�˫��Ѫ��, ������ѪЧ��
/// ����λ�ã�����Slider��Ϸ���ϱȽϺ�
/// </summary>



// Ϊ�˽�ʡ����, ���õ���ʽ, ��������updateȥ���Ѫ���仯
public class HP_Sub_Tween : MonoBehaviour
{
    // [ʵ��Ѫ��]-˲���Ѫ,  Slider������Դ����Ǹ�Fill
    public RectTransform fill_rect_trans;
    // [����Ѫ��]-������Ѫ,  �Լ����Ƴ�����Fill_1
    public RectTransform tween_rect_trans;

    private float tween_speed = 4.0f;
    private bool tween_flag = false;
    private float last_max_x = 0;
    private float start_x = 0;     // ������Ѫ��Ѫ��-�������
    private float end_x = 0;       // ������Ѫ��Ѫ��-�����յ�
    private float now_x = 0;
    private float tm_t = 0;


    // Start is called before the first frame update
    void Start()
    {
        // ȷ��[ʵ��Ѫ��]��ʾ��������(��Ӧ��Hierarchyͬ����������)
        fill_rect_trans.SetAsLastSibling();
        // ��ʼ��ʱ����[ʵ��Ѫ��]��[����Ѫ��]һ��
        tween_rect_trans.anchorMax = fill_rect_trans.anchorMax;
        last_max_x = fill_rect_trans.anchorMax.x;
    }


    // Update is called once per frame
    void Update()
    {
        // �������ɶ���
        if (tween_flag)
        {
            tm_t += tween_speed * Time.deltaTime;
            //Debug.Log(tm_t);
            if (tm_t >= 1)
            {
                tm_t = 1;
                tween_flag = false;   // �رչ���Ч��
                last_max_x = end_x;   // ��¼����ֹͣ������
            }
            // ����Lerp, ������ʱ��, ���Ե�Ѫ���Ŀ�
            now_x = Mathf.Lerp(start_x, end_x, tm_t);
            tween_rect_trans.anchorMax = new Vector2(now_x, fill_rect_trans.anchorMax.y);
        }
    }


    // ������ѪЧ��(��ʱSlider��value�Ѿ��仯����, [ʵ��Ѫ��]�Ѿ��仯)
    public void Start_Tween()
    {
        start_x = last_max_x;
        end_x = fill_rect_trans.anchorMax.x;
        tween_flag = true;
        tm_t = 0;
    }

}
