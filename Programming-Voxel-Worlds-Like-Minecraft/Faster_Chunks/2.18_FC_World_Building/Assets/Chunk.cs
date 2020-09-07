using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	public Material bMaterial;
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
		for(int x = 0; x < sizeX; x++)
			for(int y = 0; y < sizeY; y++)
				for(int z = 0; z < sizeZ; z++)
				{
					CalculateChunkBlocks(x, y, z, new Vector3(x, y, z));
				}
	}

	// For shortening the above function
	void CalculateChunkBlocks(int x, int y, int z, Vector3 pos)
    {
		if (Random.Range(0, 100) < 50)
			chunkData[x, y, z] = new Block(Block.BlockType.SAND, pos, chunk, this);
		else
			chunkData[x, y, z] = new Block(Block.BlockType.DIRT, pos, chunk, this);
    }

	public void DrawChunk(int sizeX, int sizeY, int sizeZ)
	{
		//draw blocks
		Verts.Clear();
		Norms.Clear();
		UVs.Clear();
		Tris.Clear();
		for(int x = 0; x < sizeX; x++)
			for(int y = 0; y < sizeY; y++)
				for(int z = 0; z < sizeZ; z++)
				{
					chunkData[x,y,z].Draw(Verts, Norms, UVs, Tris);
				}

		Mesh mesh = new Mesh();
	    mesh.name = "ScriptedMesh";

		// if the chunk size is set higher by user then
		// set the mesh index higher
		if (World.chunkSize * World.chunkHeight * World.chunkSize > 16384)
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
		renderer.material = bMaterial;

		chunk.gameObject.AddComponent<MeshCollider>();
	}

	/// <summary>
    /// The ChunkSomething look up listen to penny called this
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    /// <param name="sizeZ"></param>
    /// <param name="chunkPos"></param>
    /// <param name="chunkParent">The parent of the chunk GameObject</param>
    /// <param name="atlasMat">The Material WIth the Texure Atlas</param>
	public Chunk (int sizeX, int sizeY, int sizeZ, Vector3 chunkPos, GameObject chunkParent, Material atlasMat)
    {
		chunk = new GameObject(World.BuildChunkName(chunkPos));
		chunk.transform.parent = chunkParent.transform;
		chunk.transform.position = chunkPos;
		bMaterial = atlasMat;
		CalculateChunkData(sizeX, sizeY, sizeZ);
    }

	/// <summary>
    /// Calculate the chunks data, then draw it
    /// </summary>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    /// <param name="sizeZ"></param>
	public void BuildChunk (int sizeX, int sizeY, int sizeZ)
	{
		CalculateChunkData(sizeX,sizeY,sizeZ);

		// remove later for optimization
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
