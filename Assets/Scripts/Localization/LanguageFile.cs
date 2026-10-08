using System;

[Serializable]
public class LanguageFile
{
    public string code;
    public string languageName;
    public LocalizationEntry[] entries;
}

[Serializable]
public class LocalizationEntry
{
    public string key;
    public string value;
}
