using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLoader : MonoBehaviour
{

    public TextAsset mapFile;
    public GameObject platform;

    // Use this for initialization
    void Start()
    {
        using (StringReader reader = new StringReader(mapFile.text))
        {
            string line;
            int y = 0;
            while ((line = reader.ReadLine()) != null)
            {
                string[] vals = line.Split(',');
                for (int x = 0; x < vals.Length; x++)
                {
                    int type = int.Parse(vals[x]);
                    if (type != 19)
                        Instantiate(platform, new Vector2(x, -y), Quaternion.identity, transform);
                }
                y++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
