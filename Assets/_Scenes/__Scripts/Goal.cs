// MODULE PURPOSE:
// This script contains logic for the green goal object found in each level.


// Boilerplate Unity includes
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Renderer))]

public class Goal : MonoBehaviour {

    // Flag for if goal has been met or not
    static public bool goalMet = false;

    void OnTriggerEnter( Collider other ) {

        // When the trigger is hit by something...
        // Check if it is a projectile
        Projectile proj = other.GetComponent<Projectile>();

        if (proj != null) {

            // IF so, set goalMet to true
            Goal.goalMet = true;

            // Also set the alpha of the color to a higher opacity
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 0.75f;
            mat.color = c;
        }
    }
}