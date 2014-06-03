using System.Collections.Generic;
using UnityEngine;

public class GridOverlay : MonoBehaviour
{
    public GameGlobalScript GameGlobal;
    public bool Visible = false;
    public Color RectColor = new Color(0f, 1f, 0f, .75f);
    public Point FieldPoint;

    private Material lineMaterial;

    private Color mainColor = new Color(0f, .75f, 0f, .75f);    
    private int halfWidth;
    private int halfHeight;
    private Vector3 mousePosition;
    private Plane plane;
    private Rect fieldRect;

    private List<Vector3> points = new List<Vector3>();

    public void AddPoint()
    {
        points.Add(new Vector3(FieldPoint.X - halfWidth, 0, FieldPoint.Y - halfHeight));
    }

    void Start()
    {
        GameGlobal = FindObjectOfType<GameGlobalScript>();
        //halfWidth = GameGlobal.GameSettings.FieldWidth / 2;
        //halfHeight = GameGlobal.GameSettings.FieldHeight / 2;

        plane = new Plane(Vector3.up, 0);

        //fieldRect = new Rect(0, 0, GameGlobal.GameSettings.FieldWidth, GameGlobal.GameSettings.FieldHeight);
        FieldPoint = new Point();
    }

    public void Reset()
    {
        halfWidth = GameGlobal.GameSettings.FieldWidth / 2;
        halfHeight = GameGlobal.GameSettings.FieldHeight / 2;

        fieldRect = new Rect(0, 0, GameGlobal.GameSettings.FieldWidth, GameGlobal.GameSettings.FieldHeight);
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
        mousePosition.x = Mathf.Floor(mousePosition.x + .5f);
        mousePosition.z = Mathf.Floor(mousePosition.z + .5f);

        FieldPoint.X = (int)mousePosition.x + halfWidth;
        FieldPoint.Y = (int)mousePosition.z + halfHeight;
    }

    void OnPostRender()
    {
        if (!Visible) return;
        CreateLineMaterial();
        // set the current material
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);

        GL.Color(mainColor);

        for (float i = -halfWidth - 0.5f; i < halfWidth + 0.5f; i++)
        {
            GL.Vertex3(i, 0, -halfHeight - 0.5f);
            GL.Vertex3(i, 0, halfHeight - 0.5f);
        }
        for (float i = -halfHeight - 0.5f; i < halfHeight + 0.5f; i++)
        {
            GL.Vertex3(-halfWidth - 0.5f, 0, i);
            GL.Vertex3(halfWidth - 0.5f, 0, i);
        }

        GL.End();

        if (IsInField())
        {
            GL.Begin(GL.QUADS);

            GL.Color(RectColor);

            GL.Vertex3(mousePosition.x - .5f, 0, mousePosition.z - .5f);
            GL.Vertex3(mousePosition.x + .5f, 0, mousePosition.z - .5f);
            GL.Vertex3(mousePosition.x + .5f, 0, mousePosition.z + .5f);
            GL.Vertex3(mousePosition.x - .5f, 0, mousePosition.z + .5f);

            GL.End();
        }

        if (points.Count > 0)
        {
            GL.Begin(GL.QUADS);

            GL.Color(mainColor);

            for (int i = 0; i < points.Count; i++)
            {
                GL.Vertex3(points[i].x - .5f, 0 , points[i].z - .5f);
                GL.Vertex3(points[i].x + .5f, 0, points[i].z - .5f);
                GL.Vertex3(points[i].x + .5f, 0, points[i].z + .5f);
                GL.Vertex3(points[i].x - .5f, 0, points[i].z + .5f);
            }

            GL.End();
        }
    }

    public bool IsInField()
    {
        return fieldRect.Contains(new Vector2(FieldPoint.X, FieldPoint.Y));
    }
}