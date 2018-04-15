using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatePoints : MonoBehaviour {

    public int clusterDensity = 20;
    public float minimumDistance = 10f; 


    public List<Vector2> generatePoisson(float width, float height)
    {
        //http://devmag.org.za/2009/05/03/poisson-disk-sampling/
        float cellSize = minimumDistance / Mathf.Sqrt(2);
        Vector2[,] grid = new Vector2[(int)Mathf.Ceil(width / cellSize), (int)Mathf.Ceil(height / cellSize)];
        List<Vector2> processList = new List<Vector2>();
        List<Vector2> samplePoints = new List<Vector2>();

        Vector2 firstPoint = new Vector2(Random.Range(0, width), Random.Range(0, height));
        processList.Add(firstPoint);
        samplePoints.Add(firstPoint);
        grid[(int)(firstPoint.x / cellSize), (int)(firstPoint.y / cellSize)] = firstPoint;

        while(!(processList.Count == 0))
        {
            Vector2 point = processList[Random.Range(0, processList.Count - 1)];
            for (int i = 0; i < clusterDensity; i++)
            {
                Vector2 newPoint = generatePointAround(point, minimumDistance);

                if (!inNeighborhood(grid, newPoint, cellSize) && inRectangle(newPoint, width, height))
                {
                    processList.Add(newPoint);
                    samplePoints.Add(newPoint);
                    grid[(int)(newPoint.x / cellSize), (int)(newPoint.y / cellSize)] = newPoint;
                }
            }
        }
        return samplePoints;
    }

    private Vector2 generatePointAround(Vector2 point, float dist)
    {
        float r1 = Random.Range(0, 1);
        float r2 = Random.Range(0, 1);

        float radius = (r1 + 1) * dist;
        float angle = Mathf.PI * 2 * r2;
        Vector2 newPoint = new Vector2(point.x + radius * Mathf.Cos(angle), point.y + radius * Mathf.Sin(angle));

        return newPoint;
    }

    private bool inRectangle(Vector2 point, float width, float height)
    {
        if(point.x >= 0 && point.x <= width && point.y >= 0 && point.y <= height)
        {
            return true;
        }
        return false; 
    }

    private bool inNeighborhood(Vector2[,] grid, Vector2 point, float cellSize)
    {
        Vector2 gridPoint = new Vector2((int)(point.x / cellSize), (int)(point.y / cellSize));

        for(float i = gridPoint.x - 2; i < gridPoint.x + 2; i++)
        {
            for(float j = gridPoint.y - 2; i < gridPoint.y + 2; j++)
            {
                if(i == gridPoint.x && j == gridPoint.y)
                {
                    continue; 
                }
                else
                {
                    if(grid[(int)i, (int)j] != null)
                    {
                        Vector2 otherPoint = grid[(int)i, (int)j];

                        float distance = Mathf.Sqrt(Mathf.Pow(point.x - otherPoint.x, 2) + Mathf.Pow(point.y - otherPoint.y, 2));
                        if(distance < minimumDistance)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false; 
    }
}
