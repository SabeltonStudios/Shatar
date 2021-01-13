using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//clase empleada para el control de cámara
public class CameraController : MonoBehaviour
{
    //Variable booleana empleada para impedir nuevos giros hasta haberse completado el anterior
    public bool enabledMov = true;
    //Referencia al script mediante el que hacer sonar los diferentes efectos de sonido, en este caso el de giro de cámara
    private SoundManager m_soundManager = null;
    // Update is called once per frame
    void Start()
    {
        m_soundManager = FindObjectOfType<SoundManager>();
    }
    //Métodos empleados para girar la cámara a derecha, izquierda, arriba y abajo, respectivamente
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
    //En el caso de los giros susperior e inferior, se limitan los mismos entre dos ángulos, para no perder control de la cámara
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
    //Enumator empleado para el movimiento fluido de cámara a lo largo del tiempo, una vez se inicia impide nuevos movimientos, hasta haberse completado
    IEnumerator rotateSmooth(Vector3 angle, float seconds)
    {
        enabledMov = false;
        m_soundManager.Play_SoundEffect("camaraRotation");
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
    //Métodos empleados por el controlador de UI para habilitar los botones de giro superior e inferior sólo si la inclinación lo permite
    public bool checkIfCanTurnUp()
    {
        return (transform.rotation.eulerAngles.x >= 285 && transform.rotation.eulerAngles.x < 295);
    }

    public bool checkIfCanTurnDown()
    {
        return (transform.rotation.eulerAngles.x >= -5 && transform.rotation.eulerAngles.x < 5);
    }
}
