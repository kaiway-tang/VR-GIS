using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalSlashTrigger : MonoBehaviour
{
    [SerializeField] int triggerID;
    [SerializeField] DirectionalSlash directionalSlash;
    private void OnTriggerEnter(Collider other)
    {
        directionalSlash.TriggerEnter(triggerID);
    }

    private void OnTriggerExit(Collider other)
    {
        directionalSlash.TriggerExit(triggerID);
    }
}
