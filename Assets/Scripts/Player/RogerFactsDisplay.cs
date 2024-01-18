using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using TMPro;


// This class informs the player of random roger facts. 
public class RogerFactsDisplay : MonoBehaviour
{
    public List<string> RogerFacts;

    [SerializeField] TextMeshProUGUI m_Object;

    private async void Awake ( )
    {
        RogerFacts = new List<string>( );
        RogerFacts.Add( "Roger likes to take walkies." );
        RogerFacts.Add( "Roger often ponders the meaning of life." );
        RogerFacts.Add( "Roger thinks great thoughts about cheese." );
        RogerFacts.Add( "Roger has someone special in his life." );
        RogerFacts.Add( "Roger had a big white dog in the past, but now he has three brown cats." );
        RogerFacts.Add( "Roger feels great about his prospects for the future." );
        RogerFacts.Add( "Roger was fired yesterday from his job at the cheese factory." );
        RogerFacts.Add( "Roger has type O blood. That's quite nice." );
        RogerFacts.Add( "Roger's parents live just down the block. Maybe he should visit!" );
        RogerFacts.Add( "Roger saw a spider yesterday." );
        RogerFacts.Add( "Roger is never sure of what hat he should wear." );
        RogerFacts.Add( "Roger chews gum like it is going out of style!" );
        /*
        RogerFacts.Add( "Roger " );
        RogerFacts.Add( "Roger " );
        RogerFacts.Add( "Roger " );
        RogerFacts.Add( "Roger " );
        RogerFacts.Add( "Roger " );
        RogerFacts.Add( "Roger " );
        RogerFacts.Add( "Roger " );
        */
        await ChangeFact( );
    }

    protected async Task ChangeFact ( )
    {
        while ( true ) {
            await Task.Delay( 10000 );
            m_Object.text = RogerFacts[ Random.Range( 0, RogerFacts.Count ) ];
        }

    }



}
