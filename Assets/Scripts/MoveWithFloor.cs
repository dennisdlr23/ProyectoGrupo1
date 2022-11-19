using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithFloor : MonoBehaviour
{

    CharacterController player; //Variable para almacenar el character controller

    Vector3 groundPosition;     //Almacenamos la posicion actual del suelo
    Vector3 lastGroundPosition; //Almacenamos la ultima posicion conocida del suelo
    string groundName;          //Almacenamos el nombre actual del suelo
    string lastGroundName;      //Almacenamos el nombre del ultimo suelo conocido
    GameObject groundIn;

    LayerMask finalmask;

    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<CharacterController>(); //Inicializamos la variable player almacenando el componente CharacterController
        var layer1 = 9;
        var layer2 = 12;
        finalmask = ~((1 << layer1) | (1 << layer2));
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded)    //En caso de estar tocando el suelo
        {
            RaycastHit hit; //Creamos una variable para almacenar la colision del RayCast
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 3, finalmask)) //Y creamos una SphereCast, que es como un RayCast pero grueso, dandole un diametro y una direccion en la que "disparar" ese SphereCast
            {
                groundIn = hit.collider.gameObject; //Comprobmamos cual es el suelo actual y almacenamos su GameObject en una variable temporal
                groundName = groundIn.name;         //despues comprobamos el nombre del suelo
                groundPosition = groundIn.transform.position; //Una vez que tenemos el GameObject localizado, almacenamos su posicion
                if ((groundPosition != lastGroundPosition) && (groundName == lastGroundName)) //Si su posicion es distinta de la ultima posicion conocida y el nombre sigue siendo el mismo
                {
                    this.transform.position += groundPosition - lastGroundPosition; //Sumamos a nuestra posicion la diferencia entre la posicion actual del suelo y la ultimam posicion conocida del suelo.
                    player.enabled = false;
                    player.transform.position = this.transform.position;
                    player.enabled = true;
                }
                lastGroundName = groundName;
                lastGroundPosition = groundPosition;

            }
        }
        else if (!player.isGrounded) //Si no estamos tocando el suelo reseteamos todas las variables a 0 para que no tengamos problemas al saltar estando en la plataforma.
        {
            lastGroundName = null;
            lastGroundPosition = Vector3.zero;
        }

    }
    //Aqui unicamente creamos un gizmo para poder comprobar si el diametro del SphereCast es adecuado al tamaño de nuestro jugador.
    private void OnDrawGizmos()
    {
        player = this.GetComponent<CharacterController>();
        Gizmos.DrawRay(transform.position, Vector3.down * 3);
    }
}
