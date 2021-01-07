using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool enabledMov = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                turnLeft();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                turnUp();
            }else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                turnDown();
            }else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                turnRight();
            }
    }
    public void turnRight()
    {
        if (enabledMov)
        {
            StartCoroutine(rotateSmooth(new Vector3(0, -90, 0), 1));
        }
    }
    public void turnLeft()
    {
        if (enabledMov)
        {
            StartCoroutine(rotateSmooth(new Vector3(0, 90, 0), 1));
        }
    }
    public void turnUp()
    {
        if ((transform.rotation.eulerAngles.x >= 285 && transform.rotation.eulerAngles.x < 295) && enabledMov)
        {
            StartCoroutine(rotateSmooth(new Vector3(70, 0, 0), 1));
        }
    }
    public void turnDown()
    {
        if ((transform.rotation.eulerAngles.x >= -5 && transform.rotation.eulerAngles.x<5)&& enabledMov)
        {
            StartCoroutine(rotateSmooth(new Vector3(-70, 0, 0), 1));
        }
    }
    IEnumerator rotateSmooth(Vector3 angle, float seconds)
    {
        enabledMov = false;
        float elapsedTime = 0;
        Vector3 startingRot = transform.rotation.eulerAngles;
        Vector3 end = transform.rotation.eulerAngles + angle;
        while (elapsedTime < seconds)
        {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startingRot, end, (elapsedTime / seconds)));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(end);
        enabledMov = true;
    }
}
