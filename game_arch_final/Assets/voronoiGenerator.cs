using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voronoiGenerator : MonoBehaviour {

    public GameObject model;
    public generatePoints points;
    public Shader shader;
    public int clusterDensity = 20;
    public float minimumDistance = 10f;
	// Use this for initialization
	void Start ()
    {
        model.GetComponent<Renderer>().material.shader = shader;
        points.clusterDensity = clusterDensity;
        points.minimumDistance = minimumDistance;
        List<Vector2> voronoiPoints = points.generatePoisson(model.GetComponent<Renderer>().material.mainTexture.width, model.GetComponent<Renderer>().material.mainTexture.height);
        model.GetComponent<Renderer>().material.SetInt("_Points_Length", voronoiPoints.Count); 
        for(int i = 0; i < voronoiPoints.Count; i++)
        {
            model.GetComponent<Renderer>().material.SetVector("_Points" + i.ToString(), voronoiPoints[i]);
            model.GetComponent<Renderer>().material.SetVector("_Color" + i.ToString(), new Vector3(Random.ColorHSV().r, Random.ColorHSV().g, Random.ColorHSV().b));
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
