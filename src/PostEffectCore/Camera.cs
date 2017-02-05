using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;

namespace PostEffectCore
{
	/// <summary>
	/// DirectX advanced camera.
	/// </summary>
	public class Camera : IScenePlugin.ICamera
	{
		private const float MIN_DIR_LEN = 0.001f;
		private const float MOVE_RATIO = 0.0025f;
		private const float ROTATION_RATIO = 0.01f;
		private const float ZOOM_RATIO = 0.005f;
		private const float ZOOM_MIN = 0.9f;
		private const float ZOOM_MAX = 1.2f;
		private const float ROTATION_EPSILON = 0.001f;

		/// <summary>
		/// Stores the DirectX device on which will be applied project and view matrices.
		/// </summary>
		private Device m_Device;

		/// <summary>
		/// Create a camera instance based on DirectX API.
		/// </summary>
		/// <param name="device">DirectX device instance.</param>
		/// <param name="position">Position of the camera.</param>
		/// <param name="target">Position of the target.</param>
		/// <param name="up">Up (head) vector of the camera.</param>
		public Camera(Device device, Vector3 position, Vector3 target, Vector3 up)
		{
			m_Device = device;

			m_Position = position;
			m_Target = target;
			m_Up = up;
			m_UpFree = false;

			m_Device.DeviceReset += new EventHandler(OnDeviceReset);
			OnDeviceReset(m_Device, new EventArgs());

			Update();
		}

		private void OnDeviceReset(object sender, EventArgs e)
		{
			// set default perspective to 4/3

			float width = (float)m_Device.PresentationParameters.BackBufferWidth;
			float height = (float)m_Device.PresentationParameters.BackBufferHeight;

			SetPerspective(60.0f, width / height, 1.0f, 20480.0f);
			UpdateParams();
			SetResetParam();
		}

		/// <summary>
		/// Get the DirectX device.
		/// </summary>
		public Device Device
		{
			get
			{
				return (m_Device);
			}
		}

		/// <summary>
		/// Get or set the position of the camera.
		/// </summary>
		public Vector3 Position
		{
			get
			{
				return (m_Position);
			}
			set
			{
				m_Position = value;
				UpdateParams();
			}
		}

		/// <summary>
		/// Get or set the target position.
		/// </summary>
		public Vector3 Target
		{
			get
			{
				return (m_Target);
			}
			set
			{
				m_Target = value;
				UpdateParams();
			}
		}

		/// <summary>
		/// Get the up (head) vector of the camera.
		/// </summary>
		public Vector3 Up
		{
			get
			{
				return (m_Up);
			}
		}

		/// <summary>
		/// Get or set up-free camera mode.
		/// </summary>
		public bool UpFree
		{
			get
			{
				return (m_UpFree);
			}
			set
			{
				m_UpFree = value;
			}
		}

		/// <summary>
		/// Get the distance between camera position and target position (unitless).
		/// </summary>
		public float Distance
		{
			get
			{
				return (m_LookDirLen);
			}
		}

		/// <summary>
		/// Get or set the angle of the field of view, in degrees.
		/// </summary>
		public float FieldOfView
		{
			get
			{
				return (m_FovDegrees);
			}
			set
			{
				m_FovDegrees = value;
				m_FovRadians = (m_FovDegrees * (float)Math.PI) / 180.0f;
			}
		}

		/// <summary>
		/// Get the normalized view vector.
		/// </summary>
		public Vector3 ViewVector
		{
			get
			{
				return (m_LookDir);
			}
		}

		/// <summary>
		/// Get the normalized right vector.
		/// </summary>
		public Vector3 RightVector
		{
			get
			{
				return (m_LookRight);
			}
		}

		/// <summary>
		/// Get the normalized up vector.
		/// </summary>
		public Vector3 UpVector
		{
			get
			{
				return (m_LookUp);
			}
		}

		/// <summary>
		/// Get or set the aspect ratio (unitless coeficient).
		/// </summary>
		public float AspectRatio
		{
			get
			{
				return (m_AspectRatio);
			}
			set
			{
				m_AspectRatio = value;
			}
		}

		/// <summary>
		/// Get or set the near plan distance (unitless).
		/// </summary>
		public float Near
		{
			get
			{
				return (m_Near);
			}
			set
			{
				m_Near = value;
			}
		}

		/// <summary>
		/// Get or set the far plan distance (unitless).
		/// </summary>
		public float Far
		{
			get
			{
				return (m_Far);
			}
			set
			{
				m_Far = value;
			}
		}

		public void SetMouseDownLocation(int x, int y)
		{
			m_MousePrevPositionX = x;
			m_MousePrevPositionY = y;
		}

