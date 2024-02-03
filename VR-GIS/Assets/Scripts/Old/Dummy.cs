using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    [SerializeField] ParticleSystem bloodFX;
    [SerializeField] int maxHP, HP;

    float knockback;
    int zMagnitude;

    DirectionalSlashHandler directionalSlashHandler;
    
    void Start()
    {
        directionalSlashHandler = GetComponent<DirectionalSlashHandler>();
        directionalSlashHandler.OnDamageReceived += OnHit;

        zMagnitude = transform.position.z > 0 ? 1 : -1;


        HP = maxHP;
        Invoke("CreateSlash", 3);
    }

    private void FixedUpdate()
    {
        HandleKnockback();
    }

    void HandleKnockback()
    {
        if (knockback > 0)
        {
            transform.position += Vector3.forward * knockback * zMagnitude;
            knockback -= .08f;
        }
        else
        {
            if (Mathf.Abs((zMagnitude - transform.position.z) * .1f) > .05f)
            {
                transform.position += Vector3.forward * -.05f * zMagnitude;
            }
            else
            {
                transform.position += Vector3.forward * (zMagnitude - transform.position.z) * .1f;
            }
        }
    }

    void OnHit(int damage)
    {
        HP -= damage;
        bloodFX.Play();
        knockback = .3f;

        Invoke("CreateSlash", .5f);
    }

    void CreateSlash()
    {
        directionalSlashHandler.CreateSlash(10);
    }
}
