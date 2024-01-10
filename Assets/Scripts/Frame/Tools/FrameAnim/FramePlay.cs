using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramePlay : MonoBehaviour
{
    private Image image;

    private int AnimationAmount { get { return spritelist.Count; } }
    private List<Sprite> spritelist = new List<Sprite>();
    public float intervalTime = 0.03f;
    public string pathName;
    private Coroutine coro;

    private void OnEnable()
    {
        Init();
        coro = StartCoroutine(PlayAnimationForwardIEnum());
    }

    private void OnDisable()
    {
        if(coro!=null)
        {
            StopCoroutine(coro);
        }
    }

    private void Init()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
            spritelist = FrameMgr.inst.GetSprites(pathName);
        }
    }

    private IEnumerator PlayAnimationForwardIEnum()
    {
        int index = 0;//可以用来控制起始播放的动画帧索引
        gameObject.SetActive(true);
        while (true)
        {
            //当我们需要在整个动画播放完之后 重复播放后面的部分 就可以展现我们纯代码播放的自由性
            if (index == spritelist.Count)
            {
                index = 0;
            }
            image.sprite = spritelist[index];
            index++;
            yield return new WaitForSeconds(intervalTime);//等待间隔 控制动画播放速度
        }
    }
}
