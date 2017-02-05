using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using System.Xml;
using System.IO;
using Microsoft.DirectX.Direct3D;
using System.Diagnostics;

namespace ModelScenePlugin
{
	/// <summary>
	/// ModelScene plugin main settings form (SceneGraph window).
	/// </summary>
	public partial class frmSceneGraph : Form
	{
		/// <summary>
		/// DirectX device instance.
		/// </summary>
		private Device m_Device;
		/// <summary>
		/// Default plugin scenes path.
		/// </summary>
		private string m_ScenesPath;
		/// <summary>
		/// ModelScene core plugin instance.
		/// </summary>
		private ModelScene m_ModelScene;
		/// <summary>
		/// Flag indicating if the scene has been modified by the user.
		/// </summary>
		private bool m_SceneChanged;

		/// <summary>
		/// Currently loaded scene filename.
		/// </summary>
		private string m_SceneFilename;
		/// <summary>
		/// FileSystemWatcher that monitor the loaded scene file to automatically reload it in case it changed outise of the application/plugin.
		/// </summary>
		private FileSystemWatcher m_Watcher;
		/// <summary>
		/// FileSystemWatcher switch trick to avoid double load.
		/// </summary>
		private bool m_FileSystemWatcherDoubleCallSafetySwitch = false;
		/// <summary>
		/// Root node of the currently loaded scene.
		/// </summary>
		private SceneNode m_RootSceneNode;
		/// <summary>
		/// For internal use only.
		/// To set to true when plugin is terminated.
		/// </summary>
		private bool m_ForceClose = false;

		/// <summary>
		/// Instanciate the SceneGraph window.
		/// </summary>
		/// <param name="device">DirectX device instance.</param>
		/// <param name="modelScene">ModelScene core plugin instance.</param>
		/// <param name="scenesPath">Default plugin scenes path.</param>
		public frmSceneGraph(Device device, ModelScene modelScene, string scenesPath)
		{
			InitializeComponent();

			m_Device = device;
			m_ScenesPath = scenesPath;
			m_ModelScene = modelScene;

			// initializes the NumericUpDown controls min and max values
			numTransX.Minimum = decimal.MinValue;
			numTransX.Maximum = decimal.MaxValue;
			numTransY.Minimum = decimal.MinValue;
			numTransY.Maximum = decimal.MaxValue;
			numTransZ.Minimum = decimal.MinValue;
			numTransZ.Maximum = decimal.MaxValue;

			numScaleX.Minimum = decimal.MinValue;
			numScaleX.Maximum = decimal.MaxValue;
			numScaleY.Minimum = decimal.MinValue;
			numScaleY.Maximum = decimal.MaxValue;
			numScaleZ.Minimum = decimal.MinValue;
			numScaleZ.Maximum = decimal.MaxValue;

			// checks for default scene files, and create a list accordingly
			CheckAndDisplaySceneFiles();
		}

