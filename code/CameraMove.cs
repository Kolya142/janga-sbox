using Sandbox;

public sealed class CameraMove : Component
{
	[Property] public float CameraDistance = 500f;
	[Property] public Vector3 offset;
	[Property] public Vector2 CameraDistanceBorder = new Vector2(300, 1000);
	Angles eyeAngles;

	protected override void OnStart()
	{
		eyeAngles = Transform.Rotation;
	}

	protected override void OnUpdate()
	{
		eyeAngles += Input.AnalogLook;
		CameraDistance -= Input.MouseWheel.y * 20;
		CameraDistance.Clamp(CameraDistanceBorder.x, CameraDistanceBorder.y);
		Transform.Rotation = eyeAngles;
		Transform.Position = -eyeAngles.Forward * CameraDistance + offset;
	}
}
