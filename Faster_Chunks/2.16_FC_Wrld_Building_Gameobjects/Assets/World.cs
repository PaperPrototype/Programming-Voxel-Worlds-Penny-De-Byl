using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

	public static int cSizeX = 32;
	public static int cSizeY = 512;
	public static int cSizeZ = 32;
	public static Vector3[,,] allVertices = new Vector3[cSizeX + 1,cSizeY+1,cSizeZ+1];
	public static Vector3[] allNormals = new Vector3[6];
	public enum NDIR {UP, DOWN, LEFT, RIGHT, FRONT, BACK}
	public GameObject chunkPrefab;

	public Dictionary<string, GameObject> chunks = new Dictionary<string, GameObject>();

	public string BuildChunkName(Vector3 pos)
    {
		return
			pos.x + "_" +
			pos.y + "_" +
			pos.z + "_";

	}

	private void BuildChunkAt(Vector3 pos)
	{
		//build chunk here
		GameObject c = Instantiate(chunkPrefab, pos, this.transform.rotation, transform);
		c.name = BuildChunkName(pos);
		c.GetComponent<Chunk>().CreateChunk(cSizeX, cSizeY, cSizeZ);
		chunks.Add(c.name, c);
	}

	// Use this for initialization
	void Start () {

		// if the chunk size is set lower by user then
		// optimize for smaller mesh data
		if (World.cSizeX * World.cSizeY * World.cSizeZ < 16384)
		{
			Debug.Log("Lower IndexFormat for chunk meshes");
		}
		// else set max mesh data higher
		else
		{
			Debug.Log("Higher IndexFormat for chunk meshes");
		}


		//generate all vertices
		for (int z = 0; z <= cSizeZ; z++)
			for(int y = 0; y <= cSizeY; y++)
				for(int x = 0; x <= cSizeX; x++)
				{
					allVertices[x,y,z] = new Vector3(x,y,z);	 
				}

		allNormals[(int) NDIR.UP] = Vector3.up;
		allNormals[(int) NDIR.DOWN] = Vector3.down;
		allNormals[(int) NDIR.LEFT] = Vector3.left;
		allNormals[(int) NDIR.RIGHT] = Vector3.right;
		allNormals[(int) NDIR.FRONT] = Vector3.forward;
		allNormals[(int) NDIR.BACK] = Vector3.back;

		for(int y = 0; y < 3; y++)
        {
			BuildChunkAt(new Vector3(transform.position.x, transform.position.y + (y * cSizeY), transform.position.z));
        }
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
}
