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

    //LayerMask finalmask;
    Quaternion actualRot; 
    Quaternion lastRot;

    public Vector3 originOffset;
    public float factorDivision = 4.2f;

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
            if (Physics.SphereCast(transform.position + originOffset, player.radius / factorDivision, -transform.up, out hit)) //Y creamos una SphereCast, que es como un RayCast pero grueso, dandole un diametro y una direccion en la que "disparar" ese SphereCast
            {
               GameObject groundedIn = hit.collider.gameObject;
                groundName = groundedIn.name;         //despues comprobamos el nombre del suelo              
                groundPosition = groundedIn.transform.position; //Una vez que tenemos el GameObject localizado, almacenamos su posicion
                groundIn = hit.collider.gameObject; //Comprobmamos cual es el suelo actual y almacenamos su GameObject en una variable temporal
                actualRot = groundedIn.transform.rotation;


                if ((groundPosition != lastGroundPosition) && (groundName == lastGroundName)) //Si su posicion es distinta de la ultima posicion conocida y el nombre sigue siendo el mismo
                {
                    this.transform.position += groundPosition - lastGroundPosition; //Sumamos a nuestra posicion la diferencia entre la posicion actual del suelo y la ultimam posicion conocida del suelo.
                    player.enabled = false;
                    player.transform.position = this.transform.position;
                    player.enabled = true;
                }
                lastGroundName = groundName;
                lastGroundPosition = groundPosition;

                if(actualRot != lastRot && groundName == lastGroundName)
                {
                    var newRot = this.transform.rotation * (actualRot.eulerAngles - lastRot.eulerAngles);
                    this.transform.RotateAround(groundedIn.transform.position, Vector3.up, newRot.y);
                }
                lastGroundName = groundName;
                lastGroundPosition = groundPosition;
                lastRot = actualRot;

            }
        }
        else if (!player.isGrounded) //Si no estamos tocando el suelo reseteamos todas las variables a 0 para que no tengamos problemas al saltar estando en la plataforma.
        {
            lastGroundName = null;
            lastGroundPosition = Vector3.zero;
            lastRot = Quaternion.Euler(0, 0, 0);
        }

    }
    //Aqui unicamente creamos un gizmo para poder comprobar si el diametro del SphereCast es adecuado al tama�o de nuestro jugador.
    private void OnDrawGizmos()
    {
        player = this.GetComponent<CharacterController>();
        Gizmos.DrawWireSphere(transform.position + originOffset, player.radius / factorDivision);
    }
}
