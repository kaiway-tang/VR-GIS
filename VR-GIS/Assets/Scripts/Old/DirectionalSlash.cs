using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalSlash : MonoBehaviour
{
    [SerializeField] bool doubleSlash;
    [SerializeField] int window;
    int timer;
    [SerializeField] GameObject forwardsIndicator, backwardsIndicator;
    [SerializeField] Transform trfm, indicatorTrfm;
    [SerializeField] GameObject slashFX;

    DirectionalSlashHandler m_slashHandler;
    int hits, m_damage;
    // Start is called before the first frame update
    public void Init(int damage, float distance, DirectionalSlashHandler slashHandler, int angle)
    {
        if (doubleSlash) { hits = 2; }
        else { hits = 1; }
        trfm = transform;
        m_damage = damage;
        trfm.forward = GameManager.eyesTrfm.position - trfm.position;
        trfm.position += trfm.forward * distance;

        indicatorTrfm.localEulerAngles = new Vector3(0, 0, angle + 180);

        m_slashHandler = slashHandler;
        trfm.parent = slashHandler.transform;
    }

    private void Update()
    {
        trfm.forward = GameManager.eyesTrfm.position - trfm.position;
    }

    public void Hit(int speed)
    {
        Instantiate(slashFX, indicatorTrfm.position, indicatorTrfm.rotation);
        m_slashHandler.Hit(m_damage);

        hits--;
        if (hits < 1)
        {
            Cleared();
        }
    }

    public void Cleared()
    {
        Destroy(gameObject);
        return;
    }

    void EnableIndicator()
    {
        indicatorTrfm.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer > 0)
        {
            timer--;
        }
    }

    int lastTriggerID;
    public void TriggerEnter(int ID)
    {
        if (doubleSlash)
        {
            if (lastTriggerID != ID)
            {
                if (timer > 0)
                {
                    if (lastTriggerID == 1)
                    {
                        forwardsIndicator.SetActive(false);
                        Hit(timer);
                    }
                    else if (lastTriggerID == 2)
                    {
                        backwardsIndicator.SetActive(false);
                        Hit(timer);
                    }
                }

                timer = window;
            }
            lastTriggerID = ID;
        }
        else
        {
            if (ID == 2)
            {
                if (timer > 0)
                {
                    Hit(timer);
                    timer = 0;
                }
            }
        }
    }

    public void TriggerExit(int ID)
    {
        if (doubleSlash)
        {

        }
        else
        {
            if (ID == 1)
            {
                timer = window;
            }
        }
    }
}
