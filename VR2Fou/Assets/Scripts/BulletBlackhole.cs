using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBlackhole : Bullet
{
    bool grow;
    float f = 0;

    [SerializeField]float speedGrowth;
    Vector3 baseScale;
    public override void Initiate(Vector3 dir, Vector3 initPos)
    {
        base.Initiate(dir, initPos);
        baseScale = transform.localScale;
    }
    protected override void Update()
    {
        base.Update();
        if (grow)
        {
            transform.localScale = Vector3.Lerp(baseScale, baseScale * 2, f* speedGrowth);
            f += Time.deltaTime;
        }
        if (f >= 1)
        {
            Destroy(gameObject);
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Alien"))
            return;

        other.transform.parent.GetComponent<Alien>().Hitted(bulletType, transform.position);
        direction = Vector3.zero;
        grow = true;
    }
}
