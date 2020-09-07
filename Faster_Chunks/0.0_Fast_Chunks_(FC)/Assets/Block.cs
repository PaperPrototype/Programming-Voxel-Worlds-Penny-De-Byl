using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {

	enum Cubeside {BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK};
	public enum BlockType {GRASS, DIRT, STONE, AIR};

	BlockType bType;
	public bool isSolid;
	GameObject parent;
	Vector3 position;

	int x;
	int y;
	int z;

	Material cubeMaterial;

	Vector2[,] blockUVs = { 
		/*GRASS TOP*/		{new Vector2( 0.125f, 0.375f ), new Vector2( 0.1875f, 0.375f),
								new Vector2( 0.125f, 0.4375f ),new Vector2( 0.1875f, 0.4375f )},
		/*GRASS SIDE*/		{new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f),
								new Vector2( 0.1875f, 1.0f ),new Vector2( 0.25f, 1.0f )},
		/*DIRT*/			{new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
								new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
		/*STONE*/			{new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
								new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )}
						}; 

	public Block(BlockType b, Vector3 pos, GameObject p, Material c)
	{
		bType = b;
		parent = p;
		position = pos;
		cubeMaterial = c;
		if (bType == BlockType.AIR)
			isSolid = false;
		else
			isSolid = true;

		x = (int) pos.x;
		y = (int) pos.y;
		z = (int) pos.z;
	}

	void CreateQuad(Cubeside side,List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
	{
		/*
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh" + side.ToString();
		*/

        Vector3[] vertices = new Vector3[4];
		Vector3[] normals = new Vector3[4];
		Vector2[] uvs = new Vector2[4];
		int[] triangles = new int[6];

		//all possible UVs
		Vector2 uv00;
		Vector2 uv10;
		Vector2 uv01;
		Vector2 uv11;

		if(bType == BlockType.GRASS && side == Cubeside.TOP)
		{
			uv00 = blockUVs[0,0];
			uv10 = blockUVs[0,1];
			uv01 = blockUVs[0,2];
			uv11 = blockUVs[0,3];
		}
		else if(bType == BlockType.GRASS && side == Cubeside.BOTTOM)
		{
			uv00 = blockUVs[(int)(BlockType.DIRT+1),0];
			uv10 = blockUVs[(int)(BlockType.DIRT+1),1];
			uv01 = blockUVs[(int)(BlockType.DIRT+1),2];
			uv11 = blockUVs[(int)(BlockType.DIRT+1),3];
		}
		else
		{
			uv00 = blockUVs[(int)(bType+1),0];
			uv10 = blockUVs[(int)(bType+1),1];
			uv01 = blockUVs[(int)(bType+1),2];
			uv11 = blockUVs[(int)(bType+1),3];
		}

		//all possible vertices 
		Vector3 p0 = World.allVertices[x,y,z+1];
		Vector3 p1 = World.allVertices[x+1,y,z+1];
		Vector3 p2 = World.allVertices[x+1,y,z];
		Vector3 p3 = World.allVertices[x,y,z];		 
		Vector3 p4 = World.allVertices[x,y+1,z+1];
		Vector3 p5 = World.allVertices[x+1,y+1,z+1];
		Vector3 p6 = World.allVertices[x+1,y+1,z];
		Vector3 p7 = World.allVertices[x,y+1,z];
		
		int trioffset = 0;

		switch(side)
		{
			case Cubeside.BOTTOM:
				/*vertices = new Vector3[] {p0, p1, p2, p3};
				normals = new Vector3[] {Vector3.down, Vector3.down, 
											Vector3.down, Vector3.down};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] { 3, 1, 0, 3, 2, 1};*/
				trioffset = v.Count;
				v.Add(p0); v.Add(p1); v.Add(p2); v.Add(p3);
				n.Add(World.allNormals[(int)World.NDIR.DOWN]);
				n.Add(World.allNormals[(int)World.NDIR.DOWN]);
				n.Add(World.allNormals[(int)World.NDIR.DOWN]);
				n.Add(World.allNormals[(int)World.NDIR.DOWN]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

			break;
			case Cubeside.TOP:
				/*vertices = new Vector3[] {p7, p6, p5, p4};
				normals = new Vector3[] {Vector3.up, Vector3.up, 
											Vector3.up, Vector3.up};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] {3, 1, 0, 3, 2, 1};*/
				trioffset = v.Count;
				v.Add(p7); v.Add(p6); v.Add(p5); v.Add(p4);
				n.Add(World.allNormals[(int)World.NDIR.UP]);
				n.Add(World.allNormals[(int)World.NDIR.UP]);
				n.Add(World.allNormals[(int)World.NDIR.UP]);
				n.Add(World.allNormals[(int)World.NDIR.UP]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

			break;
			case Cubeside.LEFT:
				/*vertices = new Vector3[] {p7, p4, p0, p3};
				normals = new Vector3[] {Vector3.left, Vector3.left, 
											Vector3.left, Vector3.left};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] {3, 1, 0, 3, 2, 1};*/
				trioffset = v.Count;
				v.Add(p7); v.Add(p4); v.Add(p0); v.Add(p3);
				n.Add(World.allNormals[(int)World.NDIR.LEFT]);
				n.Add(World.allNormals[(int)World.NDIR.LEFT]);
				n.Add(World.allNormals[(int)World.NDIR.LEFT]);
				n.Add(World.allNormals[(int)World.NDIR.LEFT]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

			break;
			case Cubeside.RIGHT:
				/*vertices = new Vector3[] {p5, p6, p2, p1};
				normals = new Vector3[] {Vector3.right, Vector3.right, 
											Vector3.right, Vector3.right};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] {3, 1, 0, 3, 2, 1};*/
				trioffset = v.Count;
				v.Add(p5); v.Add(p6); v.Add(p2); v.Add(p1);
				n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

			break;
			case Cubeside.FRONT:
				/*vertices = new Vector3[] {p4, p5, p1, p0};
				normals = new Vector3[] {Vector3.forward, Vector3.forward, 
											Vector3.forward, Vector3.forward};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] {3, 1, 0, 3, 2, 1};*/
				trioffset = v.Count;
				v.Add(p4); v.Add(p5); v.Add(p1); v.Add(p0);
				n.Add(World.allNormals[(int)World.NDIR.FRONT]);
				n.Add(World.allNormals[(int)World.NDIR.FRONT]);
				n.Add(World.allNormals[(int)World.NDIR.FRONT]);
				n.Add(World.allNormals[(int)World.NDIR.FRONT]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

			break;
			case Cubeside.BACK:
				/*vertices = new Vector3[] {p6, p7, p3, p2};
				normals = new Vector3[] {Vector3.back, Vector3.back, 
											Vector3.back, Vector3.back};
				uvs = new Vector2[] {uv11, uv01, uv00, uv10};
				triangles = new int[] {3, 1, 0, 3, 2, 1};*/
				trioffset = v.Count;
				v.Add(p6); v.Add(p7); v.Add(p3); v.Add(p2);
				n.Add(World.allNormals[(int)World.NDIR.BACK]);
				n.Add(World.allNormals[(int)World.NDIR.BACK]);
				n.Add(World.allNormals[(int)World.NDIR.BACK]);
				n.Add(World.allNormals[(int)World.NDIR.BACK]);
				u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
				t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

			break;
		}

		/*mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		 
		mesh.RecalculateBounds();
		
		GameObject quad = new GameObject("Quad");
		quad.transform.position = position;
	    quad.transform.parent = parent.transform;

     	MeshFilter meshFilter = (MeshFilter) quad.AddComponent(typeof(MeshFilter));
		meshFilter.mesh = mesh;*/

		//MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		//renderer.material = cubeMaterial;
	}

	public bool HasSolidNeighbour(int x, int y, int z)
	{
		Block[,,] chunks = parent.GetComponent<Chunk>().chunkData;
		try
		{
			// TODO later on
			/********************************
			  Inter Chunk Optimization
			 ********************************/
			// Check if block is on edge of chunk.

			// If on edge of chunk & adjacent chunk
			// exists, get its data and optimize

			/********************************
			  Inter Chunk Optimization => Buffer Mode
			 ********************************/
			// IF there is no adjacent chunk then pre calculate
			// the adjacent chunks data but don't draw it
			// then use the adjacent chunks data to optimize

			/********************************
			  Inter Chunk Optimization => Redraw Mode
			 ********************************/
			// IF there is no adjacent chunks then continue.
			// Once there is the parent will redraw and 
			// and the edges can be calulated

			return chunks[x,y,z].isSolid;
		}
		catch(System.IndexOutOfRangeException){}

		return false;
	}

	public void Draw(List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
	{
		if(bType == BlockType.AIR) return;

		if(!HasSolidNeighbour((int)position.x,(int)position.y,(int)position.z + 1))
			CreateQuad(Cubeside.FRONT, v, n, u, t);
		if(!HasSolidNeighbour((int)position.x,(int)position.y,(int)position.z - 1))
			CreateQuad(Cubeside.BACK, v, n, u, t);
		if(!HasSolidNeighbour((int)position.x,(int)position.y + 1,(int)position.z))
			CreateQuad(Cubeside.TOP, v, n, u, t);
		if(!HasSolidNeighbour((int)position.x,(int)position.y - 1,(int)position.z))
			CreateQuad(Cubeside.BOTTOM, v, n, u, t);
		if(!HasSolidNeighbour((int)position.x - 1,(int)position.y,(int)position.z))
			CreateQuad(Cubeside.LEFT, v, n, u, t);
		if(!HasSolidNeighbour((int)position.x + 1,(int)position.y,(int)position.z))
			CreateQuad(Cubeside.RIGHT, v, n, u, t);
	}
}
