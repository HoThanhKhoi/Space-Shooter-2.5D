using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private AudioClip _explosionAudioClip;
    private AudioSource _audioSource;
    private float randomX;

    private Player _player;
    private Animator _enemyAnimator;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Enemy Audio Source is NULL");
        }
        _audioSource.clip = _explosionAudioClip;

        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("PLayer is NULL");
        }


        _enemyAnimator = GetComponent<Animator>();
        if(_enemyAnimator == null)
        {
            Debug.LogError("Enemy Animator is NULL");
        }

        randomX = Random.Range(-13f, 13f);
        transform.position = new Vector3(randomX, 9, 3);
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 movementDirection = new Vector3 (0, -1, 0);
        transform.Translate(movementDirection * _speed * Time.deltaTime);


        randomX = Random.Range(-13f, 13f);
        if (transform.position.y <= -9)
        {
            transform.position = new Vector3(randomX, 9, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if(other.gameObject.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if(_player != null)
            {
                _player.AddScore(1);
            }
            _speed = 0;
            GetComponent<Collider2D>().enabled = false;
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(this.gameObject, 3f);
        }
        else if(other.gameObject.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.Damage();
            }
            _speed = 0;
            GetComponent<Collider2D>().enabled = false;
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(this.gameObject, 3f);
        }
    }

}
