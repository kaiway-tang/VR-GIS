using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    Transform trfm;
    [SerializeField] Node[] adjacents;

    public static Node[] nodes = new Node[70];
    public static int ID;

    private void Awake()
    {
        trfm = transform;

        nodes[ID] = this;
        ID++;

        gameObject.SetActive(false);
    }

    public Vector3 Position()
    {
        return trfm.position;
    }

    static Node node;
    public Node GetRandomAdjacent(Node lastNode = null)
    {
        for (int i = 0; i < 100; i++)
        {
            node = adjacents[Random.Range(0, adjacents.Length)];
            if (node != lastNode)
            {
                return node;
            }
        }

        return node;
    }
}
