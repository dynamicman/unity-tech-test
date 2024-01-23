using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using TMPro;

public class Targets : MonoBehaviour
{
    private int CurrentTargetIndex = -1;
    public List<GameObject> TargetsToVisit;

    private List<GameObject> VisitedTarget;

    public GameObject FinalTarget;

    private async void Awake ( )
    {
        foreach ( GameObject target in TargetsToVisit ) {
            target.SetActive( false );
        }
        FinalTarget.SetActive( false );
        NextTarget( );
    }

    public async void NextTarget ( )
    {
        if ( CurrentTargetIndex != -1 ) {
            VisitedTarget.Add( TargetsToVisit[ CurrentTargetIndex ] );
            TargetsToVisit.RemoveAt( CurrentTargetIndex );
        }

        if ( TargetsToVisit.Count > 0 ) {

            await Task.Delay( 1000 );
            CurrentTargetIndex = Random.Range( 0, TargetsToVisit.Count );
            TargetsToVisit[ CurrentTargetIndex ].SetActive( true );
            //NextTarget( );

        } else {
            FinalTarget.SetActive( true );
        }
    }

    public async void ArrivedAtFinalTarget ( )
    {
        //TODO: Play victory music!
    }
}
