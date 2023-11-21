using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    void Update()
    {
        Vector3 direction = new Vector3 (0, 1, 0);
        transform.Translate(direction * _speed * Time.deltaTime);   

        if(transform.position.y >= 50f)
        {
            if(this.transform.parent != null)
            {
                Destroy(this.transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

}
