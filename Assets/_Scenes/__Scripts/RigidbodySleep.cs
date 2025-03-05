// MODULE PURPOSE:
// This module allows the Rigidbody that it is connected to to sleep
// for a specified amount of time

// Boilerplate Unity includes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class RigidbodySleep : MonoBehaviour {
    private int sleepCountdown = 4;
    private Rigidbody rigid;

    void Awake() {
        rigid = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if( sleepCountdown > 0 ) {
            rigid.Sleep();
            sleepCountdown--;
        }
    }
}
