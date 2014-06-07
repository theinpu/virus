using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GridOverlay : MonoBehaviour
{
    public GameGlobalScript GameGlobal;
    public bool Visible = false;
    public Color RectColor = new Color(0f, 1f, 0f, .75f);
    public Point FieldPoint;

    private Material lineMaterial;

    private Color mainColor = new Color(0f, .75f, 0f, .75f);
    private Vector3 mousePosition;
    private Plane plane;
    private Rect fieldRect;

    private List<GridPoint> points = new List<GridPoint>();
    private float offsetX;
    private float offsetY;

    public bool AddPoint(Color color)
    {
        var point = new GridPoint(FieldPoint, color);
        if (points.Contains(point))
        {
            return false;
        }
        points.Add(point);
        return true;
    }

    public bool Occupy(Color color)
    {
        var point = new GridPoint(FieldPoint, color);
        var id = points.IndexOf(point);
        if (id == -1) return false;
        var c = points[id].Color;        
        return c.r == color.r && c.g == color.g && c.b == color.b;
    }

    public bool RemovePoint()
    {
        var point = new GridPoint(FieldPoint, Color.white);
        var id = points.IndexOf(point);
        if (id == -1) return false;
        points.RemoveAt(id);
        return true;
    }

    void Start()
    {
        GameGlobal = FindObjectOfType<GameGlobalScript>();

        plane = new Plane(Vector3.up, 0);
        FieldPoint = new Point();
    }

    public void Reset()
    {
        fieldRect = new Rect(0, 0, GameGlobal.GameSettings.FieldWidth, GameGlobal.GameSettings.FieldHeight);
        offsetX = GameGlobal.GameSettings.FieldWidth / 2f;
        offsetY = GameGlobal.GameSettings.FieldHeight / 2f;
    }

    void CreateLineMaterial()
    {

        if (!lineMaterial)
        {
            lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
                                        "SubShader { Pass { " +
                                        "    Blend SrcAlpha OneMinusSrcAlpha " +
                                        "    ZWrite Off Cull Off Fog { Mode Off } " +
                                        "    BindChannels {" +
                                        "      Bind \"vertex\", vertex Bind \"color\", color }" +
                                        "} } }");
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dist;
        plane.Raycast(ray, out dist);
        mousePosition = ray.GetPoint(dist);

        FieldPoint.X = Mathf.FloorToInt(mousePosition.x + offsetX);
        FieldPoint.Y = Mathf.FloorToInt(mousePosition.z + offsetY);
    }

    void OnPostRender()
    {
        if (!Visible) return;
        CreateLineMaterial();
        // set the current material
        lineMaterial.SetPass(0);

        if (IsInField())
        {
            GL.Begin(GL.QUADS);

            GL.Color(RectColor);

            GL.Vertex3(FieldPoint.X - offsetX, 0, FieldPoint.Y - offsetY);
            GL.Vertex3(FieldPoint.X - offsetX + 1f, 0, FieldPoint.Y - offsetY);
            GL.Vertex3(FieldPoint.X - offsetX + 1f, 0, FieldPoint.Y - offsetY + 1f);
            GL.Vertex3(FieldPoint.X - offsetX, 0, FieldPoint.Y - offsetY + 1f);

            GL.End();
        }

        if (points.Count > 0)
        {
            GL.Begin(GL.QUADS);            

            for (int i = 0; i < points.Count; i++)
            {
                GL.Color(points[i].Color);
                GL.Vertex3(points[i].Point.X - offsetX, 0, points[i].Point.Y - offsetY);
                GL.Vertex3(points[i].Point.X - offsetX + 1f, 0, points[i].Point.Y - offsetY);
                GL.Vertex3(points[i].Point.X - offsetX + 1f, 0, points[i].Point.Y - offsetY + 1f);
                GL.Vertex3(points[i].Point.X - offsetX, 0, points[i].Point.Y - offsetY + 1f);
            }

            GL.End();
        }

        GL.Begin(GL.LINES);

        GL.Color(mainColor);

        for (float i = 0; i <= GameGlobal.GameSettings.FieldWidth; i++)
        {
            GL.Vertex3(i - offsetX, 0, -offsetY);
            GL.Vertex3(i - offsetX, 0, offsetY);
        }
        for (float i = 0; i <= GameGlobal.GameSettings.FieldHeight; i++)
        {
            GL.Vertex3(-offsetX, 0, i - offsetY);
            GL.Vertex3(offsetX, 0, i - offsetY);
        }

        GL.End();
    }

    public bool IsInField()
    {
        return fieldRect.Contains(new Vector2(FieldPoint.X, FieldPoint.Y));
    }

}