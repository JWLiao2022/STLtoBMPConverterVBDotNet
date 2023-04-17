<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripLabel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.txtInputStlFileFullPath = New System.Windows.Forms.TextBox()
        Me.rbXZPlane = New System.Windows.Forms.RadioButton()
        Me.rbYZPlane = New System.Windows.Forms.RadioButton()
        Me.rbXYPlane = New System.Windows.Forms.RadioButton()
        Me.txtThicknessofDesignPatternum = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtTotalResistLayerThicknessum = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtImageWidthUM = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbExportStreamFiles = New System.Windows.Forms.RadioButton()
        Me.rbnExportBmp = New System.Windows.Forms.RadioButton()
        Me.rbnPreview = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtImageHeightUM = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cbResolutionUM = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cbQuality = New System.Windows.Forms.ComboBox()
        Me.rbInvertedYes = New System.Windows.Forms.RadioButton()
        Me.rbInvertedNo = New System.Windows.Forms.RadioButton()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.SaveFileDialog2 = New System.Windows.Forms.SaveFileDialog()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.UserControlDMOOxy1 = New WindowsControlLibraryDMOOxy.UserControlDMOOxy()
        Me.ToolStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button1.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.Button1.Location = New System.Drawing.Point(188, 605)
        Me.Button1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(210, 112)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Go!"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripProgressBar1, Me.ToolStripStatusLabel1})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 1012)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.ToolStrip1.Size = New System.Drawing.Size(1648, 38)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(100, 33)
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Font = New System.Drawing.Font("Times New Roman", 9.0!)
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(55, 33)
        Me.ToolStripStatusLabel1.Text = "Ready"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(555, 414)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(82, 37)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "..."
        Me.Button2.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "Image Files (*.bmp)|*.bmp"
        '
        'txtInputStlFileFullPath
        '
        Me.txtInputStlFileFullPath.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.txtInputStlFileFullPath.Location = New System.Drawing.Point(51, 418)
        Me.txtInputStlFileFullPath.Name = "txtInputStlFileFullPath"
        Me.txtInputStlFileFullPath.ReadOnly = True
        Me.txtInputStlFileFullPath.Size = New System.Drawing.Size(475, 25)
        Me.txtInputStlFileFullPath.TabIndex = 3
        Me.txtInputStlFileFullPath.Text = "Select the input stl file..."
        '
        'rbXZPlane
        '
        Me.rbXZPlane.AutoSize = True
        Me.rbXZPlane.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.rbXZPlane.Location = New System.Drawing.Point(388, 26)
        Me.rbXZPlane.Name = "rbXZPlane"
        Me.rbXZPlane.Size = New System.Drawing.Size(47, 23)
        Me.rbXZPlane.TabIndex = 27
        Me.rbXZPlane.Text = "xz"
        Me.rbXZPlane.UseVisualStyleBackColor = True
        '
        'rbYZPlane
        '
        Me.rbYZPlane.AutoSize = True
        Me.rbYZPlane.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.rbYZPlane.Location = New System.Drawing.Point(334, 26)
        Me.rbYZPlane.Name = "rbYZPlane"
        Me.rbYZPlane.Size = New System.Drawing.Size(47, 23)
        Me.rbYZPlane.TabIndex = 26
        Me.rbYZPlane.Text = "yz"
        Me.rbYZPlane.UseVisualStyleBackColor = True
        '
        'rbXYPlane
        '
        Me.rbXYPlane.AutoSize = True
        Me.rbXYPlane.Checked = True
        Me.rbXYPlane.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.rbXYPlane.Location = New System.Drawing.Point(279, 26)
        Me.rbXYPlane.Name = "rbXYPlane"
        Me.rbXYPlane.Size = New System.Drawing.Size(48, 23)
        Me.rbXYPlane.TabIndex = 24
        Me.rbXYPlane.TabStop = True
        Me.rbXYPlane.Text = "xy"
        Me.rbXYPlane.UseVisualStyleBackColor = True
        '
        'txtThicknessofDesignPatternum
        '
        Me.txtThicknessofDesignPatternum.Location = New System.Drawing.Point(507, 229)
        Me.txtThicknessofDesignPatternum.Name = "txtThicknessofDesignPatternum"
        Me.txtThicknessofDesignPatternum.Size = New System.Drawing.Size(132, 26)
        Me.txtThicknessofDesignPatternum.TabIndex = 23
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.Label4.Location = New System.Drawing.Point(88, 229)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(338, 19)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "Thickness of the highest part of the design pattern (um)"
        '
        'txtTotalResistLayerThicknessum
        '
        Me.txtTotalResistLayerThicknessum.Location = New System.Drawing.Point(507, 194)
        Me.txtTotalResistLayerThicknessum.Name = "txtTotalResistLayerThicknessum"
        Me.txtTotalResistLayerThicknessum.Size = New System.Drawing.Size(132, 26)
        Me.txtTotalResistLayerThicknessum.TabIndex = 21
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.Label3.Location = New System.Drawing.Point(240, 194)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(198, 19)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Total resist layer thickness (um)"
        '
        'txtImageWidthUM
        '
        Me.txtImageWidthUM.Location = New System.Drawing.Point(507, 112)
        Me.txtImageWidthUM.Name = "txtImageWidthUM"
        Me.txtImageWidthUM.Size = New System.Drawing.Size(132, 26)
        Me.txtImageWidthUM.TabIndex = 19
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.Label1.Location = New System.Drawing.Point(327, 112)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(115, 19)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Image width (um)"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbExportStreamFiles)
        Me.GroupBox1.Controls.Add(Me.rbnExportBmp)
        Me.GroupBox1.Controls.Add(Me.rbnPreview)
        Me.GroupBox1.Font = New System.Drawing.Font("Times New Roman", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(51, 471)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(632, 68)
        Me.GroupBox1.TabIndex = 30
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Export actions"
        '
        'rbExportStreamFiles
        '
        Me.rbExportStreamFiles.AutoSize = True
        Me.rbExportStreamFiles.Font = New System.Drawing.Font("Times New Roman", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbExportStreamFiles.Location = New System.Drawing.Point(440, 26)
        Me.rbExportStreamFiles.Name = "rbExportStreamFiles"
        Me.rbExportStreamFiles.Size = New System.Drawing.Size(146, 23)
        Me.rbExportStreamFiles.TabIndex = 2
        Me.rbExportStreamFiles.Text = "Export stream files"
        Me.rbExportStreamFiles.UseVisualStyleBackColor = True
        '
        'rbnExportBmp
        '
        Me.rbnExportBmp.AutoSize = True
        Me.rbnExportBmp.Font = New System.Drawing.Font("Times New Roman", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbnExportBmp.Location = New System.Drawing.Point(228, 26)
        Me.rbnExportBmp.Name = "rbnExportBmp"
        Me.rbnExportBmp.Size = New System.Drawing.Size(149, 23)
        Me.rbnExportBmp.TabIndex = 1
        Me.rbnExportBmp.Text = "Export .bmp image"
        Me.rbnExportBmp.UseVisualStyleBackColor = True
        '
        'rbnPreview
        '
        Me.rbnPreview.AutoSize = True
        Me.rbnPreview.Checked = True
        Me.rbnPreview.Font = New System.Drawing.Font("Times New Roman", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbnPreview.Location = New System.Drawing.Point(16, 26)
        Me.rbnPreview.Name = "rbnPreview"
        Me.rbnPreview.Size = New System.Drawing.Size(137, 23)
        Me.rbnPreview.TabIndex = 0
        Me.rbnPreview.TabStop = True
        Me.rbnPreview.Text = "Preview (default)"
        Me.rbnPreview.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.Label2.Location = New System.Drawing.Point(326, 152)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(117, 19)
        Me.Label2.TabIndex = 31
        Me.Label2.Text = "Image height (um)"
        '
        'txtImageHeightUM
        '
        Me.txtImageHeightUM.Location = New System.Drawing.Point(507, 152)
        Me.txtImageHeightUM.Name = "txtImageHeightUM"
        Me.txtImageHeightUM.Size = New System.Drawing.Size(132, 26)
        Me.txtImageHeightUM.TabIndex = 32
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.Label6.Location = New System.Drawing.Point(380, 28)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(72, 19)
        Me.Label6.TabIndex = 33
        Me.Label6.Text = "Resolution"
        '
        'cbResolutionUM
        '
        Me.cbResolutionUM.FormattingEnabled = True
        Me.cbResolutionUM.Items.AddRange(New Object() {"5um", "2um", "1um", "0.6um", "0.4um"})
        Me.cbResolutionUM.Location = New System.Drawing.Point(507, 23)
        Me.cbResolutionUM.Name = "cbResolutionUM"
        Me.cbResolutionUM.Size = New System.Drawing.Size(132, 28)
        Me.cbResolutionUM.TabIndex = 34
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.Label7.Location = New System.Drawing.Point(394, 69)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(52, 19)
        Me.Label7.TabIndex = 35
        Me.Label7.Text = "Quality"
        '
        'cbQuality
        '
        Me.cbQuality.FormattingEnabled = True
        Me.cbQuality.Items.AddRange(New Object() {"Fastest", "Normal", "High", "Super"})
        Me.cbQuality.Location = New System.Drawing.Point(507, 63)
        Me.cbQuality.Name = "cbQuality"
        Me.cbQuality.Size = New System.Drawing.Size(132, 28)
        Me.cbQuality.TabIndex = 36
        '
        'rbInvertedYes
        '
        Me.rbInvertedYes.AutoSize = True
        Me.rbInvertedYes.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.rbInvertedYes.Location = New System.Drawing.Point(274, 26)
        Me.rbInvertedYes.Name = "rbInvertedYes"
        Me.rbInvertedYes.Size = New System.Drawing.Size(56, 23)
        Me.rbInvertedYes.TabIndex = 38
        Me.rbInvertedYes.Text = "Yes"
        Me.rbInvertedYes.UseVisualStyleBackColor = True
        '
        'rbInvertedNo
        '
        Me.rbInvertedNo.AutoSize = True
        Me.rbInvertedNo.Checked = True
        Me.rbInvertedNo.Font = New System.Drawing.Font("Times New Roman", 7.8!)
        Me.rbInvertedNo.Location = New System.Drawing.Point(346, 26)
        Me.rbInvertedNo.Name = "rbInvertedNo"
        Me.rbInvertedNo.Size = New System.Drawing.Size(54, 23)
        Me.rbInvertedNo.TabIndex = 39
        Me.rbInvertedNo.TabStop = True
        Me.rbInvertedNo.Text = "No"
        Me.rbInvertedNo.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.rbInvertedNo)
        Me.GroupBox3.Controls.Add(Me.rbInvertedYes)
        Me.GroupBox3.Font = New System.Drawing.Font("Times New Roman", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(198, 332)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(462, 63)
        Me.GroupBox3.TabIndex = 32
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Inverted image (No as default)"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.rbXYPlane)
        Me.GroupBox4.Controls.Add(Me.rbYZPlane)
        Me.GroupBox4.Controls.Add(Me.rbXZPlane)
        Me.GroupBox4.Font = New System.Drawing.Font("Times New Roman", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(198, 265)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(462, 63)
        Me.GroupBox4.TabIndex = 40
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Projection plane (xy plane as default)"
        '
        'SaveFileDialog2
        '
        Me.SaveFileDialog2.Filter = "Stream files (*.str)|*.str"
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.White
        Me.GroupBox2.Controls.Add(Me.UserControlDMOOxy1)
        Me.GroupBox2.Location = New System.Drawing.Point(706, 28)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox2.Size = New System.Drawing.Size(924, 811)
        Me.GroupBox2.TabIndex = 42
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Preview image"
        '
        'UserControlDMOOxy1
        '
        Me.UserControlDMOOxy1.Location = New System.Drawing.Point(20, 41)
        Me.UserControlDMOOxy1.Name = "UserControlDMOOxy1"
        Me.UserControlDMOOxy1.ShowAnnotation = False
        Me.UserControlDMOOxy1.Size = New System.Drawing.Size(878, 733)
        Me.UserControlDMOOxy1.TabIndex = 43
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1648, 1050)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.cbQuality)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.cbResolutionUM)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtImageHeightUM)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtThicknessofDesignPatternum)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtTotalResistLayerThicknessum)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtImageWidthUM)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtInputStlFileFullPath)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox4)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "Form1"
        Me.Text = "STL to BMP or STR Converter"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripProgressBar1 As ToolStripProgressBar
    Friend WithEvents ToolStripStatusLabel1 As ToolStripLabel
    Friend WithEvents Button2 As Button
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents txtInputStlFileFullPath As TextBox
    Friend WithEvents rbXZPlane As RadioButton
    Friend WithEvents rbYZPlane As RadioButton
    Friend WithEvents rbXYPlane As RadioButton
    Friend WithEvents txtThicknessofDesignPatternum As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtTotalResistLayerThicknessum As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtImageWidthUM As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents rbnExportBmp As RadioButton
    Friend WithEvents rbnPreview As RadioButton
    Friend WithEvents Label2 As Label
    Friend WithEvents txtImageHeightUM As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents cbResolutionUM As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents cbQuality As ComboBox
    Friend WithEvents rbInvertedYes As RadioButton
    Friend WithEvents rbInvertedNo As RadioButton
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents rbExportStreamFiles As RadioButton
    Friend WithEvents SaveFileDialog2 As SaveFileDialog
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents UserControlDMOOxy1 As WindowsControlLibraryDMOOxy.UserControlDMOOxy
End Class
