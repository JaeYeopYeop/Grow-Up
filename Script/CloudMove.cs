using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    private float _speed = 0.5f;
    private float _position;
    // Start is called before the first frame update
    void Start()
    {
        _position = this.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (_position > -7)
        {
            _position -= _speed * Time.deltaTime;
            this.transform.position = new Vector2(_position, 3);
        }
        else
        {
            _position = 6;
            this.transform.position = new Vector2(_position, 3);
        }
                

                

    }
}
