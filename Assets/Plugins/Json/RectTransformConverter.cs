using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WanzyeeStudio.Json
{
    public class RectTransformConverter : PartialConverter<RectTransform>
    {
        protected override string[] GetPropertyNames()
        {
            return new[] { "Rect" };
        }
    }
}
