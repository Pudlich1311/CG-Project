using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;



public class Generation : MonoBehaviour
{
    public int enemies;
    private int[,] level;
    private float chance = 1;
    public GameObject room;
    public GameObject room2;
    public GameObject room3;
    public GameObject armory;
    public bool[,] vis;
    private int offset = 88;
    // Start is called before the first frame update
    void Start()
    {
        level = new int[5, 5];
        vis = new bool[5, 5];

        generate();


        while(!checkifvalid())
        {

            generate();
        }


        StringBuilder sb = new StringBuilder();
        StringBuilder ab = new StringBuilder();
        for (int i = 0; i < level.GetLength(1); i++)
        {
            for (int j = 0; j < level.GetLength(0); j++)
            {
                sb.Append(level[i, j]);
                sb.Append(' ');
                ab.Append(vis[i, j]);
                ab.Append(' ');
            }
            sb.AppendLine();
            ab.AppendLine();
        }
        Debug.Log(sb.ToString());
        Debug.Log(ab.ToString());


        int startx = -98;
        int startz = 0;



        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {

                GameObject temp = null;

                if (level[i, j] == 0)
                {
                    startx += offset;
                    continue;
                }
                else if (level[i, j] == 5)
                {
                    temp = GameObject.Instantiate(armory, new Vector3(startx, 0, startz), Quaternion.identity);
     
                }
                else if (level[i, j] == 1)
                {
                    temp = GameObject.Instantiate(room, new Vector3(startx, 0, startz), Quaternion.identity);
                }
                else if (level[i, j] == 2)
                {
                    temp = GameObject.Instantiate(room2, new Vector3(startx, 0, startz), Quaternion.identity);

                }
                else if (level[i, j] == 3)
                {
                    temp = GameObject.Instantiate(room3, new Vector3(startx, 0, startz), Quaternion.identity);
                }



                /**
                 * Don't look at this, if you see a better solution then trust me I've tried it
                 */

                //South
                if(i==0 && j!=2)
                {
                    MeshRenderer bodyRenderer = temp.transform.Find("WallS").GetComponent<MeshRenderer>();
                    bodyRenderer.enabled = true;
                    BoxCollider box = temp.transform.Find("WallS").GetComponent<BoxCollider>();
                    box.enabled = true; 

                }
                else
                {
                    MeshRenderer bodyRenderer = temp.transform.Find("WallS").GetComponent<MeshRenderer>();
                    bodyRenderer.enabled = false;
                    BoxCollider box = temp.transform.Find("WallS").GetComponent<BoxCollider>();
                    box.enabled = false;
                }

                if(i>0)
                {
                    int z = i - 1;
                    if(level[z, j] == 0)
                    {
                        MeshRenderer bodyRenderer = temp.transform.Find("WallS").GetComponent<MeshRenderer>();
                        bodyRenderer.enabled = true;
                        BoxCollider box = temp.transform.Find("WallS").GetComponent<BoxCollider>();
                        box.enabled = true;
                    }
                }

                //north
                if (i == 4)
                {
                    MeshRenderer bodyRenderer = temp.transform.Find("WallN").GetComponent<MeshRenderer>();
                    bodyRenderer.enabled = true;
                    BoxCollider box = temp.transform.Find("WallN").GetComponent<BoxCollider>();
                    box.enabled = true;

                }
                else
                {
                    MeshRenderer bodyRenderer = temp.transform.Find("WallN").GetComponent<MeshRenderer>();
                    bodyRenderer.enabled = false;
                    BoxCollider box = temp.transform.Find("WallN").GetComponent<BoxCollider>();
                    box.enabled = false;
                }

                if (i < 4)
                {
                    int z = i + 1;
                    if (level[z, j] == 0)
                    {
                        MeshRenderer bodyRenderer = temp.transform.Find("WallN").GetComponent<MeshRenderer>();
                        bodyRenderer.enabled = true;
                        BoxCollider box = temp.transform.Find("WallN").GetComponent<BoxCollider>();
                        box.enabled = true;
                    }
                }


                //West
                if (j == 0)
                {
                    MeshRenderer bodyRenderer = temp.transform.Find("WallW").GetComponent<MeshRenderer>();
                    bodyRenderer.enabled = true;
                    BoxCollider box = temp.transform.Find("WallW").GetComponent<BoxCollider>();
                    box.enabled = true;

                }
                else
                {
                    MeshRenderer bodyRenderer = temp.transform.Find("WallW").GetComponent<MeshRenderer>();
                    bodyRenderer.enabled = false;
                    BoxCollider box = temp.transform.Find("WallW").GetComponent<BoxCollider>();
                }

                if (j > 0)
                {
                    int z = j - 1;
                    if (level[i, z] == 0)
                    {
                        MeshRenderer bodyRenderer = temp.transform.Find("WallW").GetComponent<MeshRenderer>();
                        bodyRenderer.enabled = true;
                        BoxCollider box = temp.transform.Find("WallW").GetComponent<BoxCollider>();
                        box.enabled = true;
                    }
                }

                //East
                if (j == 4)
                {
                    MeshRenderer bodyRenderer = temp.transform.Find("WallE").GetComponent<MeshRenderer>();
                    bodyRenderer.enabled = true;
                    BoxCollider box = temp.transform.Find("WallE").GetComponent<BoxCollider>();
                    box.enabled = true;

                }
                else
                {
                    MeshRenderer bodyRenderer = temp.transform.Find("WallE").GetComponent<MeshRenderer>();
                    bodyRenderer.enabled = false;
                    BoxCollider box = temp.transform.Find("WallE").GetComponent<BoxCollider>();
                }

                if (j < 4)
                {
                    int z = j + 1;
                    if (level[i, z] == 0)
                    {
                        MeshRenderer bodyRenderer = temp.transform.Find("WallE").GetComponent<MeshRenderer>();
                        bodyRenderer.enabled = true;
                        BoxCollider box = temp.transform.Find("WallE").GetComponent<BoxCollider>();
                        box.enabled = true;
                    }
                }



                startx += offset;
            }

            startx = -98;
            startz += offset;
        }


