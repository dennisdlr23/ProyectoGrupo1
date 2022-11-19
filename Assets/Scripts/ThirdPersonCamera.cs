using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Vector3 offset;
    private Transform target;
    [Range (0, 1)]public float lerpValue;
    public float sensibilidad;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Calcular la velocidad en la que se mueve la camara
        //offset es la distancia con respecto al jugador
        //lerpValue es el valor con la suavidad en la que se mueve la camara
        transform.position = Vector3.Lerp(transform.position, target.position + offset, lerpValue);
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensibilidad, Vector3.up) * offset;

        //Busca a nuestro jugador
        transform.LookAt(target);
    }
}
