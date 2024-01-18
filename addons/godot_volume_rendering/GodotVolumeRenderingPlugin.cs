#if TOOLS
using Godot;
using System;

[Tool]
public partial class GodotVolumeRenderingPlugin : EditorPlugin
{
	public override void _EnterTree()
	{
		var script = GD.Load<Script>("res://addons/godot_volume_rendering/VolumeContainerNode.cs");
        //var texture = GD.Load<Texture>("icon.png");
        AddCustomType("VolumeContainerNode", "Node", script, null);
	}

	public override void _ExitTree()
	{
	}
}
#endif
