using Godot;
using System;

public partial class CubeSpawner : Node3D
{
	[Export] PackedScene CubeScene { get; set; }


	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			GD.Print("Spawning Cube!!!");

			if (CubeScene == null)
			{
				GD.Print("CubeScene is not set!");
				return;
			}

			Node3D newCube = (Node3D)CubeScene.Instantiate();
			AddChild(newCube);
            
            GD.Print("SUCCESS: Node added. Path is: " + newCube.GetPath());
            
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
