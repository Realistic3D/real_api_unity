using System.Collections;
using System.Collections.Generic;
using REAL.Component;
using UnityEngine;

namespace REAL.Geometry
{
    public static class RealMesh
    {
        /// <summary>
        /// Create area light. Only visible in render results
        /// </summary>
        /// <param name="width">Wide</param>
        /// <param name="height">High</param>
        /// <param name="intensity">Power in watts</param>
        /// <param name="color">Light color</param>
        /// <returns>Game object</returns>
        public static GameObject AreaLight(float width=1f, float height=1f, float intensity=1f, Color color=default)
        {
            var areaLight = new GameObject();
            var light = areaLight.AddComponent<RealLight>();

            light.intensity = intensity;
            if(color != default) light.color = color;
            areaLight.transform.localScale = new Vector3(width, 1f, height);
            
            return areaLight;
        }
        
        /// <summary>
        /// Make an area light plane
        /// </summary>
        /// <param name="gameObject">Parent node on which have to apply</param>
        public static void CreateArea(GameObject gameObject)
        {
            // Create a new mesh
            var mesh = new Mesh();
            const float size = 1f;

            // Define the vertices of the plane mesh
            var vertices = new Vector3[4];
            vertices[0] = new Vector3(-size * 0.5f, 0f, -size * 0.5f);
            vertices[1] = new Vector3(size * 0.5f, 0f, -size * 0.5f);
            vertices[2] = new Vector3(-size * 0.5f, 0f, size * 0.5f);
            vertices[3] = new Vector3(size * 0.5f, 0f, size * 0.5f);

            // Define the triangles of the plane mesh
            // var triangles = new[] { 0, 2, 1, 2, 3, 1 };
            var triangles = new[] { 0, 1, 2, 2, 1, 3 };

            // Assign vertices and triangles to the mesh
            mesh.vertices = vertices;
            mesh.triangles = triangles;

            // Recalculate normals and bounds of the mesh
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            // Create a MeshFilter component and assign the mesh to it
            var meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            // Create a MeshRenderer component and assign a material to it (optional)
            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Standard"));
        }
    }
}
