using Godot;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Threading.Tasks;

public partial class robot : RigidBody2D
{

    public Godot.Collections.Array<Node2D> containedRobots = [];
	public Marker2D nestSlot;
	public Marker2D nestSlot2;
	public Vector2 offset = new Vector2(15, 0);
	public int counter = 0;
	public Label counterDisplay;
	public SpinBox counterInput;
	public int desiredCounter = 0;
    public async Task traitExecute(robot loopRobot = null) // activate each robot's unique function. (to be able to see counters tick up, we need to use an asynchronous task and await, otherwise it would all count up in one step.) 
	{

		if (IsInGroup("Print"))
		{


		}

		else if (IsInGroup("While")) // execute the trait of the while robot
		{
            desiredCounter = Convert.ToInt32(counterInput.Value); //allows user to input how many times they want it to loop

            Debug.WriteLine("Tried to execute trait of While");
            

             while (counter < desiredCounter) //until the counter reaches what the user input..
			{
				Debug.WriteLine("Tried to loop");

				foreach (robot nestedRobot in containedRobots) //for each robot inside of the while loop robot
				{

					nestedRobot.traitExecute(this); // execute unique function, passing the while loop robot as a parameter so its counter counts up instead of the nested robot.

                }

                counterDisplay.Text = counter.ToString(); //update counter each loop
				await ToSignal(GetTree().CreateTimer(0.0050f), SceneTreeTimer.SignalName.Timeout); // this makes the execution wait 0.005 seconds before looping again, letting the loops be visible to the user.
            }
		}
		else if (IsInGroup("IfElse")) { }


		else if (IsInGroup("Add")) 
		{
            Debug.WriteLine("Tried to execute trait of Add");

            
            if (loopRobot == null) // if not in a loop, increase own counter
			{
				counter++;
                counterDisplay.Text = counter.ToString();
            }
			else 
			{
				loopRobot.counter++;
			}

        }


		
		



		
	}

	
	



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{


		if (IsInGroup("While"))
		{
			nestSlot = GetNode<Marker2D>("nestSlot");
			nestSlot2 = GetNode<Marker2D>("nestSlot2");
            counterInput = GetNode<SpinBox>("counterInput");
            counterDisplay = GetNode<Label>("counterDisplay");


        }
		if (IsInGroup("Add")) 
		{
            counterDisplay = GetNode<Label>("counterDisplay");
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{



		if (IsInGroup("Held") || (IsInGroup("Nested"))) //if robot is being held or is inside of a while loop robot, disable its collision and physics
		{
			Freeze = true;
			SetCollisionLayerValue(4, false);
			SetCollisionLayerValue(3, false);
			SetCollisionMaskValue(1, false);
			SetCollisionLayerValue(1, false);
			SetCollisionLayerValue(5, true);

		}

		else if (!IsInGroup("Held")) // re-enable physics and collision when not
		{

			
			SetCollisionLayerValue(3, true);
			SetCollisionMaskValue(1, true);
			SetCollisionLayerValue(1, true);



			if (!IsInGroup("Submitted"))
			{

				Freeze = false;
			}
			else
			{
				Freeze = true;

			}
			if (!IsInGroup("While") && (!IsInGroup("Nested"))) 
			{
                SetCollisionLayerValue(4, true);
            }

		}




	}

	
	public void _on_area_2d_body_entered(Node2D insideRobot) //upon putting a robot inside of a while loop robot
	{
		if (!IsInGroup("Held")) //if it isnt being held when inside the robot
		{
			if (containedRobots.Count < 5) // and there are less than 5 robots inside
			{
				if (insideRobot.IsInGroup("Pickable") && !containedRobots.Contains(insideRobot)) // making sure that the robot being nested hasn't already been nested before by checking if it has been made not pickable and if it already appears in the array.
				containedRobots.Add(insideRobot); // add robot to execution list
				insideRobot.RemoveFromGroup("Pickable"); // stop it from being able to be picked up
				
				
				if (containedRobots.Count<= 2) //positioning the robot inside the while loop robot
				{
                    insideRobot.CallDeferred(Node.MethodName.Reparent, nestSlot, false);

                    insideRobot.SetDeferred("position", Vector2.Zero + ((containedRobots.Count-1) * offset)) ;
					
					
				}
				else 
				{
                    insideRobot.CallDeferred(Node.MethodName.Reparent, nestSlot2, false);
                    insideRobot.SetDeferred("position", Vector2.Zero + ((containedRobots.Count-3) * offset)); 
				}
                insideRobot.AddToGroup("Nested"); //make sure the program knows the robot is nested

            }
		}
	

	}

	public void _on_eject_button_pressed() //ejects robots from while loop robot
	{
		foreach (robot nestedRobot in containedRobots) 
		{
			nestedRobot.Position += Vector2.Up * 50;
			nestedRobot.AddToGroup("Pickable");
			nestedRobot.Reparent(GetParent());
			nestedRobot.RemoveFromGroup("Nested");
			
		}
		containedRobots.Clear();
	}

	public void _on_line_edit_text_changed(string newText)
    {
		

	}
}
