using Godot;
using System;

[Tool]
public class VolumeRenderedObject : Node
{
    public override void _Ready()
    {
        GD.Print("Test");
    
        string datasetPath = "/media/matias/Data/GodotProjects/GodotVolumeRendering/addons/godot_volume_rendering/DataFiles/VisMale.raw";
        RawDatasetImporter importer = new RawDatasetImporter(datasetPath, 128, 256, 256, DataContentFormat.Uint8, Endianness.LittleEndian, 0);
        VolumeDataset dataset = importer.Import();

        Texture3D dataTexture = new Texture3D();
        dataTexture.Create((uint)dataset.dimX, (uint)dataset.dimY, (uint)dataset.dimZ, Image.Format.Rgb8);

        for(int iz = 0; iz < dataset.dimX; iz++)
        {
            Image sliceIage = new Image();
            sliceIage.Create(dataset.dimX, dataset.dimY, false, Image.Format.Rgb8);
            for(int iy = 0; iy < dataset.dimX; iy++)
            {
                for(int ix = 0; ix < dataset.dimX; ix++)
                {
                    sliceIage.Lock();
                    sliceIage.SetPixel(ix, iy, new Color(dataset.data[iz * dataset.dimX * dataset.dimY + iy * dataset.dimX + ix], 0.0f, 0.0f));
                    sliceIage.Unlock();
                }
            }
            dataTexture.SetLayerData(sliceIage, iz);
        }
        GD.Print(GetTree().GetRoot().GetName());
        GD.Print(this.GetParent().GetNode("Box").GetName());

        Node boxNode = this.GetParent().GetNode("Box");
        ShaderMaterial boxMat = (ShaderMaterial)((CSGBox)boxNode).Material;//boxNode.GetSurfaceMaterial(0);
        boxMat.SetShaderParam("dataTexture", dataTexture);
        boxMat.SetShaderParam("testcol", new Vector3(1.0f, 0.0f, 0.0f));

        GD.Print("Import done");
    }
}
