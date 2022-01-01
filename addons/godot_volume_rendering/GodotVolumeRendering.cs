using Godot;
using System;

[Tool]
public class GodotVolumeRendering : EditorPlugin
{
    public override void _EnterTree()
    {
        var script = GD.Load<Script>("res://addons/godot_volume_rendering/VolumeRenderedObject.cs");
        var texture = GD.Load<Texture>("res://addons/godot_volume_rendering/icon.png");
        AddCustomType("VolumeRenderedObject", "Node", script, texture);
    }

    public override void _ExitTree()
    {
        RemoveCustomType("VolumeRenderedObject");
    }
}
