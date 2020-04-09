using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
   [SerializeField] float speed = 1f;

    void Update()
    {
        transform.Rotate( 0, 0, speed * Time.deltaTime);
    }
}
