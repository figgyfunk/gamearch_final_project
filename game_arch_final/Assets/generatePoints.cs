using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generatePoints : MonoBehaviour {

    public int clusterDensity = 5;
    public float minimumDistance = 150f;

    private float width;
    private float height; 

    public List<Vector4> generatePoisson(float _width, float _height)
    {
        height = _height;
        width = _width;
        //http://devmag.org.za/2009/05/03/poisson-disk-sampling/
        float cellSize = minimumDistance / Mathf.Sqrt(2);
        Vector4[,] grid = new Vector4[(int)Mathf.Ceil(width / cellSize), (int)Mathf.Ceil(height / cellSize)];
        List<Vector4> processList = new List<Vector4>();
        List<Vector4> samplePoints = new List<Vector4>();

        Vector4 firstPoint = new Vector4(Random.Range(0, width), Random.Range(0, height));
        processList.Add(firstPoint);
        samplePoints.Add(firstPoint);
        grid[(int)(firstPoint.x / cellSize), (int)(firstPoint.y / cellSize)] = firstPoint;

        while(!(processList.Count == 0))
        {
            int index = Random.Range(0, processList.Count);
            Vector4 point = processList[index];
            processList.RemoveAt(index);
            //Debug.Log(processList.Count);
            //if(processList.Count > 20) { break; }
            int i = 0;
            //Debug.Log("Cluster Density: "+clusterDensity);
            while (i < clusterDensity)
            {
                Vector2 newPoint = generatePointAround(point, minimumDistance);
                //Debug.Log("Original: " + point + " New Point: " + newPoint);

                if (!inNeighborhood(grid, newPoint, cellSize) && inRectangle(newPoint, width, height))
                {
                    //Debug.Log("Added another point.");
                    processList.Add(newPoint);
                    samplePoints.Add(newPoint);
                    grid[(int)(newPoint.x / cellSize), (int)(newPoint.y / cellSize)] = newPoint;
                }
                i += 1;
                //Debug.Log(i);
            }
            //Debug.Log(processList.Count);
        }
        return samplePoints;
    }

    private Vector4 generatePointAround(Vector4 point, float dist)
    {
        float r1 = Random.value;
        float r2 = Random.value;

        float radius = (r1 + 1) * dist;
        float angle = Mathf.PI * 2 * r2;
        Vector4 newPoint = new Vector4(point.x + radius * Mathf.Cos(angle), point.y + radius * Mathf.Sin(angle));

        return newPoint;
    }

    private bool inRectangle(Vector4 point, float width, float height)
    {
        if(point.x >= 0 && point.x <= width && point.y >= 0 && point.y <= height)
        {
            return true;
        }
        return false; 
    }

    private bool inNeighborhood(Vector4[,] grid, Vector4 point, float cellSize)
    {
        Vector4 gridPoint = new Vector4((int)(point.x / cellSize), (int)(point.y / cellSize));
        //Debug.Log(gridPoint);
        //Debug.Log("Passed in point: " + point+ "When accessed by grid: "+grid[(int)gridPoint.x, (int)gridPoint.y]);
        for(float i = gridPoint.x - 2; i < gridPoint.x + 2; i++)
        {
            
            for(float j = gridPoint.y - 2; j < gridPoint.y + 2; j++)
            {
                //Debug.Log("loop?");
                if ((i == gridPoint.x && j == gridPoint.y) || j < 0 || j > (int)Mathf.Ceil(height / cellSize) - 1 || i < 0 || i > (int)Mathf.Ceil(width / cellSize) -1)
                {
                    //Debug.Log("always here?");
                    continue; 
                }
                else
                {
                    //Debug.Log("butts");
                    if(grid[(int)i, (int)j] != Vector4.zero)
                    {
                        //Debug.Log("Is it ever null? "+ grid[(int)i, (int)j]);
                        Vector2 otherPoint = grid[(int)i, (int)j];

                        float distance = Mathf.Sqrt(Mathf.Pow(point.x - otherPoint.x, 2) + Mathf.Pow(point.y - otherPoint.y, 2));
                        if(distance < minimumDistance)
                        {
                            //Debug.Log("In neighborhood.");
                            return true;
                        }
                    }
                }
            }
        }
        //Debug.Log("Nothing around.");
        return false; 
    }
}
