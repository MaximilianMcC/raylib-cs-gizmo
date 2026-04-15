using System.Numerics;
using Raylib_cs;

class RaylibGizmo
{
	public static Color XColor { get; set; } = Color.Red;
	public static Color YColor { get; set; } = Color.Blue;
	public static Color ZColor { get; set; } = Color.Yellow;
	public static Color CenterColor { get; set; } = Color.RayWhite;

	// Changes how large the thing is rendered visually or whatever
	public static float DrawScale { get; set; } = 1f;
	private static float centerRadius => DrawScale * 0.03f;
	private static float axisThickness => DrawScale * 0.01f;
	private static float axisLength => DrawScale * 0.5f;
	private static float coneLength => DrawScale * 0.1f;
	private static float coneBaseThickness => axisThickness * 2f;

	public static bool HasPermissionToChangeCursor { get; set; } = true;

	public static void SetColors(Color x, Color y, Color z, Color center)
	{
		XColor = x;
		YColor = y;
		ZColor = z;
		CenterColor = center;
	}

	// Draws and updates the gizmo
	// TODO: Put update logic somewhere else maybe
	public static void DrawGizmo3D(ref Transform transform)
	{
		// Draw the little centre thingy
		Raylib.DrawSphereEx(transform.Translation, centerRadius, 6, 6, CenterColor);

		// Draw the axes globally
		DrawAxisGlobal(transform, Vector3.UnitX, XColor);
		DrawAxisGlobal(transform, Vector3.UnitY, YColor);
		DrawAxisGlobal(transform, Vector3.UnitZ, ZColor);
	}

	public static void DrawGizmo3DPro(ref Transform transform, bool local)
	{
		// Draw the little centre thingy
		Raylib.DrawSphereEx(transform.Translation, centerRadius, 6, 6, CenterColor);

		if (local)
		{
			DrawAxisLocal(transform, Vector3.UnitX, XColor);
			DrawAxisLocal(transform, Vector3.UnitY, YColor);
			DrawAxisLocal(transform, Vector3.UnitZ, ZColor);
		}
		else
		{
			DrawAxisGlobal(transform, Vector3.UnitX, XColor);
			DrawAxisGlobal(transform, Vector3.UnitY, YColor);
			DrawAxisGlobal(transform, Vector3.UnitZ, ZColor);
		}

		CheckForAxisCollision(transform, Vector3.UnitX);
	}

	// TODO: Don't just copy paste all this for local/global
	private static void DrawAxisGlobal(Transform transform, Vector3 axis, Color color)
	{
		// Make a copy of the transform so we can modify it temporarily
		Transform start = transform;
		Transform end = transform;

		// Add a bit of padding from the centre
		start.TranslateGlobal(axis * (centerRadius * 2f));

		// Draw the tube thing
		end.TranslateGlobal(axis * axisLength);
		Raylib.DrawCylinderEx(start.Translation, end.Translation, axisThickness, axisThickness, 8, color);

		// Draw the cone
		Transform coneStart = end;
		end.TranslateGlobal(axis * coneLength);
		Raylib.DrawCylinderEx(coneStart.Translation, end.Translation, coneBaseThickness, 0f, 8, color);
	}

	// TODO: Don't just copy paste all this for local/global
	private static void DrawAxisLocal(Transform transform, Vector3 axis, Color color)
	{
		// Make a copy of the transform so we can modify it temporarily
		Transform start = transform;
		Transform end = transform;

		// Add a bit of padding from the centre
		start.TranslateLocal(axis * (centerRadius * 2f));

		// Draw the tube thing
		end.TranslateLocal(axis * axisLength);
		Raylib.DrawCylinderEx(start.Translation, end.Translation, axisThickness, axisThickness, 8, color);

		// Draw the cone
		Transform coneStart = end;
		end.TranslateLocal(axis * coneLength);
		Raylib.DrawCylinderEx(coneStart.Translation, end.Translation, coneBaseThickness, 0f, 8, color);
	}

	//! This will NOT work for local/rotated ones. Maybe use like collisions instead idk
	private static void CheckForAxisCollision(Transform transform, Vector3 axis)
	{
		// Make a hitbox that's roughly the size of the axis
		float boxSize = coneBaseThickness * 2f;

		Vector3 start = transform.Translation + axis * (centerRadius * 2f);
		Vector3 end = transform.Translation + axis * ((centerRadius * 2f) + axisLength + coneLength);
		Vector3 thickness = new Vector3(boxSize);

		BoundingBox hitbox = new BoundingBox(
			Vector3.Min(start, end) - thickness,
			Vector3.Max(start, end) + thickness
		);

		//! debug draw
		Raylib.DrawCubeWiresV(hitbox.Min, hitbox.Max, Color.Magenta);

		// Raycast to check for if it's being clicked on
	}

	// TODO: DO this
	class GizmoAxis
	{
		//?	CheckForCollision()
		//?	CheckForDragging()
		//? DrawGlobal()
		//? DrawLocal()
	}
}