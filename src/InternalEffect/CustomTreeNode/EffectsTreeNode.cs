using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PostEffectCore;

namespace InternalEffect
{
	public class EffectsTreeNode : BaseTreeNode
	{
		public EffectsTreeNode()
		{
			this.Text = "Effects";
		}

		public override void Initialize()
		{
			this.ImageIndex = 5;
			this.SelectedImageIndex = 5;

			GenerateContextMenu();
		}

		private void GenerateContextMenu()
		{
			ContextMenuStrip menu = new ContextMenuStrip();
			menu.ImageList = this.TreeView.ImageList;

			ToolStripMenuItem addMenu = new ToolStripMenuItem("Add");
			menu.Items.Add(addMenu);
			addMenu.Click += new EventHandler(OnAddMenuClick);

			ToolStripMenuItem addNewEffectMenu = new ToolStripMenuItem("New Effect...");
			addMenu.DropDownItems.Add(addNewEffectMenu);
			addNewEffectMenu.Click += new EventHandler(OnAddNewEffectMenuClick);
			addNewEffectMenu.Image = this.TreeView.ImageList.Images[10];

			ToolStripMenuItem addExistingEffectMenu = new ToolStripMenuItem("Existing Effect...");
			addMenu.DropDownItems.Add(addExistingEffectMenu);
			addExistingEffectMenu.Click += new EventHandler(OnAddExistingEffectMenuClick);
			addExistingEffectMenu.Image = this.TreeView.ImageList.Images[7];

			this.ContextMenuStrip = menu;
		}

		private void OnAddMenuClick(object sender, EventArgs e)
		{
		}

		private void OnAddNewEffectMenuClick(object sender, EventArgs e)
		{
			Project project = GetProjectNode();
			if (project == null)
				return;

			frmInput input = new frmInput();
			if (input.ShowDialog() != DialogResult.OK)
				return;

			string effectFilename = string.Format("shaders\\{0}.fx", TransformName(input.Value));
			string effectFullFilename = string.Format("{0}\\{1}", project.ProjectDirectory, effectFilename);
			if (File.Exists(effectFullFilename))
			{
				MessageBox.Show(string.Format("The file '{0}' in the project directory already exists.", effectFilename), "Shader already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			string effectName = StringHelper.RemoveWhiteSpaces(StringHelper.Capitalize(input.Value));

			WriteShaderTemplate(effectName, effectFullFilename);
			AddEffect(project, effectFullFilename);
		}

		private void OnAddExistingEffectMenuClick(object sender, EventArgs e)
		{
			Project project = GetProjectNode();
			if (project == null)
				return;

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Title = "Select an existing shader file";
			ofd.Filter = "HLSL Effects (*.fx)|*.fx|All Files (*.*)|*.*";
			ofd.Multiselect = true;
			if (ofd.ShowDialog() != DialogResult.OK)
				return;

			try
			{
				foreach (string file in ofd.FileNames)
					AddEffect(project, file);
			}
			catch (InvalidProgramException ex)
			{
				MessageBox.Show(ex.Message, "Shader Compile Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void AddEffect(Project project, string effectFullFilename)
		{
			string effectFilename = project.AttachFileToProject(effectFullFilename, "shaders");

			CustomEffect efx = null;
			try
			{
				efx = new CustomEffect(project.Device, effectFullFilename, effectFilename);
				efx.Reloaded += new EventHandler(OnEffectReloaded);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, effectFilename, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			foreach (CustomTechnique tech in efx.Techniques)
			{
				if (tech.Passes.Length == 1)
					AddNode(new PassTreeNode(tech.Passes[0]));
				else
					AddNode(new TechniqueTreeNode(tech));
			}
		}

		private void OnEffectReloaded(object sender, EventArgs e)
		{
			Project project = GetProjectNode();
			if (project != null)
				project.OnEffectReloaded(sender, e);
		}

		private string TransformName(string name)
		{
			char[] invalidChars = Path.GetInvalidFileNameChars();

			char[] newName = new char[name.Length];
			for (int i = 0; i < name.Length; i++)
			{
				if (Array.Exists<char>(invalidChars, delegate(char c) { return (c == name[i]); }) == false)
					newName[i] = name[i];
				else
					newName[i] = '_';
			}
			return (new string(newName));
		}

		private void WriteShaderTemplate(string effectName, string filename)
		{
			string path = Path.GetDirectoryName(filename);
			if (Directory.Exists(path) == false)
				Directory.CreateDirectory(path);

			StreamWriter sw = new StreamWriter(filename, false, Encoding.ASCII);

			sw.WriteLine("texture Input;");
			sw.WriteLine("sampler InputSampler = sampler_state");
			sw.WriteLine("{");
			sw.WriteLine("\tTexture = <Input>;");
			sw.WriteLine("};");
			sw.WriteLine();
			sw.WriteLine("float4 {0}Func(float2 uv : TEXCOORD) : COLOR", effectName);
			sw.WriteLine("{");
			sw.WriteLine("\tfloat4 color = tex2D(InputSampler, uv);");
			sw.WriteLine("\treturn (color);");
			sw.WriteLine("}");
			sw.WriteLine();
			sw.WriteLine("technique {0}", effectName);
			sw.WriteLine("{");
			sw.WriteLine("\tpass {0}", effectName);
			sw.WriteLine("\t{");
			sw.WriteLine("\t\tVertexShader = null;");
			sw.WriteLine("\t\tPixelShader = compile ps_3_0 {0}Func();", effectName);
			sw.WriteLine("\t}");
			sw.WriteLine("}");

			sw.Close();
		}
	}
}
