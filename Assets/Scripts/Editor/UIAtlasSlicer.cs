using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

public static class UIAtlasSlicer
{
    [Serializable]
    private class AtlasRect
    {
        public int x, y, w, h;
    }

    [Serializable]
    private class AtlasPivot
    {
        public float x = 0.5f, y = 0.5f;
    }

    [Serializable]
    private class AtlasBorder
    {
        public int l, b, r, t;
    }

    [Serializable]
    private class AtlasSprite
    {
        public string name;
        public AtlasRect unityRect;
        public AtlasPivot pivot;
        public AtlasBorder border;
    }

    [Serializable]
    private class AtlasData
    {
        public string texture;
        public int width, height;
        public AtlasSprite[] sprites;
    }

    public const string JsonPath = "Assets/UI/Atlas/DeadSector_UI_Atlas.json";

    [MenuItem("Tools/Dead Sector/Slice UI Atlas")]
    public static void Slice()
    {
        var json = AssetDatabase.LoadAssetAtPath<TextAsset>(JsonPath);

        if (json == null)
        {
            Debug.LogError($"Atlas data not found at {JsonPath}");
            return;
        }

        var data = JsonUtility.FromJson<AtlasData>(json.text);
        string texturePath = Path.Combine(Path.GetDirectoryName(JsonPath), data.texture).Replace('\\', '/');

        if (AssetImporter.GetAtPath(texturePath) is not TextureImporter importer)
        {
            Debug.LogError($"Atlas texture not found at {texturePath}");
            return;
        }

        ConfigureImporter(importer, data);
        ApplySprites(importer, data);

        Debug.Log($"Sliced {data.sprites.Length} sprites into {texturePath}");
    }

    private static void ConfigureImporter(TextureImporter importer, AtlasData data)
    {
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.alphaIsTransparency = true;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Bilinear;
        importer.wrapMode = TextureWrapMode.Clamp;
        importer.maxTextureSize = Mathf.Max(data.width, data.height);
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.spritePixelsPerUnit = 100;

        var settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        settings.spriteMeshType = SpriteMeshType.FullRect;
        importer.SetTextureSettings(settings);
        importer.SaveAndReimport();
    }

    private static void ApplySprites(TextureImporter importer, AtlasData data)
    {
        var factory = new SpriteDataProviderFactories();
        factory.Init();

        var provider = factory.GetSpriteEditorDataProviderFromObject(importer);
        provider.InitSpriteEditorDataProvider();

        var existingIds = provider.GetSpriteRects().ToDictionary(rect => rect.name, rect => rect.spriteID);

        var rects = data.sprites.Select(sprite => new SpriteRect
        {
            name = sprite.name,
            rect = new Rect(sprite.unityRect.x, sprite.unityRect.y, sprite.unityRect.w, sprite.unityRect.h),
            alignment = SpriteAlignment.Custom,
            pivot = new Vector2(sprite.pivot.x, sprite.pivot.y),
            border = new Vector4(sprite.border.l, sprite.border.b, sprite.border.r, sprite.border.t),
            spriteID = existingIds.TryGetValue(sprite.name, out var id) ? id : GUID.Generate()
        }).ToArray();

        provider.SetSpriteRects(rects);
        provider.GetDataProvider<ISpriteNameFileIdDataProvider>()?.SetNameFileIdPairs(rects.Select(rect => new SpriteNameFileIdPair(rect.name, rect.spriteID)).ToList());
        provider.Apply();

        importer.SaveAndReimport();
    }
}
