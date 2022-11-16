using System;
using System.Collections;
using UnityEngine;

public class BoardMovement : MonoBehaviour
{
    private Alien[,] aliens;

    private bool reset;

    private Vector3 startPosition;
    private float moveDirection;

    private float hSpeed, vSpeed;

    [Header("Initialization")]
    [SerializeField] private Alien aliensPrefab;
    [SerializeField] private float spaceBetweenAliens;
    [SerializeField] private int rows, columns;
    [Space]
    [SerializeField] private float leftBorderOffset, rightBorderOffset;

    [Header("Movements")]
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float increaseSpeedPerDeath;
    
    private void Awake()
    {
        startPosition = transform.position;
        hSpeed = horizontalSpeed;
        vSpeed = verticalSpeed;

        moveDirection = Vector3.Distance(transform.position, transform.position + Vector3.left * leftBorderOffset) >
                        Vector3.Distance(transform.position, transform.position + Vector3.right * rightBorderOffset)
            ? -1f
            : 1f;
        
        SpawnAliens();
    }

    private void SpawnAliens()
    {
        aliens = new Alien[rows, columns];

        bool c = columns % 2 == 0;

        if (c)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = -columns / 2; j < columns / 2; j++)
                {
                    Vector3 alienPosition = transform.position + Vector3.down * spaceBetweenAliens * (i + 0.5f) +
                                            Vector3.right * spaceBetweenAliens * (j + 0.5f);
                    aliens[i, j + columns / 2] = Instantiate(aliensPrefab, alienPosition, Quaternion.identity);
                    aliens[i, j + columns / 2].Initialisation(ChangeDirection,
                        startPosition + Vector3.left * leftBorderOffset,
                        startPosition + Vector3.right * rightBorderOffset,
                        AliensIncreaseSpeed);
                    aliens[i, j + columns / 2].transform.parent = transform;
                }
            }
        }
        else
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = -columns / 2 - 1; j < columns / 2; j++)
                {
                    Vector3 alienPosition = transform.position + Vector3.down * spaceBetweenAliens * (i + 0.5f) +
                                            Vector3.right * spaceBetweenAliens * (j + 1);
                    aliens[i, j + columns / 2 + 1] = Instantiate(aliensPrefab, alienPosition, Quaternion.identity);
                    aliens[i, j + columns / 2 + 1].Initialisation(ChangeDirection,
                        startPosition + Vector3.left * leftBorderOffset,
                        startPosition + Vector3.right * rightBorderOffset,
                        AliensIncreaseSpeed);
                    aliens[i, j + columns / 2 + 1].transform.parent = transform;
                }
            }
        }
    }

    public Alien[,] GetAliens()
    {
        return aliens;
    }
    
    private void Update()
    {
        transform.position += moveDirection * hSpeed * Time.deltaTime * Vector3.right;
    }

    private void LateUpdate()
    {
        reset = true;
    }

    private void ChangeDirection()
    {
        if (!reset)
            return;

        reset = false;
        moveDirection *= -1f;
        StopAllCoroutines();
        StartCoroutine(DownDirection());
    }

    private void AliensIncreaseSpeed()
    {
        hSpeed += increaseSpeedPerDeath;
        vSpeed += increaseSpeedPerDeath;
    }

    private IEnumerator DownDirection()
    {
        float start = transform.position.y;
        float finalDownDirection = start - spaceBetweenAliens;
        float t = 0f;

        while (t <= 1f)
        {
            t += vSpeed * Time.deltaTime;
            float y = Mathf.Lerp(start, finalDownDirection, t);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);

            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (startPosition != Vector3.zero)
        {
            Gizmos.DrawLine(startPosition + Vector3.left * leftBorderOffset,
                startPosition + Vector3.right * rightBorderOffset);

            Gizmos.DrawWireSphere(startPosition + Vector3.left * leftBorderOffset, 0.5f);
            Gizmos.DrawWireSphere(startPosition + Vector3.right * rightBorderOffset, 0.5f);
        }
        else
        {
            Gizmos.DrawLine(transform.position + Vector3.left * leftBorderOffset,
                transform.position + Vector3.right * rightBorderOffset);

            Gizmos.DrawWireSphere(transform.position + Vector3.left * leftBorderOffset, 0.5f);
            Gizmos.DrawWireSphere(transform.position + Vector3.right * rightBorderOffset, 0.5f);
            
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(transform.position, 1f);
            
            Gizmos.color = Color.red;

            bool c = columns % 2 == 0;

            if (c)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = -columns / 2; j < columns / 2; j++)
                    {
                        Vector3 alienPosition = transform.position + Vector3.down * spaceBetweenAliens * (i + 0.5f) +
                                                Vector3.right * spaceBetweenAliens * (j + 0.5f);

                        Gizmos.DrawCube(alienPosition, Vector3.one);
                    }
                }
            }
            else
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = -columns / 2 - 1; j < columns / 2; j++)
                    {
                        Vector3 alienPosition = transform.position + Vector3.down * spaceBetweenAliens * (i + 0.5f) +
                                                Vector3.right * spaceBetweenAliens * (j + 1);

                        Gizmos.DrawCube(alienPosition, Vector3.one);
                    }
                }
            }
        }
    }
}