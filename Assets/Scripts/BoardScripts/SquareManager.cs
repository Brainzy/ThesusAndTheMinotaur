using System.Collections.Generic;
using BoardScripts.OnPrefabs;
using UnityEngine;

namespace BoardScripts
{
    public class SquareManager : MonoBehaviour
    {
        [SerializeField] private List<Square> allSquares = new List<Square>();
        
        public static SquareManager inst;
        private readonly Dictionary<Vector3, Square> positionToSquare = new Dictionary<Vector3, Square>();

        public List<Square> ReturnAllSquares()
        {
            return allSquares;
        }

        public Square ReturnSquareOnPosition(Vector3 pos)
        {
            return positionToSquare.ContainsKey(pos) ? positionToSquare[pos] : null;
        }

        private void Awake()
        {
            inst = this;
            for (int i = 0; i < allSquares.Count; i++)
            {
                var square = allSquares[i];
                positionToSquare.Add(square.transform.position,square);
            }
        }
        
        
        
        
    }
}
