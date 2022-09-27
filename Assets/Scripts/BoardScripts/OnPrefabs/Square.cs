using System.Collections.Generic;
using UnityEngine;

namespace BoardScripts.OnPrefabs
{
    public class Square : MonoBehaviour
    {
        public List<Square> adjacentWalkableSquares = new List<Square>();
        public List<Square> horizontalWalkableSquares = new List<Square>();
        public List<Square> verticallWalkableSquares = new List<Square>();
    }
}
