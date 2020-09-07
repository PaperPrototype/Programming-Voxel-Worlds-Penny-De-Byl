using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {

	// used if wanting different textures depending on the side
	enum CurSide {BOTTOM, TOP, SIDE};
	CurSide curSide;

	// the block type used for uv
	public enum BlockType {GRASS, DIRT, STONE, SAND, AIR};
	BlockType blockType;

	public bool isSolid;

	Chunk owner;
	GameObject parent;
	Vector3 position;

	int x;
	int y;
	int z;

	//all possible vertices 
	Vector3 p0;
	Vector3 p1;
	Vector3 p2;
	Vector3 p3;
	Vector3 p4;
	Vector3 p5;
	Vector3 p6;
	Vector3 p7;

	Vector2[,] blockUVs = { 
		/*GRASS TOP*/		{new Vector2( 0.125f, 0.375f ), new Vector2( 0.1875f, 0.375f),
								new Vector2( 0.125f, 0.4375f ), new Vector2( 0.1875f, 0.4375f )},
		/*GRASS SIDE*/		{new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f),
								new Vector2( 0.1875f, 1.0f ), new Vector2( 0.25f, 1.0f )},
		/*DIRT*/			{new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
								new Vector2( 0.125f, 1.0f ), new Vector2( 0.1875f, 1.0f )},
		/*STONE*/			{new Vector2( 0f, 0.875f ), new Vector2( 0.0625f, 0.875f),
								new Vector2( 0, 0.9375f ) ,new Vector2( 0.0625f, 0.9375f )},
		/*SAND*/            {new Vector2( 0f, 0.25f), new Vector2(0.0625f, 0.25f),
								new Vector2(0.0625f, 0.3125f), new Vector2(0f, 0.3125f)}
						}; 

	public Block(BlockType bType, Vector3 blockPos, GameObject blockParent, Chunk blockOwner)
	{
		blockType = bType;
		position = blockPos;
		parent = blockParent;
		owner = blockOwner;

		if (blockType == BlockType.AIR)
			isSolid = false;
		else
			isSolid = true;

		x = (int)blockPos.x;
		y = (int)blockPos.y;
		z = (int)blockPos.z;
	}

	void SmoothBlock()
    {

    }

	void CreateBlock(List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
        int trioffset = 0;

        //all possible vertices 
        p0 = World.allVertices[x, y, z + 1];
        p1 = World.allVertices[x + 1, y, z + 1];
        p2 = World.allVertices[x + 1, y, z];
        p3 = World.allVertices[x, y, z];
        p4 = World.allVertices[x, y + 1, z + 1];
        p5 = World.allVertices[x + 1, y + 1, z + 1];
        p6 = World.allVertices[x + 1, y + 1, z];
        p7 = World.allVertices[x, y + 1, z];

        if (!HasSolidNeighbor((int)position.x, (int)position.y, (int)position.z + 1))
            SetFront(trioffset, v, n, u, t);
        if (!HasSolidNeighbor((int)position.x, (int)position.y, (int)position.z - 1))
            SetBack(trioffset, v, n, u, t);
        if (!HasSolidNeighbor((int)position.x, (int)position.y + 1, (int)position.z))
            SetTop(trioffset, v, n, u, t);
        if (!HasSolidNeighbor((int)position.x, (int)position.y - 1, (int)position.z))
            SetBottom(trioffset, v, n, u, t);

        // the last two ifs must be inverted otherwise we get issues
        if (!HasSolidNeighbor((int)position.x - 1, (int)position.y, (int)position.z))
            SetLeft(trioffset, v, n, u, t);
        if (!HasSolidNeighbor((int)position.x + 1, (int)position.y, (int)position.z))
            SetRight(trioffset, v, n, u, t);
    }

    private void SetUVs(CurSide curSide, out Vector2 uv00, out Vector2 uv10, out Vector2 uv01, out Vector2 uv11)
    {
		// top uv mapping
        if (blockType == BlockType.GRASS && curSide == CurSide.TOP)
        {
            uv00 = blockUVs[0, 0];
            uv10 = blockUVs[0, 1];
            uv01 = blockUVs[0, 2];
            uv11 = blockUVs[0, 3];
        }
		// bottom uv mapping
        else if (blockType == BlockType.GRASS && curSide == CurSide.BOTTOM)
        {
            uv00 = blockUVs[(int)(BlockType.DIRT + 1), 0];
            uv10 = blockUVs[(int)(BlockType.DIRT + 1), 1];
            uv01 = blockUVs[(int)(BlockType.DIRT + 1), 2];
            uv11 = blockUVs[(int)(BlockType.DIRT + 1), 3];
        }
		// side uv mapping
        else
        {
            uv00 = blockUVs[(int)(blockType + 1), 0];
            uv10 = blockUVs[(int)(blockType + 1), 1];
            uv01 = blockUVs[(int)(blockType + 1), 2];
            uv11 = blockUVs[(int)(blockType + 1), 3];
        }
    }

    private void SetBottom(int trioffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
	{
		//all possible UVs
		Vector2 uv00;
		Vector2 uv10;
		Vector2 uv01;
		Vector2 uv11;

		SetUVs(CurSide.BOTTOM, out uv00, out uv10, out uv01, out uv11);

		trioffset = v.Count;
		v.Add(p0); v.Add(p1); v.Add(p2); v.Add(p3);
		n.Add(World.allNormals[(int)World.NDIR.DOWN]);
		n.Add(World.allNormals[(int)World.NDIR.DOWN]);
		n.Add(World.allNormals[(int)World.NDIR.DOWN]);
		n.Add(World.allNormals[(int)World.NDIR.DOWN]);
		u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
		t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

	}

	private void SetTop(int trioffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
	{
		//all possible UVs
		Vector2 uv00;
		Vector2 uv10;
		Vector2 uv01;
		Vector2 uv11;

		SetUVs(CurSide.TOP, out uv00, out uv10, out uv01, out uv11);

		trioffset = v.Count;
		v.Add(p7); v.Add(p6); v.Add(p5); v.Add(p4);
		n.Add(World.allNormals[(int)World.NDIR.UP]);
		n.Add(World.allNormals[(int)World.NDIR.UP]);
		n.Add(World.allNormals[(int)World.NDIR.UP]);
		n.Add(World.allNormals[(int)World.NDIR.UP]);
		u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
		t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

	}

	private void SetLeft(int trioffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
		//all possible UVs
		Vector2 uv00;
		Vector2 uv10;
		Vector2 uv01;
		Vector2 uv11;

		SetUVs(CurSide.SIDE, out uv00, out uv10, out uv01, out uv11);

		trioffset = v.Count;
		v.Add(p7); v.Add(p4); v.Add(p0); v.Add(p3);
		n.Add(World.allNormals[(int)World.NDIR.LEFT]);
		n.Add(World.allNormals[(int)World.NDIR.LEFT]);
		n.Add(World.allNormals[(int)World.NDIR.LEFT]);
		n.Add(World.allNormals[(int)World.NDIR.LEFT]);
		u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
		t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

	}

	private void SetRight(int trioffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
		//all possible UVs
		Vector2 uv00;
		Vector2 uv10;
		Vector2 uv01;
		Vector2 uv11;

		SetUVs(CurSide.SIDE, out uv00, out uv10, out uv01, out uv11);

		trioffset = v.Count;
		v.Add(p5); v.Add(p6); v.Add(p2); v.Add(p1);
		n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
		n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
		n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
		n.Add(World.allNormals[(int)World.NDIR.RIGHT]);
		u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
		t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

	}

	private void SetFront(int trioffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
		//all possible UVs
		Vector2 uv00;
		Vector2 uv10;
		Vector2 uv01;
		Vector2 uv11;

		SetUVs(CurSide.SIDE, out uv00, out uv10, out uv01, out uv11);

		trioffset = v.Count;
		v.Add(p4); v.Add(p5); v.Add(p1); v.Add(p0);
		n.Add(World.allNormals[(int)World.NDIR.FRONT]);
		n.Add(World.allNormals[(int)World.NDIR.FRONT]);
		n.Add(World.allNormals[(int)World.NDIR.FRONT]);
		n.Add(World.allNormals[(int)World.NDIR.FRONT]);
		u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
		t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

	}

	private void SetBack(int trioffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
		//all possible UVs
		Vector2 uv00;
		Vector2 uv10;
		Vector2 uv01;
		Vector2 uv11;

		SetUVs(CurSide.SIDE, out uv00, out uv10, out uv01, out uv11);

		trioffset = v.Count;
		v.Add(p6); v.Add(p7); v.Add(p3); v.Add(p2);
		n.Add(World.allNormals[(int)World.NDIR.BACK]);
		n.Add(World.allNormals[(int)World.NDIR.BACK]);
		n.Add(World.allNormals[(int)World.NDIR.BACK]);
		n.Add(World.allNormals[(int)World.NDIR.BACK]);
		u.Add(uv11); u.Add(uv01); u.Add(uv00); u.Add(uv10);
		t.Add(3 + trioffset); t.Add(1 + trioffset); t.Add(0 + trioffset); t.Add(3 + trioffset); t.Add(2 + trioffset); t.Add(1 + trioffset);

	}

	int ConvertBlockIndexToLocal(int i)
	{
		if (i == -1)
			i = World.chunkSize - 1;
		else if (i == World.chunkSize)
			i = 0;
		return i;
	}

	int ConvertYBlockIndexToLocal(int i)
    {
		if (i == -1)
			i = World.chunkHeight - 1;
		else if (i == World.chunkHeight)
			i = 0;
		return i;
	}

    public bool HasSolidNeighbor(int x, int y, int z)
    {
        Block[,,] chunkInfo;

        if (x < 0 || x >= World.chunkSize ||
           y < 0 || y >= World.chunkHeight ||
           z < 0 || z >= World.chunkSize)
        {  //block in a neighbouring chunk

            Vector3 neighbourChunkPos = this.parent.transform.position +
                                        new Vector3
										(
											(x - (int)position.x) * World.chunkSize,
                                            (y - (int)position.y) * World.chunkHeight,
                                            (z - (int)position.z) * World.chunkSize
										);
            string neighborName = World.BuildChunkName(neighbourChunkPos);

            x = ConvertBlockIndexToLocal(x);
            y = ConvertYBlockIndexToLocal(y);
            z = ConvertBlockIndexToLocal(z);

            Chunk neighborChunk;
            if (World.chunks.TryGetValue(neighborName, out neighborChunk))
            {
                chunkInfo = neighborChunk.chunkData;
            }
            else
                return false;
        }  //block in this chunk
        else
            chunkInfo = owner.chunkData;

        try
        {
            return chunkInfo[x, y, z].isSolid;
        }
        catch (System.IndexOutOfRangeException) { }

        return false;
    }

    public void Draw(List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
	{
		if (blockType == BlockType.AIR) return;

		CreateBlock(v, n, u, t);
	}
}
