using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARKit;

namespace View
{
    sealed class MeshGenerator
    {
        private readonly List<int> _baseTriangles = new();
        private readonly List<int> _classifiedTriangles = new();
        public Mesh MakeClassifiedMesh(Mesh baseMesh, NativeArray<ARMeshClassification> faceClassifications, ARMeshClassification selectedMeshClassification, Mesh classifiedMesh)
        {
            int classifiedFaceCount = 0;
            for (int i = 0; i < faceClassifications.Length; ++i)
            {
                if (faceClassifications[i] == selectedMeshClassification)
                    ++classifiedFaceCount;
            }

            classifiedMesh.Clear();

            if (classifiedFaceCount > 0)
            {
                baseMesh.GetTriangles(_baseTriangles, 0);

                _classifiedTriangles.Clear();
                _classifiedTriangles.Capacity = classifiedFaceCount * 3;

                for (int i = 0; i < faceClassifications.Length; ++i)
                {
                    if (faceClassifications[i] == selectedMeshClassification)
                    {
                        int baseTriangleIndex = i * 3;

                        _classifiedTriangles.Add(_baseTriangles[baseTriangleIndex + 0]);
                        _classifiedTriangles.Add(_baseTriangles[baseTriangleIndex + 1]);
                        _classifiedTriangles.Add(_baseTriangles[baseTriangleIndex + 2]);
                    }
                }

                classifiedMesh.vertices = baseMesh.vertices;
                classifiedMesh.normals = baseMesh.normals;
                classifiedMesh.SetTriangles(_classifiedTriangles, 0);
            }

            return classifiedMesh;
        }  
    } 
}