		/// <summary>
		/// Set perspective parameters.
		/// </summary>
		/// <param name="fov">Angle in degrees.</param>
		/// <param name="winWidth">Width of the of viewport window.</param>
		/// <param name="winHeight">Height of the of viewport window.</param>
		/// <param name="near">Near plan.</param>
		/// <param name="far">Far plan.</param>
		public void SetPerspective(float fov, float aspectRatio, float near, float far)
		{
			FieldOfView = fov;
			m_Near = near;
			m_Far = far;
			m_AspectRatio = aspectRatio;
		}

		/// <summary>
		/// Reset the camera parameters as given to the constructor.
		/// </summary>
		public void Reset()
		{
			m_Position = m_ResetPosition;
			m_Target = m_ResetTarget;
			m_Up = m_ResetUp;
			UpdateParams();
		}

		/// <summary>
		/// Move the camera in the given direction.
		/// </summary>
		/// <param name="direction">Direction vector to move the camera. Intensity of the vector is taken into account.</param>
		public void Move(Vector3 direction)
		{
			m_Position += direction;
			m_Target += direction;
			
			UpdateParams();
		}

		/// <summary>
		/// Rotate the camera around the target point.
		/// </summary>
		/// <param name="ry">Rotation angle around the Y axis, in radians.</param>
		/// <param name="rx">Rotation angle around the X axis, in radians.</param>
		public void Rotate(float ry, float rx)
		{
			Quaternion q = Quaternion.Identity;

			float f = (float)Math.Acos(Vector3.Dot(m_LookDir, m_Up));
			if (rx > 0.0f)
				f = Math.Min(rx, f - ROTATION_EPSILON);
			else
				f = Math.Max(rx, f - (float)Math.PI + ROTATION_EPSILON);

			q.RotateAxis(m_LookRight, f);
			m_LookDir = MultiplyVectorQuaternion(m_LookDir, q);

			q.RotateAxis(m_Up, ry);
			m_LookDir = MultiplyVectorQuaternion(m_LookDir, q);

			m_Position = m_Target - m_LookDir * m_LookDirLen;

			UpdateParams();
		}

		/// <summary>
		/// Zoom in or out.
		/// </summary>
		/// <remarks>This moves the camera forward/backward along the view vector, so camera position is modified when performing a zoom.</remarks>
		/// <param name="zoom">Moving coeficient relative to the distance between camera position and target (unitless).</param>
		public void Zoom(float zoom)
		{
			float len = m_LookDirLen * zoom;
			len = Math.Max(MIN_DIR_LEN, len);
			m_Position = m_Target - m_LookDir * len;

			UpdateParams();
		}

		/// <summary>
		/// Updates the camera attributes.
		/// </summary>
		public void Update()
		{
			Update(null);
		}


		private bool[] m_MouseDown = new bool[3];


		/// <summary>
		/// Updates the camera attributes.
		/// </summary>
		/// <param name="e">The mouse events to update the camera. It can be set to null.</param>
		public void Update(MouseEventArgs e)
		{
			if (e != null)
			{
				// check mouse status
				// (avoid mouse misplacement bug whith model dialogs)
				MouseCheck(e);

				// compute mouse movement delta
				float mousedx = (float)e.X - (float)m_MousePrevPositionX;
				float mousedy = (float)e.Y - (float)m_MousePrevPositionY;

				// translate camera
				if (e.Button == MouseButtons.Left)
				{
					float dx = ROTATION_RATIO * mousedx;
					float dy = ROTATION_RATIO * mousedy;
					Rotate(-dx, -dy);
				}

				// rotate camera
				if (e.Button == MouseButtons.Middle)
				{
					Vector3 vec = m_LookRight * -mousedx + m_LookUp * mousedy;
					Move(vec * m_LookDirLen * MOVE_RATIO);
				}

				// zoom
				if (e.Button == MouseButtons.Right)
				{
					float z = 1.0f + mousedy * ZOOM_RATIO;
					z = Math.Max(ZOOM_MIN, Math.Min(z, ZOOM_MAX));
					Zoom(z);
				}

				// save the current position to use it again on next frame
				m_MousePrevPositionX = e.X;
				m_MousePrevPositionY = e.Y;
			}
			else
				UpdateParams();

			// set projection matrix
			m_Device.Transform.Projection = Matrix.PerspectiveFovRH(m_FovRadians, m_AspectRatio, m_Near, m_Far);

			// set view matrix
			m_Device.Transform.View = Matrix.LookAtRH(m_Position, m_Target, m_Up);
		}

