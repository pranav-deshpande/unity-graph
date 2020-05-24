using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Graph : MonoBehaviour
{
    public Transform pointPrefab;
    [Range(10, 100)] public int resolution = 10;
    private Transform[] _points;
    public GraphFunctionName function;

    private const float Pi = Mathf.PI;

    private static readonly GraphFunction[] Functions =
    {
        SineFunction, Sine2DFunction, MultiSineFunction, MultiSine2DFunction,
        Ripple, Cylinder, Sphere, Torus
    };

    private void Awake()
    {
        var step = 2f / resolution;
        var scale = Vector3.one * step;
        _points = new Transform[resolution * resolution];
        for (var i = 0; i < _points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            _points[i] = point;
        }
    }

    private void Update()
    {
        var t = Time.time;
        GraphFunction f = Functions[(int) function];
        var step = 2f / resolution;
        for (int i = 0, z = 0; z < resolution; z++)
        {
            var v = (z + 0.5f) * step - 1f;
            for (var x = 0; x < resolution; x++, i++)
            {
                var u = (x + 0.5f) * step - 1f;
                _points[i].localPosition = f(u, v, t);
            }
        }
    }

    static Vector3 SineFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(Pi * (x + t));
        p.z = z;
        return p;
    }

    static Vector3 MultiSineFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(Pi * (x + t));
        p.y += Mathf.Sin(2f * Pi * (x + 2f * t)) / 2f;
        p.y *= 2f / 3f;
        p.z = z;
        return p;
    }

    static Vector3 Sine2DFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = Mathf.Sin(Pi * (x + t));
        p.y += Mathf.Sin(Pi * (z + t));
        p.y *= 0.5f;
        p.z = z;
        return p;
    }

    static Vector3 MultiSine2DFunction(float x, float z, float t)
    {
        Vector3 p;
        p.x = x;
        p.y = 4f * Mathf.Sin(Pi * (x + z + t * 0.5f));
        p.y += Mathf.Sin(Pi * (x + t));
        p.y += Mathf.Sin(2f * Pi * (z + 2f * t)) * 0.5f;
        p.y *= 1f / 5.5f;
        p.z = z;
        return p;
    }

    static Vector3 Ripple(float x, float z, float t)
    {
        Vector3 p;
        var d = Mathf.Sqrt(x * x + z * z);
        p.x = x;
        p.y = Mathf.Sin(Pi * (4f * d - t));
        p.y /= 1f + 10f * d;
        p.z = z;
        return p;
    }

    static Vector3 Cylinder(float u, float v, float t)
    {
        var r = 0.8f + Mathf.Sin(Pi * (6f * u + 2f * v + t)) * 0.2f;
        Vector3 p;
        p.x = r * Mathf.Sin(Pi * u);
        p.y = v;
        p.z = r * Mathf.Cos(Pi * u);
        return p;
    }

    static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p;
        var r = 0.8f + Mathf.Sin(Pi * (6f * u + t)) * 0.1f;
        r += Mathf.Sin(Pi * (4f * v + t)) * 0.1f;
        var s = r * Mathf.Cos(Pi * 0.5f * v);
        p.x = s * Mathf.Sin(Pi * u);
        p.y = r * Mathf.Sin(Pi * 0.5f * v);
        p.z = s * Mathf.Cos(Pi * u);
        return p;
    }

    static Vector3 Torus(float u, float v, float t)
    {
        Vector3 p;
        var r1 = 0.65f + Mathf.Sin(Pi * (6f * u + t)) * 0.1f;
        var r2 = 0.2f + Mathf.Sin(Pi * (4f * v + t)) * 0.05f;
        var s = r2 * Mathf.Cos(Pi * v) + r1;
        p.x = s * Mathf.Sin(Pi * u);
        p.y = r2 * Mathf.Sin(Pi * v);
        p.z = s * Mathf.Cos(Pi * u);
        return p;
    }
}