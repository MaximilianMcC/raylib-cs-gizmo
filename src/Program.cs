using System.Numerics;
using Raylib_cs;

class Program
{
	public static void Main(string[] args)
	{
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		Raylib.InitWindow(500, 500, "raylib-cs gizmo example");

		Raylib.DisableCursor();
		Camera3D camera = new Camera3D()
		{
			Position = new Vector3(0.0f, 5.0f, 5.0f),
			Target = new Vector3(0.0f, 0.0f, 0.0f),
			Up = new Vector3(0.0f, 1.0f, 0.0f),
			FovY = 45.0f,
			Projection = CameraProjection.Perspective
		};

		Mesh cube = Raylib.GenMeshCube(1f, 1f, 1f);
		Model model = Raylib.LoadModelFromMesh(cube);

		Transform transform = new Transform()
		{
			Translation = Vector3.Zero,
			Rotation = Quaternion.Identity,
			Scale = Vector3.One
		};

		while (Raylib.WindowShouldClose() == false)
		{
			Raylib.UpdateCamera(ref camera, CameraMode.Free);

			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.Black);
			Raylib.BeginMode3D(camera);

			// model.Transform = RaylibGizmo.Matrix;
			// Raylib.DrawModel(model, Vector3.Zero, 1f, Color.White);
			// RaylibGizmo.DrawGizmo3D(ref model.Transform);
			Raylib.DrawGrid(10, 1);
			RaylibGizmo.DrawGizmo3D(ref transform);

			Raylib.EndMode3D();
			Raylib.EndDrawing();
		}

		// Raylib.UnloadModel(model);
		// Raylib.UnloadMesh(cube);
		Raylib.CloseWindow();
	}
}