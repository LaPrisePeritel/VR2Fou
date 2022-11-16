using System;
using UnityEngine;

public class ShipAliensBoard : MonoBehaviour
{
    private BoardMovement aliensBoard;

    private GameObject[,] aliensObjects;

    [SerializeField] private Transform anchorPoint;
    [SerializeField] private float holoScale;

    private void Awake()
    {
        FindObjectOfType<BoardMovement>();
    }

    private void Start()
    {
        InitializeAliens();
    }

    private void InitializeAliens()
    {
        Alien[,] aliens = aliensBoard.GetAliens();

        for (int i = 0; i < aliens.GetLength(0); i++)
        {
            for (int j = 0; j < aliens.GetLength(1); j++)
            {
                GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obj.transform.localScale = Vector3.one * holoScale;
                obj.transform.localPosition = new Vector3(i * holoScale, 0, j * holoScale);
            }
        }
    }
}
