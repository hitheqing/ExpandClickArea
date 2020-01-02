using System;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class ExpandAreaImage: Image
{
    [SerializeField] public float Xfactor = 1.2f;
    [SerializeField] public float Yfactor = 1.2f;

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        Color32 color32 = color;

        if (sprite == null) {
            var r    = GetPixelAdjustedRect();
            var difx = r.width - r.width / Xfactor;
            var dify = r.height - r.height / Yfactor;
            var v = new Vector4(r.x + difx / 2, r.y + dify / 2, r.x + r.width / Xfactor + difx / 2,
                                r.y + r.height / Yfactor + dify / 2);

            toFill.Clear();

            toFill.AddVert(new Vector3(v.x, v.y), color32, new Vector2(0f, 0f));
            toFill.AddVert(new Vector3(v.x, v.w), color32, new Vector2(0f, 1f));
            toFill.AddVert(new Vector3(v.z, v.w), color32, new Vector2(1f, 1f));
            toFill.AddVert(new Vector3(v.z, v.y), color32, new Vector2(1f, 0f));
        }
        else {
            var v4      = GetDrawingDimensions(preserveAspect);
            var sprite1 = sprite;
            var uv      = (sprite1 != null) ? DataUtility.GetOuterUV(sprite1) : Vector4.zero;

            toFill.Clear();
            var width  = v4.z - v4.x;
            var height = v4.w - v4.y;
            var difx   = width - width / Xfactor;
            var dify   = height - height / Yfactor;

            toFill.AddVert(new Vector3(v4.x + difx / 2, v4.y + dify / 2), color32, new Vector2(uv.x, uv.y));
            toFill.AddVert(new Vector3(v4.x + difx / 2, v4.w - dify / 2), color32, new Vector2(uv.x, uv.w));
            toFill.AddVert(new Vector3(v4.z - difx / 2, v4.w - dify / 2), color32, new Vector2(uv.z, uv.w));
            toFill.AddVert(new Vector3(v4.z - difx / 2, v4.y + dify / 2), color32, new Vector2(uv.z, uv.y));
        }

        toFill.AddTriangle(0, 1, 2);
        toFill.AddTriangle(2, 3, 0);
    }

    private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
    {
        var padding = sprite == null ? Vector4.zero : DataUtility.GetPadding(sprite);
        var size    = sprite == null ? Vector2.zero : new Vector2(sprite.rect.width, sprite.rect.height);

        Rect r = GetPixelAdjustedRect();
        // Debug.Log(string.Format("r:{2}, size:{0}, padding:{1}", size, padding, r));

        int spriteW = Mathf.RoundToInt(size.x);
        int spriteH = Mathf.RoundToInt(size.y);

        var v = new Vector4(
                            padding.x / spriteW,
                            padding.y / spriteH,
                            (spriteW - padding.z) / spriteW,
                            (spriteH - padding.w) / spriteH);

        if (shouldPreserveAspect && size.sqrMagnitude > 0.0f) {
            PreserveSpriteAspectRatio(ref r, size);
        }

        v = new Vector4(
                        r.x + r.width * v.x,
                        r.y + r.height * v.y,
                        r.x + r.width * v.z,
                        r.y + r.height * v.w
                       );

        return v;
    }

    private void PreserveSpriteAspectRatio(ref Rect rect, Vector2 spriteSize)
    {
        var spriteRatio = spriteSize.x / spriteSize.y;
        var rectRatio   = rect.width / rect.height;

        if (spriteRatio > rectRatio) {
            var oldHeight = rect.height;
            rect.height =  rect.width * (1.0f / spriteRatio);
            rect.y      += (oldHeight - rect.height) * rectTransform.pivot.y;
        }
        else {
            var oldWidth = rect.width;
            rect.width =  rect.height * spriteRatio;
            rect.x     += (oldWidth - rect.width) * rectTransform.pivot.x;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Do Expand")]
    public void ReFill()
    {
        var size = rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(size.x * Xfactor, size.y * Yfactor);
    }

#endif
}
