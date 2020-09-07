using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateQuad : MonoBehaviour
{
	void BuildQuad()
	{
		Mesh mesh = new Mesh();
		mesh.name = "ScriptedMesh";

		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		int[] triangles = new int[6];

		//all possible UVs
		Vector2 uv00 = new Vector2(0f, 0f);
		Vector2 uv10 = new Vector2(1f, 0f);
		Vector2 uv01 = new Vector2(0f, 1f);
		Vector2 uv11 = new Vector2(1f, 1f);

		//all possible vertices 
		Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
		Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
		Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
		Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
		Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
		Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
		Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
		Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

		vertices.Add(p4);
		vertices.Add(p5);
		vertices.Add(p1);
		vertices.Add(p0);

		normals.Add(Vector3.forward);
		normals.Add(Vector3.forward);
		normals.Add(Vector3.forward);
		normals.Add(Vector3.forward);

		/* 
		   use this when trying to smooth
		   out the world
		   unit vector = vector with magnitude of 1

		normals.Add(p4.normalized);
		normals.Add(p5.normalized);
		normals.Add(p1.normalized);
		normals.Add(p0.normalized);


		*/


		uvs.Add(uv11);
		uvs.Add(uv01);
		uvs.Add(uv00);
		uvs.Add(uv10);

		triangles = new int[] { 3, 1, 0, 3, 2, 1 };

		mesh.vertices = vertices.ToArray();

		mesh.normals = normals.ToArray();

		mesh.uv = uvs.ToArray();

		mesh.RecalculateBounds();

		GameObject quad = new GameObject("quad");
		quad.transform.parent = transform;

		MeshFilter meshFilter = quad.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;

		MeshRenderer meshRenderer = quad.AddComponent<MeshRenderer>();
	}

    private void Start()
    {
		BuildQuad();
    }
}