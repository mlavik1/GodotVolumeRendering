#if TOOLS
using Godot;
using System;
using System.Diagnostics;
using UnityVolumeRendering;

[Tool]
public partial class VolumeContainerNode : Node
{
	private CsgBox3D volumeContainer = null;

	public override void _EnterTree()
	{
		Debug.WriteLine("Hello from VolumeContainerNode");
		volumeContainer = new CsgBox3D();
		RawDatasetImporter datasetImporter = new RawDatasetImporter(ProjectSettings.GlobalizePath("res://Datasets/VisMale.raw"), 128, 256, 256, DataContentFormat.Uint8, Endianness.LittleEndian, 0);
		VolumeDataset dataset = datasetImporter.Import();
		Godot.Collections.Array<Image> images = new Godot.Collections.Array<Image>();
		float minVal = float.PositiveInfinity;
		float maxVal = float.NegativeInfinity;
		foreach (float density in dataset.data)
		{
			minVal = Mathf.Min(minVal, density);
			maxVal = Mathf.Max(maxVal, density);
		}
		float[] normData = new float[dataset.data.Length];
		for (int i = 0; i < dataset.data.Length; i++)
		{
			normData[i] = (dataset.data[i] - minVal) / (maxVal - minVal);
		}
		for (int i = 0; i < dataset.dimZ; i++)
		{
			float[] slice = new float[dataset.dimX * dataset.dimY];
			Array.Copy(normData, i * (dataset.dimX * dataset.dimY), slice, 0, slice.Length);
			Image image = new Image();
			byte[] byteArray = new byte[slice.Length * 4];
			Buffer.BlockCopy(slice, 0, byteArray, 0, byteArray.Length);
			image.SetData(dataset.dimX, dataset.dimY, false, Image.Format.Rf, byteArray);
			images.Add(image);
		}
		ImageTexture3D dataTexture = new ImageTexture3D();
		dataTexture.Create(Image.Format.Rf, dataset.dimX, dataset.dimY, dataset.dimZ, false, images);
		ShaderMaterial shaderMaterial = GD.Load<ShaderMaterial>("res://addons/godot_volume_rendering/volume_rendering.tres");
		shaderMaterial.SetShaderParameter("data_tex", dataTexture);
		shaderMaterial.SetShaderParameter("tf_tex", TransferFunctionDatabase.CreateTransferFunction().GetTexture());
		volumeContainer.Material = shaderMaterial;
		this.AddChild(volumeContainer);
		base._EnterTree();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}
}
#endif
