using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Splines;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SplineRoad : MonoBehaviour
{
    private MeshFilter _meshFilter = null;
    private MeshRenderer _meshRenderer = null;
    private SplineSampler _splineSampler = null;
    private MeshCollider _collider;

    [SerializeField] private bool _drawGizmos;
    [SerializeField] private int _meshResolution = 0;
    [SerializeField] private List<Vector3> _vertsP1, _vertsP2;
    [SerializeField] private Material _roadMaterial;

    private void OnEnable()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _splineSampler = GetComponent<SplineSampler>();
        Spline.Changed += OnSplineChanged;
    }

    private void Update()
    {
        GetVerts();
    }

    private void GetVerts()
    {
        if (_splineSampler != null)
        {
            _vertsP1 = new();
            _vertsP2 = new();

            // calculates interval between one road step and the next
            float step = 1f / (float)_meshResolution;
            for (int i = 0; i < _meshResolution; i++)
            {
                //generates some intermediate points and samples them
                float t = step * i;
                _splineSampler.SampleSplineWidth(t, out Vector3 p1, out Vector3 p2);

                //store right vert and left vert for each step based on mesh resolution
                _vertsP1.Add(p1);
                _vertsP2.Add(p2);
            }
        }
    }

    public void BuildMesh()
    {
        if (_meshFilter.sharedMesh == null)
        {
            _meshFilter.sharedMesh = new();
        }
        Mesh mesh = _meshFilter.sharedMesh;

        List<Vector3> verts = new();
        List<int> triangles = new();
        int offset = 0;

        int length = _vertsP2.Count;

        //loop every vert and build a face starting from first vertices
        for (int i = 1; i <= length; i++)
        {
            //every face is built from 4 points
            Vector3 p1 = _vertsP1[i - 1];
            Vector3 p2 = _vertsP2[i - 1];
            Vector3 p3, p4;

            if (i == length)
            {
                p3 = _vertsP1[0];
                p4 = _vertsP2[0];
                Debug.Log("I== lenght");
            }
            else
            {
                p3 = _vertsP1[i];
                p4 = _vertsP2[i];
            }

            offset = 4 * (i - 1);

            int t1 = offset + 0;
            int t2 = offset + 2;
            int t3 = offset + 3;
            int t4 = offset + 3;
            int t5 = offset + 1;
            int t6 = offset + 0;

            verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
            triangles.AddRange(new List<int> { t1, t2, t3, t4, t5, t6 });

            mesh.Clear();

            mesh.SetVertices(verts);
            mesh.SetTriangles(triangles, 0);
            mesh.name = "tesdtMesh";
            //_meshFilter.mesh = mesh;
            _meshRenderer.material = _roadMaterial;
            GenerateCollider(mesh);
        }
    }

    private void GenerateCollider(Mesh mesh)
    {
        if (!TryGetComponent(out _collider))
        {
            _collider = gameObject.AddComponent<MeshCollider>();
        }
        else
        {
            _collider = GetComponent<MeshCollider>();
        }
        _collider.sharedMesh = mesh;
    }

    public void ResetAll()
    {
        //_meshFilter = GetComponent<MeshFilter>();
        //_meshRenderer = GetComponent<MeshRenderer>();
        //_splineSampler = GetComponent<SplineSampler>();
        _meshResolution = 0;
        _drawGizmos = false;

        _vertsP1.Clear();
        _vertsP2.Clear();

        if (_meshFilter.sharedMesh != null)
        {
            _meshFilter.sharedMesh.Clear();
            _meshFilter.sharedMesh = null;
        }

        if (_collider != null)
        {
            if (_collider.sharedMesh != null)
            {
                _collider.sharedMesh.Clear();
                _collider.sharedMesh = null;
            }
        }
    }

    // draw lines and points along the spline
    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;

        Gizmos.color = Color.red;
        Handles.color = Color.green;
        Handles.matrix = transform.localToWorldMatrix;

        for (int i = 0; i < _vertsP1.Count && i < _vertsP2.Count; i++)
        {
            Handles.SphereHandleCap(i, _vertsP1[i], Quaternion.identity, .2f, EventType.Repaint);
            Handles.SphereHandleCap(i, _vertsP2[i], Quaternion.identity, 0.2f, EventType.Repaint);
            Gizmos.DrawLine(_vertsP1[i], _vertsP2[i]);
        }
    }

    private void OnSplineChanged(Spline spline, int arg2, SplineModification modification)
    {
        Rebuild();
    }

    private void Rebuild()
    {
        BuildMesh();
    }

    private void OnDisable()
    {
        Spline.Changed -= OnSplineChanged;
    }
}



