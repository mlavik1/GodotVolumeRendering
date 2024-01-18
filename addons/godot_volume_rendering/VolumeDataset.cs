using System;

/// <summary>
/// An imported dataset. Contains a 3D pixel array of density values.
/// </summary>
public class VolumeDataset
{
    // Flattened 3D array of data sample values.
    public float[] data;

    public int dimX, dimY, dimZ;

    public string datasetName;
}
