using UnityEngine;

public class AfterImageFX : MonoBehaviour
{

    [SerializeField]private SpriteRenderer sr;
    private float colorLooseRate;

    public void SetupAfterImage(float _loosingSpeed ,Sprite _spriteImage)
    {
        sr.sprite = _spriteImage;
        colorLooseRate = _loosingSpeed;

    }

    private void Update()
    {
        float alpha = sr.color.a - colorLooseRate * Time.deltaTime;
        sr.color = new Color(sr.color.r,sr.color.g,sr.color.b,alpha);

        if(sr.color.a <=0)
            Destroy(gameObject);

    }


}
