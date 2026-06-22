public static class OreTextFormatter
{
    public static string GetTmpTag(OreType oreType)
    {
        if (oreType == OreType.None || oreType == OreType.Length)
        {
            return string.Empty;
        }

        OreDataSO oreData = OreDataDB.GetOreDataSO(oreType);
        if (oreData == null)
        {
            return string.Empty;
        }
        if (string.IsNullOrEmpty(oreData.TmpIconTag))
        {
            return "광물so세팅오류";
        }

        return $"<sprite name=\"{oreData.TmpIconTag}\">";
    }

    public static string GetDisplayName(OreType oreType)
    {
        if (oreType == OreType.None || oreType == OreType.Length)
        {
            return string.Empty;
        }

        OreDataSO oreData = OreDataDB.GetOreDataSO(oreType);
        if (oreData == null)
        {
            return string.Empty;
        }
        if (string.IsNullOrEmpty(oreData.DisplayName))
        {
            return "광물so세팅오류";
        }
        
        return oreData.DisplayName;
    }
}