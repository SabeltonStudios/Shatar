using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In order for the FaceGridCreator to work properly, the corner2's x and z coordinates should be greater than the corner1's

public class FaceGridCreator : MonoBehaviour
{
    // Two given nodes
    public Node corner1;
    public Node corner2;

    // Face's dimensions in squares
    public int xSquares;
    public int zSquares;

    // The forward vector of the face (only one coordinate must be non zero)
    public Vector3 forward;

    // Boolean that acts like a trigger
    public bool execute;

    // Auxiliar bidimensional array used for setting the adjacencies easier
    private Node[][] grid;

    public GameObject createdNodesParent;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize bidimensional array
        grid = new Node[xSquares][];
        for (int i = 0; i < xSquares; i++)
        {
            grid[i] = new Node[zSquares];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (execute)
        {
            execute = false;
            CreateGrid();
            SetAdjacencies();
        }
    }

    private void CreateGrid()
    {
        grid[0][0] = corner1;
        grid[xSquares - 1][zSquares - 1] = corner2;
        corner1.InitializeAdjacencies();
        corner2.InitializeAdjacencies();

        float xDistanceBetweenSquares = (corner1.transform.position.x - corner2.transform.position.x) / (xSquares - 1.0f);
        float zDistanceBetweenSquares = (corner1.transform.position.z - corner2.transform.position.z) / (zSquares - 1.0f);

        for (int i = 0; i < xSquares; i++)
        {
            for (int j = 0; j < zSquares; j++)
            {
                if (!grid[i][j])
                {
                    Node newNode = Instantiate(corner1, createdNodesParent.transform);
                    newNode.transform.position = corner1.transform.position + new Vector3(-i * xDistanceBetweenSquares, 0.0f, -j * zDistanceBetweenSquares);
                    newNode.name = "Node" + i + "" + j;
                    newNode.InitializeAdjacencies();
                    grid[i][j] = newNode;
                }
            }
        }
    }

    private void SetAdjacencies()
    {
        if (forward.x > 0)// Forward vector is (1, 0, 0)
        {
            for (int i = 0; i < xSquares; i++)
            {
                for (int j = 0; j < zSquares; j++)
                {
                    grid[i][j].adjacencies[2] = i + 1 < xSquares && j - 1 >= 0 ? grid[i + 1][j - 1] : null;
                    grid[i][j].adjacencies[4] = j - 1 >= 0 ? grid[i][j - 1] : null;
                    grid[i][j].adjacencies[7] = i - 1 >= 0 && j - 1 >= 0 ? grid[i - 1][j - 1] : null;
                    grid[i][j].adjacencies[1] = i + 1 < xSquares ? grid[i + 1][j] : null;
                    grid[i][j].adjacencies[6] = i - 1 >= 0 ? grid[i - 1][j] : null;
                    grid[i][j].adjacencies[0] = i + 1 < xSquares && j + 1 < zSquares ? grid[i + 1][j + 1] : null;
                    grid[i][j].adjacencies[3] = j + 1 < zSquares ? grid[i][j + 1] : null;
                    grid[i][j].adjacencies[5] = i - 1 >= 0 && j + 1 < zSquares ? grid[i - 1][j + 1] : null;
                }
            }
        }
        if (forward.x < 0)// Forward vector is (-1, 0, 0)
        {
            for (int i = 0; i < xSquares; i++)
            {
                for (int j = 0; j < zSquares; j++)
                {
                    grid[i][j].adjacencies[5] = i + 1 < xSquares && j - 1 >= 0 ? grid[i + 1][j - 1] : null;
                    grid[i][j].adjacencies[3] = j - 1 >= 0 ? grid[i][j - 1] : null;
                    grid[i][j].adjacencies[0] = i - 1 >= 0 && j - 1 >= 0 ? grid[i - 1][j - 1] : null;
                    grid[i][j].adjacencies[6] = i + 1 < xSquares ? grid[i + 1][j] : null;
                    grid[i][j].adjacencies[1] = i - 1 >= 0 ? grid[i - 1][j] : null;
                    grid[i][j].adjacencies[7] = i + 1 < xSquares && j + 1 < zSquares ? grid[i + 1][j + 1] : null;
                    grid[i][j].adjacencies[4] = j + 1 < zSquares ? grid[i][j + 1] : null;
                    grid[i][j].adjacencies[2] = i - 1 >= 0 && j + 1 < zSquares ? grid[i - 1][j + 1] : null;
                }
            }
        }
        if (forward.z > 0)// Forward vector is (0, 0, 1)
        {
            for (int i = 0; i < xSquares; i++)
            {
                for (int j = 0; j < zSquares; j++)
                {
                    grid[i][j].adjacencies[7] = i + 1 < xSquares && j - 1 >= 0 ? grid[i + 1][j - 1] : null;
                    grid[i][j].adjacencies[6] = j - 1 >= 0 ? grid[i][j - 1] : null;
                    grid[i][j].adjacencies[5] = i - 1 >= 0 && j - 1 >= 0 ? grid[i - 1][j - 1] : null;
                    grid[i][j].adjacencies[4] = i + 1 < xSquares ? grid[i + 1][j] : null;
                    grid[i][j].adjacencies[3] = i - 1 >= 0 ? grid[i - 1][j] : null;
                    grid[i][j].adjacencies[2] = i + 1 < xSquares && j + 1 < zSquares ? grid[i + 1][j + 1] : null;
                    grid[i][j].adjacencies[1] = j + 1 < zSquares ? grid[i][j + 1] : null;
                    grid[i][j].adjacencies[0] = i - 1 >= 0 && j + 1 < zSquares ? grid[i - 1][j + 1] : null;
                }
            }
        }
        if (forward.z < 0)// Forward vector is (0, 0, -1)
        {
            for (int i = 0; i < xSquares; i++)
            {
                for (int j = 0; j < zSquares; j++)
                {
                    grid[i][j].adjacencies[0] = i + 1 < xSquares && j - 1 >= 0 ? grid[i + 1][j - 1] : null;
                    grid[i][j].adjacencies[1] = j - 1 >= 0 ? grid[i][j - 1] : null;
                    grid[i][j].adjacencies[2] = i - 1 >= 0 && j - 1 >= 0 ? grid[i - 1][j - 1] : null;
                    grid[i][j].adjacencies[3] = i + 1 < xSquares ? grid[i + 1][j] : null;
                    grid[i][j].adjacencies[4] = i - 1 >= 0 ? grid[i - 1][j] : null;
                    grid[i][j].adjacencies[5] = i + 1 < xSquares && j + 1 < zSquares ? grid[i + 1][j + 1] : null;
                    grid[i][j].adjacencies[6] = j + 1 < zSquares ? grid[i][j + 1] : null;
                    grid[i][j].adjacencies[7] = i - 1 >= 0 && j + 1 < zSquares ? grid[i - 1][j + 1] : null;
                }
            }
        }
    }
}
