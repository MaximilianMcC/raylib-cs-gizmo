using System.Numerics;
using Raylib_cs;

class RaylibGizmo
{
	public static Color XColor { get; set; } = Color.Yellow;
	public static Color YColor { get; set; } = Color.Blue;
	public static Color ZColor { get; set; } = Color.Red;
	public static Color CenterColor { get; set; } = Color.RayWhite;

	// Changes how large the thing is rendered visually or whatever
	public static float DrawScale { get; set; } = 1f;
	private static float centerRadius => DrawScale * 0.03f;
	private static float axisThickness => DrawScale * 0.01f;
	private static float axisLength => DrawScale * 0.5f;
	private static float coneLength => DrawScale * 0.1f;
	private static float coneBaseThickness => axisThickness * 2f;

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

		// Draw the axes
		DrawAxis(transform, Vector3.UnitX, XColor);
		DrawAxis(transform, Vector3.UnitY, YColor);
		DrawAxis(transform, Vector3.UnitZ, ZColor);
	}

	private static void DrawAxis(Transform transform, Vector3 axis, Color color)
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
}