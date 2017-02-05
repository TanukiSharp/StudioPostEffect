using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;

namespace IScenePlugin
{
	public abstract class Scene
	{
		/// <summary>
		/// When overridden, gets the name of the plugin.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// When overridden, initializes the plugin.
		/// </summary>
		/// <param name="device">Instance of the DirectX device.</param>
		/// <param name="camera">The default, controllable camera.</param>
		/// <param name="prms">The default, modifiable, plugin parameters.</param>
		public abstract void Initialize(ScenePluginInitParams prms);
		/// <summary>
		/// When overridden, terminates the plugin and releases the resources.
		/// </summary>
		public virtual void Terminate() { }

		/// <summary>
		/// When overridden, called by the application after the DirectX device got reset.
		/// </summary>
		/// <param name="device">Instance of the DirectX device.</param>
		public abstract void OnDeviceReset(Device device);

		/// <summary>
		/// When overridden, called by the application when the user drag and drops some files.
		/// </summary>
		/// <param name="x">X coordinate where the files were dropped.</param>
		/// <param name="y">Y coordinate where the files were dropped.</param>
		/// <param name="files">Dropped files dropped (full path and filenames).</param>
		public virtual void OnFilesDropped(int x, int y, string[] files) { }

		/// <summary>
		/// When overridden, called by the application when a MouseDown event is fired.
		/// </summary>
		/// <param name="e">Event related data.</param>
		public virtual void OnMouseDown(MouseEventArgs e) { }
		/// <summary>
		/// When overridden, called by the application when a MouseUp event is fired.
		/// </summary>
		/// <param name="e">Event related data.</param>
		public virtual void OnMouseUp(MouseEventArgs e) { }
		/// <summary>
		/// When overridden, called by the application when a MouseWheel event is fired.
		/// </summary>
		/// <param name="e">Event related data.</param>
		public virtual void OnMouseWheel(MouseEventArgs e) { }
		/// <summary>
		/// When overridden, called by the application when a MouseMove event is fired.
		/// </summary>
		/// <param name="e">Event related data.</param>
		public virtual void OnMouseMove(MouseEventArgs e) { }

		/// <summary>
		/// When overridden, called by the application when the user click the Plugin configuration menu item.
		/// </summary>
		public virtual void ConfigPanelOpen() { }

		/// <summary>
		/// When overridden, called by the application when it's time for the implementer to update the scene elements.
		/// </summary>
		/// <param name="elapsedTime">Time elapsed since last frame render, in milliseconds.</param>
		public abstract void UpdateScene(double elapsedTime);
		/// <summary>
		/// When overridden, called by the application when it's time for the implementer to render the scene.
		/// </summary>
		/// <param name="device"></param>
		public abstract void RenderScene(Device device);
	}
}
