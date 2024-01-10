
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace UnityEngine.UI
{
    


    /// <summary>
    /// 文本控件,支持超链接
    /// </summary>
    public class HyperlinkText : Text, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        
        /// <summary>
        /// 超链接信息类
        /// </summary>
        private class HyperlinkInfo
        {
            
            public int startIndex;

            public int endIndex;

            public string name;

            public readonly List<Rect> boxes = new List<Rect>();
        }
        /// <summary>
        /// 解析完最终的文本
        /// </summary>
        private string m_OutputText;

        /// <summary>
        /// 超链接信息列表
        /// </summary>
        private readonly List<HyperlinkInfo> m_HrefInfos = new List<HyperlinkInfo>();

        /// <summary>
        /// 文本构造器
        /// </summary>
        protected static readonly StringBuilder s_TextBuilder = new StringBuilder();


        [Serializable]
        public class HrefClickEvent : UnityEvent<string> { }
        [SerializeField]
        private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

        [SerializeField]
        private HrefClickEvent m_OnHrefPress = new HrefClickEvent();

        /// <summary>
        /// 超链接点击事件
        /// </summary>
        public HrefClickEvent onHrefClick
        {
            
            get { return m_OnHrefClick; }
            set { m_OnHrefClick = value; }
        }

        /// <summary>
        /// 超链接点击事件
        /// </summary>
        public HrefClickEvent onHrefPress
        {
            
            get { return m_OnHrefPress; }
            set { m_OnHrefPress = value; }
        }

        public UnityEvent PointUpAction;

        /// <summary>
        /// 超链接正则
        /// </summary>
        private static readonly Regex s_HrefRegex = new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

        private HyperlinkText mHyperlinkText;

        public string GetHyperlinkInfo
        {
            
            get { return text; }
        }
        private bool isPointerDown = false;
        private float interval = 0.3f;
        private float recordTime;
        private Vector2 curlp;

        private GameObject exa;
        protected override void Awake()
        {
            
            base.Awake();
            mHyperlinkText = GetComponent<HyperlinkText>();


        }
        protected override void OnEnable()
        {
            
            base.OnEnable();
            mHyperlinkText.onHrefClick.AddListener(OnHyperlinkTextInfo);
        }

        protected override void OnDisable()
        {
            
            base.OnDisable();
            mHyperlinkText.onHrefClick.RemoveListener(OnHyperlinkTextInfo);

        }


        public override void SetVerticesDirty()
        {
            
            base.SetVerticesDirty();

            text = GetHyperlinkInfo;
            m_OutputText = GetOutputText(text);

        }


        int getline(IList<UILineInfo> lines, int index)
        {
            
            int line = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                
                   var info = lines[i];
                if (index > info.startCharIdx * 4 - 3)
                {
                    
                       line = i + 1;
                }
            }

            return line;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            
               var orignText = m_Text;
            m_Text = m_OutputText;
            base.OnPopulateMesh(toFill);
            m_Text = orignText;
            //print($"currentVertCount={toFill.currentVertCount},currentIndexCount={toFill.currentIndexCount}");
            var lines = cachedTextGeneratorForLayout.lines;
            var vertexList = new List<UIVertex>();
            UIVertex vertex = new UIVertex();
            int rectCount = toFill.currentVertCount / 4;
            Dictionary<int, int> afterindexofbeforeindex = new Dictionary<int, int>();//后来的索引对应以前的索引
            //https://www.zhihu.com/question/372690850
            //由于空格换行也算顶点  而且他们的坐标都一样  剔除它
            for (int i = 0; i < rectCount; i++)
            {
                
                int startIndex = i * 4;
                toFill.PopulateUIVertex(ref vertex, startIndex);
                var p1 = vertex;
                toFill.PopulateUIVertex(ref vertex, startIndex + 1);
                var p2 = vertex;
                toFill.PopulateUIVertex(ref vertex, startIndex + 2);
                var p3 = vertex;
                toFill.PopulateUIVertex(ref vertex, startIndex + 3);
                var p4 = vertex;

                if (p1.position != p2.position &&
                    p2.position != p3.position &&
                    p3.position != p4.position)
                {
                    
                    afterindexofbeforeindex.Add(vertexList.Count, startIndex);
                    afterindexofbeforeindex.Add(vertexList.Count + 1, startIndex + 1);
                    afterindexofbeforeindex.Add(vertexList.Count + 2, startIndex + 2);
                    afterindexofbeforeindex.Add(vertexList.Count + 3, startIndex + 3);
                    vertexList.AddRange(new UIVertex[] { p1, p2, p3, p4 });
                }

            }

            //   Debug.Log($" before optimize {toFill.currentVertCount},  after optimize {vertexList.Count}");
            //未解决换行 跨行问题

            // 处理超链接包围框
            Dictionary<int, List<Rect>> dicRect = new Dictionary<int, List<Rect>>();
            foreach (var hrefInfo in m_HrefInfos)
            {
                

                hrefInfo.boxes.Clear();
                dicRect.Clear();
                // print($"startIndex={ hrefInfo.startIndex}");
                //int line = 1;
                // 将超链接里面的文本顶点索引坐标加入到Rect  每个文本四个顶点           
                for (int i = hrefInfo.startIndex, m = hrefInfo.endIndex; i < m; i += 4)
                {
                    
                    if (i >= vertexList.Count)//只会展示显示的部分超出显示区域的  必须Break
                    {
                        
                        break;
                    }
                    int line = getline(lines, afterindexofbeforeindex[i]);//获得行数
                    Rect rect = new Rect();
                    UIVertex vert1 = vertexList[i];
                    UIVertex vert3 = vertexList[i + 2];

                    rect.width = Math.Abs(vert1.position.x - vert3.position.x);
                    rect.height = Math.Abs(vert1.position.y - vert3.position.y);
                    rect.x = (vert1.position.x + vert3.position.x) / 2;
                    rect.y = (vert1.position.y + vert3.position.y) / 2;
                    //print($"i={i},rect={rect.position} ,vert1={vert1.position},vert3={vert3.position}");
                    if (dicRect.Count > 0 && (rect.y + 1 < dicRect[line][dicRect[line].Count - 1].y))
                    {
                        
                           line++;
                    }

                    if (dicRect.ContainsKey(line))
                    {
                        

                           dicRect[line].Add(rect);
                    }
                    else
                    {
                        
                        dicRect.Add(line, new List<Rect>() { rect });
                    }
                    // print($"line={line},dicrectcount = {dicRect.Count}");
                }
                // print(JsonConvert.SerializeObject(dicRect));
                foreach (var item in dicRect)
                {
                    
                       //print($"item key={item.Key},[f]={item.Value[0].position},[l]={item.Value[item.Value.Count - 1].position}");
                       Rect rect = new Rect();
                    var f = item.Value[0];
                    var l = item.Value[item.Value.Count - 1];
                    rect.width = Math.Abs(f.x - l.x) + f.width / 2 + l.width / 2;
                    rect.height = f.height;
                    rect.x = f.x + (l.x - f.x) / 2;
                    rect.y = f.y;
                    // print($"last item key={item.Key},{rect.position}");
                    hrefInfo.boxes.Add(rect);
                }
            }
        }

        /// <summary>
        /// 获取超链接解析后的最后输出文本
        /// </summary>
        /// <returns></returns>
        protected virtual string GetOutputText(string outputText)
        {
            
            s_TextBuilder.Length = 0;
            m_HrefInfos.Clear();
            var indexText = 0;
            foreach (Match match in s_HrefRegex.Matches(outputText))
            {
                

                s_TextBuilder.Append(outputText.Substring(indexText, match.Index - indexText));
                var group = match.Groups[1];
                var str = Regex.Replace(s_TextBuilder.ToString(), @"\s", "");
                // print($"str={str}");
                var hrefInfo = new HyperlinkInfo
                {
                    //去除空格  以及换行 富文本标识  取到正确的位置      
                  
                    startIndex = Regex.Replace(str, @"<(.*?)>", "").Length * 4, // 超链接里的文本起始顶点索引
                    endIndex = (Regex.Replace(str, @"<(.*?)>", "").Length +
                    Regex.Replace(Regex.Replace(match.Groups[2].ToString(), @"\s", "")
                    , @"<(.*?)>", "").Length - 1) * 4 + 3,
                    name = group.Value
                };
                m_HrefInfos.Add(hrefInfo);
                //print("")
                s_TextBuilder.Append(match.Groups[2].Value);
                indexText = match.Index + match.Length;
            }
            s_TextBuilder.Append(outputText.Substring(indexText, outputText.Length - indexText));
            return s_TextBuilder.ToString();
        }


        RectTransform createImage(Transform parent)
        {
            
               GameObject image = new GameObject();
            var rect = image.AddComponent<RectTransform>();
            var img = image.AddComponent<Image>();
            img.raycastTarget = false;
            img.color = new Color(1, 1, 1, 0.4f);
            image.transform.SetParent(parent);
            image.transform.localPosition = Vector3.zero;
            image.transform.localScale = Vector3.one;

            return rect;
        }

        void trigger(Action<string> cb, Vector2 lp)
        {
            
               //可以创建一个图片 大小2*2  来显示出点击点Lp  TO
               var point = createImage(transform);
            point.name = "clickpoint";
            point.anchoredPosition = lp;
            point.sizeDelta = Vector2.one * 2;
            point.GetComponent<Image>().color = Color.red;

            foreach (HyperlinkInfo hrefInfo in m_HrefInfos)
            {
                
                   var boxes = hrefInfo.boxes;
                // print($"{Newtonsoft.Json.JsonConvert.SerializeObject(hrefInfo)}");
                for (var i = 0; i < boxes.Count; ++i)
                {
                    
                    print($"boxes[{i}]={boxes[i]}");
                    //可以创建一个图片 大小为boxes[i]  来显示出点击区域  TO  //可以注释
                    //var box = createImage(transform);
                    //  box.name = "box" + hrefInfo.name;
                    // box.sizeDelta = new Vector2(boxes[i].width, boxes[i].height);
                    //    box.anchoredPosition = new Vector2(boxes[i].x, boxes[i].y);

                    if ((boxes[i].x - boxes[i].width / 2) < lp.x && (boxes[i].x + boxes[i].width / 2) > lp.x &&
                        (boxes[i].y - boxes[i].height / 2) < lp.y && (boxes[i].y + boxes[i].height / 2) > lp.y)
                    {
                        
                        print("点击了" + hrefInfo.name);
                        cb(hrefInfo.name);
                        return;
                    }
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
               Vector2 lp = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position,
                GameObject.FindGameObjectWithTag("UICamera") ? GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>()
                : eventData.pressEventCamera, out lp);

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                
               // DestroyImmediate(transform.GetChild(i).gameObject);
            }
            trigger((s) => { m_OnHrefClick.Invoke(s); }, lp);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            
               recordTime = Time.time;
            isPointerDown = true;
            curlp = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out curlp);


        }

        public void OnPointerUp(PointerEventData eventData)
        {
            

               isPointerDown = false;
            PointUpAction.Invoke();
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                
                //DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        void Update()
        {
            
            if (isPointerDown)
            {
                
                if (Time.timeScale != 1)
                {
                    
                       interval = 0.6f;
                }
                else
                {
                    
                       interval = 0.3f;
                }
                if ((Time.time - recordTime) > interval)
                {
                    
                       isPointerDown = false;
                    // trigger((s) => { m_OnHrefPress.Invoke(s); }, curlp);                   
                }
            }
        }

        /// <summary>
        /// 当前点击超链接回调
        /// </summary>
        /// <param name="info">回调信息</param>
        private void OnHyperlinkTextInfo(string info)
        {
            
            //print("点击了"+info);
        }

    }
}