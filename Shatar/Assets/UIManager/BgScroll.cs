using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgScroll : MonoBehaviour
{
    private float speed = 10f;
    private float newPosY;
    private float endPosY;

    private void Start()
    {
        RectTransform objectRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        endPosY = objectRectTransform.rect.height;
    }

    void Update()
    {
        newPosY = transform.localPosition.y - Time.deltaTime * speed;
        if (newPosY > (-1 * endPosY))
        {
            transform.localPosition = new Vector3(transform.localPosition.x, newPosY, transform.localPosition.z);
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, endPosY, transform.localPosition.z);
        }
    }
}
