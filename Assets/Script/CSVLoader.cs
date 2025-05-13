using UnityEngine;
using UnityEngine.Tilemaps;

public class CSVLoader : MonoBehaviour
{
    public TextAsset csvMap;             // CSVファイルをInspectorから指定
    public CameraController cameraController;
    public GameObject player;
    public GameObject floor;
    public GameObject wall;
    public GameObject cheese;
    public GameObject cat_white;
    public float tileSize = 1f;

    void Start()
    {
        string[] lines = csvMap.text.Trim().Split('\n');
        int width = lines[0].Split(',').Length;
        int height = lines.Length;
        Vector3 origin = new Vector3(-(width - 1) * tileSize / 2f, -(height - 1) * tileSize / 2f, 0);

        for (int y = 0; y < lines.Length; y++)
        {
            string[] cells = lines[y].Trim().Split(',');

            for (int x = 0; x < cells.Length; x++)
            {
                int value = int.Parse(cells[x]);
                Vector3 position = origin + new Vector3(x * tileSize, (height - 1 - y) * tileSize, 0);
                GameObject obj;

                switch (value)
                {
                    case 100:
                        obj = Instantiate(floor, position, Quaternion.identity, transform);
                        obj = Instantiate(player, position, Quaternion.identity, transform);
                        cameraController.SetTarget(obj.transform);
                        break;
                    case 0:
                        obj = Instantiate(floor, position, Quaternion.identity, transform);
                        break;
                    case 1:
                        obj = Instantiate(wall, position, Quaternion.identity, transform);
                        break;
                    case 2:
                        obj = Instantiate(floor, position, Quaternion.identity, transform);
                        obj = Instantiate(cheese, position, Quaternion.identity, transform);
                        break;
                    case 3:
                        obj = Instantiate(floor, position, Quaternion.identity, transform);
                        obj = Instantiate(cat_white, position, Quaternion.identity, transform);
                        obj.GetComponent<CatWhite>().SetArm(3, (int)ARM_DIRECTION.DOWN);
                        break;
                }
            }
        }

        // カメラにサイズを渡す
        cameraController.SetBounds(width, height);
    }


}
