using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerationScript : MonoBehaviour {
	//GameObject 
	public GameObject boostPrefab;
	public GameObject penalityPrefab;
	public GameObject reversePrefab;
	public GameObject pickUps;
	public GameObject wallPrefab;
	public GameObject jumpPrefab;
	public GameObject singlePalmPrefab;
	public GameObject bananaPrefab;
	public Texture wallTexture;
	public Texture finishTexture;
	private GameObject ground;

	//List of gameObject created
	private List<GameObject> jumps;
	private List<GameObject> boosts;
	private List<GameObject> penality;
	private List<GameObject> reverse;
	private List<GameObject> walls;
	private List<GameObject> bananas;

	//Limitof each gameobject
	private int jumpsNumber;
	private int boostsNumber;
	private int penalitiesNumber;
	private int reversesNumber;

	//Dimension map
	private float mapWidth;
	private float mapLength;

	//The grid to see where there is obbjects
	private bool[][] grid;

	//The number of cases to put object when procedural generation
	private int step;

	void Start () 
	{
		//Initialization of lists, and numbers of objects
		boosts = new List<GameObject> ();
		penality = new List<GameObject> ();
		reverse = new List<GameObject> ();
		jumps = new List<GameObject> ();
		walls = new List<GameObject> ();
		bananas = new List<GameObject> ();
		jumpsNumber = 0;
		boostsNumber = 0;
		penalitiesNumber = 0;
		reversesNumber = 0;
		mapWidth = 20.0f;
		mapLength = 1000.0f;
		step = 25;

		//Initialization of grid
		grid = new bool[(int)mapLength][];
		for (int i=0; i<mapLength; i++) {
			grid [i] = new bool[(int)mapWidth];
			for (int j=0; j<mapWidth; j++) {
				grid [i] [j] = false;
			}
		}

		//Génération du sol
		ground = GameObject.CreatePrimitive (PrimitiveType.Cube);
		ground.transform.localScale = new Vector3 (mapWidth,4.0f,mapLength);
		ground.transform.position = new Vector3 (0.0f,-2.0f,(mapLength/2)-5);
		ground.GetComponent<Renderer>().material.mainTexture = wallTexture;

		GameObject g;

		//Loop to generate objects
		for (int i=step; i<mapLength; i++) {
			if (i % step == 0) {
				int objectType = (int)(Random.Range (0, 7));
				int xScale;
				int xPos;
				switch (objectType) {
				case 0:
					//Walls
					float yScale;
					xScale = (int)(Random.Range (1.0f, mapWidth / 2));
					xPos = (int)(Random.Range (-(mapWidth / 2) + xScale, (mapWidth / 2) - xScale));
					float jumpable = (Random.Range (0.0f, 1.0f));
					if (jumpable >= 0.5f) {
						yScale = (Random.Range (1.5f, 3.5f));
					} else {
						yScale = (Random.Range (0.1f, 0.7f));
					}
					createElement (new Vector3 (xPos, yScale / 2, i), new Vector3 (xScale, yScale, 1.0f), wallPrefab, 4);
					break;

				case 1:
					//Palm + wall
					for(int j=0; j<10; j=j+2)
					{
						xPos = (int)(-mapWidth/2 +1);
						yScale = (Random.Range(0.3f,1.0f));
						createElement(new Vector3(xPos, 0.0f, i+j), new Vector3(1.0f, yScale, 1.0f), singlePalmPrefab, -1);
						if(j==2)
						{
							xPos=0;
							xScale = (int)mapWidth/2;
							yScale = (Random.Range (1.5f, 3.5f));
							createElement (new Vector3 (-xScale/2-1, 0.5f, i+j-1), penalityPrefab.transform.localScale,penalityPrefab, 1); 
							createElement(new Vector3(xPos, yScale/2, i+j), new Vector3(xScale, yScale, 1.0f), wallPrefab, 4);
							for(int k=1; k<xScale-1; k++)
							{
								xPos = -xScale/2+k;
								createElement (new Vector3 (xPos, 0.5f, i+j-1), boostPrefab.transform.localScale,boostPrefab, 0); 
							}
							createElement (new Vector3 (xScale/2+1, 0.5f, i+j-1), penalityPrefab.transform.localScale,penalityPrefab, 1); 
						}
						xPos = (int)(mapWidth/2 -1);
						yScale = (Random.Range(0.3f,1.0f));
						createElement(new Vector3(xPos, 0.0f, i+j), new Vector3(1.0f, yScale, 1.0f), singlePalmPrefab, -1);
					}  
					break;
				case 2:
					//Bannanas
					xPos = (int)(Random.Range (-(mapWidth / 4) , (mapWidth / 4)));
					//createElement(new Vector3(xPos, 1.0f, i), new Vector3(1.0f,1.0f,1.0f),bananaPrefab, 5);
					g = GameObject.Instantiate (bananaPrefab, new Vector3(xPos, bananaPrefab.transform.position.y, i), bananaPrefab.transform.rotation) as GameObject;
					bananas.Add (g);
					break;

				case 3:
					//Pickables
					int nbAlea = (int)(Random.Range (0, 3));
					xPos = (int)(Random.Range (-(mapWidth / 4) , (mapWidth / 4)));
					GameObject prefab;
					if (nbAlea == 0) {
						prefab = boostPrefab;
					} else if (nbAlea == 1) {
						prefab = penalityPrefab;
					} else {
						prefab = reversePrefab;
					}
					createElement (new Vector3 (xPos, 0.5f, i), prefab.transform.localScale, prefab, nbAlea); 
					
					break;

				case 4:
					//Pattern palms
					nbAlea = (int)(Random.Range(-mapWidth /2+2, mapWidth/2-2));
					for(int j=(int)(-mapWidth/2); j<(int)(mapWidth/2); j++)
					{
						if(j!=nbAlea && j!=nbAlea+1)
						{
							float yScalePalm = (Random.Range(0.3f,1.0f));
							createElement(new Vector3(j,0.0f, i), new Vector3(1.0f,yScalePalm,1.0f), singlePalmPrefab, -1);
						}
						else
						{
							g = GameObject.Instantiate (bananaPrefab, new Vector3(j, bananaPrefab.transform.position.y, i), bananaPrefab.transform.rotation) as GameObject;
							bananas.Add (g);
						}
					}
					break;

				case 5:
					//Pattern jumps + walls
					xPos = (int)(Random.Range(-mapWidth /2+2.0f , mapWidth/2 -9.5f));
					yScale = (Random.Range(2.5f,3.5f));
					createElement(new Vector3(xPos,0.0f,i), new Vector3(1.5f, 1.0f, 1.0f), jumpPrefab,3);
					createElement(new Vector3(0.0f,0.0f, i+5), new Vector3(mapWidth, yScale, 1.0f), wallPrefab, 4);
					//Bonuses
					for(int j=0; j<3; j++)
					{
						int bonusesAlea = (int)(Random.Range (0.0f, 3.0f));
						GameObject prefabBonuses;
						if (bonusesAlea == 0) {
							prefabBonuses = boostPrefab;
						} else if (bonusesAlea == 1) {
							prefabBonuses = penalityPrefab;
						} else {
							prefabBonuses = reversePrefab;
						}
						createElement (new Vector3 (xPos + 3 +j*2, yScale, i+5), prefabBonuses.transform.localScale, prefabBonuses, bonusesAlea); 
						g = GameObject.Instantiate (bananaPrefab, new Vector3(xPos + 3 + j*2, yScale +0.5f, i+7), bananaPrefab.transform.rotation) as GameObject;
						bananas.Add (g);
					}
					break;

				case 6:
					//Walls patterns
					xScale = (int)(mapWidth/2-0.5f);
					xPos = (int)(-mapWidth/2+xScale/2)+1;
					yScale = (Random.Range (1.5f, 3.5f));
					createElement (new Vector3 (xPos, yScale / 2, i), new Vector3 (xScale, yScale, 1.0f), wallPrefab, 4);
					xPos = (int)(mapWidth/2-xScale/2)-1;
					createElement (new Vector3 (xPos, yScale / 2, i+10), new Vector3 (xScale, yScale, 1.0f), wallPrefab, 4);
					xPos = (int)(-mapWidth/2+xScale/2)+1;
					createElement (new Vector3 (xPos, yScale / 2, i+20), new Vector3 (xScale, yScale, 1.0f), wallPrefab, 4);
					i=i+20;
					break;
				default:
					break;
				}
			}
		}

		GameObject finish = GameObject.CreatePrimitive (PrimitiveType.Plane);
		finish.transform.position = new Vector3 (0.0f, 0.001f, mapLength - 5.0f);
		finish.GetComponent<Renderer>().material.mainTexture = finishTexture;;
		finish.transform.rotation = new Quaternion (0.0f, 0.0f, 0.0f, 0.0f);
		finish.transform.localScale = new Vector3 (2.0f, 1.0f, 2.0f);
		finish.AddComponent (typeof(BoxCollider));;
		BoxCollider collid = finish.GetComponent (typeof(BoxCollider)) as BoxCollider;
		collid.isTrigger = true;;
		finish.tag = "Finish";
		finish.name = "Finish";

		//To put the object "pickable"
		for (int i=0; i<boosts.Count; i++) {
			boosts [i].transform.parent = pickUps.transform;
		}
		for (int i=0; i<penality.Count; i++) {
			penality [i].transform.parent = pickUps.transform;
		}
		for (int i=0; i<reverse.Count; i++) {
			reverse [i].transform.parent = pickUps.transform;
		}
		for (int i=0; i<bananas.Count; i++) {
			bananas[i].transform.parent = pickUps.transform;
			bananas[i].tag = "Banana";
			bananas[i].AddComponent (typeof(BoxCollider));
			BoxCollider collider = bananas[i].GetComponent(typeof(BoxCollider)) as BoxCollider;
			collider.isTrigger = true;
		}
		for (int i=0; i<jumps.Count; i++)
		{
			jumps[i].AddComponent(typeof(MeshCollider));
		}
		for(int i=0; i<walls.Count; i++)
		{
			walls[i].tag = "Wall";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void createElement(Vector3 position, Vector3 scale, GameObject prefab, int list)
	{
		GameObject g = GameObject.Instantiate (prefab, position, prefab.transform.rotation) as GameObject;
		g.transform.localScale = scale;
		if (list == 0) {
			boosts.Add (g);
		} else if (list == 1) {
			penality.Add (g);
		} else if (list == 2) {
			reverse.Add (g);
		} else if (list == 3) {
			jumps.Add (g);
		} else if (list == 4) {
			walls.Add (g);
		} else if (list == 5) {
			bananas.Add(g);
		}
	}
}