using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardMovement : MonoBehaviour
{
    private Ship ship;

    private Alien[,] aliens;
    private int[] linesDirection;
    private bool moveNextLineFaster;
    private int lineFasterIndex;
    private float nextTimer;

    private bool reset;

    private Vector3 startPosition;
    private float moveDirection;

    private float hSpeed, vSpeed, shootTimeBetween;

    private float shootTimer;

    private int nbAliensAlive;

    [Header("Initialization")]
    [SerializeField] private Alien aliensPrefab;
    [SerializeField] private float spaceBetweenAliens;
    [Space]
    [SerializeField] private float alienDownLength;
    [Space]
    [SerializeField] private int rows, columns;
    [Space]
    [SerializeField] private float leftBorderOffset, rightBorderOffset;

    [Header("Movements")]
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float speedAcceleration;
    [SerializeField] private float timeBetweenAliensAcceleration;
    [SerializeField] private float increaseSpeedPerDeath;

    [Header("Shooting")]
    [SerializeField] private float timeBetweenShoot;
    [SerializeField] private float timeShootDecreasePerDeath;

    public bool CanShoot = true;

    private void Awake()
    {
        ship = FindObjectOfType<Ship>();

        nbAliensAlive = rows * columns;

        startPosition = transform.position;
        hSpeed = horizontalSpeed;
        vSpeed = verticalSpeed;
        shootTimeBetween = timeBetweenShoot;
        shootTimer = shootTimeBetween;

        linesDirection = new int[rows];

        moveDirection = Vector3.Distance(transform.position, transform.position + Vector3.left * leftBorderOffset) >
                        Vector3.Distance(transform.position, transform.position + Vector3.right * rightBorderOffset)
            ? -1f
            : 1f;

        for (int i = 0; i < linesDirection.Length; i++)
        {
            linesDirection[i] = (int)-moveDirection;
        }

        SpawnAliens();

        moveNextLineFaster = true;
    }

    private void SpawnAliens()
    {
        aliens = new Alien[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 alienPosition = transform.position + Vector3.back * spaceBetweenAliens * (i + 0.5f) +
                                        Vector3.right * spaceBetweenAliens * j;
                aliens[i, j] = Instantiate(aliensPrefab, alienPosition, Quaternion.identity);
                aliens[i, j].Initialisation(ChangeDirection, i,
                    () => GameManager.instance.End(false),
                    startPosition + Vector3.left * leftBorderOffset,
                    startPosition + Vector3.right * rightBorderOffset,
                    ship.transform.position,
                    AliensIncreaseSpeed);
                aliens[i, j].transform.parent = transform;
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < rows; i++)
            ChangeDirection(i);

        MoveNextLineFaster();
    }

    public Alien[,] GetAliens()
    {
        return aliens;
    }

    private void Update()
    {
        if (moveNextLineFaster)
        {
            if (nextTimer > 0f)
            {
                nextTimer -= Time.deltaTime;
            }
            else
            {
                moveNextLineFaster = false;
                MoveNextLineFaster();
            }
        }

        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
        else
        {
            RandomAlienShoot();
        }
    }

    private void ChangeDirection(int _lineIndex)
    {
        if (_lineIndex < 0 || _lineIndex >= rows)
            return;

        linesDirection[_lineIndex] *= -1;

        Debug.Log($"Direction {linesDirection[_lineIndex]}");

        for (int i = 0; i < columns; i++)
        {
            if (aliens[_lineIndex, i] != null)
            {
                aliens[_lineIndex, i].Rigidbody.velocity = Vector3.right * linesDirection[_lineIndex] * hSpeed;
                aliens[_lineIndex, i].SetDirection(linesDirection[_lineIndex] < 0);
            }
        }

        StartCoroutine(DownDirection(_lineIndex));
    }

    private void MoveNextLineFaster()
    {
        for (int i = 0; i < columns; i++)
        {
            if (aliens[lineFasterIndex, i] != null)
            {
                aliens[lineFasterIndex, i].Rigidbody.velocity = Vector3.right * linesDirection[lineFasterIndex] * hSpeed * speedAcceleration;
            }
        }

        lineFasterIndex++;

        if (lineFasterIndex < aliens.GetLength(0))
        {
            moveNextLineFaster = true;
            nextTimer = timeBetweenAliensAcceleration;
        }
    }

    private void AliensIncreaseSpeed()
    {
        hSpeed += increaseSpeedPerDeath;
        vSpeed += increaseSpeedPerDeath;
        timeBetweenShoot -= timeShootDecreasePerDeath;

        nbAliensAlive--;
        if (nbAliensAlive <= 0)
        {
            GameManager.instance.PassRound();
        }
    }

    private void RandomAlienShoot()
    {
        if (!CanShoot) return;

        List<Alien> aliensCanShoot = new List<Alien>();

        for (int i = 0; i < columns; i++)
        {
            for (int j = rows - 1; j >= 0; j--)
            {
                if (aliens[j, i] != null)
                {
                    aliensCanShoot.Add(aliens[j, i]);
                    break;
                }
            }
        }

        if (aliensCanShoot.Count > 0)
        {
            aliensCanShoot[Random.Range(0, aliensCanShoot.Count)].Shoot();
        }

        shootTimer = timeBetweenShoot;
    }

    private IEnumerator DownDirection(int _line)
    {
        float start = aliens[_line, 0].transform.position.z;
        float finalDownDirection = start - alienDownLength;
        float t = 0f;

        while (t <= 1f)
        {
            t += vSpeed * Time.deltaTime;
            float z = Mathf.Lerp(start, finalDownDirection, t);

            for (int i = 0; i < columns; i++)
            {
                if (aliens[_line, i] != null)
                    aliens[_line, i].Rigidbody.MovePosition(new Vector3(aliens[_line, i].Rigidbody.position.x, aliens[_line, i].Rigidbody.position.y, z));
            }

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

            for (int i = 0; i < rows; i++)
            {
                Gizmos.color = Color.red;

                for (int j = 0; j < columns; j++)
                {
                    Vector3 alienPosition = transform.position + Vector3.back * spaceBetweenAliens * (i + 0.5f) +
                                            Vector3.right * spaceBetweenAliens * j;

                    if (j == columns - 1)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawLine(alienPosition, alienPosition + new Vector3(-1f, 0f, -1f) * alienDownLength);
                        Gizmos.color = Color.red;
                    }

                    Gizmos.DrawCube(alienPosition, Vector3.one);
                }
            }
        }
    }
}