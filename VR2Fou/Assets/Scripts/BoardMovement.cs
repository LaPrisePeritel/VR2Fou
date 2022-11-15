using System;
using UnityEngine;

public class BoardMovement : MonoBehaviour
{
    private Alien[,] aliens;

    private bool reset;

    private Vector3 startPosition;
    private float moveDirection;

    [Header("Initialization")]
    [SerializeField] private Alien aliensPrefab;
    [SerializeField] private float spaceBetweenAliens;
    [SerializeField] private int rows, columns;
    [Space]
    [SerializeField] private float leftBorderOffset, rightBorderOffset;

    private void Awake()
    {
        startPosition = transform.position;

        moveDirection = Vector3.Distance(transform.position, transform.position + Vector3.left * leftBorderOffset) >
                        Vector3.Distance(transform.position, transform.position + Vector3.right * rightBorderOffset)
            ? -1f
            : 1f;

        aliens = new Alien[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 alienPosition = transform.position + Vector3.down * spaceBetweenAliens * i +
                                        j * spaceBetweenAliens * Vector3.right;
                aliens[i, j] = Instantiate(aliensPrefab, alienPosition, Quaternion.identity);
                aliens[i, j].Initialisation(ChangeDirection, startPosition + Vector3.left * leftBorderOffset, startPosition + Vector3.right * rightBorderOffset);
                aliens[i, j].transform.parent = transform;
            }
        }
    }

    private void Update()
    {
        transform.position += moveDirection * Time.deltaTime * Vector3.right;
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
        transform.position += spaceBetweenAliens * Vector3.down;
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
        }
    }
}