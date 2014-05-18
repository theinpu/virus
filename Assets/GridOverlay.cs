using UnityEngine;
using System.Collections;

public class GridOverlay : MonoBehaviour
{
    public GameScript gameField;

    private Material lineMaterial;

    private Color mainColor = new Color(0f, 1f, 0f, 1f);

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
        CreateLineMaterial();
        // set the current material
        lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);

        GL.Color(mainColor);

        for (float i = -gameField.halfWidth - 0.5f; i < gameField.halfWidth + 0.5f; i++)
        {
            GL.Vertex3(i, 0, -gameField.halfHeight - 0.5f);
            GL.Vertex3(i, 0, gameField.halfHeight - 0.5f);
        }
        for (float i = -gameField.halfHeight - 0.5f; i < gameField.halfHeight + 0.5f; i++)
        {
            GL.Vertex3(-gameField.halfWidth - 0.5f, 0, i);
            GL.Vertex3(gameField.halfWidth - 0.5f, 0, i);
        }

        GL.End();
    }
}