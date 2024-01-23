using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private NavGridPathNode[] _currentPath = Array.Empty<NavGridPathNode>();
    private int _currentPathIndex = 0;
    
    [SerializeField]
    private NavGrid _grid;
    [SerializeField]
    private float _speed = 10.0f;

    private bool WALKING = false;
    public Transform Shoulder_Left, Shoulder_Right;

    public GameObject Confetti;
    private GameObject ActiveConfetti;

    void Update()
    {
        if ( WALKING ) {
            DoSwingArms( );
        }

        // Check Input
        if (Input.GetMouseButtonUp(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                _currentPath = _grid.GetPath(transform.position, hitInfo.point);
                _currentPathIndex = 0;
            }
        }

        // Traverse
        if (_currentPathIndex < _currentPath.Length)
        {
            var currentNode = _currentPath[_currentPathIndex];
            
            var maxDistance = _speed * Time.deltaTime;
            var vectorToDestination = currentNode._position - transform.position;
            var moveDistance = Mathf.Min(vectorToDestination.magnitude, maxDistance);

            var moveVector = vectorToDestination.normalized * moveDistance;
            moveVector.y = 0f; // Ignore Y
            transform.position += moveVector;
            transform.forward = moveVector;

            if (transform.position == currentNode._position)
                _currentPathIndex++;

            WALKING = true;
        } else {
            if ( WALKING ) {
                if ( ActiveConfetti  != null ) { Destroy( ActiveConfetti ); }
                ActiveConfetti = Instantiate( Confetti, transform.position, Quaternion.identity );
            }
            WALKING = false;
        }
    }

    private void DoSwingArms ( )
    {
        float sin = Mathf.Sin(Time.time * 10.0f);
        Shoulder_Left.localEulerAngles = new Vector3( sin * 30, 0, 0 );
        Shoulder_Right.localEulerAngles = new Vector3( sin * -30, 0, 0 );
    }
}
