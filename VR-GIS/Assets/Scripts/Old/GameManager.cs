using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform m_eyesTrfm;
    public static Transform eyesTrfm;
    // Start is called before the first frame update
    void Awake()
    {
        eyesTrfm = m_eyesTrfm;
    }
}
