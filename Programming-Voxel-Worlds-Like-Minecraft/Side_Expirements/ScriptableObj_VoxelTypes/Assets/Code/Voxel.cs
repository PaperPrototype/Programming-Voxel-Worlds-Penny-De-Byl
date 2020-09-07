using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Voxel
{
	VoxelType m_voxelType;

	static Vector3 m_position;

	//all possible vertices 
	Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f) + m_position;
	Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f) + m_position;
	Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f) + m_position;
	Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f) + m_position;
	Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f) + m_position;
	Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f) + m_position;
	Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f) + m_position;
	Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f) + m_position;

	public Voxel (VoxelType voxelType, Vector3 position)
    {
		m_voxelType = voxelType;
		m_position = position;
	}

	public void DrawVoxel(List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
	{
		int triOffset = 0;

		SetRight(triOffset, v, n, u, t);
		SetLeft(triOffset, v, n, u, t);

		SetTop(triOffset, v, n, u, t);
		SetBottom(triOffset, v, n, u, t);

		SetFront(triOffset, v, n, u, t);
		SetBack(triOffset, v, n, u, t);
	}

	private void SetRight(int triOffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
		//all possible UVs
		Vector2 uv00 = m_voxelType.uv00(VoxelType.CurSide.SIDE);
		Vector2 uv10 = m_voxelType.uv10(VoxelType.CurSide.SIDE);
		Vector2 uv01 = m_voxelType.uv01(VoxelType.CurSide.SIDE);
		Vector2 uv11 = m_voxelType.uv11(VoxelType.CurSide.SIDE);

		triOffset = v.Count;

		v.Add(p5);
		v.Add(p6);
		v.Add(p2);
		v.Add(p1);

		n.Add(Vector3.right);
		n.Add(Vector3.right);
		n.Add(Vector3.right);
		n.Add(Vector3.right);

		u.Add(uv11);
		u.Add(uv01);
		u.Add(uv00);
		u.Add(uv10);

		t.Add(3 + triOffset);
		t.Add(1 + triOffset);
		t.Add(0 + triOffset);
		t.Add(3 + triOffset);
		t.Add(2 + triOffset);
		t.Add(1 + triOffset);
	}

	private void SetLeft(int triOffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
		//all possible UVs
		Vector2 uv00 = m_voxelType.uv00(VoxelType.CurSide.SIDE);
		Vector2 uv10 = m_voxelType.uv10(VoxelType.CurSide.SIDE);
		Vector2 uv01 = m_voxelType.uv01(VoxelType.CurSide.SIDE);
		Vector2 uv11 = m_voxelType.uv11(VoxelType.CurSide.SIDE);

		triOffset = v.Count;

		v.Add(p7);
		v.Add(p4);
		v.Add(p0);
		v.Add(p3);

		n.Add(Vector3.left);
		n.Add(Vector3.left);
		n.Add(Vector3.left);
		n.Add(Vector3.left);

		u.Add(uv11);
		u.Add(uv01);
		u.Add(uv00);
		u.Add(uv10);

		t.Add(3 + triOffset);
		t.Add(1 + triOffset);
		t.Add(0 + triOffset);
		t.Add(3 + triOffset);
		t.Add(2 + triOffset);
		t.Add(1 + triOffset);
	}

	private void SetTop(int triOffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
		//all possible UVs
		Vector2 uv00 = m_voxelType.uv00(VoxelType.CurSide.TOP);
		Vector2 uv10 = m_voxelType.uv10(VoxelType.CurSide.TOP);
		Vector2 uv01 = m_voxelType.uv01(VoxelType.CurSide.TOP);
		Vector2 uv11 = m_voxelType.uv11(VoxelType.CurSide.TOP);

		triOffset = v.Count;

		v.Add(p7);
		v.Add(p6);
		v.Add(p5);
		v.Add(p4);

		n.Add(Vector3.up);
		n.Add(Vector3.up);
		n.Add(Vector3.up);
		n.Add(Vector3.up);

		u.Add(uv11);
		u.Add(uv01);
		u.Add(uv00);
		u.Add(uv10);

		t.Add(3 + triOffset);
		t.Add(1 + triOffset);
		t.Add(0 + triOffset);
		t.Add(3 + triOffset);
		t.Add(2 + triOffset);
		t.Add(1 + triOffset);
	}

	private void SetBottom(int triOffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
		//all possible UVs
		Vector2 uv00 = m_voxelType.uv00(VoxelType.CurSide.BOTTOM);
		Vector2 uv10 = m_voxelType.uv10(VoxelType.CurSide.BOTTOM);
		Vector2 uv01 = m_voxelType.uv01(VoxelType.CurSide.BOTTOM);
		Vector2 uv11 = m_voxelType.uv11(VoxelType.CurSide.BOTTOM);

		triOffset = v.Count;

		v.Add(p0);
		v.Add(p1);
		v.Add(p2);
		v.Add(p3);

		n.Add(Vector3.down);
		n.Add(Vector3.down);
		n.Add(Vector3.down);
		n.Add(Vector3.down);

		u.Add(uv11);
		u.Add(uv01);
		u.Add(uv00);
		u.Add(uv10);

		t.Add(3 + triOffset);
		t.Add(1 + triOffset);
		t.Add(0 + triOffset);
		t.Add(3 + triOffset);
		t.Add(2 + triOffset);
		t.Add(1 + triOffset);
	}

	private void SetFront(int triOffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
		//all possible UVs
		Vector2 uv00 = m_voxelType.uv00(VoxelType.CurSide.SIDE);
		Vector2 uv10 = m_voxelType.uv10(VoxelType.CurSide.SIDE);
		Vector2 uv01 = m_voxelType.uv01(VoxelType.CurSide.SIDE);
		Vector2 uv11 = m_voxelType.uv11(VoxelType.CurSide.SIDE);

		triOffset = v.Count;

		v.Add(p4);
		v.Add(p5);
		v.Add(p1);
		v.Add(p0);

		n.Add(Vector3.forward);
		n.Add(Vector3.forward);
		n.Add(Vector3.forward);
		n.Add(Vector3.forward);

		u.Add(uv11);
		u.Add(uv01);
		u.Add(uv00);
		u.Add(uv10);

		t.Add(3 + triOffset);
		t.Add(1 + triOffset);
		t.Add(0 + triOffset);
		t.Add(3 + triOffset);
		t.Add(2 + triOffset);
		t.Add(1 + triOffset);

	}


	private void SetBack(int triOffset, List<Vector3> v, List<Vector3> n, List<Vector2> u, List<int> t)
    {
		//all possible UVs
		Vector2 uv00 = m_voxelType.uv00(VoxelType.CurSide.SIDE);
		Vector2 uv10 = m_voxelType.uv10(VoxelType.CurSide.SIDE);
		Vector2 uv01 = m_voxelType.uv01(VoxelType.CurSide.SIDE);
		Vector2 uv11 = m_voxelType.uv11(VoxelType.CurSide.SIDE);

		triOffset = v.Count;

		v.Add(p6);
		v.Add(p7);
		v.Add(p3);
		v.Add(p2);

		n.Add(Vector3.back);
		n.Add(Vector3.back);
		n.Add(Vector3.back);
		n.Add(Vector3.back);

		u.Add(uv11);
		u.Add(uv01);
		u.Add(uv00);
		u.Add(uv10);

		t.Add(3 + triOffset);
		t.Add(1 + triOffset);
		t.Add(0 + triOffset);
		t.Add(3 + triOffset);
		t.Add(2 + triOffset);
		t.Add(1 + triOffset);
	}
}