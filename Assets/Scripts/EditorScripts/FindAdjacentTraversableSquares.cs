using System.Collections.Generic;
using BoardScripts;
using BoardScripts.OnPrefabs;
using UnityEngine;

namespace EditorScripts
{
    public class FindAdjacentTraversableSquares : MonoBehaviour
    {
        [SerializeField] private LayerMask squareLayer;
        [SerializeField] private bool executeSearch;
        [SerializeField] private SquareManager squareManager;
        [SerializeField] private List<Vector3> rayCastDirections = new List<Vector3>();

        private void OnValidate()
        {
            if (!executeSearch)
                return;

            executeSearch = false;
            
            var allSquares = squareManager.ReturnAllSquares();

            AddAdjacentWalkableSquares(allSquares);

            DifferentiateHorizontalAndVerticalAdjacentSquares(allSquares);
        }

        private static void DifferentiateHorizontalAndVerticalAdjacentSquares(List<Square> allSquares)
        {
            for (int i = 0; i < allSquares.Count; i++)
            {
                var square = allSquares[i];
                square.horizontalWalkableSquares.Clear();
                square.verticallWalkableSquares.Clear();

                for (int j = 0; j < square.adjacentWalkableSquares.Count; j++)
                {
                    var squarePosition = square.transform.position;

                    var adjacentSquare = square.adjacentWalkableSquares[j];
                    var adjacentSquarePosition = adjacentSquare.transform.position;

                    var difference = squarePosition - adjacentSquarePosition;

                    if (Mathf.Abs(difference.x) > 0.1f)
                    {
                        square.horizontalWalkableSquares.Add(adjacentSquare);
                    }
                    else
                    {
                        square.verticallWalkableSquares.Add(adjacentSquare);
                    }
                }
            }
        }

        private void AddAdjacentWalkableSquares(List<Square> allSquares)
        {
            for (int i = 0; i < allSquares.Count; i++)
            {
                var square = allSquares[i];
                square.adjacentWalkableSquares.Clear();
                var squarePos = square.transform.position;
                squarePos = new Vector3(squarePos.x, squarePos.y, 0.1f);

                for (int j = 0; j < rayCastDirections.Count; j++)
                {
                    var hits = Physics.RaycastAll(squarePos, rayCastDirections[j], 1,squareLayer );

                    if (hits.Length == 1) // no wall hit, means adjacent square is walkable
                    {
                        square.adjacentWalkableSquares.Add(hits[0].collider.gameObject.GetComponent<Square>());
                    }
                }
            }
        }
    }
}
        
