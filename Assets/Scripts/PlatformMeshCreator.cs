using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples {
    public class PlatformMeshCreator : PathSceneTool {
        [Header ("Road settings")]
        public float roadWidth = .4f;
        [Range (0, .5f)]
        public float thickness = .15f;
        public bool flattenSurface;

        [Header ("Material settings")]
        public Material roadMaterial;
        public Material undersideMaterial;
        public float textureTiling = 1;

        [SerializeField, HideInInspector]
        GameObject meshHolder;

        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        MeshCollider meshCollider;
        Mesh mesh;

        protected override void PathUpdated () {
            if (pathCreator != null) {
                AssignMeshComponents ();
                AssignMaterials ();
                CreateRoadMesh ();
            }
        }

        void CreateRoadMesh () {
            // This mesh creator is modified so that the ends of the platform also have mesh on them
            // These will be 8 seperate verticies added onto the end of each array
            Vector3[] verts = new Vector3[path.NumPoints * 8 + 8];
            Vector2[] uvs = new Vector2[verts.Length];
            Vector3[] normals = new Vector3[verts.Length];

            int numTris = 2 * (path.NumPoints - 1) + ((path.isClosedLoop) ? 2 : 0);
            int[] roadTriangles = new int[numTris * 3];
            int[] underRoadTriangles = new int[numTris * 3];
            int[] sideOfRoadTriangles = new int[numTris * 2 * 3];
            int[] endOfTheRoadTriangles = new int[12];

            int vertIndex = 0;
            int triIndex = 0;

            // Vertices for the top of the road are layed out:
            // 0  1
            // 8  9
            // and so on... So the triangle map 0,8,1 for example, defines a triangle from top left to bottom left to bottom right.
            int[] triangleMap = { 0, 8, 1, 1, 8, 9 };
            int[] sidesTriangleMap = { 4, 6, 14, 12, 4, 14, 5, 15, 7, 13, 15, 5 };
            int[] endsTriangleMap = { 2, 0, 3, 3, 0, 1};

            bool usePathNormals = !(path.space == PathSpace.xyz && flattenSurface);

            for (int i = 0; i < path.NumPoints; i++) {
                Vector3 localUp = (usePathNormals) ? Vector3.Cross (path.GetTangent (i), path.GetNormal (i)) : path.up;
                Vector3 localRight = (usePathNormals) ? path.GetNormal (i) : Vector3.Cross (localUp, path.GetTangent (i));

                // Find position to left and right of current path vertex
                Vector3 vertSideA = path.GetPoint (i) - localRight * Mathf.Abs (roadWidth);
                Vector3 vertSideB = path.GetPoint (i) + localRight * Mathf.Abs (roadWidth);

                // Add top of road vertices
                verts[vertIndex + 0] = vertSideA;
                verts[vertIndex + 1] = vertSideB;
                // Add bottom of road vertices
                verts[vertIndex + 2] = vertSideA - localUp * thickness;
                verts[vertIndex + 3] = vertSideB - localUp * thickness;

                // Duplicate vertices to get flat shading for sides of road
                verts[vertIndex + 4] = verts[vertIndex + 0];
                verts[vertIndex + 5] = verts[vertIndex + 1];
                verts[vertIndex + 6] = verts[vertIndex + 2];
                verts[vertIndex + 7] = verts[vertIndex + 3];

                // Set uv on y axis to path time (0 at start of path, up to 1 at end of path)
                uvs[vertIndex + 0] = new Vector2 (0, path.times[i]);
                uvs[vertIndex + 1] = new Vector2 (1, path.times[i]);

                // Top of road normals
                normals[vertIndex + 0] = localUp;
                normals[vertIndex + 1] = localUp;
                // Bottom of road normals
                normals[vertIndex + 2] = -localUp;
                normals[vertIndex + 3] = -localUp;
                // Sides of road normals
                normals[vertIndex + 4] = -localRight;
                normals[vertIndex + 5] = localRight;
                normals[vertIndex + 6] = -localRight;
                normals[vertIndex + 7] = localRight;

                // Set triangle indices
                if (i < path.NumPoints - 1 || path.isClosedLoop) {
                    for (int j = 0; j < triangleMap.Length; j++) {
                        roadTriangles[triIndex + j] = (vertIndex + triangleMap[j]) % verts.Length;
                        // reverse triangle map for under road so that triangles wind the other way and are visible from underneath
                        underRoadTriangles[triIndex + j] = (vertIndex + triangleMap[triangleMap.Length - 1 - j] + 2) % verts.Length;
                    }
                    for (int j = 0; j < sidesTriangleMap.Length; j++) {
                        sideOfRoadTriangles[triIndex * 2 + j] = (vertIndex + sidesTriangleMap[j]) % verts.Length;
                    }
                }

                vertIndex += 8;
                triIndex += 6;
            }

            // Adds the ends of the platform
            // Verticies are the same as the first 4 and the last 4
            verts[vertIndex + 0] = verts[0];
            verts[vertIndex + 1] = verts[1];
            verts[vertIndex + 2] = verts[2];
            verts[vertIndex + 3] = verts[3];
            verts[vertIndex + 4] = verts[vertIndex - 7];
            verts[vertIndex + 5] = verts[vertIndex - 8];
            verts[vertIndex + 6] = verts[vertIndex - 5];
            verts[vertIndex + 7] = verts[vertIndex - 6];

            //uvs[vertIndex + 0] = new Vector2 (0, path.times[0]);
            //uvs[vertIndex + 1] = new Vector2 (1, path.times[0]);

            normals[vertIndex + 0] = -path.GetTangent(0);
            normals[vertIndex + 1] = -path.GetTangent(0);
            normals[vertIndex + 2] = -path.GetTangent(0);
            normals[vertIndex + 3] = -path.GetTangent(0);
            normals[vertIndex + 4] = path.GetTangent(path.NumPoints - 1);
            normals[vertIndex + 5] = path.GetTangent(path.NumPoints - 1);
            normals[vertIndex + 6] = path.GetTangent(path.NumPoints - 1);
            normals[vertIndex + 7] = path.GetTangent(path.NumPoints - 1);

            endOfTheRoadTriangles[0] = endsTriangleMap[0] + vertIndex;
            endOfTheRoadTriangles[1] = endsTriangleMap[1] + vertIndex;
            endOfTheRoadTriangles[2] = endsTriangleMap[2] + vertIndex;
            endOfTheRoadTriangles[3] = endsTriangleMap[3] + vertIndex;
            endOfTheRoadTriangles[4] = endsTriangleMap[4] + vertIndex;
            endOfTheRoadTriangles[5] = endsTriangleMap[5] + vertIndex;
            endOfTheRoadTriangles[6] = endsTriangleMap[0] + vertIndex + 4;
            endOfTheRoadTriangles[7] = endsTriangleMap[1] + vertIndex + 4;
            endOfTheRoadTriangles[8] = endsTriangleMap[2] + vertIndex + 4;
            endOfTheRoadTriangles[9] = endsTriangleMap[3] + vertIndex + 4;
            endOfTheRoadTriangles[10] = endsTriangleMap[4] + vertIndex + 4;
            endOfTheRoadTriangles[11] = endsTriangleMap[5] + vertIndex + 4;

            mesh.Clear ();
            mesh.vertices = verts;
            mesh.uv = uvs;
            mesh.normals = normals;
            mesh.subMeshCount = 4;
            mesh.SetTriangles (roadTriangles, 0);
            mesh.SetTriangles (underRoadTriangles, 1);
            mesh.SetTriangles (sideOfRoadTriangles, 2);
            mesh.SetTriangles (endOfTheRoadTriangles, 3);
            mesh.RecalculateBounds ();
        }

        // Add MeshRenderer and MeshFilter components to this gameobject if not already attached
        void AssignMeshComponents () {
            if (meshHolder == null) {
                meshHolder = new GameObject ("Track Mesh Holder");
                meshHolder.transform.parent = transform;
            }

            meshHolder.transform.rotation = Quaternion.identity;
            meshHolder.transform.position = Vector3.zero;
            meshHolder.transform.localScale = Vector3.one;

            // Ensure mesh renderer and filter components are assigned
            if (!meshHolder.gameObject.GetComponent<MeshFilter> ()) {
                meshHolder.gameObject.AddComponent<MeshFilter> ();
            }
            if (!meshHolder.GetComponent<MeshRenderer> ()) {
                meshHolder.gameObject.AddComponent<MeshRenderer> ();
            }
            if (!meshHolder.gameObject.GetComponent<MeshCollider> ()) {
                meshHolder.gameObject.AddComponent<MeshCollider> ();
            }

            meshRenderer = meshHolder.GetComponent<MeshRenderer> ();
            meshFilter = meshHolder.GetComponent<MeshFilter> ();
            meshCollider = meshHolder.GetComponent<MeshCollider> ();
            if (mesh == null) {
                mesh = new Mesh ();
            }
            meshFilter.sharedMesh = mesh;
            meshCollider.sharedMesh = mesh;
        }

        void AssignMaterials () {
            if (roadMaterial != null && undersideMaterial != null) {
                meshRenderer.sharedMaterials = new Material[] { roadMaterial, undersideMaterial, undersideMaterial, undersideMaterial };
                meshRenderer.sharedMaterials[0].mainTextureScale = new Vector3 (1, textureTiling);
            }
        }

    }
}
