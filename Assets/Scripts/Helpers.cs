using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class Helpers
{
    public static T GetComponentInChildrenExclusive<T>(this Component comp) where T : Component
    {
        var children = comp.GetComponentsInChildren<T>().Where(c => c.transform != comp.transform);
        return children.FirstOrDefault();
    }

	public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t)
            Object.Destroy(child.gameObject);
    }

	public static void SetLayerInclChildren(this GameObject go, int layer)
    {
        go.layer = layer;
        var children = go.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }

	public static void SetAlpha(this SpriteRenderer sr, float alpha)
	{
		var color = sr.color;
		color.a = alpha;
		sr.color = color;
	}

	public static void SetAlpha(this Image img, float alpha)
	{
		var color = img.color;
		color.a = alpha;
		img.color = color;
	}

	public static Color Color255(int r, int g, int b, int a = 255)
	{
		return new Color(
			(float)r / 255f,
			(float)g / 255f,
			(float)b / 255f,
			(float)a / 255f);
	}

	public static T GetClamped<T>(this T[] source, int index)
    {
        return source[Mathf.Clamp(index, 0, source.Length - 1)];
	}

    public static Vector2Int GridFlip(this Vector2Int vec)
    {
        (vec.x, vec.y) = (-vec.y, vec.x);
        return vec;
    }

    public static Vector2Int DirectionTo(this Vector2Int from, Vector2Int to)
    {
        return (to - from);
    }

    public static string CapitalizeFirst(this string str)
    {
        if (str.Length == 0) 
            return "";
        else if (str.Length == 1) 
            return char.ToUpper(str[0]).ToString();

		return char.ToUpper(str[0]) + str.Substring(1);
    }

    public static void Extend<T>(this List<T> list, int newCount)
    {
        if (list.Count > newCount) return;
        list.AddRange(new T[newCount - list.Count]);
	}

	public static void SnapToY(this ScrollRect scrollRect, RectTransform target, float offset = 0)
	{
        if (scrollRect == null)
            return;

    	scrollRect.movementType = ScrollRect.MovementType.Clamped;
        Canvas.ForceUpdateCanvases();

        var contentPanel = scrollRect.content;
        var max = scrollRect.content.anchorMax.y;

		var newPos =
			(Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - 
            (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
        newPos.x = contentPanel.anchoredPosition.x;
        newPos.y -= 1080f / 2f; //canvas is scale based on 1080p
        newPos.y += offset;
        //newPos.y = Mathf.Clamp(newPos.y, 0, max);

        Debug.Log(newPos.y);
        contentPanel.anchoredPosition = newPos;
	}

	public static void SnapToY(this ScrollRect scrollRect, float y)
	{
		if (scrollRect == null)
			return;

		var contentPanel = scrollRect.content;
		var pos = contentPanel.anchoredPosition;
		pos.y = y;
		contentPanel.anchoredPosition = pos;
	}

	public static void SetLeft(this RectTransform rt, float left)
	{
		rt.offsetMin = new Vector2(left, rt.offsetMin.y);
	}

	public static void SetRight(this RectTransform rt, float right)
	{
		rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
	}

	public static void SetTop(this RectTransform rt, float top)
	{
		rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
	}

	public static void SetBottom(this RectTransform rt, float bottom)
	{
		rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
	}
}
