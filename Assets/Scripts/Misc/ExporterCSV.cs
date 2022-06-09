using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
public static class ExporterCSV
{
    public static void SaveToFile(string _sContent)
    {
        string sFolder = Application.persistentDataPath;

        if (!Directory.Exists(sFolder))
        {
            Directory.CreateDirectory(sFolder);
        }

        string sFilePath = Path.Combine(sFolder, "export.csv");

        using (var writer = new StreamWriter(sFilePath, false))
        {
            writer.Write(_sContent);
        }

        // Or just
        //File.WriteAllText(content);

        Debug.Log($"CSV file written to \"{sFilePath}\"");

    #if UNITY_EDITOR
        AssetDatabase.Refresh();
    #endif
    }
}
