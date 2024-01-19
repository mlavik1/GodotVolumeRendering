#if TOOLS
using Godot;
using System;

[Tool]
public partial class GodotVolumeRenderingPlugin : EditorPlugin
{
	private VolumeContainerNodeInspector volumeContainerNodeInspector;

	public override void _EnterTree()
	{
		var script = GD.Load<Script>("res://addons/godot_volume_rendering/source/VolumeContainerNode.cs");
        //var texture = GD.Load<Texture>("icon.png");
        AddCustomType("VolumeContainerNode", "Node", script, null);
		volumeContainerNodeInspector = new VolumeContainerNodeInspector();
		AddInspectorPlugin(volumeContainerNodeInspector);
	}

	public override void _ExitTree()
	{
		RemoveInspectorPlugin(volumeContainerNodeInspector);
	}
}
#endif
