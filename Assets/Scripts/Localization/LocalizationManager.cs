using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    private const string ResourcesFolder = "Localization";
    private const string LanguagePrefsKey = "Language";
    private const string DefaultLanguage = "en";

    private static LocalizationManager instance;

    private readonly Dictionary<string, LanguageFile> languages = new();
    private readonly Dictionary<string, string> entries = new();

    public static LocalizationManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameObject("LocalizationManager").AddComponent<LocalizationManager>();

            return instance;
        }
    }

    public static bool HasInstance => instance != null;

    public string CurrentLanguage { get; private set; }
    public IEnumerable<LanguageFile> AvailableLanguages => languages.Values;

    public event Action OnLanguageChanged;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadLanguages();
        SetLanguage(PlayerPrefs.GetString(LanguagePrefsKey, DefaultLanguage));
    }

    public void SetLanguage(string code)
    {
        if (!languages.TryGetValue(code, out var language))
            language = languages.Values.FirstOrDefault();

        if (language == null || language.code == CurrentLanguage) return;

        entries.Clear();

        foreach (var entry in language.entries)
            entries[entry.key] = entry.value;

        CurrentLanguage = language.code;
        PlayerPrefs.SetString(LanguagePrefsKey, language.code);

        OnLanguageChanged?.Invoke();
    }

    public string Get(string key)
    {
        if (string.IsNullOrEmpty(key)) return string.Empty;

        return entries.TryGetValue(key, out string value) ? value : key;
    }

    public string Resolve(LocalizedMessage message)
    {
        if (message.IsEmpty) return string.Empty;

        string format = Get(message.Key);

        if (message.Args == null || message.Args.Length == 0)
            return format;

        var args = message.Args.Select(arg => arg is string text ? Get(text) : arg).ToArray();
        return string.Format(format, args);
    }

    private void LoadLanguages()
    {
        foreach (var asset in Resources.LoadAll<TextAsset>(ResourcesFolder))
        {
            var language = JsonUtility.FromJson<LanguageFile>(asset.text);

            if (language != null && !string.IsNullOrEmpty(language.code))
                languages[language.code] = language;
        }
    }
}
