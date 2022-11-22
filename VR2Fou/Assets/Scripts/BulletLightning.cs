using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLightning : Bullet
{
    private void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent(out Alien alien))
        {
            alien.Hitted(bulletType, transform.position); 
        }
    }

}
