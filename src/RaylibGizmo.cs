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

	private static bool draggingRn = false;
	private static Vector3 dragStartPosition;
	private static Vector3 gizmoStartPosition;

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

	public static void UpdateGizmoTranslationGlobal(ref Transform transform, Vector3 axis, Camera3D camera)
	{
		// Check for if we're hovering over the hitbox
		BoundingBox hitbox = GenerateAxisHitbox(transform, axis);

		// Raycast to check if the mouse is over the axis
		Ray ray = Raylib.GetScreenToWorldRay(Raylib.GetMousePosition(), camera);
		RayCollision collision = Raylib.GetRayCollisionBox(ray, hitbox);

		// Get the plane normal facing the camera
		Vector3 plane = camera.Target - camera.Position;
		plane = Vector3.Normalize(plane);

		// Check for if we are beginning a drag
		if (collision.Hit && Raylib.IsMouseButtonPressed(MouseButton.Left))
		{
			draggingRn = true;

			// Save our start position
			dragStartPosition = Get3DPositionOnPlane(ray, transform.Translation, plane);
			gizmoStartPosition = transform.Translation;
		}

		// Check for if we are gonna stop dragging
		if (draggingRn && Raylib.IsMouseButtonReleased(MouseButton.Left))
		{
			draggingRn = false;
			return;
		}

		// Check for if we are currently dragging
		if (draggingRn)
		{
			// Check for how much we've moved
			Vector3 currentPointOnPlane = Get3DPositionOnPlane(ray, transform.Translation, plane);
			Vector3 amountDragged = currentPointOnPlane - dragStartPosition;

			// Project it onto the plane so we
			// just move on the single axis
			float amount = Vector3.Dot(amountDragged, axis);
			Vector3 movement = axis * amount;

			// Move the gizmo
			transform.Translation = gizmoStartPosition + movement;
		}
	}

	//? I do not understand any of this
	private static Vector3 Get3DPositionOnPlane(Ray ray, Vector3 planeOrigin, Vector3 normal)
	{
		float difference = Vector3.Dot(normal, ray.Direction);
		if (MathF.Abs(difference) < 0.00001f) return normal;

		float distanceAlongRay = Vector3.Dot(planeOrigin - ray.Position, normal) / difference;
		return ray.Position + ray.Direction * distanceAlongRay;
	}

	//! This will NOT work for local/rotated ones. Maybe use like collisions instead idk
	private static BoundingBox GenerateAxisHitbox(Transform transform, Vector3 axis)
	{
		// Make a hitbox that's roughly the size of the axis
		float boxSize = coneBaseThickness * 2f;
		float boxLength = axisLength + coneLength;

		// 'Extend' the box on the axis by the length
		Vector3 end = new Vector3(boxSize) + (axis * boxLength);
		Vector3 start = transform.Translation;

		// TODO: Make the box in the centre of the thing (x / 2f)
		BoundingBox hitbox = new BoundingBox(
			Vector3.Min(start, end),
			Vector3.Max(start, end)
		);

		//! debug draw
		Raylib.DrawBoundingBox(hitbox, Color.Magenta);

		return hitbox;
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