using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    float horizontalInput;
    float verticalInput;
    [SerializeField] private Transform _laserPrefab;
    
    
    [SerializeField] private Transform _tripleShotPrefab;
    [SerializeField] private bool _enabledTripleShot = false;


    [SerializeField] private bool _enabledSpeedBoost = false;
    [SerializeField] private float _speedBoostMultiplier = 2f;

    
    [SerializeField] private bool _enabledShield = false;


    [SerializeField] private bool _enabledSuperBuff = false;
    [SerializeField] private Transform _superBuffShotPrefab;


    [SerializeField] private float _fireRate = 0.2f;
    private float _canFire = 0f;

    [SerializeField] private int _life = 3;

    private int _score;

    private SpawnManager _spawnManager;

    private UIManager _uiManager;

    [SerializeField] private AudioClip _laserAudioClip;
    [SerializeField] private AudioClip _playerDeathAudioClip;
    private AudioSource _audioSource;
    void Start()
    {
        transform.position = new Vector3(0, -5, 3);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if( _spawnManager == null)
        {
            Debug.LogError("Spawner is NULL");
        }

        if( _uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        if(_audioSource == null)
        {
            Debug.LogError("Audio Source on the Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserAudioClip;
        }


        //set damaged right/left engines to not appear yet
        this.transform.GetChild(2).gameObject.SetActive(false);
        this.transform.GetChild(3).gameObject.SetActive(false);
    }

    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= _canFire)
        {
            FireLaser();
        }

        
    }

    void CalculateMovement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //transform.Translate(new Vector3(1, 0, 0) * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(new Vector3(0, 1, 0) * verticalInput * _speed * Time.deltaTime);
        
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if (_enabledSuperBuff == false)
        {
            if (_enabledSpeedBoost == false)
            {
                transform.Translate(direction * _speed * Time.deltaTime);
            }
            else if (_enabledSpeedBoost == true)
            {
                transform.Translate(direction * (_speed * _speedBoostMultiplier) * Time.deltaTime);
            }
        }
        else if(_enabledSuperBuff == true)
        {
            transform.Translate(0, 0, 0);
        }

        //if the players move out of bound, warp them the the opposite side of the screen
        //(ex: move to the right and out of screen -> jump back to the left side of the screen)
        if (transform.position.x >= 15f)
        {
            transform.position = new Vector3(-15f, transform.position.y, 3);
        }
        else if (transform.position.x <= -15f)
        {
            transform.position = new Vector3(15f, transform.position.y, 3);
        }

        if (transform.position.y >= 9f)
        {
            transform.position = new Vector3(transform.position.x, -9f, 3);
        }
        else if (transform.position.y <= -9f)
        {
            transform.position = new Vector3(transform.position.x, 9f, 3);
        }
    }

    void FireLaser()
    {
        Vector3 laserOffset = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        Vector3 tripleShotOffset = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        _canFire = Time.time + _fireRate;

        //either use a premade vector 3 variable like "laserOffset" to solve this problem
        //or just add straight into the Instantiate
        //Instantiate(_laserPrefab, transform.position + Vector 3(0, 1f, 0), Quaternion.identity);


        if (_enabledTripleShot == false && _enabledSuperBuff == false)
        {
            Instantiate(_laserPrefab, laserOffset, Quaternion.identity);
        }
        else if(_enabledTripleShot == true && _enabledSuperBuff == false) 
        {
            Instantiate(_tripleShotPrefab, tripleShotOffset, Quaternion.identity);
        }
        else if(_enabledSuperBuff == true)
        {
            Vector3 superBuffOffset = Quaternion.Euler(0, 0, transform.eulerAngles.z) * tripleShotOffset;

            Instantiate(_superBuffShotPrefab, superBuffOffset, Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if(_enabledShield == false)
        {
            _life--;

            if (_life == 2)
            {
                this.transform.GetChild(2).gameObject.SetActive(true);
            }
            if (_life == 1)
            {
                this.transform.GetChild(3).gameObject.SetActive(true);
            }

            _uiManager.UpdateLives(_life);


        }
        else if (_enabledShield)
        {
            _enabledShield = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
        }

        if( _life <= 0)
        {
            _audioSource.clip = _playerDeathAudioClip;
            _audioSource.Play();
            _spawnManager.OnPlayerDeath();
            _uiManager.EndGame();
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _enabledTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _enabledTripleShot = false;      
    }


    public void SpeedBoostActive()
    {
        _enabledSpeedBoost = true;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _enabledSpeedBoost = false;
    }


    public void ShieldActive()
    {
        _enabledShield = true;
        this.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SuperBuffActive()
    {
        _enabledSuperBuff = true;
        _enabledShield = true;
        this.transform.GetChild(0).gameObject.SetActive(true);


        StartCoroutine(Rotate(5.0f));
        StartCoroutine(SuperBuffPowerDownRoutine());
    }

    IEnumerator SuperBuffPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _enabledSuperBuff = false;
    }

    IEnumerator Rotate(float duration)
    {
        Vector3 startRotation = transform.eulerAngles;
        float endRotation = startRotation.z + 3600.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation.z, endRotation, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(startRotation.x, startRotation.y, zRotation); // Changed yRotation to zRotation
            yield return null;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    
}