		/// <summary>
		/// Check mouse status to avoid camera misplacement when mouse moves while a model window disappears.
		/// </summary>
		/// <param name="e">Mouse event information.</param>
		private void MouseCheck(MouseEventArgs e)
		{
			MouseCheckElement(e, MouseButtons.Left, 0);
			MouseCheckElement(e, MouseButtons.Middle, 1);
			MouseCheckElement(e, MouseButtons.Right, 2);
		}

		/// <summary>
		/// Check a specific mouse button and stores the proper button status.
		/// </summary>
		/// <param name="e">Mouse event information.</param>
		/// <param name="btn">Specific button to check.</param>
		/// <param name="index">Index of the button (for internal use).</param>
		private void MouseCheckElement(MouseEventArgs e, MouseButtons btn, int index)
		{
			if (e.Button == btn)
			{
				if (m_MouseDown[index] == false)
				{
					SetMouseDownLocation(e.X, e.Y);
					m_MouseDown[index] = true;
				}
			}
			else
				m_MouseDown[index] = false;
		}

		/// <summary>
		/// Recompute the view, right and up vectors.
		/// </summary>
		private void UpdateParams()
		{
			m_LookDir = m_Target - m_Position;
			m_LookDirLen = m_LookDir.Length();
			m_LookRight = Vector3.Cross(m_LookDir, m_Up);
			m_LookUp = Vector3.Cross(m_LookRight, m_LookDir);
			m_LookDir.Normalize();
			m_LookRight.Normalize();
			m_LookUp.Normalize();
			if (m_UpFree)
				m_Up = m_LookUp;
		}

		/// <summary>
		/// Saves the default camera parameters. They can be restored calling the 'Reset' method.
		/// </summary>
		private void SetResetParam()
		{
			m_ResetPosition = m_Position;
			m_ResetTarget = m_Target;
			m_ResetUp = m_Up;
		}

		/// <summary>
		/// Multiply a vector by a quaternion (code from nVidia SDK).
		/// </summary>
		/// <param name="vector">Vector (vertex) to multiply.</param>
		/// <param name="rotation">Quaternion to use for the multiplication.</param>
		/// <returns></returns>
		private Vector3 MultiplyVectorQuaternion(Vector3 vector, Quaternion rotation)
		{
			// nVidia SDK implementation

			Vector3 uv;
			Vector3 uuv;
			Vector3 qvec = new Vector3(rotation.X, rotation.Y, rotation.Z);
			uv = Vector3.Cross(qvec, vector);
			uuv = Vector3.Cross(qvec, uv);
			uv *= (2.0f * rotation.W);
			uuv *= 2.0f;
			return (vector + uv + uuv);
		}


		/// <summary>
		/// Camera position.
		/// </summary>
		private Vector3 m_Position;
		/// <summary>
		/// Camera target position.
		/// </summary>
		private Vector3 m_Target;
		/// <summary>
		/// Camera up vector.
		/// </summary>
		private Vector3 m_Up;

		/// <summary>
		/// Normalized view vector.
		/// </summary>
		private Vector3 m_LookDir;
		/// <summary>
		/// Normalized right vector.
		/// </summary>
		private Vector3 m_LookRight;
		/// <summary>
		/// Distance between camera and target (unitless).
		/// </summary>
		private float m_LookDirLen;
		/// <summary>
		/// Normalized up vector for 'up-free' camera mode.
		/// </summary>
		private Vector3 m_LookUp;

		/// <summary>
		/// Stores the default camera position, set at constructor.
		/// </summary>
		private Vector3 m_ResetPosition;
		/// <summary>
		/// Stores the default camera target position, set at constructor.
		/// </summary>
		private Vector3 m_ResetTarget;
		/// <summary>
		/// Stores the default camera up vector, set at constructor.
		/// </summary>
		private Vector3 m_ResetUp;

		/// <summary>
		/// Angle of the field of view of the camera, in degrees.
		/// </summary>
		private float m_FovDegrees;
		/// <summary>
		/// Angle of the field of view of the camera, in radians.
		/// </summary>
		private float m_FovRadians;
		/// <summary>
		/// Distance of the near plan (unitless).
		/// </summary>
		private float m_Near;
		/// <summary>
		/// Distance of the far plan (unitless).
		/// </summary>
		private float m_Far;
		/// <summary>
		/// Perspective aspect ratio. (unitless, default is set to 4/3).
		/// </summary>
		private float m_AspectRatio;

		/// <summary>
		/// Flag telling if camera is in 'up-free' mode or not. (default is set to false).
		/// </summary>
		private bool m_UpFree;

		/// <summary>
		/// Stores the previous mouse X position in pixels, for camera movement computation.
		/// </summary>
		private int m_MousePrevPositionX;
		/// <summary>
		/// Stores the previous mouse Y position in pixels, for camera movement computation.
		/// </summary>
		private int m_MousePrevPositionY;
	}
}
