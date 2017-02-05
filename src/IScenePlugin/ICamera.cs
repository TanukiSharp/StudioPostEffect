using System;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace IScenePlugin
{
	/// <summary>
	/// Controllable orbital camera.
	/// </summary>
	public interface ICamera
	{
		/// <summary>
		/// Gets or sets the render aspect ratio.
		/// </summary>
		float AspectRatio { get; set; }
		/// <summary>
		/// Gets the DirectX device instance.
		/// </summary>
		Device Device { get; }
		/// <summary>
		/// Gets the distance between camera location and target location.
		/// </summary>
		float Distance { get; }
		/// <summary>
		/// Gets or sets the far plan distance.
		/// </summary>
		float Far { get; set; }
		/// <summary>
		/// Gets or sets the field of view angle, in degrees.
		/// </summary>
		float FieldOfView { get; set; }
		/// <summary>
		/// Moves (travels) the camera.
		/// </summary>
		/// <param name="direction">Directional vector, representing the movement offset to apply to the current camera location.</param>
		void Move(Vector3 direction);
		/// <summary>
		/// Gets or sets the near plan distance.
		/// </summary>
		float Near { get; set; }
		/// <summary>
		/// Gets or sets the camera location.
		/// </summary>
		Vector3 Position { get; set; }
		/// <summary>
		/// Resets the camera default parameters.
		/// </summary>
		void Reset();
		/// <summary>
		/// Gets the camera right vector (vector perpendicular to view and up vectors).
		/// </summary>
		Vector3 RightVector { get; }
		/// <summary>
		/// Rotates the camera around the target.
		/// </summary>
		/// <param name="ry">Rotating movement offset around the Y axis (horizontal move).</param>
		/// <param name="rx">Rotating movement offset around the X axis (vertical move).</param>
		void Rotate(float ry, float rx);
		/// <summary>
		/// Gets or sets the target location.
		/// </summary>
		Vector3 Target { get; set; }
		/// <summary>
		/// Gets the head vector.
		/// </summary>
		Vector3 Up { get; }
		/// <summary>
		/// Updates the camera.
		/// </summary>
		void Update();
		/// <summary>
		/// Updates the camera.
		/// </summary>
		/// <param name="e">Mouse related data to determine new camera parameters.</param>
		void Update(MouseEventArgs e);
		/// <summary>
		/// Gets of sets the camera head behavior.
		/// </summary>
		bool UpFree { get; set; }
		/// <summary>
		/// Gets the up normal vector.
		/// </summary>
		Vector3 UpVector { get; }
		/// <summary>
		/// Gets the view normal vector (targeting vector).
		/// </summary>
		Vector3 ViewVector { get; }
		/// <summary>
		/// Zooms the view in or out.
		/// </summary>
		/// <param name="zoom">Zoom offset to apply to the current zoom.</param>
		void Zoom(float zoom);
	}
}
