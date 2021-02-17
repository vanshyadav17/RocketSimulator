using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpin : MonoBehaviour
{
    public float spinRateX;
    public float spinRateY;
    public float spinRateZ;
    void Update()
    {
        gameObject.transform.Rotate(spinRateX, spinRateY, spinRateZ, Space.Self);

    }
}
