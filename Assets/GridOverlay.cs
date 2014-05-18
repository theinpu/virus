using UnityEngine;

public class GridOverlay : MonoBehaviour
{
    public GameGlobalScript GameGlobal;
    public bool Visible = false;

    private Material lineMaterial;

    private Color mainColor = new Color(0f, 1f, 0f, 1f);
    private int halfWidth;
    private int halfHeight;

    void Start()
    {
        halfWidth = GameGlobal.GameSettings.FieldWidth / 2;
        halfHeight = GameGlobal.GameSettings.FieldHeight / 2;
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
    }
}