using UnityEngine;

public class AreaMusic : MonoBehaviour
{
    [SerializeField] private int areaMusic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            AudioManager.instance.PlayBGM(areaMusic);
    }
    /*
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            AudioManager.instance.StopSFXWithTime(areaSoundIndex);
        //AudioManager.instance.StopSFX(areaSoundIndex);

    }
    */
}
