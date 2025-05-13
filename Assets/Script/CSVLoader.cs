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
                string[] parts = cells[x].Trim().Split('.');
                int value = int.Parse(parts[0]);
                string dirStr = parts.Length > 1 ? parts[1] : null;
                string extraStr = parts.Length > 2 ? parts[2] : null;

                Vector3 position = origin + new Vector3(x * tileSize, (height - 1 - y) * tileSize, 0);
                GameObject obj;

                switch (value)
                {
                    case 100:
                        Instantiate(floor, position, Quaternion.identity, transform);
                        obj = Instantiate(player, position, Quaternion.identity, transform);
                        cameraController.SetTarget(obj.transform);
                        SetDirection(obj.transform, dirStr);
                        break;

                    case 0:
                        Instantiate(floor, position, Quaternion.identity, transform);
                        break;

                    case 1:
                        obj = Instantiate(wall, position, Quaternion.identity, transform);
                        SetDirection(obj.transform, dirStr);
                        break;

                    case 2:
                        Instantiate(floor, position, Quaternion.identity, transform);
                        obj = Instantiate(cheese, position, Quaternion.identity, transform);
                        SetDirection(obj.transform, dirStr);
                        break;

                    case 3:
                        Instantiate(floor, position, Quaternion.identity, transform);
                        obj = Instantiate(cat_white, position, Quaternion.identity, transform);
                        SetDirection(obj.transform, dirStr);
                        if (int.TryParse(extraStr, out int armIndex))
                        {
                            //obj.GetComponent<CatWhite>()?.SetArm(armIndex);
                        }
                        break;
                }
            }
        }


        // カメラにサイズを渡す
        cameraController.SetBounds(width, height);
    }

    enum Direction { Up, Right, Down, Left }

    Direction DirectionFromString(string dir)
    {
        return dir?.ToLower() switch
        {
            "u" => Direction.Up,
            "r" => Direction.Right,
            "d" => Direction.Down,
            "l" => Direction.Left,
            _ => Direction.Up,
        };
    }

    void SetDirection(Transform objTransform, string dir)
    {
        if (string.IsNullOrEmpty(dir)) return;

        switch (dir.ToLower())
        {
            case "u": objTransform.rotation = Quaternion.Euler(0, 0, 0); break;
            case "r": objTransform.rotation = Quaternion.Euler(0, 0, -90); break;
            case "d": objTransform.rotation = Quaternion.Euler(0, 0, 180); break;
            case "l": objTransform.rotation = Quaternion.Euler(0, 0, 90); break;
        }
    }
}
