using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

	public Material cubeMaterial;
	public Block[,,] chunkData;
	int cw;
	int ch;
	int cd;

	List<Vector3> Verts = new List<Vector3>();
	List<Vector3> Norms = new List<Vector3>();
	List<Vector2> UVs = new List<Vector2>();
	List<int> Tris = new List<int>();

	enum NeighborChunk { SELF, NEIGHBOR, OTHER };


	void CalculateChunkData(int sizeX, int sizeY, int sizeZ)
	{
		chunkData = new Block[sizeX,sizeY,sizeZ];
		cw = sizeX;
		ch = sizeY;
		cd = sizeZ;

		//create blocks
		for(int z = 0; z < sizeZ; z++)
			for(int y = 0; y < sizeY; y++)
				for(int x = 0; x < sizeX; x++)
				{
					CalculateChunkBlocks(x, y, z, new Vector3(x, y, z));
				}
	}

	void CalculateChunkBlocks(int x, int y, int z, Vector3 pos)
    {
		if (Random.Range(0, 100) < 50)
			chunkData[x, y, z] = new Block(Block.BlockType.DIRT, pos,
							this.gameObject, cubeMaterial);
		else
			chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos,
							this.gameObject, cubeMaterial);
	}

	void DrawChunk()
	{
		//draw blocks
		Verts.Clear();
		Norms.Clear();
		UVs.Clear();
		Tris.Clear();
		for(int z = 0; z < cd; z++)
			for(int y = 0; y < ch; y++)
				for(int x = 0; x < cw; x++)
				{
					chunkData[x,y,z].Draw(Verts, Norms, UVs, Tris);
					
				}

		Mesh mesh = new Mesh();
	    mesh.name = "ScriptedMesh";

		// if the chunk size is set lower by user then
		// optimize for smaller mesh data
		if (World.cSizeX * World.cSizeY * World.cSizeZ < 16384)
		{
			mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
			Debug.Log("Lower IndexFormat for chunk meshes");
		}
		// else set max mesh data higher
		else
		{
			mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
			Debug.Log("Higher IndexFormat for chunk meshes");
		}

		mesh.vertices = Verts.ToArray();
		mesh.normals = Norms.ToArray();
		mesh.uv = UVs.ToArray();
		mesh.triangles = Tris.ToArray();
		 
		mesh.RecalculateBounds();

		MeshFilter meshFilter = (MeshFilter) this.gameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;

		MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
		renderer.material = cubeMaterial;
	}

	// Use this for initialization
	public void CreateChunk (int w, int h, int d)
	{
		CalculateChunkData(w,h,d);

		// remove for later chunk optimization
		DrawChunk();
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
		if (pos == transform.position)
			return NeighborChunk.SELF;
		else if (true)
			return NeighborChunk.NEIGHBOR;
		else
			return NeighborChunk.OTHER;
    }

	// Update is called once per frame
	void Update ()
	{
		// if all neighbor chunks exist then
		// DrawChunk();
	}

}
