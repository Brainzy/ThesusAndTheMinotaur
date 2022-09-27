using System.Collections.Generic;
using BoardScripts;
using BoardScripts.OnPrefabs;
using UnityEngine;

namespace EditorScripts
{
   public class GenerateGridWithSquares : MonoBehaviour
   {
      [SerializeField] private Vector2 gridSizeToGenerate;
      [SerializeField] private bool executeGeneration;
      [SerializeField] private GameObject squarePrefab;

      [SerializeField] private List<Square> allSquares = new List<Square>();
      
      private void OnValidate()
      {
         if (!executeGeneration)
            return;

         allSquares.Clear();
         executeGeneration = false;

         int counter = 0;
         for (int i = 0; i < gridSizeToGenerate.x; i++)
         {
            for (int j = 0; j < gridSizeToGenerate.y; j++)
            {
               counter++;
               var square =Instantiate(squarePrefab);
               var squareTransform = square.transform;
               var squareScript = square.GetComponent<Square>();
               square.name = "Square " + counter.ToString();
               allSquares.Add(squareScript);
               squareTransform.position = new Vector3(i, j);
            }
         }

      }
   }
}
