using System;

public class PathInfos
{
    public static readonly string AppData = Environment.GetEnvironmentVariable("appdata");
    public static readonly string AppDataPath = AppData + @"\MWR FoV Changer";
    public static readonly string LogsPath = AppDataPath + @"\Logs";
}

