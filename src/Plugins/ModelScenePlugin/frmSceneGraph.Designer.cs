namespace ModelScenePlugin
{
	partial class frmSceneGraph
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.trvSceneGraph = new System.Windows.Forms.TreeView();
			this.grpTranslation = new System.Windows.Forms.GroupBox();
			this.numTransZ = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.numTransY = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.numTransX = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.lstTransforms = new System.Windows.Forms.ListView();
			this.colTransformType = new System.Windows.Forms.ColumnHeader();
			this.colTransformValues = new System.Windows.Forms.ColumnHeader();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.btnSaveScene = new System.Windows.Forms.Button();
			this.btnDownTransform = new System.Windows.Forms.Button();
			this.btnUpTransform = new System.Windows.Forms.Button();
			this.btnRemoveTransform = new System.Windows.Forms.Button();
			this.btnAddTransform = new System.Windows.Forms.Button();
			this.grpScale = new System.Windows.Forms.GroupBox();
			this.numScaleZ = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.numScaleY = new System.Windows.Forms.NumericUpDown();
			this.label8 = new System.Windows.Forms.Label();
			this.numScaleX = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.grpRotation = new System.Windows.Forms.GroupBox();
			this.numRotate = new System.Windows.Forms.NumericUpDown();
			this.trkRotate = new System.Windows.Forms.TrackBar();
			this.lblRotateZ = new System.Windows.Forms.Label();
			this.lblRotateY = new System.Windows.Forms.Label();
			this.lblRotateX = new System.Windows.Forms.Label();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabScenes = new System.Windows.Forms.TabPage();
			this.btnLoadModel = new System.Windows.Forms.Button();
			this.btnLoadScene = new System.Windows.Forms.Button();
			this.lstDefaultScenes = new System.Windows.Forms.ListView();
			this.colSceneName = new System.Windows.Forms.ColumnHeader();
			this.colFilename = new System.Windows.Forms.ColumnHeader();
			this.label4 = new System.Windows.Forms.Label();
			this.tabSceneTree = new System.Windows.Forms.TabPage();
			this.tabOptions = new System.Windows.Forms.TabPage();
			this.chkUseCache = new System.Windows.Forms.CheckBox();
			this.grpOptionLight = new System.Windows.Forms.GroupBox();
			this.trkSpinningLight = new System.Windows.Forms.TrackBar();
			this.chkSpinningLight = new System.Windows.Forms.CheckBox();
			this.grpOptionAxis = new System.Windows.Forms.GroupBox();
			this.chkShowCurrentTransformAxis = new System.Windows.Forms.CheckBox();
			this.chkShowParentTransformAxis = new System.Windows.Forms.CheckBox();
			this.grpOptionGrid = new System.Windows.Forms.GroupBox();
			this.chkShowGrid = new System.Windows.Forms.CheckBox();
			this.grpTranslation.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numTransZ)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numTransY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numTransX)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.grpScale.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numScaleZ)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numScaleY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numScaleX)).BeginInit();
			this.grpRotation.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRotate)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trkRotate)).BeginInit();
			this.tabControl.SuspendLayout();
			this.tabScenes.SuspendLayout();
			this.tabSceneTree.SuspendLayout();
			this.tabOptions.SuspendLayout();
			this.grpOptionLight.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkSpinningLight)).BeginInit();
			this.grpOptionAxis.SuspendLayout();
			this.grpOptionGrid.SuspendLayout();
			this.SuspendLayout();
			// 
			// trvSceneGraph
			// 
			this.trvSceneGraph.Dock = System.Windows.Forms.DockStyle.Fill;
			this.trvSceneGraph.HideSelection = false;
			this.trvSceneGraph.Location = new System.Drawing.Point(0, 0);
			this.trvSceneGraph.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.trvSceneGraph.Name = "trvSceneGraph";
			this.trvSceneGraph.Size = new System.Drawing.Size(123, 293);
			this.trvSceneGraph.TabIndex = 0;
			this.trvSceneGraph.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvSceneGraph_AfterSelect);
			// 
			// grpTranslation
			// 
			this.grpTranslation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpTranslation.Controls.Add(this.numTransZ);
			this.grpTranslation.Controls.Add(this.label3);
			this.grpTranslation.Controls.Add(this.numTransY);
			this.grpTranslation.Controls.Add(this.label2);
			this.grpTranslation.Controls.Add(this.numTransX);
			this.grpTranslation.Controls.Add(this.label1);
			this.grpTranslation.Location = new System.Drawing.Point(0, 172);
			this.grpTranslation.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.grpTranslation.Name = "grpTranslation";
			this.grpTranslation.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.grpTranslation.Size = new System.Drawing.Size(240, 121);
			this.grpTranslation.TabIndex = 8;
			this.grpTranslation.TabStop = false;
			this.grpTranslation.Text = "Translation";
			this.grpTranslation.Visible = false;
			// 
			// numTransZ
			// 
			this.numTransZ.DecimalPlaces = 8;
			this.numTransZ.Location = new System.Drawing.Point(34, 86);
			this.numTransZ.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numTransZ.Name = "numTransZ";
			this.numTransZ.Size = new System.Drawing.Size(139, 21);
			this.numTransZ.TabIndex = 2;
			this.numTransZ.ValueChanged += new System.EventHandler(this.numTranslation_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.ForeColor = System.Drawing.Color.Blue;
			this.label3.Location = new System.Drawing.Point(6, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(20, 15);
			this.label3.TabIndex = 4;
			this.label3.Text = "Z :";
			// 
			// numTransY
			// 
			this.numTransY.DecimalPlaces = 8;
			this.numTransY.Location = new System.Drawing.Point(34, 55);
			this.numTransY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numTransY.Name = "numTransY";
			this.numTransY.Size = new System.Drawing.Size(139, 21);
			this.numTransY.TabIndex = 1;
			this.numTransY.ValueChanged += new System.EventHandler(this.numTranslation_ValueChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(230)))), ((int)(((byte)(0)))));
			this.label2.Location = new System.Drawing.Point(6, 57);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(20, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "Y :";
			// 
			// numTransX
			// 
			this.numTransX.DecimalPlaces = 8;
			this.numTransX.Location = new System.Drawing.Point(34, 23);
			this.numTransX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numTransX.Name = "numTransX";
			this.numTransX.Size = new System.Drawing.Size(139, 21);
			this.numTransX.TabIndex = 0;
			this.numTransX.ValueChanged += new System.EventHandler(this.numTranslation_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(6, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(20, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "X :";
			// 
			// lstTransforms
			// 
			this.lstTransforms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstTransforms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTransformType,
            this.colTransformValues});
			this.lstTransforms.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstTransforms.FullRowSelect = true;
			this.lstTransforms.GridLines = true;
			this.lstTransforms.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstTransforms.HideSelection = false;
			this.lstTransforms.Location = new System.Drawing.Point(0, 26);
			this.lstTransforms.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.lstTransforms.MultiSelect = false;
			this.lstTransforms.Name = "lstTransforms";
			this.lstTransforms.Size = new System.Drawing.Size(240, 137);
			this.lstTransforms.TabIndex = 5;
			this.lstTransforms.UseCompatibleStateImageBehavior = false;
			this.lstTransforms.View = System.Windows.Forms.View.Details;
			this.lstTransforms.SelectedIndexChanged += new System.EventHandler(this.lstTransforms_SelectedIndexChanged);
			// 
			// colTransformType
			// 
			this.colTransformType.Text = "Type";
			this.colTransformType.Width = 89;
			// 
			// colTransformValues
			// 
			this.colTransformValues.Text = "Values";
			this.colTransformValues.Width = 111;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.trvSceneGraph);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.btnSaveScene);
			this.splitContainer1.Panel2.Controls.Add(this.btnDownTransform);
			this.splitContainer1.Panel2.Controls.Add(this.btnUpTransform);
			this.splitContainer1.Panel2.Controls.Add(this.btnRemoveTransform);
			this.splitContainer1.Panel2.Controls.Add(this.btnAddTransform);
			this.splitContainer1.Panel2.Controls.Add(this.lstTransforms);
			this.splitContainer1.Panel2.Controls.Add(this.grpTranslation);
			this.splitContainer1.Panel2.Controls.Add(this.grpScale);
			this.splitContainer1.Panel2.Controls.Add(this.grpRotation);
			this.splitContainer1.Size = new System.Drawing.Size(369, 293);
			this.splitContainer1.SplitterDistance = 123;
			this.splitContainer1.SplitterWidth = 5;
			this.splitContainer1.TabIndex = 6;
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
			// 
			// btnSaveScene
			// 
			this.btnSaveScene.Location = new System.Drawing.Point(0, 0);
			this.btnSaveScene.Name = "btnSaveScene";
			this.btnSaveScene.Size = new System.Drawing.Size(63, 23);
			this.btnSaveScene.TabIndex = 0;
			this.btnSaveScene.Text = "Save";
			this.btnSaveScene.UseVisualStyleBackColor = true;
			this.btnSaveScene.Click += new System.EventHandler(this.btnSaveScene_Click);
			// 
			// btnDownTransform
			// 
			this.btnDownTransform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDownTransform.Location = new System.Drawing.Point(207, 0);
			this.btnDownTransform.Name = "btnDownTransform";
			this.btnDownTransform.Size = new System.Drawing.Size(34, 23);
			this.btnDownTransform.TabIndex = 4;
			this.btnDownTransform.Text = "Dn";
			this.btnDownTransform.UseVisualStyleBackColor = true;
			this.btnDownTransform.Click += new System.EventHandler(this.btnDownTransform_Click);
			// 
			// btnUpTransform
			// 
			this.btnUpTransform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpTransform.Location = new System.Drawing.Point(171, 0);
			this.btnUpTransform.Name = "btnUpTransform";
			this.btnUpTransform.Size = new System.Drawing.Size(34, 23);
			this.btnUpTransform.TabIndex = 3;
			this.btnUpTransform.Text = "Up";
			this.btnUpTransform.UseVisualStyleBackColor = true;
			this.btnUpTransform.Click += new System.EventHandler(this.btnUpTransform_Click);
			// 
			// btnRemoveTransform
			// 
			this.btnRemoveTransform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemoveTransform.Location = new System.Drawing.Point(146, 0);
			this.btnRemoveTransform.Name = "btnRemoveTransform";
			this.btnRemoveTransform.Size = new System.Drawing.Size(23, 23);
			this.btnRemoveTransform.TabIndex = 2;
			this.btnRemoveTransform.Text = "-";
			this.btnRemoveTransform.UseVisualStyleBackColor = true;
			this.btnRemoveTransform.Click += new System.EventHandler(this.btnRemoveTransform_Click);
			// 
			// btnAddTransform
			// 
			this.btnAddTransform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddTransform.Location = new System.Drawing.Point(121, 0);
			this.btnAddTransform.Name = "btnAddTransform";
			this.btnAddTransform.Size = new System.Drawing.Size(23, 23);
			this.btnAddTransform.TabIndex = 1;
			this.btnAddTransform.Text = "+";
			this.btnAddTransform.UseVisualStyleBackColor = true;
			this.btnAddTransform.Click += new System.EventHandler(this.btnAddTransform_Click);
			// 
			// grpScale
			// 
			this.grpScale.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpScale.Controls.Add(this.numScaleZ);
			this.grpScale.Controls.Add(this.label7);
			this.grpScale.Controls.Add(this.numScaleY);
			this.grpScale.Controls.Add(this.label8);
			this.grpScale.Controls.Add(this.numScaleX);
			this.grpScale.Controls.Add(this.label9);
			this.grpScale.Location = new System.Drawing.Point(0, 172);
			this.grpScale.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.grpScale.Name = "grpScale";
			this.grpScale.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.grpScale.Size = new System.Drawing.Size(240, 121);
			this.grpScale.TabIndex = 6;
			this.grpScale.TabStop = false;
			this.grpScale.Text = "Scale";
			this.grpScale.Visible = false;
			// 
			// numScaleZ
			// 
			this.numScaleZ.DecimalPlaces = 8;
			this.numScaleZ.Location = new System.Drawing.Point(34, 86);
			this.numScaleZ.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numScaleZ.Name = "numScaleZ";
			this.numScaleZ.Size = new System.Drawing.Size(139, 21);
			this.numScaleZ.TabIndex = 5;
			this.numScaleZ.ValueChanged += new System.EventHandler(this.numScale_ValueChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.ForeColor = System.Drawing.Color.Red;
			this.label7.Location = new System.Drawing.Point(6, 88);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(20, 15);
			this.label7.TabIndex = 4;
			this.label7.Text = "Z :";
			// 
			// numScaleY
			// 
			this.numScaleY.DecimalPlaces = 8;
			this.numScaleY.Location = new System.Drawing.Point(34, 55);
			this.numScaleY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numScaleY.Name = "numScaleY";
			this.numScaleY.Size = new System.Drawing.Size(139, 21);
			this.numScaleY.TabIndex = 3;
			this.numScaleY.ValueChanged += new System.EventHandler(this.numScale_ValueChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(230)))), ((int)(((byte)(0)))));
			this.label8.Location = new System.Drawing.Point(6, 57);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(20, 15);
			this.label8.TabIndex = 2;
			this.label8.Text = "Y :";
			// 
			// numScaleX
			// 
			this.numScaleX.DecimalPlaces = 8;
			this.numScaleX.Location = new System.Drawing.Point(34, 23);
			this.numScaleX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.numScaleX.Name = "numScaleX";
			this.numScaleX.Size = new System.Drawing.Size(139, 21);
			this.numScaleX.TabIndex = 1;
			this.numScaleX.ValueChanged += new System.EventHandler(this.numScale_ValueChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.ForeColor = System.Drawing.Color.Blue;
			this.label9.Location = new System.Drawing.Point(6, 26);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(20, 15);
			this.label9.TabIndex = 0;
			this.label9.Text = "X :";
			// 
			// grpRotation
			// 
			this.grpRotation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpRotation.Controls.Add(this.numRotate);
			this.grpRotation.Controls.Add(this.trkRotate);
			this.grpRotation.Controls.Add(this.lblRotateZ);
			this.grpRotation.Controls.Add(this.lblRotateY);
			this.grpRotation.Controls.Add(this.lblRotateX);
			this.grpRotation.Location = new System.Drawing.Point(0, 172);
			this.grpRotation.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.grpRotation.Name = "grpRotation";
			this.grpRotation.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.grpRotation.Size = new System.Drawing.Size(240, 121);
			this.grpRotation.TabIndex = 10;
			this.grpRotation.TabStop = false;
			this.grpRotation.Text = "Rotation";
			this.grpRotation.Visible = false;
			// 
			// numRotate
			// 
			this.numRotate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.numRotate.DecimalPlaces = 2;
			this.numRotate.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.numRotate.Location = new System.Drawing.Point(167, 23);
			this.numRotate.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
			this.numRotate.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
			this.numRotate.Name = "numRotate";
			this.numRotate.Size = new System.Drawing.Size(67, 20);
			this.numRotate.TabIndex = 8;
			this.numRotate.Visible = false;
			this.numRotate.ValueChanged += new System.EventHandler(this.numRotate_ValueChanged);
			// 
			// trkRotate
			// 
			this.trkRotate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.trkRotate.AutoSize = false;
			this.trkRotate.Location = new System.Drawing.Point(32, 22);
			this.trkRotate.Maximum = 360;
			this.trkRotate.Minimum = -360;
			this.trkRotate.Name = "trkRotate";
			this.trkRotate.Size = new System.Drawing.Size(133, 23);
			this.trkRotate.TabIndex = 5;
			this.trkRotate.TickFrequency = 30;
			this.trkRotate.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trkRotate.Visible = false;
			this.trkRotate.Scroll += new System.EventHandler(this.trkRotate_Scroll);
			// 
			// lblRotateZ
			// 
			this.lblRotateZ.AutoSize = true;
			this.lblRotateZ.ForeColor = System.Drawing.Color.Blue;
			this.lblRotateZ.Location = new System.Drawing.Point(6, 28);
			this.lblRotateZ.Name = "lblRotateZ";
			this.lblRotateZ.Size = new System.Drawing.Size(20, 15);
			this.lblRotateZ.TabIndex = 4;
			this.lblRotateZ.Text = "Z :";
			this.lblRotateZ.Visible = false;
			// 
			// lblRotateY
			// 
			this.lblRotateY.AutoSize = true;
			this.lblRotateY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(230)))), ((int)(((byte)(0)))));
			this.lblRotateY.Location = new System.Drawing.Point(6, 28);
			this.lblRotateY.Name = "lblRotateY";
			this.lblRotateY.Size = new System.Drawing.Size(20, 15);
			this.lblRotateY.TabIndex = 2;
			this.lblRotateY.Text = "Y :";
			this.lblRotateY.Visible = false;
			// 
			// lblRotateX
			// 
			this.lblRotateX.AutoSize = true;
			this.lblRotateX.ForeColor = System.Drawing.Color.Red;
			this.lblRotateX.Location = new System.Drawing.Point(6, 28);
			this.lblRotateX.Name = "lblRotateX";
			this.lblRotateX.Size = new System.Drawing.Size(20, 15);
			this.lblRotateX.TabIndex = 0;
			this.lblRotateX.Text = "X :";
			this.lblRotateX.Visible = false;
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabScenes);
			this.tabControl.Controls.Add(this.tabSceneTree);
			this.tabControl.Controls.Add(this.tabOptions);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(383, 327);
			this.tabControl.TabIndex = 7;
			// 
			// tabScenes
			// 
			this.tabScenes.Controls.Add(this.btnLoadModel);
			this.tabScenes.Controls.Add(this.btnLoadScene);
			this.tabScenes.Controls.Add(this.lstDefaultScenes);
			this.tabScenes.Controls.Add(this.label4);
			this.tabScenes.Location = new System.Drawing.Point(4, 24);
			this.tabScenes.Name = "tabScenes";
			this.tabScenes.Size = new System.Drawing.Size(375, 299);
			this.tabScenes.TabIndex = 2;
			this.tabScenes.Text = "Scenes";
			this.tabScenes.UseVisualStyleBackColor = true;
			// 
			// btnLoadModel
			// 
			this.btnLoadModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadModel.Location = new System.Drawing.Point(157, 268);
			this.btnLoadModel.Name = "btnLoadModel";
			this.btnLoadModel.Size = new System.Drawing.Size(102, 23);
			this.btnLoadModel.TabIndex = 3;
			this.btnLoadModel.Text = "Load Model...";
			this.btnLoadModel.UseVisualStyleBackColor = true;
			this.btnLoadModel.Click += new System.EventHandler(this.btnLoadModel_Click);
			// 
			// btnLoadScene
			// 
			this.btnLoadScene.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnLoadScene.Location = new System.Drawing.Point(265, 268);
			this.btnLoadScene.Name = "btnLoadScene";
			this.btnLoadScene.Size = new System.Drawing.Size(102, 23);
			this.btnLoadScene.TabIndex = 2;
			this.btnLoadScene.Text = "Load Scene...";
			this.btnLoadScene.UseVisualStyleBackColor = true;
			this.btnLoadScene.Click += new System.EventHandler(this.btnLoadScene_Click);
			// 
			// lstDefaultScenes
			// 
			this.lstDefaultScenes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstDefaultScenes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSceneName,
            this.colFilename});
			this.lstDefaultScenes.FullRowSelect = true;
			this.lstDefaultScenes.GridLines = true;
			this.lstDefaultScenes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstDefaultScenes.HideSelection = false;
			this.lstDefaultScenes.Location = new System.Drawing.Point(8, 26);
			this.lstDefaultScenes.MultiSelect = false;
			this.lstDefaultScenes.Name = "lstDefaultScenes";
			this.lstDefaultScenes.Size = new System.Drawing.Size(359, 236);
			this.lstDefaultScenes.TabIndex = 1;
			this.lstDefaultScenes.UseCompatibleStateImageBehavior = false;
			this.lstDefaultScenes.View = System.Windows.Forms.View.Details;
			this.lstDefaultScenes.SelectedIndexChanged += new System.EventHandler(this.lstDefaultScenes_SelectedIndexChanged);
			this.lstDefaultScenes.DoubleClick += new System.EventHandler(this.lstDefaultScenes_DoubleClick);
			// 
			// colSceneName
			// 
			this.colSceneName.Text = "Name";
			this.colSceneName.Width = 140;
			// 
			// colFilename
			// 
			this.colFilename.Text = "Filename";
			this.colFilename.Width = 226;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 15);
			this.label4.TabIndex = 0;
			this.label4.Text = "Default scenes :";
			// 
			// tabSceneTree
			// 
			this.tabSceneTree.Controls.Add(this.splitContainer1);
			this.tabSceneTree.Location = new System.Drawing.Point(4, 24);
			this.tabSceneTree.Name = "tabSceneTree";
			this.tabSceneTree.Padding = new System.Windows.Forms.Padding(3);
			this.tabSceneTree.Size = new System.Drawing.Size(375, 299);
			this.tabSceneTree.TabIndex = 0;
			this.tabSceneTree.Text = "Scene Tree";
			this.tabSceneTree.UseVisualStyleBackColor = true;
			// 
			// tabOptions
			// 
			this.tabOptions.BackColor = System.Drawing.Color.Transparent;
			this.tabOptions.Controls.Add(this.chkUseCache);
			this.tabOptions.Controls.Add(this.grpOptionLight);
			this.tabOptions.Controls.Add(this.grpOptionAxis);
			this.tabOptions.Controls.Add(this.grpOptionGrid);
			this.tabOptions.Location = new System.Drawing.Point(4, 24);
			this.tabOptions.Name = "tabOptions";
			this.tabOptions.Padding = new System.Windows.Forms.Padding(3);
			this.tabOptions.Size = new System.Drawing.Size(375, 299);
			this.tabOptions.TabIndex = 1;
			this.tabOptions.Text = "Options";
			this.tabOptions.UseVisualStyleBackColor = true;
			// 
			// chkUseCache
			// 
			this.chkUseCache.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.chkUseCache.AutoSize = true;
			this.chkUseCache.Checked = true;
			this.chkUseCache.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkUseCache.Location = new System.Drawing.Point(8, 6);
			this.chkUseCache.Name = "chkUseCache";
			this.chkUseCache.Size = new System.Drawing.Size(123, 19);
			this.chkUseCache.TabIndex = 6;
			this.chkUseCache.Text = "Use model cache";
			this.chkUseCache.UseVisualStyleBackColor = true;
			this.chkUseCache.CheckedChanged += new System.EventHandler(this.chkUseCache_CheckedChanged);
			// 
			// grpOptionLight
			// 
			this.grpOptionLight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpOptionLight.Controls.Add(this.trkSpinningLight);
			this.grpOptionLight.Controls.Add(this.chkSpinningLight);
			this.grpOptionLight.Location = new System.Drawing.Point(8, 170);
			this.grpOptionLight.Name = "grpOptionLight";
			this.grpOptionLight.Size = new System.Drawing.Size(359, 61);
			this.grpOptionLight.TabIndex = 5;
			this.grpOptionLight.TabStop = false;
			this.grpOptionLight.Text = "Light";
			// 
			// trkSpinningLight
			// 
			this.trkSpinningLight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.trkSpinningLight.AutoSize = false;
			this.trkSpinningLight.Location = new System.Drawing.Point(113, 17);
			this.trkSpinningLight.Maximum = 360;
			this.trkSpinningLight.Minimum = -360;
			this.trkSpinningLight.Name = "trkSpinningLight";
			this.trkSpinningLight.Size = new System.Drawing.Size(240, 22);
			this.trkSpinningLight.TabIndex = 1;
			this.trkSpinningLight.Scroll += new System.EventHandler(this.trkSpinningLight_Scroll);
			// 
			// chkSpinningLight
			// 
			this.chkSpinningLight.AutoSize = true;
			this.chkSpinningLight.Location = new System.Drawing.Point(6, 20);
			this.chkSpinningLight.Name = "chkSpinningLight";
			this.chkSpinningLight.Size = new System.Drawing.Size(101, 19);
			this.chkSpinningLight.TabIndex = 0;
			this.chkSpinningLight.Text = "Spinning light";
			this.chkSpinningLight.UseVisualStyleBackColor = true;
			this.chkSpinningLight.CheckedChanged += new System.EventHandler(this.chkSpinningLight_CheckedChanged);
			// 
			// grpOptionAxis
			// 
			this.grpOptionAxis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpOptionAxis.Controls.Add(this.chkShowCurrentTransformAxis);
			this.grpOptionAxis.Controls.Add(this.chkShowParentTransformAxis);
			this.grpOptionAxis.Location = new System.Drawing.Point(8, 87);
			this.grpOptionAxis.Name = "grpOptionAxis";
			this.grpOptionAxis.Size = new System.Drawing.Size(359, 77);
			this.grpOptionAxis.TabIndex = 4;
			this.grpOptionAxis.TabStop = false;
			this.grpOptionAxis.Text = "Axis";
			// 
			// chkShowCurrentTransformAxis
			// 
			this.chkShowCurrentTransformAxis.AutoSize = true;
			this.chkShowCurrentTransformAxis.Checked = true;
			this.chkShowCurrentTransformAxis.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowCurrentTransformAxis.Location = new System.Drawing.Point(6, 20);
			this.chkShowCurrentTransformAxis.Name = "chkShowCurrentTransformAxis";
			this.chkShowCurrentTransformAxis.Size = new System.Drawing.Size(179, 19);
			this.chkShowCurrentTransformAxis.TabIndex = 1;
			this.chkShowCurrentTransformAxis.Text = "Show current transform axis";
			this.chkShowCurrentTransformAxis.UseVisualStyleBackColor = true;
			// 
			// chkShowParentTransformAxis
			// 
			this.chkShowParentTransformAxis.AutoSize = true;
			this.chkShowParentTransformAxis.Checked = true;
			this.chkShowParentTransformAxis.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowParentTransformAxis.Location = new System.Drawing.Point(6, 45);
			this.chkShowParentTransformAxis.Name = "chkShowParentTransformAxis";
			this.chkShowParentTransformAxis.Size = new System.Drawing.Size(176, 19);
			this.chkShowParentTransformAxis.TabIndex = 2;
			this.chkShowParentTransformAxis.Text = "Show parent transform axis";
			this.chkShowParentTransformAxis.UseVisualStyleBackColor = true;
			// 
			// grpOptionGrid
			// 
			this.grpOptionGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.grpOptionGrid.Controls.Add(this.chkShowGrid);
			this.grpOptionGrid.Location = new System.Drawing.Point(8, 31);
			this.grpOptionGrid.Name = "grpOptionGrid";
			this.grpOptionGrid.Size = new System.Drawing.Size(361, 50);
			this.grpOptionGrid.TabIndex = 3;
			this.grpOptionGrid.TabStop = false;
			this.grpOptionGrid.Text = "Grid";
			// 
			// chkShowGrid
			// 
			this.chkShowGrid.AutoSize = true;
			this.chkShowGrid.Checked = true;
			this.chkShowGrid.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowGrid.Location = new System.Drawing.Point(6, 20);
			this.chkShowGrid.Name = "chkShowGrid";
			this.chkShowGrid.Size = new System.Drawing.Size(83, 19);
			this.chkShowGrid.TabIndex = 0;
			this.chkShowGrid.Text = "Show Grid";
			this.chkShowGrid.UseVisualStyleBackColor = true;
			// 
			// frmSceneGraph
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(383, 327);
			this.Controls.Add(this.tabControl);
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSceneGraph";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Model Scene";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSceneGraph_FormClosing);
			this.grpTranslation.ResumeLayout(false);
			this.grpTranslation.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numTransZ)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numTransY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numTransX)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.grpScale.ResumeLayout(false);
			this.grpScale.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numScaleZ)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numScaleY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numScaleX)).EndInit();
			this.grpRotation.ResumeLayout(false);
			this.grpRotation.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRotate)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trkRotate)).EndInit();
			this.tabControl.ResumeLayout(false);
			this.tabScenes.ResumeLayout(false);
			this.tabScenes.PerformLayout();
			this.tabSceneTree.ResumeLayout(false);
			this.tabOptions.ResumeLayout(false);
			this.tabOptions.PerformLayout();
			this.grpOptionLight.ResumeLayout(false);
			this.grpOptionLight.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trkSpinningLight)).EndInit();
			this.grpOptionAxis.ResumeLayout(false);
			this.grpOptionAxis.PerformLayout();
			this.grpOptionGrid.ResumeLayout(false);
			this.grpOptionGrid.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView trvSceneGraph;
		private System.Windows.Forms.GroupBox grpTranslation;
		private System.Windows.Forms.NumericUpDown numTransZ;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown numTransY;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numTransX;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView lstTransforms;
		private System.Windows.Forms.ColumnHeader colTransformType;
		private System.Windows.Forms.ColumnHeader colTransformValues;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button btnDownTransform;
		private System.Windows.Forms.Button btnUpTransform;
		private System.Windows.Forms.Button btnRemoveTransform;
		private System.Windows.Forms.Button btnAddTransform;
		private System.Windows.Forms.GroupBox grpScale;
		private System.Windows.Forms.NumericUpDown numScaleZ;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.NumericUpDown numScaleY;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.NumericUpDown numScaleX;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.GroupBox grpRotation;
		private System.Windows.Forms.Label lblRotateZ;
		private System.Windows.Forms.Label lblRotateY;
		private System.Windows.Forms.Label lblRotateX;
		private System.Windows.Forms.TrackBar trkRotate;
		private System.Windows.Forms.Button btnSaveScene;
		private System.Windows.Forms.NumericUpDown numRotate;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabSceneTree;
		private System.Windows.Forms.TabPage tabOptions;
		internal System.Windows.Forms.CheckBox chkShowCurrentTransformAxis;
		internal System.Windows.Forms.CheckBox chkShowParentTransformAxis;
		internal System.Windows.Forms.CheckBox chkShowGrid;
		private System.Windows.Forms.TabPage tabScenes;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListView lstDefaultScenes;
		private System.Windows.Forms.ColumnHeader colSceneName;
		private System.Windows.Forms.ColumnHeader colFilename;
		private System.Windows.Forms.Button btnLoadScene;
		private System.Windows.Forms.Button btnLoadModel;
		private System.Windows.Forms.GroupBox grpOptionLight;
		private System.Windows.Forms.GroupBox grpOptionAxis;
		private System.Windows.Forms.GroupBox grpOptionGrid;
		internal System.Windows.Forms.CheckBox chkSpinningLight;
		private System.Windows.Forms.TrackBar trkSpinningLight;
		private System.Windows.Forms.CheckBox chkUseCache;
	}
}