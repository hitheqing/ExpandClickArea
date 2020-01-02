using System.Collections.Generic;
using UnityEditor;

public class UiToolEditor
{
    [MenuItem("GameObject/扩大点击区域", false, -1)]
    public static void ExpandClickRect()
    {
        var go = Selection.activeGameObject;
        if (go == null) {
            return;
        }

        var xImage = go.GetComponent<XImage>();
        if (xImage) {
            return;
        }

        var image = go.GetComponent<Image>();
        if (image == null) {
            Debug.LogError("没有image组件");
            return;
        }

        if (image.type != Image.Type.Simple || image.useSpriteMesh) {
            Debug.LogError("只支持简单图片类型。不支持拉升/不支持优化网格");
        }

       
        var sprite         = image.sprite;
        var color          = image.color;
        var raycastTarget  = image.raycastTarget;
        var type           = image.type;
        var useSpriteMesh  = image.useSpriteMesh;
        var preserveAspect = image.preserveAspect;

        bool replaceGraphic = false;
        var  button         = go.GetComponent<Button>();
        if (button != null && button.targetGraphic == image) {
            replaceGraphic = true;
        }

        Undo.DestroyObjectImmediate(image);
        
        xImage = go.AddComponent<XImage>();
        Undo.RegisterCreatedObjectUndo(xImage,"expand_image");

        if (replaceGraphic) {
            button.targetGraphic = null;
            button.targetGraphic = xImage;
        }
        
        xImage.sprite         = sprite;
        xImage.color          = color;
        xImage.raycastTarget  = raycastTarget;
        xImage.type           = type;
        xImage.useSpriteMesh  = useSpriteMesh;
        xImage.preserveAspect = preserveAspect;

        var size = xImage.rectTransform.sizeDelta;
        xImage.rectTransform.sizeDelta = size * 1.2f;

        
    }
}