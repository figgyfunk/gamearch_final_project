using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voronoiGenerator : MonoBehaviour {

    public GameObject model;
    public generatePoints points;
    public Shader shader;
    public int clusterDensity = 5;
    public float minimumDistance = 100f;
    public bool water = false;
    public bool lava = false;
    public bool stones = false;
    public float gaps = 5f;
	// Use this for initialization
	void Start ()
    {

        points.clusterDensity = clusterDensity;
        points.minimumDistance = minimumDistance;
        List <Vector4> voronoiPoints = points.generatePoisson(model.GetComponent<Renderer>().material.mainTexture.width, model.GetComponent<Renderer>().material.mainTexture.height);
        model.GetComponent<Renderer>().material.SetInt("_Points_Length", voronoiPoints.Count);
        model.GetComponent<Renderer>().material.SetFloat("_Dist", minimumDistance);
        model.GetComponent<Renderer>().material.SetFloat("_Gap", gaps);
        model.GetComponent<Renderer>().material.SetVectorArray("_Points", voronoiPoints);
        if (water)
        {
            List<Vector4> colors = new List<Vector4>();
            for (int i = 0; i < voronoiPoints.Count; i++)
            {
                colors.Add(new Vector4(.529f, .741f, .839f, 1));
            }
            model.GetComponent<Renderer>().material.SetVector("_Gap_Color", new Vector4(.812f, .882f, .910f, 1));
            model.GetComponent<Renderer>().material.SetVectorArray("_Colors", colors);


        }
        else if (lava)
        {
            List<Vector4> colors = new List<Vector4>();
            for (int i = 0; i < voronoiPoints.Count; i++)
            {
                colors.Add(new Vector4(.941f, .314f, .204f, 1));
            }
            model.GetComponent<Renderer>().material.SetVectorArray("_Colors", colors);
            model.GetComponent<Renderer>().material.SetVector("_Gap_Color", new Vector4(1f, .545f, .224f, 1));
        }
        else if (stones)
        {
            List<Vector4> colors = new List<Vector4>();
            for (int i = 0; i < voronoiPoints.Count; i++)
            {
                colors.Add(new Vector4(.808f, .796f, .808f, 1));
            }
            model.GetComponent<Renderer>().material.SetVectorArray("_Colors", colors);
            model.GetComponent<Renderer>().material.SetVector("_Gap_Color", new Vector4(.498f, .490f, .494f, 1));
        }
        else
        {
            List<Vector4> colors = new List<Vector4>();
            for (int i = 0; i < voronoiPoints.Count; i++)
            {
                colors.Add(new Vector4(Random.value, Random.value, Random.value, 1));
            }

            model.GetComponent<Renderer>().material.SetVectorArray("_Colors", colors);
            model.GetComponent<Renderer>().material.SetVector("_Gap_Color", new Vector4(0, 0, 0, 1));
        }
        
        
    }

	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            print (model.GetComponent<Renderer>().material.GetInt("_Points_Length").ToString());
            print(model.GetComponent<Renderer>().material.GetVectorArray("_Points")[0].ToString());
        }
    }
}
