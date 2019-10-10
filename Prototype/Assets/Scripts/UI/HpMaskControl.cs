using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpMaskControl : MonoBehaviour
{
    private float widthMaskBar;

    private Vector2 startScale;

    void Start()
    {
        startScale = transform.localScale;
        widthMaskBar = transform.GetComponent<SpriteRenderer>().bounds.size.x;
        transform.parent.GetComponent<BaseEnemy>().hpMask = this;
    }

    public void SetRatio(float decrease)
    {
        transform.localScale -= Vector3.right * startScale.x * decrease;

        //transform.position -= Vector3.right * startScale.x * decrease * widthMaskBar / 2;
    }

    public void ResetHealthBar()
    {
        //transform.position = new Vector2(0, transform.position.y);

        transform.localScale = startScale;

    }
}
