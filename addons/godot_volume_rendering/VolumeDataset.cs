using Godot;
using System;
using System.IO;

/// <summary>
/// An imported dataset. Has a dimension and a 3D pixel array.
/// </summary>
[Serializable]
public class VolumeDataset
{
    public string filePath;
    public string datasetName;
    
    // Flattened 3D array of data sample values.
    public float[] data;

    public int dimX, dimY, dimZ;

    private float minDataValue = float.MaxValue;
    private float maxDataValue = float.MinValue;

    public float GetMinDataValue()
    {
        if (minDataValue == float.MaxValue)
            CalculateValueBounds();
        return minDataValue;
    }

    public float GetMaxDataValue()
    {
        if (maxDataValue == float.MinValue)
            CalculateValueBounds();
        return maxDataValue;
    }

    private void CalculateValueBounds()
    {
        minDataValue = float.MaxValue;
        maxDataValue = float.MinValue;

        if (data != null)
        {
            for (int i = 0; i < dimX * dimY * dimZ; i++)
            {
                float val = data[i];
                minDataValue = Mathf.Min(minDataValue, val);
                maxDataValue = Mathf.Max(maxDataValue, val);
            }
        }
    }
}
