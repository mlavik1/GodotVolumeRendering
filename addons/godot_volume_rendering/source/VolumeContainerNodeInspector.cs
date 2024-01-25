#if TOOLS
using System.Diagnostics;
using Godot;

public partial class VolumeContainerNodeInspector : EditorInspectorPlugin
{
    public override bool _CanHandle(GodotObject @object)
    {
        Debug.WriteLine("_CanHandle CALLED");
        Debug.WriteLine(@object.GetType() == typeof(VolumeContainerNode));
        return @object.GetType() == typeof(VolumeContainerNode);
    }

    public override void _ParseEnd(GodotObject @object)
    {
        Button btnImport = new Button();
        btnImport.Text = "Import dataset";
        btnImport.Pressed += () => {
            VolumeContainerNode volumeContainerNode = @object as VolumeContainerNode;

            EditorFileDialog fileDialog = new EditorFileDialog();
            fileDialog.FileMode = EditorFileDialog.FileModeEnum.OpenFile;
            fileDialog.CurrentDir = "res://Datasets";
            EditorInterface.Singleton.GetEditorViewport3D().AddChild(fileDialog);
            fileDialog.FileSelected += (string filePath) => {
                RawDatasetImporterDialog importerDialog = new RawDatasetImporterDialog(ProjectSettings.GlobalizePath("res://Datasets/VisMale.raw"), 128, 256, 256, DataContentFormat.Uint8, Endianness.LittleEndian, 0);
                importerDialog.Title = "Raw dataset import settings";
                importerDialog.onDatasetLoaded += (VolumeDataset dataset) => {
                    volumeContainerNode.LoadDataset(dataset);
                };
                EditorInterface.Singleton.GetEditorViewport3D().AddChild(importerDialog);
                importerDialog.PopupCentered(new Vector2I(400, 300));
            };
            fileDialog.Popup();
        };
        AddCustomControl(btnImport);
    }
}
#endif
