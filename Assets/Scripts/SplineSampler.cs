using System;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class SplineSampler : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;

    [SerializeField] private int _splineIndex;
    [SerializeField]
    [Range(0f, 1f)] private float _time;
    [SerializeField] private float _laneWidth;

    float3 position;
    float3 forward;
    float3 upVector;

    public void SampleSplineWidth(float t, out Vector3 p1, out Vector3 p2)
    {
        p1 = Vector3.zero;
        p2 = Vector3.zero;

        if (_splineContainer != null)
        {
            //prevent index from going out of list bounds
            _splineIndex = _splineIndex >= _splineContainer.Splines.Count ? _splineContainer.Splines.Count - 1 : _splineIndex;
            _splineIndex = _splineIndex < 0 ? 0 : _splineIndex;

            //evaluate spline
            //forward = spline tangent = direction of travel along the spline to the next point 
            _splineContainer.Evaluate(_splineIndex, t, out position, out forward, out upVector);

            //extend out from a given point on the spline path 
            float3 right = Vector3.Cross(forward, upVector).normalized;
            p1 = position + (right * _laneWidth);
            p2 = position + (-right * _laneWidth);
        }
    }
}
//private void Update()
//{
//if (_splineContainer != null)
//if (_splineContainer != null)
//{
//    //prevent index from going out of list bounds
//    _splineIndex = _splineIndex >= _splineContainer.Splines.Count ? _splineContainer.Splines.Count - 1 : _splineIndex;
//    _splineIndex = _splineIndex < 0 ? 0 : _splineIndex;

//    //evaluate spline
//    //forward = spline tangent = direction of travel along the spline to the next point 
//    _splineContainer.Evaluate(_splineIndex, _time, out position, out forward, out upVector);

//    //extend out from a given point on the spline path 
//    float3 right = Vector3.Cross(forward, upVector).normalized;
//    p1 = position + (right * _laneWidth);
//    p2 = position + (-right * _laneWidth);
//}
//}
//private void OnDrawGizmos()
//{
//    Handles.matrix = transform.localToWorldMatrix;
//    Handles.SphereHandleCap(0, position, Quaternion.identity, .2f, EventType.Repaint);
//    Handles.SphereHandleCap(1, (Vector3)p1, Quaternion.identity, .2f, EventType.Repaint);
//    Handles.SphereHandleCap(2, (Vector3)p2, Quaternion.identity, .2f, EventType.Repaint);
//    Gizmos.DrawLine(p1, p2);
//}
