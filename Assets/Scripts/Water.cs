using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    //Our physics arrays
    float[] xpositions;
    float[] ypositions;
    float[] velocities;
    float[] accelerations;
	float[] leftDeltas;
	float[] rightDeltas;

	//Our meshes and colliders
	GameObject[] meshobjects;
    GameObject[] colliders;
    Mesh[] meshes;

	Vector3[][] meshVertices;

	//Our particle system
	public GameObject splash;

    //The GameObject we're using for a mesh
    public GameObject watermesh;

    //All our constants
    public float springconstant = 0.02f;
    public float damping = 0.04f;
    public float spread = 0.05f;
    public float z = -1f;

    //The properties of our water
    float top;
    float left;
    float bottom;
    
    void Awake() {
        
    }

    void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
		spriteRenderer.enabled = false;
		SpawnWater(spriteRenderer.bounds);
	}

    
    public void Splash(float xpos, float velocity) {
        //If the position is within the bounds of the water:
        if (xpos >= xpositions[0] && xpos <= xpositions[xpositions.Length-1])
        {
            //Offset the x position to be the distance from the left side
            xpos -= xpositions[0];

            //Find which spring we're touching
            int index = Mathf.RoundToInt((xpositions.Length-1)*(xpos / (xpositions[xpositions.Length-1] - xpositions[0])));

            //Add the velocity of the falling object to the spring
            velocities[index] += velocity;

            //Set the lifetime of the particle system.
            float lifetime = 0.93f + Mathf.Abs(velocity)*0.07f;

			

            //Create the splash and tell it to destroy itself.
            GameObject splish = ObjectPoolManager.GetObject(splash);

			//this should be done AFTER instantiating, otherwise you're editing the prefab!
			//Set the splash to be between two values in Shuriken by setting it twice.
			ParticleSystem ps = splish.GetComponent<ParticleSystem>();
			ParticleSystem.MainModule main = ps.main;
			main.startSpeed = new ParticleSystem.MinMaxCurve(
				10 + 5 * Mathf.Pow(Mathf.Abs(velocity), 0.5f),
				15 + 5 * Mathf.Pow(Mathf.Abs(velocity), 0.5f)
			);
			main.startLifetime = lifetime;

			//Set the correct position of the particle system.
			Vector3 position = new Vector3(xpositions[index], ypositions[index] - 0.35f, 5);

			splish.transform.position = position;
			ObjectPoolManager.ReturnObject(splish, lifetime + .5f);
        }
    }

    public void SpawnWater(Bounds bounds)  {
        left = bounds.center.x - bounds.extents.x;
        top = bounds.center.y + bounds.extents.y;
        bottom = bounds.center.y - bounds.extents.y;

        float width = bounds.extents.x * 2;

        //Calculating the number of edges and nodes we have
        int edgecount = Mathf.RoundToInt(width) * 5;
        int nodecount = edgecount + 1;

        //Declare our physics arrays
        xpositions = new float[nodecount];
        ypositions = new float[nodecount];
        velocities = new float[nodecount];
        accelerations = new float[nodecount];

		leftDeltas = new float[nodecount];
		rightDeltas = new float[nodecount];

		//Declare our mesh arrays
		meshobjects = new GameObject[edgecount];
        meshes = new Mesh[edgecount];
        colliders = new GameObject[edgecount];

		meshVertices = new Vector3[edgecount][];

		//For each node, set the line renderer and our physics arrays
		for (int i = 0; i < nodecount; i++)
        {
            ypositions[i] = top;
            xpositions[i] = left + width * i / edgecount;
            accelerations[i] = 0;
            velocities[i] = 0;
        }

        //Setting the meshes now:
        for (int i = 0; i < edgecount; i++)
        {
            //Make the mesh
            meshes[i] = new Mesh();

			//Create the corners of the mesh
			meshVertices[i] = new Vector3[4];
			meshVertices[i][0] = new Vector3(xpositions[i], ypositions[i], z);
			meshVertices[i][1] = new Vector3(xpositions[i + 1], ypositions[i + 1], z);
			meshVertices[i][2] = new Vector3(xpositions[i], bottom, z);
			meshVertices[i][3] = new Vector3(xpositions[i+1], bottom, z);

            //Set the UVs of the texture
            Vector2[] UVs = new Vector2[4];
            UVs[0] = new Vector2(0, 1);
            UVs[1] = new Vector2(1, 1);
            UVs[2] = new Vector2(0, 0);
            UVs[3] = new Vector2(1, 0);

            //Set where the triangles should be.
            int[] tris = new int[6] { 0, 1, 3, 3, 2, 0};

            //Add all this data to the mesh.
            meshes[i].vertices = meshVertices[i];
            meshes[i].uv = UVs;
            meshes[i].triangles = tris;

            //Create a holder for the mesh, set it to be the manager's child
            meshobjects[i] = Instantiate(watermesh,Vector3.zero,Quaternion.identity) as GameObject;
            meshobjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
            meshobjects[i].transform.parent = transform;

            //Create our colliders, set them be our child
            colliders[i] = new GameObject();
            colliders[i].name = "Trigger";
            colliders[i].AddComponent<BoxCollider2D>();
            colliders[i].transform.parent = transform;

            //Set the position and scale to the correct dimensions
            colliders[i].transform.position = new Vector3(left + width * (i + 0.5f) / edgecount, top - 0.5f, 0);
            colliders[i].transform.localScale = new Vector3(width / edgecount, 1, 1);

            //Add a WaterDetector and make sure they're triggers
            colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
            colliders[i].AddComponent<WaterDetector>();

        }

        
        
        
    }

    //Same as the code from in the meshes before, set the new mesh positions
    void UpdateMeshes() {
        for (int i = 0; i < meshes.Length; i++)
        {

			meshVertices[i][0] = new Vector3(xpositions[i], ypositions[i], z);
			meshVertices[i][1] = new Vector3(xpositions[i+1], ypositions[i+1], z);
			meshVertices[i][2] = new Vector3(xpositions[i], bottom, z);
			meshVertices[i][3] = new Vector3(xpositions[i+1], bottom, z);

            meshes[i].vertices = meshVertices[i];
        }
    }



    void FixedUpdate() {
        //Here we use the Euler method to handle all the physics of our springs:
        for (int i = 0; i < xpositions.Length ; i++)
        {
            float force = springconstant * (ypositions[i] - top) + velocities[i]*damping ;
            accelerations[i] = -force;
            ypositions[i] += velocities[i];
            velocities[i] += accelerations[i];
        }

        //Now we store the difference in heights:

        //We make 8 small passes for fluidity:
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < xpositions.Length; i++)
            {
                //We check the heights of the nearby nodes, adjust velocities accordingly, record the height differences
                if (i > 0)
                {
                    leftDeltas[i] = spread * (ypositions[i] - ypositions[i-1]);
                    velocities[i - 1] += leftDeltas[i];
                }
                if (i < xpositions.Length - 1)
                {
                    rightDeltas[i] = spread * (ypositions[i] - ypositions[i + 1]);
                    velocities[i + 1] += rightDeltas[i];
                }
            }

            //Now we apply a difference in position
            for (int i = 0; i < xpositions.Length; i++)
            {
                if (i > 0)
                    ypositions[i-1] += leftDeltas[i];
                if (i < xpositions.Length - 1)
                    ypositions[i + 1] += rightDeltas[i];
            }
        }
        //Finally we update the meshes to reflect this
        UpdateMeshes();
	}

}
