using Sandbox;

public sealed class Brick : Component
{
	Player player;
	bool IsUse;
	Rotation rotation1;
	bool rotating;
	protected override void OnUpdate()
	{
		if (!IsUse)
		{
			return;
		}
		if (true)
		{
			Vector3 move = Input.AnalogMove * Transform.Rotation * Time.Delta * 50;
			Move( move );
			if ( Input.Pressed( "reload" ) && !rotating )
			{
				var angles = new Angles( 0, -90, 0 );
				Rotate( Rotation.From(angles) );
			}
			if (Transform.Rotation == rotation1)
			{
				Transform.Scale = new Vector3();
			}
			Transform.Rotation = Rotation.Lerp( Transform.Rotation, rotation1, 5f * Time.Delta );
		}
	}
	[Broadcast]
	public void Move(Vector3 movement)
	{
		Transform.Position += movement;
	}
	[Broadcast]
	public void Rotate( Rotation rotation )
	{
		rotation1 = Transform.Rotation * rotation;
		rotating = true;
	}
	[Broadcast]
	public void DisableBody()
	{
		this.Components.Get<Rigidbody>().MotionEnabled = false;
	}
	[Broadcast]
	public void EnableBody()
	{
		this.Components.Get<Rigidbody>().MotionEnabled = true;
	}
	public SceneTraceResult Trace( Vector3 start, Vector3 end )
	{
		return Scene.Trace.Ray( start, end )
			.UsePhysicsWorld()
			.Radius( 2f )
			.IgnoreGameObject( this.GameObject )
			.Run();
	}
	public void Use(Player player)
	{
		if (IsUse)
		{
			if ( player == this.player && !Trace( Transform.Position, Transform.Position + Vector3.Up * 100 ).Hit )
			{
				this.player = null;
				EnableBody();
				IsUse = false;
				player.brick = null;
			}
			return;
		}
		if ( !Trace(Transform.Position, Transform.Position + Vector3.Up * 100).Hit )
		{
			return;
		}
		DisableBody();
		this.player = player;
		IsUse = true;
		player.brick = this;
	}
}
