using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Trigger when player enters the target
public class TargetTrigger : MonoBehaviour
{
    private void OnTriggerEnter  ( Collider other )
    {
        // Debug.LogError( "GOT TRIGGER " + other.gameObject.name );
        if ( other.gameObject.name  != "Player" ) { return; }
        Targets.m.NextTarget( );
    }
}
