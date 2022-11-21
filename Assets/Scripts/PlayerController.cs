using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movimiento
    private float horizontalMove;
    private float verticalMove;

    private Vector3 playerInput;

    public CharacterController player;
    //Velocidad
    public float playerSpeed;
    //Almacenar valores de camara
    private Vector3 movePlayer;
    //Gravedad 
    public float gravity = 9.8f;
    public float fallVelocity;
    public float jumpForce;

    //Camara
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    //Deslizamiento
    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float slideVelocity;
    public float slopeForceDown;

    //variables animacion
    public Animator playerAnimatorController;

    // Start is called before the first frame update
    void Start()
    {
        //Movimiento
        player = GetComponent<CharacterController>();
        playerAnimatorController = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //Movimiento
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        playerAnimatorController.SetFloat("PlayerWalkVelocity",  playerInput.magnitude * playerSpeed);

        //Mover camara con el jugador
        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        movePlayer = movePlayer * playerSpeed;

        //Hacer que el jugador vea hacia donde se esta moviendo
        player.transform.LookAt(player.transform.position + movePlayer);

        SetGravity();

        PlayerSkills();

        //Se mueve el jugador
        player.Move(movePlayer * Time.deltaTime);

        Debug.Log(player.velocity.magnitude);

    }
    //Funcion para determinar la direccion a la que mira la camara.
    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    //Funcion para las habilidades de nuestro jugador
    public void PlayerSkills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
            playerAnimatorController.SetTrigger("PlayerJump");
        }
    }

    //Funcion para la gravedad.
    void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
            playerAnimatorController.SetFloat("PlayerVerticalVelocity", player.velocity.y);
        }
        playerAnimatorController.SetBool("isGrounded", player.isGrounded); //----No la esta tomando--------
        SlideDown();
    }

    //detectar el angulo
    public void SlideDown()
    {
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;

        if (isOnSlope)
        {
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * slideVelocity;
            movePlayer.z += ((1f - hitNormal.y) * hitNormal.z) * slideVelocity;

            movePlayer.y += slopeForceDown;
        }
    }

    //Funcion de unity que detecta cuando chocamos con algo
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }
    //agregadas 
    private void OnTriggerStay(Collider other) {
        if(other.tag == "MovingPlatform")
        {
            Debug.Log("UNA PLATAFORMA!");
            player.transform.SetParent (other.transform);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.tag == "MovingPlatform")
        {            
            player.transform.SetParent (null);
        }
    }
    private void OnAnimatorMove() {
        
    }
}
