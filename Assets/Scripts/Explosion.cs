using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    [SerializeField] private AudioClip _explosionAudioClip;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Asteroid Audio Source is NULL");
        }
        _audioSource.clip = _explosionAudioClip;
        _audioSource.Play();
        Destroy(this.gameObject, 3f);
    }
}
