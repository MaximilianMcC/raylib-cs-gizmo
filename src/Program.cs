using System.Numerics;
using Raylib_cs;

class Program
{
	public static void Main(string[] args)
	{
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		Raylib.InitWindow(800, 800, "raylib-cs gizmo example");
		Raylib.SetExitKey(KeyboardKey.Null);

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
			Rotation = Quaternion.CreateFromYawPitchRoll(45, 0, 0),
			Scale = Vector3.One
		};

		bool local = false;

		Raylib.DisableCursor();
		bool useCamera = true;

		while (Raylib.WindowShouldClose() == false)
		{
			if (useCamera) Raylib.UpdateCamera(ref camera, CameraMode.Free);

			if (Raylib.IsKeyPressed(KeyboardKey.Space)) local = !local;
			if (Raylib.IsKeyPressed(KeyboardKey.Escape))
			{
				useCamera = !useCamera;

				if (useCamera) Raylib.DisableCursor();
				else Raylib.EnableCursor();
			}

			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.Black);
			Raylib.BeginMode3D(camera);

			// model.Transform = RaylibGizmo.Matrix;
			// Raylib.DrawModel(model, Vector3.Zero, 1f, Color.White);
			// RaylibGizmo.DrawGizmo3D(ref model.Transform);

			Raylib.DrawGrid(10, 1);
			// RaylibGizmo.DrawGizmo3D(ref transform);
			RaylibGizmo.DrawGizmo3DPro(ref transform, local);

			Raylib.DrawText("Press space to toggle between local and global", 10, 10, 16, Color.White);

			Raylib.EndMode3D();
			Raylib.EndDrawing();
		}

		// Raylib.UnloadModel(model);
		// Raylib.UnloadMesh(cube);
		Raylib.CloseWindow();
	}
}