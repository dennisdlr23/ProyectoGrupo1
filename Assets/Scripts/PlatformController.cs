using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    //Variables para mover la plataforma
    public Rigidbody platformRB;
    public Transform[] platformPositions;
    public float platformSpeed;

    //Variables para detectar la posicion de la plataforma
    private int actualPosition = 0;
    private int nextPosition = 1;

    public bool moveToTheNext = true;
    public float waitTime;

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if (moveToTheNext)
        {
            StopCoroutine(WaitForMove(0));
            platformRB.MovePosition(Vector3.MoveTowards(platformRB.position, platformPositions[nextPosition].position, platformSpeed * Time.deltaTime));
        }
        
        if (Vector3.Distance(platformRB.position, platformPositions[nextPosition].position) <= 0)
        {
            StartCoroutine(WaitForMove(waitTime));
            actualPosition = nextPosition;
            nextPosition++;

            if (nextPosition > platformPositions.Length - 1)
            {
                nextPosition = 0;
            }
        }
    }

    IEnumerator WaitForMove(float time)
    {
        moveToTheNext = false;
        yield return new WaitForSeconds(time);
        moveToTheNext = true;
    }
}
