using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private int _powerupID;
    //_powerupID = 0: triple shot; = 1: speed; = 2: shields.
    private float _randomX;

    [SerializeField] private AudioClip _powerUpAudioClip;
    void Start()
    {
        
        _randomX = Random.Range(-13f, 13f);
        transform.position = new Vector3(_randomX, 9, 3);
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 movingDirection = new Vector3(0, -1f, 0);

        transform.Translate(movingDirection * _speed * Time.deltaTime);

        if(transform.position.y <= -9)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent <Player>();

            if (other.gameObject.CompareTag("Player"))
            {
                AudioSource.PlayClipAtPoint(_powerUpAudioClip, transform.position);
                if (player != null)
                {
                    switch(_powerupID)
                    {
                        case 0:
                        {
                            player.TripleShotActive();
                            break;
                        }
                        case 1:
                        {
                            player.SpeedBoostActive();
                            break;
                        }
                        case 2:
                        {
                            player.ShieldActive();
                            break;
                        }
                        case 3:
                        {
                            player.SuperBuffActive();
                            break;
                        }
                    }
                }
                
                Destroy(this.gameObject);
            }
    }
}

