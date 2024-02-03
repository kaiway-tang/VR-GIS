using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalSlashHandler : MonoBehaviour
{
    [SerializeField] string slashDirections; //leave blank to use Random
    [SerializeField] float distanceFromSelf;
    [SerializeField] GameObject directionalSlashObject;

    public delegate void DamageHandler(int damage);
    public event DamageHandler OnDamageReceived;

    int directionsIndex;
    string[] m_directions;

    private void Start()
    {
        ParseDirections(slashDirections);
    }

    public void ParseDirections(string directions)
    {
        m_directions = directions.Split(',');
    }

    public void Hit(int damage)
    {
        OnDamageReceived.Invoke(damage);
    }
    
    public void CreateSlash(int damage)
    {
        int angle = 0;
        if (m_directions.Length == 0 || m_directions[directionsIndex] == "r")
        {
            angle = Random.Range(-90, 90);
        }
        else
        {
            angle = int.Parse(m_directions[directionsIndex]);
        }

        Instantiate(directionalSlashObject, transform.position, transform.rotation)
            .GetComponent<DirectionalSlash>().Init(damage, distanceFromSelf, this, angle);

        directionsIndex++;
        if (directionsIndex >= m_directions.Length) { directionsIndex = 0; }
    }
}
