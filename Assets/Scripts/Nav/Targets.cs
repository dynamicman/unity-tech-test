using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using TMPro;

// Move to the target game!
public class Targets : MonoBehaviour
{
    public static Targets m;
    private int CurrentTargetIndex = -1;
    public List<GameObject> TargetsToVisit;
    private List<GameObject> VisitedTarget;

    public GameObject FinalTarget;

    private async void Awake ( )
    {
        m = this;

        VisitedTarget = new List<GameObject>( );

        foreach ( GameObject target in TargetsToVisit ) {
            target.SetActive( false );
        }
        FinalTarget.SetActive( false );
        NextTarget( );
    }

    // Deactivate the current target and activate the next one
    public async void NextTarget ( )
    {
        if ( FinalTarget.active ) {
            ArrivedAtFinalTarget( );
            return;
        }

        if ( CurrentTargetIndex != -1 ) {
            TargetsToVisit[ CurrentTargetIndex ].SetActive( false ); 
            VisitedTarget.Add( TargetsToVisit[ CurrentTargetIndex ] );
            TargetsToVisit.RemoveAt( CurrentTargetIndex );
        }

        if ( TargetsToVisit.Count > 5 ) {

            await Task.Delay( 1000 );
            CurrentTargetIndex = Random.Range( 0, TargetsToVisit.Count - 1 );
            TargetsToVisit[ CurrentTargetIndex ].SetActive( true );
            //NextTarget( );

        } else {
            await Task.Delay( 1000 );
            FinalTarget.SetActive( true );
        }
    }

    // Reset the game
    public async void ArrivedAtFinalTarget ( )
    {
        FinalTarget.SetActive( false );
        //TODO: Play victory music!
        foreach ( GameObject target in VisitedTarget ) {
            TargetsToVisit.Add( target );
        }
        VisitedTarget = new List<GameObject>( );
        NextTarget( );
    }
}
