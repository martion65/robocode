using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public partial class player : CharacterBody2D
{
	public const float Speed = 150.0f;
	public const float JumpVelocity = -200.0f;

    private Sprite2D debugSprite;
	
	private Marker2D handPosition;
    private bool isInRange = false;
    private Node2D targetObject;
    private RigidBody2D heldObject;



    public override void _Ready()
	{
        debugSprite = GetNode<Sprite2D>("debug");

		handPosition = GetNode<Marker2D>("handPosition");
	}

	public override void _PhysicsProcess(double delta)
	{



        PickupObject();
        DropObject();

        Vector2 velocity = Velocity;
		

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();



	}

    private void _on_area_2d_body_entered(Node2D body) //checks if object has entered range
    {
        if (body.IsInGroup("Pickable"))
        {
            debugSprite.Visible = true;
            isInRange = true;
            targetObject = body;
        }
    }

    private void _on_area_2d_body_exited(Node2D body) //checks if object has exited range
    {
        if (body.IsInGroup("Pickable"))
        {
            debugSprite.Visible = false;
            isInRange = false;
            targetObject = null;
        }
    }

    private void PickupObject()
    {

        


        if (Input.IsActionJustPressed("pickup") && heldObject == null && targetObject != null) // if you press the pickup button while an object is in range, and you're not already holding something:
        {

            

            heldObject = (RigidBody2D)targetObject; // the object is picked up
            
            heldObject.Reparent(handPosition); //moves object to marker hand position
            heldObject.AddToGroup("Held");
            heldObject.Position = Vector2.Zero;
        }
    }

    private void PerformDrop(Vector2 offset)
    {
        heldObject.Reparent(GetParent());
        heldObject.Position = Position + offset;
        heldObject = null;
    }


    private void DropObject()
    {
        if (heldObject == null) return;

        if (Input.IsActionJustPressed("drop right")) //drops object to the right of the player
        {
            heldObject.RemoveFromGroup("Held");
            PerformDrop(Vector2.Right * 20);
        }
        else if (Input.IsActionJustPressed("drop left")) //drops object to the left of the player
        {
            heldObject.RemoveFromGroup("Held");
            PerformDrop(Vector2.Left * 20);
        }
    }

    


}

