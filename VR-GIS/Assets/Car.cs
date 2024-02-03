using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] float topSpd, effectiveSpd;
    [SerializeField] Node destination;
    int currentNode;
    Rigidbody rb;
    Transform trfm;

    float distanceToNode, lastDistance;

    void Start()
    {
        trfm = transform;

        destination = Node.nodes[Random.Range(0, Node.ID)];
        trfm.position = destination.Position();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastDistance = distanceToNode;
        distanceToNode = Vector3.Distance(destination.Position(), trfm.position);

        //HandleAcceleration();
        MoveTowardsNode();
    }

    [SerializeField] float close;
    void HandleAcceleration()
    {
        if (distanceToNode < topSpd * close)
        {
            if (effectiveSpd > topSpd / 12)
            {
                effectiveSpd -= topSpd / 25;
                if (effectiveSpd < topSpd / 25)
                {
                    effectiveSpd = topSpd / 25;
                }
            }
        }
        else if (effectiveSpd < topSpd)
        {
            effectiveSpd += topSpd / 50;
        }
    }

    [SerializeField] float testSpd;
    [SerializeField] float rotationSpd;
    void MoveTowardsNode()
    {
        trfm.rotation = Quaternion.RotateTowards(trfm.rotation, Quaternion.LookRotation(destination.Position() - trfm.position), rotationSpd);
        //trfm.forward += ((destination.Position() - trfm.position).normalized - trfm.forward) * .02f;
        trfm.position += trfm.forward * Scale(testSpd);

        if (AtNode())
        {
            destination = destination.GetRandomAdjacent();
        }
    }

    bool AtNode()
    {
        return distanceToNode < Scale(testSpd) * 25;
        //return distanceToNode < topSpd * close * .5f;
    }

    float Scale(float value)
    {
        return value * MapController.scale / 70;
    }
}