        countEnemies();
    }

    void countEnemies()
    {
        //1 - 2
        //2 - 3
        //3 - 3
        for(int i=0; i<5; i++)
        {
            for(int j=0; j<5; j++)
            {
                if(level[i, j] == 1)
                {
                    enemies += 2;
                }
                else if(level[i, j] == 2)
                {
                    enemies += 3;
                }
                else if(level[i, j] == 3)
                {
                    enemies += 4;
                }

            }
        }
    }

    void generate()
    {
        int armory_pos = random_except_list(5, new int[] { 2 });

        for (int i = 0; i < level.GetLength(1); i++)
        {
            for (int j = 0; j < level.GetLength(0); j++)
            {
                level[i, j] = assignroom();

                if (i == 0 && j == 2)
                {
                    level[i, j] = 1;
                }
                if (i == 0 && j == armory_pos)
                {
                    level[i, j] = 5;
                }
            }
        }



        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                vis[i, j] = false;

                if (level[i, j] == 0)
                {
                    vis[i, j] = true;
                }
            }
        }

        DFS(0, 2, level, vis);
    }


    bool checkifvalid()
    {
        for (int i = 0; i < level.GetLength(1); i++)
        {
            for (int j = 0; j < level.GetLength(0); j++)
            {
                if (vis[i, j] == false)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private int assignroom()
    {
        return UnityEngine.Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public static int random_except_list(int n, int[] x)
    {
        System.Random r = new System.Random();
        int result = r.Next(n - x.Length);

        for (int i = 0; i < x.Length; i++)
        {
            if (result < x[i])
                return result;
            result++;
        }
        return result;
    }




    static int ROW = 5;
    static int COL = 5;

    // Initialize direction vectors
    static int[] dRow = { 0, 1, 0, -1 };
    static int[] dCol = { -1, 0, 1, 0 };

    bool isValid(bool[,] vis, int row, int col)
    {

        // If cell is out of bounds
        if (row < 0 || col < 0 ||
            row >= ROW || col >= COL)
            return false;

        // If the cell is already visited
        if (vis[row, col])
            return false;

        // Otherwise, it can be visited
        return true;
    }

    // Function to perform DFS
    // Traversal on the matrix grid[]
    void DFS(int row, int col,
                    int[,] grid, bool[,] vis)
    {

        // Initialize a stack of pairs and
        // push the starting cell into it
        Stack st = new Stack();
        st.Push(new Tuple<int, int>(row, col));

        // Iterate until the
        // stack is not empty
        while (st.Count > 0)
        {

            // Pop the top pair
            Tuple<int, int> curr = (Tuple<int, int>)st.Peek();
            st.Pop();

            row = curr.Item1;
            col = curr.Item2;


            // Check if the current popped
            // cell is a valid cell or not
            if (!isValid(vis, row, col))
                continue;


            // Mark the current
            // cell as visited
            vis[row, col] = true;

            // Print the element at
            // the current top cell
            Console.Write(grid[row, col] + " ");

            // Push all the adjacent cells
            for (int i = 0; i < 4; i++)
            {
                int adjx = row + dRow[i];
                int adjy = col + dCol[i];
                st.Push(new Tuple<int, int>(adjx, adjy));
            }
        }

    }
}


