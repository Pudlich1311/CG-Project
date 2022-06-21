using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;


public class Generation : MonoBehaviour
{
    private int[,] level;
    private float chance = 1;
    public GameObject room;
    public GameObject room2;
    public GameObject room3;
    public GameObject armory;
    private GameObject temp;
    private int offset = 88;
    // Start is called before the first frame update
    void Start()
    {
        temp = room; //for now
        level = new int[5,5];

        

        for (int i = 0; i < level.GetLength(1); i++)
        {
            for (int j = 0; j < level.GetLength(0); j++)
            {
                level[i, j] = assignroom();

                if (i == 0 && j==2)
                {
                    level[i, j] = 1;
                }

            }
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < level.GetLength(1); i++)
        {
            for (int j = 0; j < level.GetLength(0); j++)
            {
                sb.Append(level[i, j]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());


        int startx = -98;
        int startz = 0;

        int armory_pos = random_except_list(5, new int[] { 2 });

        for (int i = 0; i < level.GetLength(0); i++)
        {
            for (int j = 0; j < level.GetLength(1); j++)
            {


                if (level[i, j] == 0)
                {
                    startx += offset;
                    continue;
                }
                else if (i == 0 && j == armory_pos)
                {
                    temp = armory;
                }
                else if (level[i, j] == 1)
                {
                    temp = room;
                    
                }
                else if (level[i, j] == 2)
                {
                    temp = room2;
                    
                }
                else if (level[i, j] == 3)
                {
                    temp = room3;
                }


                /*
                if(i==0)
                {
                    if(i!=0 && j!=2)
                    {
                        GameObject child = findChildFromParent("temp", "WallS");
                        GameObject.Find("temp/WallS").GetComponent<BoxCollider>().enabled = true;
                        GameObject.Find("temp/WallS").GetComponent<MeshRenderer>().enabled = true;
                        child.GetComponent<BoxCollider>().enabled = true;
                        child.GetComponent<MeshRenderer>().enabled = true;
                    }

                }
                if(j==4 || level[i,++j]==0)
                {
                    GameObject child = findChildFromParent("temp", "WallW");
                    child.GetComponent<BoxCollider>().enabled = true;
                    child.GetComponent<MeshRenderer>().enabled = true;
                }
                if(j==0 || level[i,--j]==0)
                {
                    GameObject child = findChildFromParent("temp", "WallE");
                    child.GetComponent<BoxCollider>().enabled = true;
                    child.GetComponent<MeshRenderer>().enabled = true;
                }
                if(i==5 || level[++i,j]==0)
                {
                    GameObject child = findChildFromParent("temp", "WallN");
                    child.GetComponent<BoxCollider>().enabled = true;
                    child.GetComponent<MeshRenderer>().enabled = true;
                }
              */

                Instantiate(temp, new Vector3(startx, 0, startz), Quaternion.identity);
                    

                startx += offset;
            }
            startx = -98;
            startz += offset;
        }

    }

    void fill(int x, int y)
    {

        
    }

    private int assignroom()
    {
        return UnityEngine.Random.Range(0, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject findChildFromParent(string parentName, string childNameToFind)
    {
        string childLocation = "/" + parentName + "/" + childNameToFind;
        GameObject childObject = GameObject.Find(childLocation);
        return childObject;
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

}
