using System.IO;
using System.Collections.Generic;
using UnityEngine;


public class FileManager : MonoBehaviour
{
    private string chassisSettingsFilePath = "StreamingAssets/ChassisSettings.csv";
    private string engineSettingsFilePath = "StreamingAssets/EnginesSettings.csv";
    private string wheelsSettingsFilePath = "StreamingAssets/WheelsSettings.csv";
    private string accessoriesSettingsFilePath = "StreamingAssets/AccessoriesSettings.csv";


    public string LoadChassisValue(int columnIndex, int rowIndex)
    {
        return GetCSVCellValues(chassisSettingsFilePath, columnIndex, rowIndex);
    }

    public string LoadWheelsValue(int columnIndex, int rowIndex)
    {
        return GetCSVCellValues(wheelsSettingsFilePath, columnIndex, rowIndex);
    }

    public string LoadEngineValue(int columnIndex, int rowIndex)
    {
        return GetCSVCellValues(engineSettingsFilePath, columnIndex, rowIndex);
    }

    public string LoadAccessoriesValue(int columnIndex, int rowIndex)
    {
        return GetCSVCellValues(accessoriesSettingsFilePath, columnIndex, rowIndex);
    }

    string GetCSVCellValues(string filePath, int columnIndex, int rowIndex)
    {
        try
        {
            using (var reader = new StreamReader(Application.dataPath + "/" + filePath))
            {
                List<string> columnValues = new List<string>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split('|');

                    columnValues.Add(values[columnIndex]);
                }

                if (columnValues[rowIndex].Trim() != "")
                {
                    return columnValues[rowIndex].Trim();
                }
            }
        }
        catch (System.Exception ex)
        {
            return (0).ToString();
        }

        return (0).ToString();
    }
}
