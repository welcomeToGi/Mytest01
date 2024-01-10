using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameMgr : MonoBehaviour
{
    public static FrameMgr inst;
    public string[] paths;
    public List<List<Sprite>> sprites = new List<List<Sprite>>();
    private Dictionary<string, List<Sprite>> dic = new Dictionary<string, List<Sprite>>();

    private void Awake()
    {
        inst = this;
        for (int i = 0; i < paths.Length; i++)
        {
            List<Sprite> list = new List<Sprite>();
            LoadSprites(paths[i], list);
            dic.Add(paths[i], list);
            //Debug.Log("数量："+ list.Count);
        }
    }

    private void LoadSprites(string path,List<Sprite> sprites)
    {
        Sprite[] array = Resources.LoadAll<Sprite>(path);
        for (int i = 0; i < array.Length; i++)
        {
            sprites.Add(array[i]);
        }
    }

    public List<Sprite> GetSprites(string pathName)
    {
        return dic[pathName];
    }

}
