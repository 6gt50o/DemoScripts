using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public partial class CustomEditorTools {

    static Texture2D mBackdropTex;
    static Texture2D mContrastTex;
    static Texture2D mGradientTex;
    static GameObject mPrevious;

    /// <summary>
    /// Returns a blank usable 1x1 white texture.
    /// </summary>

    public static Texture2D blankTexture
    {
        get
        {
            return EditorGUIUtility.whiteTexture;
        }
    }

    /// <summary>
    /// Returns a usable texture that looks like a dark checker board.
    /// </summary>

    public static Texture2D backdropTexture
    {
        get
        {
            if (mBackdropTex == null) mBackdropTex = CreateCheckerTex(
                new Color(0.1f, 0.1f, 0.1f, 0.5f),
                new Color(0.2f, 0.2f, 0.2f, 0.5f));
            return mBackdropTex;
        }
    }

    /// <summary>
    /// Returns a usable texture that looks like a high-contrast checker board.
    /// </summary>

    public static Texture2D contrastTexture
    {
        get
        {
            if (mContrastTex == null) mContrastTex = CreateCheckerTex(
                new Color(0f, 0.0f, 0f, 0.5f),
                new Color(1f, 1f, 1f, 0.5f));
            return mContrastTex;
        }
    }

    /// <summary>
    /// Gradient texture is used for title bars / headers.
    /// </summary>

    public static Texture2D gradientTexture
    {
        get
        {
            if (mGradientTex == null) mGradientTex = CreateGradientTex();
            return mGradientTex;
        }
    }

    /// <summary>
    /// Create a white dummy texture.
    /// </summary>

    static Texture2D CreateDummyTex()
    {
        Texture2D tex = new Texture2D(1, 1);
        tex.name = "[Generated] Dummy Texture";
        tex.hideFlags = HideFlags.DontSave;
        tex.filterMode = FilterMode.Point;
        tex.SetPixel(0, 0, Color.yellow);
        tex.Apply();
        return tex;
    }

    /// <summary>
    /// Create a checker-background texture
    /// </summary>

    static Texture2D CreateCheckerTex(Color c0, Color c1)
    {
        Texture2D tex = new Texture2D(16, 16);
        tex.name = "[Generated] Checker Texture";
        tex.hideFlags = HideFlags.DontSave;

        for (int y = 0; y < 8; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c1);
        for (int y = 8; y < 16; ++y) for (int x = 0; x < 8; ++x) tex.SetPixel(x, y, c0);
        for (int y = 0; y < 8; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c0);
        for (int y = 8; y < 16; ++y) for (int x = 8; x < 16; ++x) tex.SetPixel(x, y, c1);

        tex.Apply();
        tex.filterMode = FilterMode.Point;
        return tex;
    }

    /// <summary>
    /// Create a gradient texture
    /// </summary>

    static Texture2D CreateGradientTex()
    {
        Texture2D tex = new Texture2D(1, 16);
        tex.name = "[Generated] Gradient Texture";
        tex.hideFlags = HideFlags.DontSave;

        Color c0 = new Color(1f, 1f, 1f, 0f);
        Color c1 = new Color(1f, 1f, 1f, 0.4f);

        for (int i = 0; i < 16; ++i)
        {
            float f = Mathf.Abs((i / 15f) * 2f - 1f);
            f *= f;
            tex.SetPixel(0, i, Color.Lerp(c0, c1, f));
        }

        tex.Apply();
        tex.filterMode = FilterMode.Bilinear;
        return tex;
    }

    /// <summary>
    /// Draws the tiled texture. Like GUI.DrawTexture() but tiled instead of stretched.
    /// </summary>

    public static void DrawTiledTexture(Rect rect, Texture tex)
    {
        GUI.BeginGroup(rect);
        {
            int width = Mathf.RoundToInt(rect.width);
            int height = Mathf.RoundToInt(rect.height);

            for (int y = 0; y < height; y += tex.height)
            {
                for (int x = 0; x < width; x += tex.width)
                {
                    GUI.DrawTexture(new Rect(x, y, tex.width, tex.height), tex);
                }
            }
        }
        GUI.EndGroup();
    }

    /// <summary>
    /// Draw a single-pixel outline around the specified rectangle.
    /// </summary>

    public static void DrawOutline(Rect rect)
    {
        if (Event.current.type == EventType.Repaint)
        {
            Texture2D tex = contrastTexture;
            GUI.color = Color.white;
            DrawTiledTexture(new Rect(rect.xMin, rect.yMax, 1f, -rect.height), tex);
            DrawTiledTexture(new Rect(rect.xMax, rect.yMax, 1f, -rect.height), tex);
            DrawTiledTexture(new Rect(rect.xMin, rect.yMin, rect.width, 1f), tex);
            DrawTiledTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1f), tex);
        }
    }

    /// <summary>
    /// Draw a single-pixel outline around the specified rectangle.
    /// </summary>

    public static void DrawOutline(Rect rect, Color color)
    {
        if (Event.current.type == EventType.Repaint)
        {
            Texture2D tex = blankTexture;
            GUI.color = color;
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, 1f, rect.height), tex);
            GUI.DrawTexture(new Rect(rect.xMax, rect.yMin, 1f, rect.height), tex);
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, 1f), tex);
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1f), tex);
            GUI.color = Color.white;
        }
    }

    /// <summary>
    /// Draw a selection outline around the specified rectangle.
    /// </summary>

    public static void DrawOutline(Rect rect, Rect relative, Color color)
    {
        if (Event.current.type == EventType.Repaint)
        {
            // Calculate where the outer rectangle would be
            float x = rect.xMin + rect.width * relative.xMin;
            float y = rect.yMax - rect.height * relative.yMin;
            float width = rect.width * relative.width;
            float height = -rect.height * relative.height;
            relative = new Rect(x, y, width, height);

            // Draw the selection
            DrawOutline(relative, color);
        }
    }

    /// <summary>
    /// Draw a selection outline around the specified rectangle.
    /// </summary>

    public static void DrawOutline(Rect rect, Rect relative)
    {
        if (Event.current.type == EventType.Repaint)
        {
            // Calculate where the outer rectangle would be
            float x = rect.xMin + rect.width * relative.xMin;
            float y = rect.yMax - rect.height * relative.yMin;
            float width = rect.width * relative.width;
            float height = -rect.height * relative.height;
            relative = new Rect(x, y, width, height);

            // Draw the selection
            DrawOutline(relative);
        }
    }

    /// <summary>
    /// Draw a 9-sliced outline.
    /// </summary>

    public static void DrawOutline(Rect rect, Rect outer, Rect inner)
    {
        if (Event.current.type == EventType.Repaint)
        {
            Color green = new Color(0.4f, 1f, 0f, 1f);

            DrawOutline(rect, new Rect(outer.x, inner.y, outer.width, inner.height));
            DrawOutline(rect, new Rect(inner.x, outer.y, inner.width, outer.height));
            DrawOutline(rect, outer, green);
        }
    }

    /// <summary>
    /// Draw a checkered background for the specified texture.
    /// </summary>

    public static Rect DrawBackground(Texture2D tex, float ratio)
    {
        Rect rect = GUILayoutUtility.GetRect(0f, 0f);
        rect.width = Screen.width - rect.xMin;
        rect.height = rect.width * ratio;
        GUILayout.Space(rect.height);

        if (Event.current.type == EventType.Repaint)
        {
            Texture2D blank = blankTexture;
            Texture2D check = backdropTexture;

            // Lines above and below the texture rectangle
            GUI.color = new Color(0f, 0f, 0f, 0.2f);
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMin - 1, rect.width, 1f), blank);
            GUI.DrawTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1f), blank);
            GUI.color = Color.white;

            // Checker background
            DrawTiledTexture(rect, check);
        }
        return rect;
    }

    /// <summary>
    /// Draw a visible separator in addition to adding some padding.
    /// </summary>

    public static void DrawSeparator()
    {
        GUILayout.Space(12f);

        if (Event.current.type == EventType.Repaint)
        {
            Texture2D tex = blankTexture;
            Rect rect = GUILayoutUtility.GetLastRect();
            GUI.color = new Color(0f, 0f, 0f, 0.25f);
            GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
            GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
            GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
            GUI.color = Color.white;
        }
    }

    /// <summary>
    /// Convenience function that displays a list of sprites and returns the selected value.
    /// </summary>

    public static string DrawList(string field, string[] list, string selection, params GUILayoutOption[] options)
    {
        if (list != null && list.Length > 0)
        {
            int index = 0;
            if (string.IsNullOrEmpty(selection)) selection = list[0];

            // We need to find the sprite in order to have it selected
            if (!string.IsNullOrEmpty(selection))
            {
                for (int i = 0; i < list.Length; ++i)
                {
                    if (selection.Equals(list[i], System.StringComparison.OrdinalIgnoreCase))
                    {
                        index = i;
                        break;
                    }
                }
            }

            // Draw the sprite selection popup
            index = string.IsNullOrEmpty(field) ?
                EditorGUILayout.Popup(index, list, options) :
                EditorGUILayout.Popup(field, index, list, options);

            return list[index];
        }
        return null;
    }

    /// <summary>
    /// Convenience function that displays a list of sprites and returns the selected value.
    /// </summary>

    public static string DrawAdvancedList(string text, string[] list, string selection, params GUILayoutOption[] options)
    {
        if (list != null && list.Length > 0)
        {
            int index = 0;
            if (string.IsNullOrEmpty(selection)) selection = list[0];

            // We need to find the sprite in order to have it selected
            if (!string.IsNullOrEmpty(selection))
            {
                for (int i = 0; i < list.Length; ++i)
                {
                    if (selection.Equals(list[i], System.StringComparison.OrdinalIgnoreCase))
                    {
                        index = i;
                        break;
                    }
                }
            }

            // Draw the sprite selection popup
            index = string.IsNullOrEmpty(text) ?
                DrawPrefixList(index, list, options) :
                DrawPrefixList(text, index, list, options);

            return list[index];
        }
        return null;
    }

    /// <summary>
    /// Struct type for the integer vector field below.
    /// </summary>

    public struct IntVector
    {
        public int x;
        public int y;
    }

    /// <summary>
    /// Integer vector field.
    /// </summary>

    public static IntVector IntPair(string prefix, string leftCaption, string rightCaption, int x, int y)
    {
        GUILayout.BeginHorizontal();

        if (string.IsNullOrEmpty(prefix))
        {
            GUILayout.Space(82f);
        }
        else
        {
            GUILayout.Label(prefix, GUILayout.Width(74f));
        }

        IntVector retVal;
        retVal.x = EditorGUILayout.IntField(leftCaption, x, GUILayout.MinWidth(30f));
        retVal.y = EditorGUILayout.IntField(rightCaption, y, GUILayout.MinWidth(30f));

        GUILayout.EndHorizontal();
        return retVal;
    }

    /// <summary>
    /// Integer rectangle field.
    /// </summary>

    public static Rect IntRect(string prefix, Rect rect)
    {
        int left = Mathf.RoundToInt(rect.xMin);
        int top = Mathf.RoundToInt(rect.yMin);
        int width = Mathf.RoundToInt(rect.width);
        int height = Mathf.RoundToInt(rect.height);

        IntVector a = IntPair(prefix, "Left", "Top", left, top);
        IntVector b = IntPair(null, "Width", "Height", width, height);

        return new Rect(a.x, a.y, b.x, b.y);
    }

    /// <summary>
    /// Integer vector field.
    /// </summary>

    public static Vector4 IntPadding(string prefix, Vector4 v)
    {
        int left = Mathf.RoundToInt(v.x);
        int top = Mathf.RoundToInt(v.y);
        int right = Mathf.RoundToInt(v.z);
        int bottom = Mathf.RoundToInt(v.w);

        IntVector a = IntPair(prefix, "Left", "Top", left, top);
        IntVector b = IntPair(null, "Right", "Bottom", right, bottom);

        return new Vector4(a.x, a.y, b.x, b.y);
    }

    public static bool DrawPrefixButton(string text)
    {
        return GUILayout.Button(text, "DropDown", GUILayout.Width(76f));
    }

    public static bool DrawPrefixButton(string text, params GUILayoutOption[] options)
    {
        return GUILayout.Button(text, "DropDown", options);
    }

    public static int DrawPrefixList(int index, string[] list, params GUILayoutOption[] options)
    {
        return EditorGUILayout.Popup(index, list, "DropDown", options);
    }

    public static int DrawPrefixList(string text, int index, string[] list, params GUILayoutOption[] options)
    {
        return EditorGUILayout.Popup(text, index, list, "DropDown", options);
    }


    /// <summary>
    /// Draw a sprite preview.
    /// </summary>

    public static void DrawSprite(Texture2D tex, Rect drawRect, Color color, Rect textureRect, Vector4 border)
    {
        DrawSprite(tex, drawRect, color, null,
        Mathf.RoundToInt(textureRect.x),
        Mathf.RoundToInt(tex.height - textureRect.y - textureRect.height),
        Mathf.RoundToInt(textureRect.width),
        Mathf.RoundToInt(textureRect.height),
        Mathf.RoundToInt(border.x),
        Mathf.RoundToInt(border.y),
        Mathf.RoundToInt(border.z),
        Mathf.RoundToInt(border.w));
    }

    /// <summary>
    /// Draw a sprite preview.
    /// </summary>

    public static void DrawSprite(Texture2D tex, Rect drawRect, Color color, Material mat,
        int x, int y, int width, int height, int borderLeft, int borderBottom, int borderRight, int borderTop)
    {
        if (!tex) return;

        // Create the texture rectangle that is centered inside rect.
        Rect outerRect = drawRect;
        outerRect.width = width;
        outerRect.height = height;

        if (width > 0)
        {
            float f = drawRect.width / outerRect.width;
            outerRect.width *= f;
            outerRect.height *= f;
        }

        if (drawRect.height > outerRect.height)
        {
            outerRect.y += (drawRect.height - outerRect.height) * 0.5f;
        }
        else if (outerRect.height > drawRect.height)
        {
            float f = drawRect.height / outerRect.height;
            outerRect.width *= f;
            outerRect.height *= f;
        }

        if (drawRect.width > outerRect.width) outerRect.x += (drawRect.width - outerRect.width) * 0.5f;

        // Draw the background
        DrawTiledTexture(outerRect, backdropTexture);

        // Draw the sprite
        GUI.color = color;

        if (mat == null)
        {
            Rect uv = new Rect(x, y, width, height);
            uv = ConvertToTexCoords(uv, tex.width, tex.height);
            GUI.DrawTextureWithTexCoords(outerRect, tex, uv, true);
        }
        else
        {
            // NOTE: There is an issue in Unity that prevents it from clipping the drawn preview
            // using BeginGroup/EndGroup, and there is no way to specify a UV rect... le'suq.
            UnityEditor.EditorGUI.DrawPreviewTexture(outerRect, tex, mat);
        }

        if (Selection.activeGameObject == null || Selection.gameObjects.Length == 1)
        {
            // Draw the border indicator lines
            GUI.BeginGroup(outerRect);
            {
                tex = contrastTexture;
                GUI.color = Color.white;

                if (borderLeft > 0)
                {
                    float x0 = (float)borderLeft / width * outerRect.width - 1;
                    DrawTiledTexture(new Rect(x0, 0f, 1f, outerRect.height), tex);
                }

                if (borderRight > 0)
                {
                    float x1 = (float)(width - borderRight) / width * outerRect.width - 1;
                    DrawTiledTexture(new Rect(x1, 0f, 1f, outerRect.height), tex);
                }

                if (borderBottom > 0)
                {
                    float y0 = (float)(height - borderBottom) / height * outerRect.height - 1;
                    DrawTiledTexture(new Rect(0f, y0, outerRect.width, 1f), tex);
                }

                if (borderTop > 0)
                {
                    float y1 = (float)borderTop / height * outerRect.height - 1;
                    DrawTiledTexture(new Rect(0f, y1, outerRect.width, 1f), tex);
                }
            }
            GUI.EndGroup();

            // Draw the lines around the sprite
            Handles.color = Color.black;
            Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMin), new Vector3(outerRect.xMin, outerRect.yMax));
            Handles.DrawLine(new Vector3(outerRect.xMax, outerRect.yMin), new Vector3(outerRect.xMax, outerRect.yMax));
            Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMin), new Vector3(outerRect.xMax, outerRect.yMin));
            Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMax), new Vector3(outerRect.xMax, outerRect.yMax));

            // Sprite size label
            string text = string.Format("Sprite Size: {0}x{1}", Mathf.RoundToInt(width), Mathf.RoundToInt(height));
            EditorGUI.DropShadowLabel(GUILayoutUtility.GetRect(Screen.width, 18f), text);
        }
    }

    /// <summary>
    /// Draw the specified sprite.
    /// </summary>

    public static void DrawTexture(Texture2D tex, Rect rect, Rect uv, Color color)
    {
        DrawTexture(tex, rect, uv, color, null);
    }

    /// <summary>
    /// Draw the specified sprite.
    /// </summary>

    public static void DrawTexture(Texture2D tex, Rect rect, Rect uv, Color color, Material mat)
    {
        int w = Mathf.RoundToInt(tex.width * uv.width);
        int h = Mathf.RoundToInt(tex.height * uv.height);

        // Create the texture rectangle that is centered inside rect.
        Rect outerRect = rect;
        outerRect.width = w;
        outerRect.height = h;

        if (outerRect.width > 0f)
        {
            float f = rect.width / outerRect.width;
            outerRect.width *= f;
            outerRect.height *= f;
        }

        if (rect.height > outerRect.height)
        {
            outerRect.y += (rect.height - outerRect.height) * 0.5f;
        }
        else if (outerRect.height > rect.height)
        {
            float f = rect.height / outerRect.height;
            outerRect.width *= f;
            outerRect.height *= f;
        }

        if (rect.width > outerRect.width) outerRect.x += (rect.width - outerRect.width) * 0.5f;

        // Draw the background
        DrawTiledTexture(outerRect, backdropTexture);

        // Draw the sprite
        GUI.color = color;

        if (mat == null)
        {
            GUI.DrawTextureWithTexCoords(outerRect, tex, uv, true);
        }
        else
        {
            // NOTE: There is an issue in Unity that prevents it from clipping the drawn preview
            // using BeginGroup/EndGroup, and there is no way to specify a UV rect... le'suq.
            UnityEditor.EditorGUI.DrawPreviewTexture(outerRect, tex, mat);
        }
        GUI.color = Color.white;

        // Draw the lines around the sprite
        Handles.color = Color.black;
        Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMin), new Vector3(outerRect.xMin, outerRect.yMax));
        Handles.DrawLine(new Vector3(outerRect.xMax, outerRect.yMin), new Vector3(outerRect.xMax, outerRect.yMax));
        Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMin), new Vector3(outerRect.xMax, outerRect.yMin));
        Handles.DrawLine(new Vector3(outerRect.xMin, outerRect.yMax), new Vector3(outerRect.xMax, outerRect.yMax));

        // Sprite size label
        string text = string.Format("Texture Size: {0}x{1}", w, h);
        EditorGUI.DropShadowLabel(GUILayoutUtility.GetRect(Screen.width, 18f), text);
    }


    /// <summary>
    /// Select the specified game object and remember what was selected before.
    /// </summary>

    public static void Select(GameObject go)
    {
        mPrevious = Selection.activeGameObject;
        Selection.activeGameObject = go;
    }

    /// <summary>
    /// Select the previous game object.
    /// </summary>

    public static void SelectPrevious()
    {
        if (mPrevious != null)
        {
            Selection.activeGameObject = mPrevious;
            mPrevious = null;
        }
    }

    /// <summary>
    /// Previously selected game object.
    /// </summary>

    public static GameObject previousSelection { get { return mPrevious; } }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    public static bool DrawMinimalisticHeader(string text) { return DrawHeader(text, text, false, true); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    public static bool DrawHeader(string text) { return DrawHeader(text, text, false, false); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    public static bool DrawHeader(string text, string key) { return DrawHeader(text, key, false, false); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    public static bool DrawHeader(string text, bool detailed) { return DrawHeader(text, text, detailed, !detailed); }

    /// <summary>
    /// Draw a distinctly different looking header label
    /// </summary>

    public static bool DrawHeader(string text, string key, bool forceOn, bool minimalistic)
    {
        bool state = EditorPrefs.GetBool(key, true);

        if (!minimalistic) GUILayout.Space(3f);
        if (!forceOn && !state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        GUILayout.BeginHorizontal();
        GUI.changed = false;

        if (minimalistic)
        {
            if (state) text = "\u25BC" + (char)0x200a + text;
            else text = "\u25BA" + (char)0x200a + text;

            GUILayout.BeginHorizontal();
            GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
            if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f))) state = !state;
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
        }
        else
        {
            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f))) state = !state;
        }

        if (GUI.changed) EditorPrefs.SetBool(key, state);

        if (!minimalistic) GUILayout.Space(2f);
        GUILayout.EndHorizontal();
        GUI.backgroundColor = Color.white;
        if (!forceOn && !state) GUILayout.Space(3f);
        return state;
    }

    /// <summary>
    /// Begin drawing the content area.
    /// </summary>

    public static void BeginContents() { BeginContents(false); }

    static bool mEndHorizontal = false;

    /// <summary>
    /// Begin drawing the content area.
    /// </summary>

    public static void BeginContents(bool minimalistic)
    {
        if (!minimalistic)
        {
            mEndHorizontal = true;
            GUILayout.BeginHorizontal();
            EditorGUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(10f));
        }
        else
        {
            mEndHorizontal = false;
            EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
            GUILayout.Space(10f);
        }
        GUILayout.BeginVertical();
        GUILayout.Space(2f);
    }

    /// <summary>
    /// End drawing the content area.
    /// </summary>

    public static void EndContents()
    {
        GUILayout.Space(3f);
        GUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        if (mEndHorizontal)
        {
            GUILayout.Space(3f);
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(3f);
    }

    /// <summary>
    /// Draw a list of fields for the specified list of delegates.
    /// </summary>

    public static void DrawEvents(string text, Object undoObject, List<EventDelegate> list)
    {
        DrawEvents(text, undoObject, list, null, null, false);
    }

    /// <summary>
    /// Draw a list of fields for the specified list of delegates.
    /// </summary>

    public static void DrawEvents(string text, Object undoObject, List<EventDelegate> list, bool minimalistic)
    {
        DrawEvents(text, undoObject, list, null, null, minimalistic);
    }

    /// <summary>
    /// Draw a list of fields for the specified list of delegates.
    /// </summary>

    public static void DrawEvents(string text, Object undoObject, List<EventDelegate> list, string noTarget, string notValid, bool minimalistic)
    {
        if (!DrawHeader(text, text, false, minimalistic)) return;

        if (!minimalistic)
        {
            BeginContents(minimalistic);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            EventDelegateEditor.Field(undoObject, list, notValid, notValid, minimalistic);

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            EndContents();
        }
        else EventDelegateEditor.Field(undoObject, list, notValid, notValid, minimalistic);
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    public static SerializedProperty DrawProperty(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
    {
        return DrawProperty(null, serializedObject, property, false, options);
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    public static SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
    {
        return DrawProperty(label, serializedObject, property, false, options);
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    public static SerializedProperty DrawPaddedProperty(SerializedObject serializedObject, string property, params GUILayoutOption[] options)
    {
        return DrawProperty(null, serializedObject, property, true, options);
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    public static SerializedProperty DrawPaddedProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options)
    {
        return DrawProperty(label, serializedObject, property, true, options);
    }

    /// <summary>
    /// Helper function that draws a serialized property.
    /// </summary>

    public static SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, bool padding, params GUILayoutOption[] options)
    {
        SerializedProperty sp = serializedObject.FindProperty(property);

        if (sp != null)
        {
            if (padding) EditorGUILayout.BeginHorizontal();

            if (label != null) EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
            else EditorGUILayout.PropertyField(sp, options);

            if (padding)
            {
                EditorGUILayout.EndHorizontal();
            }
        }
        return sp;
    }

    /// <summary>
    /// Add a soft shadow to the specified color buffer.
    /// The buffer must have some padding around the edges in order for this to work properly.
    /// </summary>

    public static void AddShadow(Color32[] colors, int width, int height, Color shadow)
    {
        Color sh = shadow;
        sh.a = 1f;

        for (int y2 = 0; y2 < height; ++y2)
        {
            for (int x2 = 0; x2 < width; ++x2)
            {
                int index = x2 + y2 * width;
                Color32 uc = colors[index];
                if (uc.a == 255) continue;

                Color original = uc;
                float val = original.a;
                int count = 1;
                float div1 = 1f / 255f;
                float div2 = 2f / 255f;
                float div3 = 3f / 255f;

                // Left
                if (x2 != 0)
                {
                    val += colors[x2 - 1 + y2 * width].a * div1;
                    count += 1;
                }

                // Top
                if (y2 + 1 != height)
                {
                    val += colors[x2 + (y2 + 1) * width].a * div2;
                    count += 2;
                }

                // Top-left
                if (x2 != 0 && y2 + 1 != height)
                {
                    val += colors[x2 - 1 + (y2 + 1) * width].a * div3;
                    count += 3;
                }

                val /= count;

                Color c = Color.Lerp(original, sh, shadow.a * val);
                colors[index] = Color.Lerp(c, original, original.a);
            }
        }
    }

    /// <summary>
    /// Add a visual depth effect to the specified color buffer.
    /// The buffer must have some padding around the edges in order for this to work properly.
    /// </summary>

    public static void AddDepth(Color32[] colors, int width, int height, Color shadow)
    {
        Color sh = shadow;
        sh.a = 1f;

        for (int y2 = 0; y2 < height; ++y2)
        {
            for (int x2 = 0; x2 < width; ++x2)
            {
                int index = x2 + y2 * width;
                Color32 uc = colors[index];
                if (uc.a == 255) continue;

                Color original = uc;
                float val = original.a * 4f;
                int count = 4;
                float div1 = 1f / 255f;
                float div2 = 2f / 255f;

                if (x2 != 0)
                {
                    val += colors[x2 - 1 + y2 * width].a * div2;
                    count += 2;
                }

                if (x2 + 1 != width)
                {
                    val += colors[x2 + 1 + y2 * width].a * div2;
                    count += 2;
                }

                if (y2 != 0)
                {
                    val += colors[x2 + (y2 - 1) * width].a * div2;
                    count += 2;
                }

                if (y2 + 1 != height)
                {
                    val += colors[x2 + (y2 + 1) * width].a * div2;
                    count += 2;
                }

                if (x2 != 0 && y2 != 0)
                {
                    val += colors[x2 - 1 + (y2 - 1) * width].a * div1;
                    ++count;
                }

                if (x2 != 0 && y2 + 1 != height)
                {
                    val += colors[x2 - 1 + (y2 + 1) * width].a * div1;
                    ++count;
                }

                if (x2 + 1 != width && y2 != 0)
                {
                    val += colors[x2 + 1 + (y2 - 1) * width].a * div1;
                    ++count;
                }

                if (x2 + 1 != width && y2 + 1 != height)
                {
                    val += colors[x2 + 1 + (y2 + 1) * width].a * div1;
                    ++count;
                }

                val /= count;

                Color c = Color.Lerp(original, sh, shadow.a * val);
                colors[index] = Color.Lerp(c, original, original.a);
            }
        }
    }
}
