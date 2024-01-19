#if TOOLS
using Godot;
using System;

[Tool]
public partial class VolumeContainerNode : Node
{
	private CsgBox3D volumeContainer = null;

	public void LoadDataset(VolumeDataset dataset)
	{
		if (dataset == null)
		{
			return;
		}

		if (volumeContainer != null)
		{
			this.RemoveChild(volumeContainer);
		}
		volumeContainer = new CsgBox3D();

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
		ShaderMaterial shaderMaterial = GD.Load<ShaderMaterial>("res://addons/godot_volume_rendering/shaders/volume_rendering.tres");
		shaderMaterial.SetShaderParameter("data_tex", dataTexture);
		shaderMaterial.SetShaderParameter("tf_tex", TransferFunctionDatabase.CreateTransferFunction().GetTexture());
		NoiseTexture2D noiseTexture = new NoiseTexture2D();
		noiseTexture.Noise = new FastNoiseLite();
		shaderMaterial.SetShaderParameter("noise_tex", noiseTexture);
		volumeContainer.Material = shaderMaterial;
		this.AddChild(volumeContainer);
	}

	public override void _EnterTree()
	{
		base._EnterTree();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}
}
#endif
