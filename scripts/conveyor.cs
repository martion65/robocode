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

		if (!lockedIn) //if there aren't already robots locked into the conveyor belt
		{
			robotList = GetOverlappingBodies(); // keep track of robots inside the conveyor belt
			List<Node2D> sortedList = new List<Node2D>(robotList); // make a temporary list to contain the positions of the robots  
			sortedList.Sort((a, b) => a.Position.X.CompareTo(b.Position.X)); // sort the list of robots in the order they are in the world from left > right
			robotList.Clear(); // clear out original list so we can add sorted list instead
			foreach (Node2D node in sortedList)
			{
				robotList.Add(node);
			}




			foreach (Node2D robot in robotList)
			{

				robot.Reparent(slotStart); 
				robot.Position = Vector2.Zero + robot.GetIndex() * offset; //move robots into position


				robot.AddToGroup("Submitted");
				robot.RemoveFromGroup("Pickable");
				Debug.WriteLine(robot.GetGroups());
			}

			lockedIn = true; //notify that the conveyor already has robots locked in

		}
		else //if there ARE already robots locked into the conveyor belt
		{


			foreach (Node2D robot in robotList)
			{


				robot.Reparent(GetParent()); //release them
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


