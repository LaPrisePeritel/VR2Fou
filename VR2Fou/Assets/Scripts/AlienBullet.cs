using UnityEngine;

public class AlienBullet : Bullet
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Player has been hit by an alien's bullet");

        JSAM.AudioManager.PlaySound(JSAM.Sounds.Player_Hit);

        Destroy(gameObject);
    }
}