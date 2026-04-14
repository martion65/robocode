using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class conveyor : Area2D
{

	public Godot.Collections.Array<Node2D> robotList = [];
	public Marker2D slotStart;
	public Vector2 offset = new Vector2(15, 0);
	public bool lockedIn = false;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		slotStart = GetNode<Marker2D>("slotStart");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_lockin_pressed()
	{

		if (!lockedIn)
		{
			robotList = GetOverlappingBodies();
			List<Node2D> sortedList = new List<Node2D>(robotList);
			sortedList.Sort((a, b) => a.Position.X.CompareTo(b.Position.X));
			robotList.Clear();
			foreach (Node2D node in sortedList)
			{
				robotList.Add(node);
			}




			foreach (Node2D robot in robotList)
			{

				robot.Reparent(slotStart);
				robot.Position = Vector2.Zero + robot.GetIndex() * offset;


				robot.AddToGroup("Submitted");
				robot.RemoveFromGroup("Pickable");
				Debug.WriteLine(robot.GetGroups());
			}

			lockedIn = true;

		}
		else
		{


			foreach (Node2D robot in robotList)
			{


				robot.Reparent(GetParent());
				robot.RemoveFromGroup("Submitted");
				robot.AddToGroup("Pickable");

			}
			lockedIn = false;
			robotList.Clear();

		}

    }

	public void _on_execute_pressed() 
	{

		foreach (robot robot in robotList) 
		{

			robot.traitExecute();

		}


	}





}