		/// <summary>
		/// Checks for default scene files, and create a selectable list on the UI.
		/// </summary>
		private void CheckAndDisplaySceneFiles()
		{
			try
			{
				// look for all the xml files in the sub-directories
				string[] files = Directory.GetFiles(m_ScenesPath, "*.xml", SearchOption.AllDirectories);
				XmlDocument doc = new XmlDocument();

				lstDefaultScenes.Items.Clear();
				foreach (string file in files)
				{
					doc.Load(file);
					// ensure these xml files are scene definition files
					if (doc.DocumentElement.Name == "ModelScene")
						lstDefaultScenes.Items.Add(new ListViewItem(new string[] { Path.GetFileNameWithoutExtension(file), file }));
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// Loads a XML scene file.
		/// </summary>
		/// <param name="sceneFilename">Filename of the XML scene definition file.</param>
		public void LoadSceneFile(string sceneFilename)
		{
			// ensure filename is properly set
			if (sceneFilename == null)
				return;

			m_SceneFilename = sceneFilename;

			// prepare the FileSystemWatcher to monitor the scene file
			m_Watcher = new FileSystemWatcher(Path.GetDirectoryName(m_SceneFilename), Path.GetFileName(m_SceneFilename));
			m_Watcher.EnableRaisingEvents = true;
			m_Watcher.IncludeSubdirectories = false;
			m_Watcher.Changed += new FileSystemEventHandler(OnSceneFileChanged);

			// loads the XML scene file
			XmlDocument doc = new XmlDocument();
			doc.Load(m_SceneFilename);
			XmlNodeList sceneNodes = doc.DocumentElement.SelectNodes("Node");

			// get the base path of the XML scene file
			string path = Path.GetDirectoryName(m_SceneFilename);

			// starts a 'garbage collecting' session (see comments of the ResourceManager class for more information)
			ResourceManager.StartSession();

			// if there is only one child node at the root of the scene definition, it is used as the root node
			if (sceneNodes.Count == 1)
				m_RootSceneNode = new SceneNode(m_Device, sceneNodes[0]);
			else if (sceneNodes.Count > 1)
			{
				// if there are several child nodes at the root of the scene definition, they are all included in an automatically created root node
				m_RootSceneNode = new SceneNode(m_Device, path);
				foreach (XmlNode sceneNode in sceneNodes)
					m_RootSceneNode.Nodes.Add(new SceneNode(m_Device, sceneNode));
			}

			// ends the 'garbage collecting' session (see comments of the ResourceManager class for more information)
			ResourceManager.EndSession();

			// tells the core ModeScene instance about the root scene node
			m_ModelScene.SetRootNode(m_RootSceneNode);

			// set the UI TreeView with the root node
			trvSceneGraph.SuspendLayout();
			trvSceneGraph.Nodes.Clear();
			trvSceneGraph.Nodes.Add(m_RootSceneNode);
			trvSceneGraph.SelectedNode = m_RootSceneNode;
			trvSceneGraph.ExpandAll();
			trvSceneGraph.ResumeLayout();

			// allow scene modifications saving
			btnSaveScene.Enabled = true;

			// scene is just loaded, so makes sure it is marked as not yet modified
			SceneChanged = false;
		}

		/// <summary>
		/// Loads a DirectX mesh file.
		/// </summary>
		/// <param name="modelFilename">DirectX model filename (.x file).</param>
		public void LoadModelFile(string modelFilename)
		{
			// ensure filename is properly set
			if (modelFilename == null)
				return;

			// prepare the FileSystemWatcher to monitor the model file
			m_Watcher = new FileSystemWatcher(Path.GetDirectoryName(modelFilename), Path.GetFileName(modelFilename));
			m_Watcher.EnableRaisingEvents = true;
			m_Watcher.IncludeSubdirectories = false;
			m_Watcher.Changed += new FileSystemEventHandler(OnModelFileChanged);

			// starts a 'garbage collecting' session (see comments of the ResourceManager class for more information)
			ResourceManager.StartSession();
			// creates the scene root node directly giving it a model instance
			m_RootSceneNode = new SceneNode(m_Device, new Model(m_Device, modelFilename));
			// ends the 'garbage collecting' session (see comments of the ResourceManager class for more information)
			ResourceManager.EndSession();

			// tells the core ModeScene instance about the root scene node
			m_ModelScene.SetRootNode(m_RootSceneNode);

			// set the UI TreeView with the root node
			trvSceneGraph.SuspendLayout();
			trvSceneGraph.Nodes.Clear();
			trvSceneGraph.Nodes.Add(m_RootSceneNode);
			trvSceneGraph.SelectedNode = m_RootSceneNode;
			trvSceneGraph.ExpandAll();
			trvSceneGraph.ResumeLayout();

			// disable scene modifications saving (model checking is just for fast review)
			btnSaveScene.Enabled = false;

			// makes sure the UI does not display the 'modified' mark as long as model scene can't be modified
			SceneChanged = false;
		}

		/// <summary>
		/// Raised by the system when the scene file is modified outside the application/plugin.
		/// </summary>
		/// <param name="sender">FileSystemWatcher that raised the event.</param>
		/// <param name="e">FileSystem event.</param>
		private void OnSceneFileChanged(object sender, FileSystemEventArgs e)
		{
			// trick to avoid loading twice while the FileSystemWatcher always calls this event twice
			if (m_FileSystemWatcherDoubleCallSafetySwitch)
				// cross-threading safe call
				this.Invoke((MethodInvoker)delegate() { LoadSceneFile(e.FullPath); });
			m_FileSystemWatcherDoubleCallSafetySwitch = !m_FileSystemWatcherDoubleCallSafetySwitch;
		}

		/// <summary>
		/// Raised by the system when the model file is modified outside the application/plugin.
		/// </summary>
		/// <param name="sender">FileSystemWatcher that raised the event.</param>
		/// <param name="e">FileSystem event.</param>
		private void OnModelFileChanged(object sender, FileSystemEventArgs e)
		{
			// trick to avoid loading twice while the FileSystemWatcher always calls this event twice
			if (m_FileSystemWatcherDoubleCallSafetySwitch)
				// cross-threading safe call
				this.Invoke((MethodInvoker)delegate() { LoadModelFile(e.FullPath); });
			m_FileSystemWatcherDoubleCallSafetySwitch = !m_FileSystemWatcherDoubleCallSafetySwitch;
		}

		/// <summary>
		/// Gets or sets the flag indicating the scene is modified or not.
		/// </summary>
		public bool SceneChanged
		{
			get
			{
				return (m_SceneChanged);
			}
			set
			{
				if (m_SceneFilename == null)
					return;
				m_SceneChanged = value;
				// automatically updates the UI
				// (a small star symbol in the window title is displayed when modified)
				UpdateWindowTitle();
			}
		}

		/// <summary>
		/// Updates the window title according to the scene modification flag.
		/// </summary>
		private void UpdateWindowTitle()
		{
			// set name to display in the window title bar
			string name = "";
			if (m_SceneFilename != null)
				name = Path.GetFileName(m_SceneFilename);

			// construct the final text to display according to several conditions
			this.Text = string.Format("Model Scene{0}{1}{2}",
				name.Length > 0 ? " - " : "", name, m_SceneChanged ? " *" : "");
		}

		/// <summary>
		/// Retrieves the scene node selected from the UI.
		/// </summary>
		/// <returns>Returns the selected node instance or null if there is no node selected.</returns>
		private SceneNode GetSelectedNode()
		{
			return (trvSceneGraph.SelectedNode as SceneNode);
		}

		/// <summary>
		/// Retrieves the primitive transform selected from the UI.
		/// </summary>
		/// <returns>Returns the selected primitive transform or null if there is no transform item selected.</returns>
		private PrimitiveTransform GetNodeTransform()
		{
			// check for scene node
			SceneNode node = GetSelectedNode();
			if (node == null)
				return (null);

			// check for transform node selection
			if (lstTransforms.SelectedItems.Count == 0)
				return (null);

			// return the selected transform node
			return (lstTransforms.SelectedItems[0] as PrimitiveTransform);
		}


		#region Transformation UI events

		/// <summary>
		/// Called by the system when the value of a NumericUpDown control related to translation is changed.
		/// </summary>
		/// <param name="sender">NumericUpDown control which raised the event.</param>
		/// <param name="e">Base .NET event.</param>
		private void numTranslation_ValueChanged(object sender, EventArgs e)
		{
			// get selected primitive transform
			PrimitiveTransform transform = GetNodeTransform();
			if (transform == null)
				return;

			// update transform vectorial value
			transform.VectorTransform = new Vector3((float)numTransX.Value, (float)numTransY.Value, (float)numTransZ.Value);
			// update the transform matrix of the selected scene node
			GetSelectedNode().UpdateTransform();

			// scene has changed!
			SceneChanged = true;
		}

		/// <summary>
		/// Called by the system when the TrackBar control for rotations is scrolled.
		/// </summary>
		/// <param name="sender">TrackBar control which raised the event.</param>
		/// <param name="e">Base .NET event.</param>
		private void trkRotate_Scroll(object sender, EventArgs e)
		{
			// get selected primitive transform
			PrimitiveTransform transform = GetNodeTransform();
			if (transform == null)
				return;

			// get value from the rotation TrackBar control
			float a = (float)trkRotate.Value;

			// desactives rotation NumericUpDown control ValueChanged event to set its value
			// without encountering redundency event raising with the rotation TrackBar control
			numRotate.ValueChanged -= new EventHandler(numRotate_ValueChanged);
			numRotate.Value = (decimal)a;
			// reactivates the rotation NumericUpDown control ValueChanged event
			numRotate.ValueChanged += new EventHandler(numRotate_ValueChanged);

			// update transform scalar value
			transform.ScalarTransform = a;
			// update the transform matrix of the selected scene node
			GetSelectedNode().UpdateTransform();

			// scene has changed!
			SceneChanged = true;
		}

		/// <summary>
		/// Called by the system when the value of the NumericUpDown control for rotations is changed.
		/// </summary>
		/// <param name="sender">NumericUpDown control which raised the event.</param>
		/// <param name="e">Base .NET event.</param>
		private void numRotate_ValueChanged(object sender, EventArgs e)
		{
			// get selected primitive transform
			PrimitiveTransform transform = GetNodeTransform();
			if (transform == null)
				return;

			// get value from the rotation NumericUpDown control
			float a = (float)numRotate.Value;

			// desactives rotation TrackBar control Scroll event to set its value without
			// encountering redundency event raising with the rotation NumericUpDown control
			trkRotate.Scroll -= new EventHandler(trkRotate_Scroll);
			trkRotate.Value = (int)a;
			// reactivates the rotation TrackBar control Scrool event
			trkRotate.Scroll += new EventHandler(trkRotate_Scroll);

			// update transform scalar value
			transform.ScalarTransform = a;
			// update the transform matrix of the selected scene node
			GetSelectedNode().UpdateTransform();

			// scene has changed!
			SceneChanged = true;
		}

		/// <summary>
		/// Called by the system when the value of the NumericUpDown control for scale is changed.
		/// </summary>
		/// <param name="sender">NumericUpDown control which raised the event.</param>
		/// <param name="e">Base .NET event.</param>
		private void numScale_ValueChanged(object sender, EventArgs e)
		{
			// get selected primitive transform
			PrimitiveTransform transform = GetNodeTransform();
			if (transform == null)
				return;

			// update transform vectorial value
			transform.VectorTransform = new Vector3((float)numScaleX.Value, (float)numScaleY.Value, (float)numScaleZ.Value);
			// update the transform matrix of the selected scene node
			GetSelectedNode().UpdateTransform();

			// scene has changed!
			SceneChanged = true;
		}

		#endregion


		// Just a UI trick, focused splitter displays a ugly blinking dotted bar.
		// Ensure it can't remain focused by focusing another control.
		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			trvSceneGraph.Focus();
		}

		/// <summary>
		/// Called by the system when a scene node is selected in the TreeView.
		/// </summary>
		/// <param name="sender">TreeView control which raised the event.</param>
		/// <param name="e">TreeView specific event.</param>
		private void trvSceneGraph_AfterSelect(object sender, TreeViewEventArgs e)
		{
			// redefine the primitive transforms list according to the newly selected node
			UpdateTransformsListView();
		}

		/// <summary>
		/// Resets the UI primitive transforms list.
		/// </summary>
		private void UpdateTransformsListView()
		{
			// check for currently selected scene node
			SceneNode node = GetSelectedNode();
			if (node == null)
				return;

			// hides all transformations controls (to show them only if they must be)
			HideAllTransformControls();

			// ensure current scene node matrix is properly computed
			node.UpdateTransform();

			lstTransforms.SuspendLayout();

			// stores the selected item index
			int idx = 0;
			if (lstTransforms.SelectedIndices.Count > 0)
				idx = lstTransforms.SelectedIndices[0];

			// removes all the primitive transform nodes from the UI control and
			// re-add them to the UI control from the in-memory list
			// (this is to ensure synchronization between memory data and UI view)
			lstTransforms.Items.Clear();
			foreach (PrimitiveTransform tr in node.PrimitiveTransforms)
				lstTransforms.Items.Add(tr);

			// restores the previously selected item
			if (lstTransforms.Items.Count > 0)
			{
				if (idx < lstTransforms.Items.Count)
					lstTransforms.Items[idx].Selected = true;
				else
					lstTransforms.Items[lstTransforms.Items.Count - 1].Selected = true;
			}

			lstTransforms.ResumeLayout();
		}

		/// <summary>
		/// Hides all the UI controls related to primitive transformations.
		/// </summary>
		private void HideAllTransformControls()
		{
			// hides all transform group controls
			grpTranslation.Visible = false;
			grpRotation.Visible = false;
			grpScale.Visible = false;

			// hides all rotation related labels
			lblRotateX.Visible = false;
			lblRotateY.Visible = false;
			lblRotateZ.Visible = false;
		}

		/// <summary>
		/// Raised when the user clicks the '+' button in the 'Scene Tree' tab.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAddTransform_Click(object sender, EventArgs e)
		{
			// checks for selected scene node
			SceneNode node = GetSelectedNode();
			if (node == null)
				return;

			// instanciate and shows a form to let the user to select a transform type
			frmSelectTransform frm = new frmSelectTransform();
			if (frm.ShowDialog() != DialogResult.OK)
				return;

			TransformType type = frm.TransformType;

			// add a primitive transform to the transform collection, according to the primitive transform type
			if (type == TransformType.Translation)
				node.PrimitiveTransforms.Add(new PrimitiveTransform(type, new Vector3()));
			else if (type == TransformType.Scale)
				node.PrimitiveTransforms.Add(new PrimitiveTransform(type, new Vector3(1.0f, 1.0f, 1.0f)));
			else
				node.PrimitiveTransforms.Add(new PrimitiveTransform(type, 0.0f));

			// updates the primitive transforms UI list
			UpdateTransformsListView();
		}

		/// <summary>
		/// Raised when the user clicks the '-' button in the 'Scene Tree' tab.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRemoveTransform_Click(object sender, EventArgs e)
		{
			// checks for selected scene node
			SceneNode node = GetSelectedNode();
			if (node == null)
				return;

			// ensure there is at least one primitive transform to remove
			if (lstTransforms.SelectedItems.Count == 0)
				return;

			// remove the primitive transform
			int idx = lstTransforms.SelectedIndices[0];
			node.PrimitiveTransforms.RemoveAt(idx);

			// updates the primitive transforms UI list
			UpdateTransformsListView();
		}

		/// <summary>
		/// Raised when the user clicks the 'Up' button in the 'Scene Tree' tab.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnUpTransform_Click(object sender, EventArgs e)
		{
			// checks for primitive transform selected node
			PrimitiveTransform transform = GetNodeTransform();
			if (transform == null)
				return;

			// retrieve the selected scene node
			SceneNode node = GetSelectedNode();

			// check if current primitive transform can be displaced
			if (transform.Index > 0)
			{
				// stores index, removes the element and re-add it one position before
				int idx = transform.Index;
				node.PrimitiveTransforms.RemoveAt(idx);
				node.PrimitiveTransforms.Insert(idx - 1, transform);
				// updates the primitive transforms UI list
				UpdateTransformsListView();
				// reselect the just moved item
				lstTransforms.Items[idx - 1].Selected = true;
			}
		}

		/// <summary>
		/// Raised when the user clicks the 'Dn' button in the 'Scene Tree' tab.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDownTransform_Click(object sender, EventArgs e)
		{
			// checks for primitive transform selected node
			PrimitiveTransform transform = GetNodeTransform();
			if (transform == null)
				return;

			// retrieve the selected scene node
			SceneNode node = GetSelectedNode();

			// check if current primitive transform can be displaced
			if (transform.Index < lstTransforms.Items.Count - 1)
			{
				// stores index, removes the element and re-add it one position after
				int idx = transform.Index;
				node.PrimitiveTransforms.RemoveAt(idx);
				node.PrimitiveTransforms.Insert(idx + 1, transform);
				// updates the primitive transforms UI list
				UpdateTransformsListView();
				// reselect the just moved item
				lstTransforms.Items[idx + 1].Selected = true;
			}
		}

		/// <summary>
		/// Raised when a primitive transform list item is selected from the UI list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lstTransforms_SelectedIndexChanged(object sender, EventArgs e)
		{
			// hide all the UI controls related to transforms
			HideAllTransformControls();

			// retrieve the currently selected primitive transform
			PrimitiveTransform transform = GetNodeTransform();

			if (transform != null)
			{
				if (transform.TransformType == TransformType.Translation)
				{
					// displays the translation related UI controls
					grpTranslation.Visible = true;

					// unsets the UI control events to set values
					numTransX.ValueChanged -= new EventHandler(numTranslation_ValueChanged);
					numTransY.ValueChanged -= new EventHandler(numTranslation_ValueChanged);
					numTransZ.ValueChanged -= new EventHandler(numTranslation_ValueChanged);
					// set values
					numTransX.Value = (decimal)transform.VectorTransform.X;
					numTransY.Value = (decimal)transform.VectorTransform.Y;
					numTransZ.Value = (decimal)transform.VectorTransform.Z;
					// resets the UI control events
					numTransX.ValueChanged += new EventHandler(numTranslation_ValueChanged);
					numTransY.ValueChanged += new EventHandler(numTranslation_ValueChanged);
					numTransZ.ValueChanged += new EventHandler(numTranslation_ValueChanged);
				}
				else if (transform.TransformType == TransformType.RotationX)
				{
					// displays the rotation related UI controls
					grpRotation.Visible = true;
					lblRotateX.Visible = true;
					trkRotate.Visible = true;
					numRotate.Visible = true;

					// unsets the UI control events to set values
					trkRotate.Scroll -= new EventHandler(trkRotate_Scroll);
					numRotate.ValueChanged -= new EventHandler(numRotate_ValueChanged);
					// set values
					trkRotate.Value = (int)transform.ScalarTransform;
					numRotate.Value = (decimal)transform.ScalarTransform;
					// resets the UI control events
					trkRotate.Scroll += new EventHandler(trkRotate_Scroll);
					numRotate.ValueChanged += new EventHandler(numRotate_ValueChanged);
				}
				else if (transform.TransformType == TransformType.RotationY)
				{
					// displays the rotation related UI controls
					grpRotation.Visible = true;
					lblRotateY.Visible = true;
					trkRotate.Visible = true;
					numRotate.Visible = true;

					// unsets the UI control events to set values
					trkRotate.Scroll -= new EventHandler(trkRotate_Scroll);
					numRotate.ValueChanged -= new EventHandler(numRotate_ValueChanged);
					// set values
					trkRotate.Value = (int)transform.ScalarTransform;
					numRotate.Value = (decimal)transform.ScalarTransform;
					// resets the UI control events
					trkRotate.Scroll += new EventHandler(trkRotate_Scroll);
					numRotate.ValueChanged += new EventHandler(numRotate_ValueChanged);
				}
				else if (transform.TransformType == TransformType.RotationZ)
				{
					// displays the rotation related UI controls
					grpRotation.Visible = true;
					lblRotateZ.Visible = true;
					trkRotate.Visible = true;
					numRotate.Visible = true;

					// unsets the UI control events to set values
					trkRotate.Scroll -= new EventHandler(trkRotate_Scroll);
					numRotate.ValueChanged -= new EventHandler(numRotate_ValueChanged);
					// set values
					trkRotate.Value = (int)transform.ScalarTransform;
					numRotate.Value = (decimal)transform.ScalarTransform;
					// resets the UI control events
					trkRotate.Scroll += new EventHandler(trkRotate_Scroll);
					numRotate.ValueChanged += new EventHandler(numRotate_ValueChanged);
				}
				else //if (transform.TransformType == TransformType.Scale)
				{
					// displays the scale related UI controls
					grpScale.Visible = true;
					numScaleX.Focus();

					// unsets the UI control events to set values
					numScaleX.ValueChanged -= new EventHandler(numScale_ValueChanged);
					numScaleY.ValueChanged -= new EventHandler(numScale_ValueChanged);
					numScaleZ.ValueChanged -= new EventHandler(numScale_ValueChanged);
					// set values
					numScaleX.Value = (decimal)transform.VectorTransform.X;
					numScaleY.Value = (decimal)transform.VectorTransform.Y;
					numScaleZ.Value = (decimal)transform.VectorTransform.Z;
					// resets the UI control events
					numScaleX.ValueChanged += new EventHandler(numScale_ValueChanged);
					numScaleY.ValueChanged += new EventHandler(numScale_ValueChanged);
					numScaleZ.ValueChanged += new EventHandler(numScale_ValueChanged);
				}
			}
		}

		/// <summary>
		/// Force the window closing.
		/// </summary>
		internal void ForceClose()
		{
			// set force window closing flag
			m_ForceClose = true;
			// close the window
			this.Close();
		}

		/// <summary>
		/// Raised when the window is about to be closed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmSceneGraph_FormClosing(object sender, FormClosingEventArgs e)
		{
			// force window closing
			if (m_ForceClose)
				return;

			// hides the window and cancels the window closing (destruction)
			this.Hide();
			e.Cancel = true;
		}

		/// <summary>
		/// Checks if the scene has changed, and if yes opens a dialog box to propose to save it.
		/// </summary>
		/// <returns>Returns true if the caller can go on its normal flow, false to indicates the caller to not perform any further actions.</returns>
		private bool CheckSceneChanged()
		{
			// no problem if scene didn't change
			if (SceneChanged == false)
				return (true);

			// otherwise, shows a question dialog to the user
			DialogResult res = MessageBox.Show("The current scene has been modified, do you want to save the modification ?", "Save modifications ?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
			if (res == DialogResult.Cancel)
				// cancelled, the use should not perform any futher actions
				return (false);
			if (res == DialogResult.Yes)
				// yes, saves the scene
				SaveScene();

			// otherwise just continue nornally
			return (true);
		}

		/// <summary>
		/// Raised when the user clicks the 'Load Scene...' button in the 'Scenes' tab.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLoadScene_Click(object sender, EventArgs e)
		{
			// shows file selection dialog
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select a scene file";
			ofd.Filter = "Xml Files (*.xml)|*.xml|All Files (*.*)|*.*";
			ofd.InitialDirectory = m_ScenesPath;
			if (ofd.ShowDialog() != DialogResult.OK)
				return;

			// loads the scene file
			LoadSceneFile(ofd.FileName);
		}

		/// <summary>
		/// Raised when the user clicks the 'Load Model...' button in the 'Scenes' tab.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLoadModel_Click(object sender, EventArgs e)
		{
			// shows file selection dialog
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select a model file";
			ofd.Filter = "DirectX Model Files (*.x)|*.x|All Files (*.*)|*.*";
			ofd.InitialDirectory = m_ScenesPath;
			if (ofd.ShowDialog() != DialogResult.OK)
				return;

			// loads the model file
			LoadModelFile(ofd.FileName);
		}

		/// <summary>
		/// Raised when the user clicks the 'Save' button in the 'Scene Tree' tab.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSaveScene_Click(object sender, EventArgs e)
		{
			// save the current scene to its respective file
			SaveScene();
		}

		/// <summary>
		/// Save all the modifications made to the current scene.
		/// </summary>
		private void SaveScene()
		{
			// ensure a scene has been loaded before trying to save it
			if (m_SceneFilename == null)
				return;

			// make sure the FileSystemWatcher will not fire a reload event when the file will be saved to hard drive
			m_Watcher.EnableRaisingEvents = false;

			XmlTextWriter xw = null;

			try
			{
				// creates a XmlTextWriter representing the XML file to write data to
				xw = new XmlTextWriter(m_SceneFilename, Encoding.UTF8);
				xw.Formatting = Formatting.Indented;
				xw.IndentChar = '\t';
				xw.Indentation = 1;

				// write document stuffs
				xw.WriteStartDocument();
				{
					// write XML root node
					xw.WriteStartElement("ModelScene");
					{
						// check if scene contains nodes
						if (trvSceneGraph.Nodes.Count > 0)
						{
							// gather the proper node collection to save
							TreeNodeCollection nodes = null;
							if (trvSceneGraph.Nodes[0].Name == "_root_")
								nodes = trvSceneGraph.Nodes[0].Nodes;
							else
								nodes = trvSceneGraph.Nodes;

							// write the XML content of each scene node
							foreach (SceneNode node in nodes)
								node.WriteXml(xw);
						}
					}
					xw.WriteEndElement();
				}
				xw.WriteEndDocument();
				// close the file, very important
				xw.Close();

				// now that scene has been saved, it is not 'modified' anymore
				SceneChanged = false;
			}
			catch
			{
				// in case of failure, check the XML file and close it if possible
				if (xw != null)
					xw.Close();
			}
			finally
			{
				// ensure the FileSystemWatcher can go on raising events
				m_Watcher.EnableRaisingEvents = true;
			}
		}

		/// <summary>
		/// Raised when the user select scene in the default scenes list, in the 'Scenes' tab
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lstDefaultScenes_SelectedIndexChanged(object sender, EventArgs e)
		{
			// check if at least one scene is available
			if (lstDefaultScenes.SelectedItems.Count == 0)
				return;

			// check if current scene has been modified
			if (CheckSceneChanged() == false)
				return; // break the current flow if the 'CheckSceneChanged' method indicates it by returning false

			// get filename of new scene to load
			string sceneFilename = lstDefaultScenes.SelectedItems[0].SubItems[1].Text;
			// load scene only if there is no scene currently loaded or if the currently
			// loaded scene is a different one than the one requested to load
			if (m_SceneFilename == null || m_SceneFilename.ToLower() != sceneFilename.ToLower())
				LoadSceneFile(sceneFilename);
		}

		private void trkSpinningLight_Scroll(object sender, EventArgs e)
		{
			m_ModelScene.m_LightRotationAngle = trkSpinningLight.Value * (float)Math.PI / 180.0f;
		}

		private void chkSpinningLight_CheckedChanged(object sender, EventArgs e)
		{
			// enable or disable the spinning light TrackBar according to the spinning light option CheckBox
			trkSpinningLight.Enabled = !chkSpinningLight.Checked;
			// sets the TrackBar to the current light roation angle when the spinning rotation becomes manual
			if (chkSpinningLight.Checked == false)
				trkSpinningLight.Value = (int)(m_ModelScene.m_LightRotationAngle / (float)Math.PI * 180.0f);
		}

		private void chkUseCache_CheckedChanged(object sender, EventArgs e)
		{
			// tells the ResourceManager about the cache capability, according to CheckBox option
			ResourceManager.UseCache = chkUseCache.Checked;
		}

		private void lstDefaultScenes_DoubleClick(object sender, EventArgs e)
		{
			if (lstDefaultScenes.SelectedItems.Count == 0)
				return;

			string sceneFilename = lstDefaultScenes.SelectedItems[0].SubItems[1].Text;
			Process.Start("notepad", sceneFilename);
		}
	}
}
