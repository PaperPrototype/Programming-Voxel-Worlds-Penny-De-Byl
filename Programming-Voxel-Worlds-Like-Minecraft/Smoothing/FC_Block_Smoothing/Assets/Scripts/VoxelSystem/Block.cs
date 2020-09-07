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

	bool front = false, back = false, top = false, bot = false, left = false, right = false;

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

	public void SmoothChunkVerts(float smoothAmount, float extrudeAmount)
	{
        // calculate sides
        if (!HasSolidNeighbor((int)position.x, (int)position.y, (int)position.z + 1))
            front = true;
        if (!HasSolidNeighbor((int)position.x, (int)position.y, (int)position.z - 1))
            back = true;
        if (!HasSolidNeighbor((int)position.x, (int)position.y + 1, (int)position.z))
            top = true;
        if (!HasSolidNeighbor((int)position.x, (int)position.y - 1, (int)position.z))
            bot = true;
        if (!HasSolidNeighbor((int)position.x + 1, (int)position.y, (int)position.z))
            right = true;
        if (!HasSolidNeighbor((int)position.x - 1, (int)position.y, (int)position.z))
            left = true;

		/*
			vertices reference

		// the bottom
		p0 = owner.smoothedChunkVerts[x, y, z + 1];      // front   // left
		p1 = owner.smoothedChunkVerts[x + 1, y, z + 1]; // front   // right
		p2 = owner.smoothedChunkVerts[x + 1, y, z];    // back    // right
		p3 = owner.smoothedChunkVerts[x, y, z];       // back    // left

		// the top
		p4 = owner.smoothedChunkVerts[x, y + 1, z + 1];      // front  // left
		p5 = owner.smoothedChunkVerts[x + 1, y + 1, z + 1]; // front  // right
		p6 = owner.smoothedChunkVerts[x + 1, y + 1, z];    // back   // right
		p7 = owner.smoothedChunkVerts[x, y + 1, z];       // back   // left
		*/


		if (blockType != BlockType.AIR)
		{
			if (front) // p4 p5 p1 p0
			{
				if (top)
				{
					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.BACK] * smoothAmount;

					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.BACK] * smoothAmount;

				}

				if (bot)
				{
					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.BACK] * smoothAmount;

					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.BACK] * smoothAmount;

				}

				if (right)
				{
					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.BACK] * smoothAmount;

					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.BACK] * smoothAmount;

				}

				if (left)
				{
					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.BACK] * smoothAmount;

					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.BACK] * smoothAmount;

				}
			}

			if (back) // p6 p7 p3 p2
			{
				if (top)
				{
					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.FRONT] * smoothAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.FRONT] * smoothAmount;

				}

				if (bot)
				{
					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.FRONT] * smoothAmount;

					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.FRONT] * smoothAmount;

				}


				if (left)
				{
					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.FRONT] * smoothAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.FRONT] * smoothAmount;

				}

				if (right)
				{
					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.FRONT] * smoothAmount;

					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.FRONT] * smoothAmount;

				}
			}

			if (top) // p7 p6 p5 p4
			{
				if (front)
				{
					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.DOWN] * smoothAmount;

					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.DOWN] * smoothAmount;

				}

				if (back)
				{
					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.DOWN] * smoothAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.DOWN] * smoothAmount;

				}

				if (left)
				{
					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.DOWN] * smoothAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.DOWN] * smoothAmount;

				}

				if (right)
				{
					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.DOWN] * smoothAmount;

					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.DOWN] * smoothAmount;

				}
			}

			if (bot) // p0 p1 p2 p3
			{
				if (front)
				{
					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.UP] * smoothAmount;

					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.UP] * smoothAmount;

				}
				if (back)
				{
					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.UP] * smoothAmount;

					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.UP] * smoothAmount;

				}
				if (left)
				{
					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.UP] * smoothAmount;

					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.UP] * smoothAmount;

				}
				if (right)
				{
					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.UP] * smoothAmount;

					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.UP] * smoothAmount;

				}
			}

			if (left) // p7 p4 p0 p3
			{
				if (front)
				{
					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.RIGHT] * smoothAmount;

					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.RIGHT] * smoothAmount;

				}
				if (back)
				{
					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.RIGHT] * smoothAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.RIGHT] * smoothAmount;

				}
				if (top)
				{
					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.RIGHT] * smoothAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.RIGHT] * smoothAmount;

				}
				if (bot)
				{
					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.RIGHT] * smoothAmount;

					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.RIGHT] * smoothAmount;

				}
			}

			if (right)
			{
				if (front)
				{
					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.LEFT] * smoothAmount;

					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.LEFT] * smoothAmount;

				}
				if (back)
				{
					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.LEFT] * smoothAmount;

					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.LEFT] * smoothAmount;

				}
				if (top)
				{
					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.LEFT] * smoothAmount;

					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.LEFT] * smoothAmount;

				}
				if (bot)
				{
					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.LEFT] * smoothAmount;

					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.LEFT] * smoothAmount;

				}
			}
		}
		/*
			AIR BLOCKS
			- change later to only calculate surface blocks?
			  (might not actually optimize that much)
		*/
		else
        {
			if (front) // p4 p5 p1 p0
			{
				if (top)
				{
					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.BACK] * extrudeAmount;

					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.BACK] * extrudeAmount;

				}

				if (bot)
				{
					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.BACK] * extrudeAmount;

					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.BACK] * smoothAmount;

				}

				if (right)
				{
					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.BACK] * extrudeAmount;

					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.BACK] * extrudeAmount;

				}

				if (left)
				{
					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.BACK] * extrudeAmount;

					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.BACK] * extrudeAmount;

				}
			}

			if (back) // p6 p7 p3 p2
			{
				if (top)
				{
					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.FRONT] * extrudeAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.FRONT] * extrudeAmount;

				}

				if (bot)
				{
					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.FRONT] * extrudeAmount;

					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.FRONT] * extrudeAmount;

				}


				if (left)
				{
					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.FRONT] * extrudeAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.FRONT] * extrudeAmount;

				}

				if (right)
				{
					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.FRONT] * extrudeAmount;

					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.FRONT] * extrudeAmount;

				}
			}

			if (top) // p7 p6 p5 p4
			{
				if (front)
				{
					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.DOWN] * extrudeAmount;

					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.DOWN] * extrudeAmount;

				}

				if (back)
				{
					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.DOWN] * extrudeAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.DOWN] * extrudeAmount;

				}

				if (left)
				{
					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.DOWN] * extrudeAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.DOWN] * extrudeAmount;

				}

				if (right)
				{
					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.DOWN] * extrudeAmount;

					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.DOWN] * extrudeAmount;

				}
			}

			if (bot) // p0 p1 p2 p3
			{
				if (front)
				{
					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.UP] * extrudeAmount;

					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.UP] * extrudeAmount;

				}
				if (back)
				{
					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.UP] * extrudeAmount;

					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.UP] * extrudeAmount;

				}
				if (left)
				{
					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.UP] * extrudeAmount;

					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.UP] * extrudeAmount;

				}
				if (right)
				{
					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.UP] * extrudeAmount;

					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.UP] * extrudeAmount;

				}
			}

			if (left) // p7 p4 p0 p3
			{
				if (front)
				{
					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.RIGHT] * extrudeAmount;

					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.RIGHT] * extrudeAmount;

				}
				if (back)
				{
					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.RIGHT] * extrudeAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.RIGHT] * extrudeAmount;

				}
				if (top)
				{
					// p4
					owner.allChunkVerts[x, y + 1, z + 1] += World.allNormals[(int)World.NDIR.RIGHT] * extrudeAmount;

					// p7
					owner.allChunkVerts[x, y + 1, z] += World.allNormals[(int)World.NDIR.RIGHT] * extrudeAmount;

				}
				if (bot)
				{
					// p0
					owner.allChunkVerts[x, y, z + 1] += World.allNormals[(int)World.NDIR.RIGHT] * extrudeAmount;

					// p3
					owner.allChunkVerts[x, y, z] += World.allNormals[(int)World.NDIR.RIGHT] * extrudeAmount;

				}
			}

			if (right)
			{
				if (front)
				{
					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.LEFT] * extrudeAmount;

					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.LEFT] * extrudeAmount;

				}
				if (back)
				{
					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.LEFT] * extrudeAmount;

					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.LEFT] * extrudeAmount;

				}
				if (top)
				{
					// p5
					owner.allChunkVerts[x + 1, y + 1, z + 1] += World.allNormals[(int)World.NDIR.LEFT] * extrudeAmount;

					// p6
					owner.allChunkVerts[x + 1, y + 1, z] += World.allNormals[(int)World.NDIR.LEFT] * extrudeAmount;

				}
				if (bot)
				{
					// p1
					owner.allChunkVerts[x + 1, y, z + 1] += World.allNormals[(int)World.NDIR.LEFT] * extrudeAmount;

					// p2
					owner.allChunkVerts[x + 1, y, z] += World.allNormals[(int)World.NDIR.LEFT] * extrudeAmount;

				}
			}
		}

    }

	public void Draw(List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
		if (blockType == BlockType.AIR) return;

        // calculate sides
        if (!HasSolidNeighbor((int)position.x, (int)position.y, (int)position.z + 1))
            front = true;
        if (!HasSolidNeighbor((int)position.x, (int)position.y, (int)position.z - 1))
            back = true;
        if (!HasSolidNeighbor((int)position.x, (int)position.y + 1, (int)position.z))
            top = true;
        if (!HasSolidNeighbor((int)position.x, (int)position.y - 1, (int)position.z))
            bot = true;
        if (!HasSolidNeighbor((int)position.x + 1, (int)position.y, (int)position.z))
            right = true;
        if (!HasSolidNeighbor((int)position.x - 1, (int)position.y, (int)position.z))
            left = true;

        if (World.smoothing)
		{
			// the bottom
			p0 = owner.allChunkVerts[x, y, z + 1];      // front   // left
			p1 = owner.allChunkVerts[x + 1, y, z + 1]; // front   // right
			p2 = owner.allChunkVerts[x + 1, y, z];    // back    // right
			p3 = owner.allChunkVerts[x, y, z];       // back    // left

			// the top
			p4 = owner.allChunkVerts[x, y + 1, z + 1];      // front  // left
			p5 = owner.allChunkVerts[x + 1, y + 1, z + 1]; // front  // right
			p6 = owner.allChunkVerts[x + 1, y + 1, z];    // back   // right
			p7 = owner.allChunkVerts[x, y + 1, z];       // back   // left
		}
		else
		{
			//all possible vertices 
			p0 = World.allVertices[x, y, z + 1];
			p1 = World.allVertices[x + 1, y, z + 1];
			p2 = World.allVertices[x + 1, y, z];
			p3 = World.allVertices[x, y, z];
			p4 = World.allVertices[x, y + 1, z + 1];
			p5 = World.allVertices[x + 1, y + 1, z + 1];
			p6 = World.allVertices[x + 1, y + 1, z];
			p7 = World.allVertices[x, y + 1, z];

		}

		int trioffset = 0;

		if (front)
            SetFront(trioffset, v, n, u, t);
        if (back)
            SetBack(trioffset, v, n, u, t);
        if (top)
            SetTop(trioffset, v, n, u, t);
        if (bot)
            SetBottom(trioffset, v, n, u, t);
        if (right)
            SetRight(trioffset, v, n, u, t);
        if (left)
            SetLeft(trioffset, v, n, u, t);
    }

    private void SetUVs(CurSide curSide, out Vector2 uv00, out Vector2 uv10, out Vector2 uv01, out Vector2 uv11)
    {
		// if we're building the top
		// and the block type is grass
		// select a different part of the texture atlas
		if (blockType == BlockType.GRASS)
        {
			if (curSide == CurSide.TOP)
			{
				uv00 = blockUVs[0, 0];
				uv10 = blockUVs[0, 1];
				uv01 = blockUVs[0, 2];
				uv11 = blockUVs[0, 3];
			}
			// if we're building the bottom
			// and the block type is grass
			// select a different part of the texture atlas
			else if (curSide == CurSide.BOTTOM)
			{
				uv00 = blockUVs[(int)(BlockType.DIRT + 1), 0];
				uv10 = blockUVs[(int)(BlockType.DIRT + 1), 1];
				uv01 = blockUVs[(int)(BlockType.DIRT + 1), 2];
				uv11 = blockUVs[(int)(BlockType.DIRT + 1), 3];
			}
			// use the texture (on the atlas) that corresponds
			// to the blockType
			else
			{
				uv00 = blockUVs[(int)(blockType + 1), 0];
				uv10 = blockUVs[(int)(blockType + 1), 1];
				uv01 = blockUVs[(int)(blockType + 1), 2];
				uv11 = blockUVs[(int)(blockType + 1), 3];
			}
		}
		// use the texture (on the atlas) that corresponds
		// to the blockType
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

        if (
			x < 0 || x >= World.chunkSize ||
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
}
