using System;
using System.Collections;
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
    [SerializeField] private float speedAcceleration;
    [SerializeField] private float timeBetweenAliensAcceleration;
    [SerializeField] private float increaseSpeedPerDeath;

    [Header("Acceleration")]
    [SerializeField] private AnimationCurve accelerationCurve;
    [SerializeField] Vector2 randomSpacingBetweenAccelerations;
    private float timeBeforeNextAcceleration;
    [SerializeField] private float acceleration;
    [SerializeField] private float durationAccelerationValue;
    private float durationAcceleration;
    private bool accelerate;
    

    private void Awake()
    {
        ship = FindObjectOfType<Ship>();
        
        startPosition = transform.position;
        hSpeed = horizontalSpeed;
        vSpeed = verticalSpeed;

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
        timeBeforeNextAcceleration = Random.Range(randomSpacingBetweenAccelerations.x, randomSpacingBetweenAccelerations.y);
        durationAccelerationValue = accelerationCurve.keys[^1].time;

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
                                        Vector3.right * spaceBetweenAliens * j ;
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
        for (int i = 0 ; i < rows; i++)
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
        //transform.position += moveDirection * hSpeed * Time.deltaTime * Vector3.right;

        /*if (!nextAlienMove)
        {
            aliensAccelerationTimer -= Time.deltaTime;

            if (aliensAccelerationTimer <= 0f)
            {
                aliensMoveIndex.y++;
                if (aliensMoveIndex.y >= aliens.GetLength(1))
                {
                    aliensMoveIndex.y = 0;
                    aliensMoveIndex.x++;
                }
            }
        }*/
        
        //MoveAliens();
        
        /*timeBeforeNextAcceleration -= Time.deltaTime;
        if (timeBeforeNextAcceleration <= 0 && !accelerate)
            Accelerate();*/

        /*if(accelerate)
        {
            hSpeed = horizontalSpeed + acceleration * accelerationCurve.Evaluate(durationAcceleration);
            vSpeed = verticalSpeed + acceleration * accelerationCurve.Evaluate(durationAcceleration);
            durationAcceleration += Time.deltaTime;
            if (durationAcceleration >= durationAccelerationValue && accelerate)
                Decelerate();
        }*/
    }

    private void MoveAliens()
    {
        // Slow last alien if needed
        /*if (slowLastAlien)
        {
            if (aliensMoveIndex.y > 0)
            {
                aliens[(int)aliensMoveIndex.x, (int)aliensMoveIndex.y - 1].Rigidbody.velocity = Vector3.right * moveDirection * horizontalSpeed;
            }
            else if (aliensMoveIndex.y == 0 && aliensMoveIndex.x > 0)
            {
                aliens[(int)aliensMoveIndex.x - 1, aliens.GetLength(1) - 1].Rigidbody.velocity = Vector3.right * moveDirection * horizontalSpeed;
            }

            slowLastAlien = false;
        }*/
        
        //aliensAccelerationTimer = timeBetweenAliensAcceleration;
    }
    
    private void ChangeDirection(int _lineIndex)
    {
        if (_lineIndex < 0 || _lineIndex >= rows)
            return;

        linesDirection[_lineIndex] *= -1;

        for (int i = 0; i < columns; i++)
        {
            if (aliens[_lineIndex, i] != null)
            {
                aliens[_lineIndex, i].Rigidbody.velocity = Vector3.right * linesDirection[_lineIndex] * hSpeed;
            }
        }

        //aliensMoveIndex = Vector2.zero;
        /*StopAllCoroutines();
        StartCoroutine(DownDirection());*/
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
            nextTimer = 0.2f;
        }
    }

    private void AliensIncreaseSpeed()
    {
        hSpeed += increaseSpeedPerDeath;
        vSpeed += increaseSpeedPerDeath;
    }

    private void Accelerate()
    {
        durationAcceleration = 0;
        accelerate = true;
    }
    
    private void Decelerate()
    {
        timeBeforeNextAcceleration = Random.Range(randomSpacingBetweenAccelerations.x, randomSpacingBetweenAccelerations.y);
        accelerate = false;
    }

    private IEnumerator DownDirection(int _line)
    {
        float start = aliens[_line, 0].transform.position.z;
        float finalDownDirection = start - spaceBetweenAliens;
        float t = 0f;

        while (t <= 1f)
        {
            t += vSpeed * Time.deltaTime;
            float z = Mathf.Lerp(start, finalDownDirection, t);

            for (int i = 0; i < columns; i++)
            {
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
            
            Gizmos.color = Color.red;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Vector3 alienPosition = transform.position + Vector3.back * spaceBetweenAliens * (i + 0.5f) +
                                            Vector3.right * spaceBetweenAliens * j;

                    Gizmos.DrawCube(alienPosition, Vector3.one);
                }
            }
        }
    }
}