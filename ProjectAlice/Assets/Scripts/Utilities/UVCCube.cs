using UnityEngine;
using System.Collections;

public class UVCube : MonoBehaviour
{
    private MeshFilter mf;
    public float tileSize = 0.25f;

    // Use this for initialization
    void Start()
    {

        ApplyTexture();

    }

    public void ApplyTexture()
    {
        mf = gameObject.GetComponent<MeshFilter>();
        if (mf)
        {
            Mesh mesh = mf.sharedMesh;
            if (mesh)
            {
                Vector2[] uvs = mesh.uv;

                //这里采用的UV设置顺序原则：FRBLUD - Freeblood
                // 前
                uvs[0] = new Vector2(0f, 0f); //左下
                uvs[1] = new Vector2(tileSize, 0f); //右下
                uvs[2] = new Vector2(0f, 1f); //左上
                uvs[3] = new Vector2(tileSize, 1f); //右上
                                                    // 右
                uvs[16] = new Vector2(tileSize * 1.0001f, 0f);
                uvs[19] = new Vector2(tileSize * 2.0001f, 0f);
                uvs[17] = new Vector2(tileSize * 1.0001f, 1f);
                uvs[18] = new Vector2(tileSize * 2.0001f, 1f);
                // 后
                uvs[10] = new Vector2((tileSize * 2.0001f), 1f);
                uvs[11] = new Vector2((tileSize * 3.0001f), 1f);
                uvs[6] = new Vector2((tileSize * 2.0001f), 0f);
                uvs[7] = new Vector2((tileSize * 3.0001f), 0f);

                // 左
                uvs[20] = new Vector2(tileSize * 3.0001f, 0f);
                uvs[23] = new Vector2(tileSize * 4.0001f, 0f);
                uvs[21] = new Vector2(tileSize * 3.0001f, 1f);
                uvs[22] = new Vector2(tileSize * 4.0001f, 1f);
                // 上
                uvs[8] = new Vector2(tileSize * 4.0001f, 0f);
                uvs[9] = new Vector2(tileSize * 5.0001f, 0f);
                uvs[4] = new Vector2(tileSize * 4.0001f, 1f);
                uvs[5] = new Vector2(tileSize * 5.0001f, 1f);
                // 下
                uvs[14] = new Vector2(tileSize * 5.0001f, 0f);
                uvs[13] = new Vector2(tileSize * 6.0001f, 0f);
                uvs[15] = new Vector2(tileSize * 5.0001f, 1f);
                uvs[12] = new Vector2(tileSize * 6.0001f, 1f);

                mesh.uv = uvs;
            }
        }
        else
            Debug.Log("No mesh filter attached");
    }
}