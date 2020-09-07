using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	public Material cMaterial;
	public Block[,,] chunkData;
	public GameObject chunk;
	public GameObject parent;

    List<Vector3> Verts = new List<Vector3>();
	List<Vector3> Norms = new List<Vector3>();
	List<Vector2> UVs = new List<Vector2>();
	List<int> Tris = new List<int>();

	enum NeighborChunk { SELF, NEIGHBOR, OTHER };


	public void CalculateChunkData(int sizeX, int sizeY, int sizeZ)
	{
		chunkData = new Block[sizeX,sizeY,sizeZ];

		//create blocks
		for(int z = 0; z < sizeX; z++)
			for(int y = 0; y < sizeY; y++)
				for(int x = 0; x < sizeZ; x++)
				{
					CalculateChunkBlocks(x, y, z, new Vector3(x, y, z));
				}
	}

	// For shortening the above function
	void CalculateChunkBlocks(int x, int y, int z, Vector3 pos)
    {
		if (y == World.chunkHeight - 1)
			chunkData[x, y, z] = new Block(Block.BlockType.GRASS, pos, this, cMaterial);
		else if (Random.Range(0, 100) < 50)
			chunkData[x, y, z] = new Block(Block.BlockType.DIRT, pos, this, cMaterial);
		else
			chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos, this, cMaterial);
    }

	public void DrawChunk(int sizeX, int sizeY, int sizeZ)
	{
		//draw blocks
		Verts.Clear();
		Norms.Clear();
		UVs.Clear();
		Tris.Clear();
		for(int z = 0; z < sizeX; z++)
			for(int y = 0; y < sizeY; y++)
				for(int x = 0; x < sizeZ; x++)
				{
					chunkData[x,y,z].Draw(Verts, Norms, UVs, Tris);
				}

		Mesh mesh = new Mesh();
	    mesh.name = "ScriptedMesh";

		// if the chunk size is set lower by user then
		// optimize for smaller mesh data
		if (World.chunkSize * World.chunkHeight * World.chunkSize < 16384)
		{
			mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
		}
		// else set max mesh data higher
		else
		{
			mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
		}

		mesh.vertices = Verts.ToArray();
		mesh.normals = Norms.ToArray();
		mesh.uv = UVs.ToArray();
		mesh.triangles = Tris.ToArray();
		 
		mesh.RecalculateBounds();

		MeshFilter meshFilter = chunk.gameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;

		MeshRenderer renderer = chunk.gameObject.AddComponent<MeshRenderer>();
		renderer.material = cMaterial;

		chunk.gameObject.AddComponent<MeshCollider>();
	}

	/// <summary>
    /// The ChunkSomething look up listen to penny called this
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    /// <param name="sizeZ"></param>
    /// <param name="position"></param>
    /// <param name="p">The parent of the chunk GameObject</param>
    /// <param name="mat">The Material WIth the Texure Atlas</param>
	public Chunk (int sizeX, int sizeY, int sizeZ, Vector3 position, GameObject p, Material mat)
    {
		chunk = new GameObject(World.BuildChunkName(position));
		chunk.transform.parent = p.transform;
		chunk.transform.position = position;
		cMaterial = mat;
		CreateChunk(sizeX, sizeY, sizeZ);
    }

	// Use this for initialization
	public void CreateChunk (int sizeX, int sizeY, int sizeZ)
	{
		CalculateChunkData(sizeX,sizeY,sizeZ);

		// remove for later chunk optimization
		DrawChunk(sizeX, sizeY, sizeZ);
	}

	/*
	  Inter Chunk Optimization => Buffer Mode
	 */
	// TODO make method for checking for chunk neighbors
	// if all chunk neigbors are calculated then draw this chunk

	/*
	  Inter Chunk Optimization => Redraw Mode
	 */
	// IF there is no adjacent chunks then continue
	// and optimize the sides with adjacent chunks.
	// Once all adjacent chunks have been loaded then
	// redraw this chunk with optimizations

	/*
	  Regular Optimization
	 */
	// Whenever the chunk is drawn optimize with
	// currently loaded adjacent chunks

	// make conditional setting set by designer ^

	NeighborChunk HasNeighborChunk(Vector3 pos)
    {
		if (pos == chunk.transform.position)
			return NeighborChunk.SELF;
		else if (true)
			return NeighborChunk.NEIGHBOR;
		//else
		//	return NeighborChunk.OTHER;
    }

	// Update is called once per frame
	void Update ()
	{
		// if all neighbor chunks exist then
		// DrawChunk();
	}

}
