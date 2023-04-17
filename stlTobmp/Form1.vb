Imports System.Collections.Concurrent
Imports System.IO

Public Class Form1

    Shared stlFileFullPath As String
    Shared isInverted As Boolean
    Shared greyScaleLevel As Integer
    Shared saveStreamFilePath As String
    Shared saveStreamFileNameWithoutExtension As String
    Shared saveStreamFileNameFullPath As String
    Shared bmpImageWidthPxs As Integer
    Shared bmpImageHeightPxs As Integer
    Shared paddedBMPImageWidthPxs As Integer
    Shared paddedBMPImageHeightPxs As Integer
    Shared eachBMPImageWidthPxs As Integer
    Shared eachBMPImageHeightPxs As Integer
    Shared bmpTotalPxs As Long
    Shared numbersOfProcessors As Integer = Environment.ProcessorCount
    Shared Numtri As UInt32
    Shared numOfTriPerProcessor As Integer
    Shared DMOHeatMapPlot As clsLibDMOOxy.clsDMOOxyPlotView
    Shared output1DByteArray() As Byte
    Public Shared output2DdblArray(,) As Double

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        UserControlDMOOxy1.XLinearAxis.Title = "Relative Position"
        UserControlDMOOxy1.YLinearAxis.Title = "Relative Position"


    End Sub


    Public Enum ProjectionPlane
        xy
        yz
        xz
    End Enum


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            stlFileFullPath = Path.GetFullPath(OpenFileDialog1.FileName)
        End If

        txtInputStlFileFullPath.Text = stlFileFullPath

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim saveFileNameFullPath As String
        Dim totalVortexesArray As Single(,) = New Single(,) {}
        Dim intResolution As Integer
        Dim intQuality As Integer
        Dim inputImageWidthUM As Double
        Dim inputImageHeightUM As Double
        Dim pixelSizeUMperPxs As Double
        Dim totalResistLayerThicknessUM As Single
        Dim patternThicknessUM As Single
        Dim bitmapPlane As ProjectionPlane
        Dim outputBMP(,) As Bitmap
        Dim output1DByteArray() As Byte
        Dim saveFilePath As String
        Dim saveFileNameWithoutExtension As String
        Dim sw As New Stopwatch
        Dim testTimems As Integer



        'User input area
        If cbResolutionUM.SelectedIndex > -1 Then
            intResolution = cbResolutionUM.SelectedIndex
        End If
        If cbQuality.SelectedIndex > -1 Then
            intQuality = cbQuality.SelectedIndex
        End If
        If IsNumeric(txtImageWidthUM.Text) = True Then
            inputImageWidthUM = txtImageWidthUM.Text
        End If
        If IsNumeric(txtImageHeightUM.Text) = True Then
            inputImageHeightUM = txtImageHeightUM.Text
        End If
        If IsNumeric(txtTotalResistLayerThicknessum.Text) = True Then
            totalResistLayerThicknessUM = txtTotalResistLayerThicknessum.Text
        End If
        If IsNumeric(txtThicknessofDesignPatternum.Text) = True Then
            patternThicknessUM = txtThicknessofDesignPatternum.Text
        End If
        If rbXYPlane.Checked = True Then
            bitmapPlane = ProjectionPlane.xy
        ElseIf rbYZPlane.Checked = True Then
            bitmapPlane = ProjectionPlane.yz
        ElseIf rbXZPlane.Checked = True Then
            bitmapPlane = ProjectionPlane.xz
        Else
            bitmapPlane = ProjectionPlane.xy
        End If
        If rbInvertedNo.Checked = True Then
            isInverted = False
        ElseIf rbInvertedYes.Checked = True Then
            isInverted = True
        Else
            isInverted = False
        End If

        'obtain the pixel size
        Try
            pixelSizeUMperPxs = fnReturnPixelSizeUM(intResolution, intQuality)
        Catch ex As Exception
            MessageBox.Show("Please select the resolution and the quality")
            Exit Sub
        End Try

        'calculate the image pixels
        bmpImageWidthPxs = Math.Ceiling(inputImageWidthUM / pixelSizeUMperPxs)
        bmpImageHeightPxs = Math.Ceiling(inputImageHeightUM / pixelSizeUMperPxs)


        'read the stl file 
        'save all the vortexes into a 2D array named totalVortexesArray
        'check if it is a binary or ascii file format
        'a ASCii fileformat is with the string "facet" in the second line
        'Some binary STL file only has one line
        Dim inputFileAllLines As String()
        Try
            inputFileAllLines = IO.File.ReadAllLines(stlFileFullPath)
        Catch ex As Exception
            MessageBox.Show("Please select the input STL file.")
            Exit Sub
        End Try

        Dim inputFileLinesNumbers As Integer
        Dim firstLine() As String
        Dim secondLine() As String

        inputFileLinesNumbers = inputFileAllLines.Length

        If inputFileLinesNumbers > 1 Then
            firstLine = Split(inputFileAllLines(0), " ")
            secondLine = Split(inputFileAllLines(1), " ")
        Else
            ReDim firstLine(0)
            ReDim secondLine(0)
            firstLine(0) = ""
            secondLine(0) = ""
        End If

        'If firstLine(0) = "STLB" Then
        '    Try
        '        totalVortexesArray = functionReadBinaryStlFile(stlFileFullPath)
        '    Catch ex As Exception
        '        MessageBox.Show("Please select a valid binary stl file.")
        '        txtInputStlFileFullPath.Text = "Select the input stl file..."
        '        Exit Sub
        '    End Try
        'ElseIf firstLine(0) = "STLA" Then
        '    Try
        '        totalVortexesArray = functionReadAsciiSTL(stlFileFullPath)
        '    Catch ex As Exception
        '        MessageBox.Show("Please select a valid ASCii stl file.")
        '        txtInputStlFileFullPath.Text = "Select the input stl file..."
        '        Exit Sub
        '    End Try
        'End If

        If firstLine(0) = "STLA" Or (firstLine(0) = "solid" And secondLine(0) = "facet") Then
                Try
                    totalVortexesArray = functionReadAsciiSTL(stlFileFullPath)
                Catch ex As Exception
                    MessageBox.Show("Please select a valid ASCii stl file.")
                    txtInputStlFileFullPath.Text = "Select the input stl file..."
                    Exit Sub
                End Try
            Else
                Try
                totalVortexesArray = functionReadBinaryStlFile(stlFileFullPath)
            Catch ex As Exception
                MessageBox.Show("Please select a valid binary stl file.")
                txtInputStlFileFullPath.Text = "Select the input stl file..."
                Exit Sub
            End Try
        End If

        ToolStripStatusLabel1.Text = "Start processing"
        ToolStrip1.Update()
        ToolStripProgressBar1.Value = 0

        Cursor = Cursors.WaitCursor
        Button1.Text = "Processing..."
        Button1.Enabled = False


        'business logic layer
        'produced bmp file based on the input sum 2D array
        'normalised the total resist thickness to 255
        'return nothing when one of the user input parameters is wrong

        If rbnExportBmp.Checked = True Then

            'examine the user input parameters
            If (bmpImageWidthPxs <= 0) Or (bmpImageHeightPxs <= 0) Then
                MessageBox.Show("The export image resolution needs to be larger than zero.")
                Exit Sub
            ElseIf ((bmpImageWidthPxs) > 10000) Or ((bmpImageHeightPxs) > 10000) Then
                MessageBox.Show("Please reduce the export image resolution lower than 10000.")
                Exit Sub
            ElseIf (totalResistLayerThicknessUM < 0) Or (totalResistLayerThicknessUM = 0) Then
                MessageBox.Show("The total resist layer thickness must be larger than zero.")
                Exit Sub
            ElseIf (patternThicknessUM < 0) Or (patternThicknessUM = 0) Then
                MessageBox.Show("The highest part of the design pattern needs to be larger than zero.")
                Exit Sub
            ElseIf (totalResistLayerThicknessUM < patternThicknessUM) Then
                MessageBox.Show("The total resist layer thickness should be larger than the highest part of the design pattern.")
                Exit Sub
            End If

            'save the bmp files
            If SaveFileDialog1.ShowDialog = DialogResult.Cancel Then
                Exit Sub
            End If

            saveFilePath = IO.Path.GetDirectoryName(SaveFileDialog1.FileName)

            'Conversion starts
            sw.Reset()
            sw.Start()

            greyScaleLevel = 256

            outputBMP = functionVortexestoBitmap(totalVortexesArray, totalResistLayerThicknessUM, patternThicknessUM, bitmapPlane)

            'when something wrong in the user input parameters, outputBMP = nothing
            If (outputBMP Is Nothing) Then
                Exit Sub
            End If

            For i = 0 To outputBMP.GetLength(0) - 1
                For j = 0 To outputBMP.GetLength(1) - 1
                    saveFileNameWithoutExtension = Path.GetFileNameWithoutExtension(SaveFileDialog1.FileName) + "(" + i.ToString + "," + j.ToString + ").bmp"
                    saveFileNameFullPath = Path.Combine(saveFilePath, saveFileNameWithoutExtension)
                    outputBMP(i, j).Save(saveFileNameFullPath, Drawing.Imaging.ImageFormat.Bmp)
                Next
            Next

            sw.Stop()
            testTimems = sw.ElapsedMilliseconds

            'reset the toolstrip
            ToolStripStatusLabel1.Text = "It took " & testTimems & " ms to process the file. Ready for the next file."
            ToolStrip1.Update()
            ToolStripProgressBar1.Value = 0



        ElseIf rbnPreview.Checked = True Then
            'in the preview mode
            'set the bmpImageResultionPxs = 128 to speed up the process
            'bmpImageResolutionPxs = 128

            greyScaleLevel = 128

            'Estimate the reduced width pxs and height pxs
            If bmpImageWidthPxs > bmpImageHeightPxs Then
                bmpImageHeightPxs = bmpImageHeightPxs / (bmpImageWidthPxs \ greyScaleLevel)
                bmpImageWidthPxs = greyScaleLevel

                'Give a small pxs value to height in the case of very different width and height 
                If bmpImageHeightPxs = 0 Then
                    bmpImageHeightPxs = 10
                End If
            ElseIf bmpImageWidthPxs < bmpImageHeightPxs Then
                bmpImageWidthPxs = bmpImageWidthPxs / (bmpImageHeightPxs \ greyScaleLevel)
                bmpImageHeightPxs = greyScaleLevel

                'Give a small pxs value to width in the case of very different width and height 
                If bmpImageWidthPxs = 0 Then
                    bmpImageWidthPxs = 10
                End If
            Else
                bmpImageWidthPxs = greyScaleLevel * 10
                bmpImageHeightPxs = greyScaleLevel * 10
            End If

            'examine the user input parameters
            If (bmpImageWidthPxs <= 0) Or (bmpImageHeightPxs <= 0) Then
                MessageBox.Show("The export image resolution needs to be larger than zero.")
                Exit Sub
            ElseIf ((bmpImageWidthPxs) > 10000) Or ((bmpImageHeightPxs) > 100000) Then
                MessageBox.Show("Please reduce the export image resolution lower than 10000, or choose to export stream files instead.")
                Exit Sub
            ElseIf (totalResistLayerThicknessUM < 0) Or (totalResistLayerThicknessUM = 0) Then
                MessageBox.Show("The total resist layer thickness must be larger than zero.")
                Exit Sub
            ElseIf (patternThicknessUM < 0) Or (patternThicknessUM = 0) Then
                MessageBox.Show("The highest part of the design pattern needs to be larger than zero.")
                Exit Sub
            ElseIf (totalResistLayerThicknessUM < patternThicknessUM) Then
                MessageBox.Show("The total resist layer thickness should be larger than the highest part of the design pattern.")
                Exit Sub
            End If

            eachBMPImageWidthPxs = bmpImageWidthPxs
            eachBMPImageHeightPxs = bmpImageHeightPxs

            'outputBMP = functionVortexestoBitmap(totalVortexesArray, totalResistLayerThicknessUM, patternThicknessUM, bitmapPlane)
            'pbPreviewBmp.Image = outputBMP(0, 0)

            'pbPreviewBmp.Width = greyScaleLevel * 2
            'pbPreviewBmp.Height = greyScaleLevel * 2

            'pbPreviewBmp.SizeMode = PictureBoxSizeMode.StretchImage

            'Output the plotview
            'DMOHeatMapPlot = New clsLibDMOOxy.clsDMOOxyPlotView(pvPreviewImage)

            ''ReDim output2DdblArray(bmpImageWidthPxs - 1, bmpImageHeightPxs - 1)


            subVortexestodbl2DArray(totalVortexesArray, totalResistLayerThicknessUM, patternThicknessUM, bitmapPlane)

            'If DMOHeatMapPlot.YLinearColourAxis.PlotModel Is Nothing Then
            '    DMOHeatMapPlot.DMOPlotModel.Axes.Add(DMOHeatMapPlot.YLinearColourAxis)
            'End If

            'DMOHeatMapPlot.XLinearAxis.Title = "Relative Position"
            'DMOHeatMapPlot.YLinearAxis.Title = "Relative Position"
            'DMOHeatMapPlot.YLinearColourAxis.Position = OxyPlot.Axes.AxisPosition.Right
            'DMOHeatMapPlot.YLinearColourAxis.Palette = OxyPlot.OxyPalettes.Gray(256)
            'DMOHeatMapPlot.YLinearColourAxis.Title = "Greyscale Level"
            'DMOHeatMapPlot.YLinearColourAxis.Maximum = 255
            'DMOHeatMapPlot.YLinearColourAxis.Minimum = 0

            'DMOHeatMapPlot.plotHeatmap(output2DdblArray)

            'pvPreviewImage.Width = 600
            'pvPreviewImage.Height = 500

            If UserControlDMOOxy1.YLinearColourAxis.PlotModel Is Nothing Then
                UserControlDMOOxy1.corePlotView.Model.Axes.Add(UserControlDMOOxy1.YLinearColourAxis)
            End If



            UserControlDMOOxy1.YLinearColourAxis.Position = OxyPlot.Axes.AxisPosition.Right
            UserControlDMOOxy1.YLinearColourAxis.Palette = OxyPlot.OxyPalettes.Gray(256)
            UserControlDMOOxy1.YLinearColourAxis.Maximum = 255
            UserControlDMOOxy1.YLinearColourAxis.Minimum = 0

            UserControlDMOOxy1.plotHeatmap(output2DdblArray)

            UserControlDMOOxy1.corePlotView.Width = 600
            UserControlDMOOxy1.corePlotView.Height = 900
            UserControlDMOOxy1.corePlotView.Update()




        ElseIf rbExportStreamFiles.Checked = True Then

                'Determine the file name and choose the output file save location
                If SaveFileDialog2.ShowDialog = DialogResult.Cancel Then
                Exit Sub
            End If

            saveStreamFilePath = IO.Path.GetDirectoryName(SaveFileDialog2.FileName)

            saveStreamFileNameWithoutExtension = Path.GetFileNameWithoutExtension(SaveFileDialog2.FileName) + ".str"
            saveStreamFileNameFullPath = Path.Combine(saveStreamFilePath, saveStreamFileNameWithoutExtension)

            'Conversion start
            sw.Reset()
            sw.Start()

            greyScaleLevel = 256

            'Get the 1D byte array from the vortexes array
            output1DByteArray = functionOutput1DByteArray(totalVortexesArray, bmpImageWidthPxs, bmpImageHeightPxs, totalResistLayerThicknessUM, patternThicknessUM, bitmapPlane)
            'Save the 1D byte array
            RussellsSub(output1DByteArray)

            sw.Stop()

            testTimems = sw.ElapsedMilliseconds

            'reset the toolstrip
            ToolStripStatusLabel1.Text = "It took " & testTimems & " ms to process the file. Ready for the next file."
            ToolStrip1.Update()
            ToolStripProgressBar1.Value = 0

        End If

        Cursor = Cursors.Arrow
        Button1.Enabled = True
        Button1.Text = "Go!"

        'txtInputStlFileFullPath.Text = "Select the input stl file..."

    End Sub

    Private Function fnReturnPixelSizeUM(ByVal chosenResolution As Integer, ByVal chosenQuality As Integer) As Double

        Dim returnedPixelSizeUM As Double

        If chosenResolution = 0 Then
            If chosenQuality = 0 Then
                returnedPixelSizeUM = 2.0
            ElseIf chosenQuality = 1 Then
                returnedPixelSizeUM = 2.0
            ElseIf chosenQuality = 2 Then
                returnedPixelSizeUM = 1.0
            ElseIf chosenQuality = 3 Then
                returnedPixelSizeUM = 0.5
            End If

        ElseIf chosenResolution = 1 Then
            If chosenQuality = 0 Then
                returnedPixelSizeUM = 1.0
            ElseIf chosenQuality = 1 Then
                returnedPixelSizeUM = 1.0
            ElseIf chosenQuality = 2 Then
                returnedPixelSizeUM = 0.5
            ElseIf chosenQuality = 3 Then
                returnedPixelSizeUM = 0.2
            End If

        ElseIf chosenResolution = 2 Then
            If chosenQuality = 0 Then
                returnedPixelSizeUM = 0.5
            ElseIf chosenQuality = 1 Then
                returnedPixelSizeUM = 0.5
            ElseIf chosenQuality = 2 Then
                returnedPixelSizeUM = 0.2
            ElseIf chosenQuality = 3 Then
                returnedPixelSizeUM = 0.1
            End If

        ElseIf chosenResolution = 3 Then
            If chosenQuality = 0 Then
                returnedPixelSizeUM = 0.5
            ElseIf chosenQuality = 1 Then
                returnedPixelSizeUM = 0.5
            ElseIf chosenQuality = 2 Then
                returnedPixelSizeUM = 0.2
            ElseIf chosenQuality = 3 Then
                returnedPixelSizeUM = 0.1
            End If

        ElseIf chosenResolution = 4 Then
            If chosenQuality = 0 Then
                returnedPixelSizeUM = 0.25
            ElseIf chosenQuality = 1 Then
                returnedPixelSizeUM = 0.25
            ElseIf chosenQuality = 2 Then
                returnedPixelSizeUM = 0.1
            ElseIf chosenQuality = 3 Then
                returnedPixelSizeUM = 0.1
            End If
        End If

        Return returnedPixelSizeUM

    End Function
    Private Function functionReadBinaryStlFile(ByVal stlFilePath As String) As Single(,)

        Dim header As String
        'Dim Numtri As UInt32
        Dim normalsArray(,) As Single
        Dim totalVortexesArray(,) As Single
        Dim atttr() As UInt16
        Dim finalPositon As Integer

        Using reader As New FileStream(stlFileFullPath, FileMode.Open, FileAccess.Read)
            Using rdr As New BinaryReader(reader)
                header = BitConverter.ToString(rdr.ReadBytes(80))
                Numtri = BitConverter.ToUInt32(rdr.ReadBytes(4), 0)

                ReDim normalsArray(Numtri - 1, 2)
                ReDim totalVortexesArray(3 * Numtri - 1, 2)
                ReDim atttr(Numtri - 1)

                finalPositon = 3 * (Numtri)

                For i = 0 To finalPositon - 3 Step 3
                    For j = 0 To 2
                        normalsArray(i / 3, j) = BitConverter.ToSingle(rdr.ReadBytes(4), 0)
                    Next
                    For k = 0 To 2
                        totalVortexesArray(i, k) = BitConverter.ToSingle(rdr.ReadBytes(4), 0)
                    Next
                    For l = 0 To 2
                        totalVortexesArray(i + 1, l) = BitConverter.ToSingle(rdr.ReadBytes(4), 0)
                    Next
                    For m = 0 To 2
                        totalVortexesArray(i + 2, m) = BitConverter.ToSingle(rdr.ReadBytes(4), 0)
                    Next
                    atttr(i / 3) = BitConverter.ToInt16(rdr.ReadBytes(2), 0)
                Next
            End Using
        End Using

        Return totalVortexesArray


    End Function

    Private Function functionReadAsciiSTL(ByVal stlFilePath As String) As Single(,)

        Dim inputFileLines = IO.File.ReadAllLines(stlFilePath)
        'Dim Numtri As Integer = (inputFileLines.GetLength(0) - 2) / 7
        Dim totalVortexArray(3 * Numtri - 1, 2) As Single
        Dim triCount As Integer

        For Each line As String In inputFileLines
            Dim lineElements() As String = line.Split(New String() {" "}, StringSplitOptions.None)
            If lineElements.Length = 10 Then
                If lineElements(6) = "vertex" Then
                    For i = 0 To 2
                        totalVortexArray(triCount, i) = lineElements(i + 7)
                    Next
                    triCount += 1
                End If
            End If
        Next

        functionReadAsciiSTL = totalVortexArray

    End Function

    Private Sub subVortexestodbl2DArray(ByVal totalVortexesArray(,) As Single, ByVal totalResistLayerThicknessUM As Single, ByVal PatternThicknessUM As Single, ByVal bmpProjectionPlane As ProjectionPlane)

        Dim shiftedandScaledTrinalgMeshArray(,) As Single
        Dim thresholdResolutionPxs As Integer
        Dim linesList As New List(Of List(Of Tuple(Of Single, Single, Single)))
        Dim sum1DArray() As Byte
        Dim normSum1DArray() As Byte
        Dim ratioPatternToResistThickness As Single = PatternThicknessUM / totalResistLayerThicknessUM
        Dim int1DArrayLength As Integer

        thresholdResolutionPxs = 10000
        subCalculateBmpSizes(thresholdResolutionPxs, 0, 0)

        shiftedandScaledTrinalgMeshArray = functionShiftandScaleArray(totalVortexesArray, bmpProjectionPlane, ratioPatternToResistThickness)

        sum1DArray = functionListTupletoVortexArrayXY(shiftedandScaledTrinalgMeshArray, 0, 0)
        normSum1DArray = functionCalNormSum1DArray(sum1DArray, totalResistLayerThicknessUM, PatternThicknessUM)

        int1DArrayLength = sum1DArray.Length

        ReDim output2DdblArray(eachBMPImageWidthPxs - 1, eachBMPImageHeightPxs - 1)

        For j = 0 To eachBMPImageHeightPxs - 1
            For i = 0 To eachBMPImageWidthPxs - 1
                output2DdblArray(i, eachBMPImageHeightPxs - 1 - j) = normSum1DArray(i + (j * eachBMPImageWidthPxs))
            Next
        Next


    End Sub

    Private Function functionVortexestoBitmap(ByVal totalVortexesArray(,) As Single, ByVal totalResistLayerThicknessUM As Single, ByVal PatternThicknessUM As Single, ByVal bmpProjectionPlane As ProjectionPlane) As Bitmap(,)

        Dim shiftedandScaledTrinalgMeshArray(,) As Single
        Dim thresholdResolutionPxs As Integer
        Dim linesList As New List(Of List(Of Tuple(Of Single, Single, Single)))
        Dim sum1DArray() As Byte
        Dim bmp(,) As Bitmap
        Dim bmpRowNumbers As Integer
        Dim bmpColumnNumbers As Integer
        Dim ratioPatternToResistThickness As Single = PatternThicknessUM / totalResistLayerThicknessUM
        Dim int1DArrayLength As Integer




        'calculate numbers of pixels of each output bmp file and total numbers of output bmp files
        'threshold resolution pxs is the largest pxs numbers each bmp file has.
        'If total pxs > this threshold pxs, there will be more than one bmp files generated.
        thresholdResolutionPxs = 10000
        subCalculateBmpSizes(thresholdResolutionPxs, bmpRowNumbers, bmpColumnNumbers)


        'shift and scale the raw vortexes with the bmp image resolution
        'return a list of tuple name shiftedandScaledTriangleMesh and an array bounding pixels in x, y, and z named bondingBox
        shiftedandScaledTrinalgMeshArray = functionShiftandScaleArray(totalVortexesArray, bmpProjectionPlane, ratioPatternToResistThickness)


        'output bmp base on the color code stored in sum2DArray
        'plot bmp one by one to avoid creating a array with a too large size causing the system out of memory error
        'use multiple threads to speed up the process
        ReDim bmp(bmpColumnNumbers - 1, bmpRowNumbers - 1)

        For i = 0 To bmpColumnNumbers - 1
            For j = 0 To bmpRowNumbers - 1

                'sum2DArray = functionListTupletoVortexArrayXY(shiftedandScaledTrinalgMeshArray, eachBmpImageWidthPxs, eachBmpImageHeightPxs, paddedImageWidthPxs, paddedImageHeightPxs, i, j)
                sum1DArray = functionListTupletoVortexArrayXY(shiftedandScaledTrinalgMeshArray, i, j)

                'bmp(i, j) = functionBmpGenerator(sum2DArray, i, j, eachBmpImageWidthPxs, eachBmpImageHeightPxs, totalResistLayerThicknessUM, PatternThicknessUM)
                bmp(i, j) = functionBmpGenerator(sum1DArray, i, j, totalResistLayerThicknessUM, PatternThicknessUM)
            Next
        Next

        Return bmp

    End Function

    Private Function functionOutput1DByteArray(ByVal totalVortexesArray(,) As Single, ByVal bmpImageWidthPxs As Int64, ByVal bmpImageHeightPxs As Int64, ByVal totalResistLayerThicknessUM As Single, ByVal PatternThicknessUM As Single, ByVal bmpProjectionPlane As ProjectionPlane) As Byte()

        Dim shiftedandScaledTrinalgMeshArray(,) As Single
        Dim sum1DArray() As Byte
        Dim normSum1DArray() As Byte
        Dim ratioPatternToResistThickness As Single = PatternThicknessUM / totalResistLayerThicknessUM


        eachBMPImageWidthPxs = bmpImageWidthPxs
        eachBMPImageHeightPxs = bmpImageHeightPxs
        paddedBMPImageWidthPxs = bmpImageWidthPxs
        paddedBMPImageHeightPxs = bmpImageHeightPxs

        bmpTotalPxs = bmpImageWidthPxs * bmpImageHeightPxs

        shiftedandScaledTrinalgMeshArray = functionShiftandScaleArray(totalVortexesArray, bmpProjectionPlane, ratioPatternToResistThickness)

        'Convert Vortexes array into a 1D integer array
        sum1DArray = functionListTupletoVortexArrayXY(shiftedandScaledTrinalgMeshArray, 0, 0)

        'Normilise the 1D array to 0 - 255 and the pattern thickness to the resist thickness ratio
        normSum1DArray = functionCalNormSum1DArray(sum1DArray, totalResistLayerThicknessUM, PatternThicknessUM)

        Return normSum1DArray

    End Function

    Private Function functionShiftandScaleArray(ByVal totalVortexesArray(,) As Single, ByVal BitmapProjectionPlane As ProjectionPlane, ByVal ratioPatternToResistThickness As Single) As Single(,)

        'Dim Numtri As UInt32
        Dim totalTriPoints As UInt32 = Numtri * 3
        Dim mins(2) As Single
        Dim maxs(2) As Single
        Dim shift(2) As Single
        Dim xyScale As Single
        Dim yzScale As Single
        Dim xzScale As Single
        Dim planeNormalScale As Single
        Dim scale() As Single
        Dim shiftedandScaledVortexesArray As Single(,) = New Single(3 * Numtri - 1, 2) {}
        Dim numTriPointsPerPrecessor As Integer = totalTriPoints \ numbersOfProcessors

        'find the min values and the max values of x, y, and z, in the order of x, y, and z

        For i = 0 To totalTriPoints - 1
            For j = 0 To 2
                If totalVortexesArray(i, j) < mins(j) Then
                    mins(j) = totalVortexesArray(i, j)
                End If
                If totalVortexesArray(i, j) > maxs(j) Then
                    maxs(j) = totalVortexesArray(i, j)
                End If
            Next
        Next


        If BitmapProjectionPlane = ProjectionPlane.xy Then
            'rescale the vertexes points to the resolution of the output bitmap file
            If (maxs(0) - mins(0)) > (maxs(1) - mins(1)) Then
                xyScale = (paddedBMPImageWidthPxs - 1) / (maxs(0) - mins(0))
            Else
                xyScale = (paddedBMPImageHeightPxs - 1) / (maxs(1) - mins(1))
            End If

            planeNormalScale = ((greyScaleLevel - 1) / (maxs(2) - mins(2)))

            'calculate the shift and put the image at the center of the padded bmp pixels
            'paddedBMPImageWidthPxs = Math.Ceiling((paddedBMPImageWidthPxs - bmpImageWidthPxs) / 2)
            'paddedBMPImageHeightPxs = Math.Ceiling((paddedBMPImageHeightPxs - bmpImageHeightPxs) / 2)



            scale = New Single(2) {xyScale, xyScale, planeNormalScale}

            'ReDim shiftedandScaledVortexesArray(totalTriPoints - 1, 2)

            shift(0) = -mins(0) '+ (paddedBMPImageWidthPxs / scale(0))
            shift(1) = -mins(1) '+ (paddedBMPImageHeightPxs / scale(1))
            shift(2) = -mins(2)

            For i = 0 To totalTriPoints - 1
                For j = 0 To 2
                    shiftedandScaledVortexesArray(i, j) = (totalVortexesArray(i, j) + shift(j)) * scale(j)
                Next
            Next


        ElseIf BitmapProjectionPlane = ProjectionPlane.yz Then
            'rescale the vertexes points to the resolution of the output bitmap file
            If (maxs(1) - mins(1)) > (maxs(2) - mins(2)) Then
                yzScale = (paddedBMPImageHeightPxs - 1) / (maxs(1) - mins(1))
            Else
                yzScale = (paddedBMPImageWidthPxs - 1) / (maxs(2) - mins(2))
            End If

            planeNormalScale = ((greyScaleLevel - 1) / (maxs(0) - mins(0)))

            'calculate the shift and put the image at the center of the padded bmp pixels
            'paddedBMPImageWidthPxs = Math.Ceiling((paddedBMPImageWidthPxs - bmpImageWidthPxs) / 2)
            'paddedBMPImageHeightPxs = Math.Ceiling((paddedBMPImageHeightPxs - bmpImageHeightPxs) / 2)


            scale = New Single(2) {planeNormalScale, yzScale, yzScale}
            'ReDim shiftedandScaledVortexesArray(3 * Numtri - 1, 2)

            shift(1) = -mins(1) '+ (paddedBMPImageWidthPxs / scale(1))
            shift(2) = -mins(2) '+ (paddedBMPImageHeightPxs / scale(2))
            shift(0) = -mins(0)

            For i = 0 To totalTriPoints - 1
                For j = 0 To 2
                    shiftedandScaledVortexesArray(i, 0) = (totalVortexesArray(i, 2) + shift(2)) * scale(2)
                    shiftedandScaledVortexesArray(i, 1) = (totalVortexesArray(i, 1) + shift(1)) * scale(1)
                    shiftedandScaledVortexesArray(i, 2) = (totalVortexesArray(i, 0) + shift(0)) * scale(0)
                Next
            Next


        ElseIf BitmapProjectionPlane = ProjectionPlane.xz Then
            'rescale the vertexes points to the resolution of the output bitmap file
            If (maxs(0) - mins(0)) > (maxs(2) - mins(2)) Then
                xzScale = (paddedBMPImageWidthPxs - 1) / (maxs(0) - mins(0))
            Else
                xzScale = (paddedBMPImageHeightPxs - 1) / (maxs(2) - mins(2))
            End If

            planeNormalScale = ((greyScaleLevel - 1) / (maxs(1) - mins(1)))

            'calculate the shift and put the image at the center of the padded bmp pixels
            'paddedBMPImageWidthPxs = Math.Ceiling((paddedBMPImageWidthPxs - bmpImageWidthPxs) / 2)
            'paddedBMPImageHeightPxs = Math.Ceiling((paddedBMPImageHeightPxs - bmpImageHeightPxs) / 2)

            scale = New Single(2) {xzScale, planeNormalScale, xzScale}

            shift(0) = -mins(0) '+ (paddedBMPImageWidthPxs / scale(0))
            shift(2) = -mins(2) '+ (paddedBMPImageHeightPxs / scale(2))
            shift(1) = -mins(1)

            'ReDim shiftedandScaledVortexesArray(3 * Numtri - 1, 2)

            'change (x, y, z) to (z, x, y)

            For i = 0 To totalTriPoints - 1
                For j = 0 To 2
                    shiftedandScaledVortexesArray(i, 0) = (totalVortexesArray(i, 2) + shift(2)) * scale(2)
                    shiftedandScaledVortexesArray(i, 1) = (totalVortexesArray(i, 0) + shift(0)) * scale(0)
                    shiftedandScaledVortexesArray(i, 2) = (totalVortexesArray(i, 1) + shift(1)) * scale(1)
                Next
            Next

        End If

        Return shiftedandScaledVortexesArray


    End Function

    Private Sub subCalculateBmpSizes(ByVal thresholdPxs As Integer, ByRef bmpRowNumbers As Integer, ByRef bmpColumnNumbers As Integer)

        bmpRowNumbers = Math.Ceiling(bmpImageHeightPxs / thresholdPxs)
        bmpColumnNumbers = Math.Ceiling(bmpImageWidthPxs / thresholdPxs)

        eachBMPImageHeightPxs = Math.Ceiling(bmpImageHeightPxs / bmpRowNumbers)
        paddedBMPImageHeightPxs = eachBMPImageHeightPxs * bmpRowNumbers

        eachBMPImageWidthPxs = Math.Ceiling(bmpImageWidthPxs / bmpColumnNumbers)
        paddedBMPImageWidthPxs = eachBMPImageWidthPxs * bmpColumnNumbers

    End Sub

    Private Function functionListTupletoVortexArrayXY(ByVal shiftedandScaledTrinalgMesh As Single(,), ByVal currentBMPColumnNumbers As Integer, ByVal currentBMPRowNumbers As Integer) As Byte()

        bmpTotalPxs = eachBMPImageWidthPxs * eachBMPImageHeightPxs

        Dim returnedSum1DArray(bmpTotalPxs - 1) As Byte
        Dim prePixels1D(bmpTotalPxs - 1) As Boolean
        Dim linesList() As Single
        Dim totalProcessingHeightLayers As Integer

        Dim sizeForEachProcessor As Integer

        Dim sw2 As New Stopwatch
        Dim timeMS As Integer

        totalProcessingHeightLayers = greyScaleLevel

        'serial for
        For h = 0 To totalProcessingHeightLayers - 1

            sw2.Reset()
            sw2.Start()

            ReDim prePixels1D(bmpTotalPxs - 1)

            'find the intersection line on the plane at the particular heigth h
            'the line is represented by a list of two points; each point is represented by a tuple (x, y, z)
            'linesList = functionLinesSingleArrayOnZHeightParallelFor(shiftedandScaledTrinalgMesh, h)
            linesList = functionLinesSingleArrayOnZHeightSerialFor(shiftedandScaledTrinalgMesh, h)

            'convert the intersection line into pixels
            'prePixels1D = functionLinesToVoxels(linesList, currentBMPColumnNumbers, currentBMPRowNumbers)
            'prePixels1D = serialForfunctionLinesToVoxels(linesList, currentBMPColumnNumbers, currentBMPRowNumbers)
            prePixels1D = functionLinesToVoxelsAlongX(linesList, currentBMPColumnNumbers, currentBMPRowNumbers)
            'prePixels1D = functionLinesToVoxelsAlongXSerialFor(linesList, currentBMPColumnNumbers, currentBMPRowNumbers)

            sizeForEachProcessor = bmpTotalPxs / numbersOfProcessors

            'project the pixels at h onto the xy plane

            Parallel.For(0, numbersOfProcessors, Sub(i)
                                                     Dim initialArrayPosition As Integer = i * sizeForEachProcessor
                                                     Dim finalArrayPosition As Integer

                                                     'initialArrayPosition = i * sizeForEachProcessor

                                                     If i = (numbersOfProcessors - 1) Then
                                                         finalArrayPosition = bmpTotalPxs
                                                     Else
                                                         finalArrayPosition = initialArrayPosition + sizeForEachProcessor
                                                     End If

                                                     For j = initialArrayPosition To finalArrayPosition - 1
                                                         If prePixels1D(j) And returnedSum1DArray(j) < 255 Then
                                                             returnedSum1DArray(j) += 1
                                                         End If
                                                     Next

                                                 End Sub)

            sw2.Stop()
            timeMS = sw2.ElapsedMilliseconds

            ToolStripStatusLabel1.Text = "Processed layer " & (h + 1) & "/" & (totalProcessingHeightLayers) & ". It took " & timeMS & " ms."
            ToolStripProgressBar1.Maximum = totalProcessingHeightLayers
            ToolStripProgressBar1.Value = h + 1
            ToolStrip1.Update()

        Next

        Return returnedSum1DArray

    End Function


    Private Function functionBmpGenerator(ByVal Sum1DArray() As Byte, ByVal bmpColumnNumber As Integer, ByVal bmpRowNumber As Integer, ByVal totalResistLayerThicknessUM As Single, ByVal patternThicknessUM As Single) As Bitmap

        Dim normSum2DArray(eachBMPImageWidthPxs - 1, eachBMPImageHeightPxs - 1) As Byte
        'Dim totalPixels As Long = eachBMPImageWidthPxs * eachBMPImageHeightPxs
        Dim normSum1DArray() As Byte
        Dim output1DByteArray(bmpTotalPxs - 1) As Byte
        Dim bmp As Bitmap
        Dim g As Graphics
        Dim colorCode As Byte


        'normSum2DArray = functionCalNormSum2DArray(Sum2DArray, eachBMPImageWidthPxs, eachBMPImageHeightPxs, totalResistLayerThicknessUM, patternThicknessUM)
        normSum1DArray = functionCalNormSum1DArray(Sum1DArray, totalResistLayerThicknessUM, patternThicknessUM)

        'generate the bmp file 
        'plot each output bmp by setting color code of each pixel
        'color code information of each pixel is stored in normSum2DArray

        bmp = New Bitmap(eachBMPImageWidthPxs, eachBMPImageHeightPxs, Imaging.PixelFormat.Format24bppRgb)
        g = Graphics.FromImage(bmp)

        'g.Clear(Color.White)

        For k = 0 To eachBMPImageHeightPxs - 1
            For m = 0 To eachBMPImageWidthPxs - 1
                'colorCode = normSum2DArray(m, k)
                colorCode = normSum1DArray(m + (k * eachBMPImageWidthPxs))
                bmp.SetPixel(m, k, Color.FromArgb(colorCode, colorCode, colorCode))
            Next
        Next



        Return bmp

    End Function
    Private Sub RussellsSub(ByVal output1DByteArray() As Byte)
        Dim fs As IO.FileStream
        Dim bmpImageWidthByte() As Byte
        Dim bmpImageHeightByte() As Byte

        'Convert the image width and height into bytes
        bmpImageWidthByte = BitConverter.GetBytes(eachBMPImageWidthPxs)
        bmpImageHeightByte = BitConverter.GetBytes(eachBMPImageHeightPxs)

        'Write the image width and image height into the file first
        fs = New IO.FileStream(saveStreamFileNameFullPath, FileMode.OpenOrCreate)

        fs.Write(bmpImageWidthByte, 0, 4)
        fs.Write(bmpImageHeightByte, 0, 4)
        fs.Write(output1DByteArray, 0, bmpTotalPxs)

        fs.Close()

    End Sub
    Private Function functionLinesSingleArrayOnZHeightParallelFor(ByVal triangleMeshArray(,) As Single, ByVal h As Integer) As Single()

        Dim finalLinesList() As Single
        Dim totalArraySize As Integer
        Dim arrayNumOfLines(numbersOfProcessors - 1) As Integer
        Dim totalNumOfLines As Integer
        Dim parallelFortempLineList(numbersOfProcessors, (6 * Numtri) - 1) As Single

        numOfTriPerProcessor = Numtri \ numbersOfProcessors

        Parallel.For(0, numbersOfProcessors, Sub(p)
                                                 Dim initialPosition As Integer
                                                 Dim finalPosition As Integer
                                                 Dim firstSingleArrayPointCrossZPlane(2) As Single
                                                 Dim secondsingleArrayPointCrossZPlane(2) As Single
                                                 Dim firstTempSingleArrayPoint(2) As Single
                                                 Dim secondTempSingleArrayPoint(2) As Single

                                                 initialPosition = p * numOfTriPerProcessor

                                                 If p = numbersOfProcessors - 1 Then
                                                     finalPosition = Numtri
                                                 Else
                                                     finalPosition = initialPosition + numOfTriPerProcessor
                                                 End If

                                                 For i = initialPosition To finalPosition - 1
                                                     Dim AboveHeight(8) As Single
                                                     Dim EqualHeight(8) As Single
                                                     Dim BelowHeight(8) As Single
                                                     Dim currentPosition As Integer
                                                     Dim tempNumbersOfPoints As Integer
                                                     Dim tempNumbersofLines As Integer
                                                     Dim numbersOfPointsAboveHeight As New Integer
                                                     Dim numbersOfPointsEqualHeight As New Integer
                                                     Dim numbersOfPointsBelowHeight As New Integer

                                                     For j = 0 To 2
                                                         currentPosition = (3 * i) + j

                                                         If triangleMeshArray(currentPosition, 2) > h Then
                                                             'one d array with (x1, y1, z1) represents a coordination point
                                                             tempNumbersOfPoints = numbersOfPointsAboveHeight * 3
                                                             For k = 0 To 2
                                                                 AboveHeight((tempNumbersOfPoints) + k) = triangleMeshArray(currentPosition, k)
                                                             Next

                                                             numbersOfPointsAboveHeight += 1

                                                         ElseIf triangleMeshArray(currentPosition, 2) < h Then
                                                             tempNumbersOfPoints = numbersOfPointsBelowHeight * 3
                                                             For k = 0 To 2
                                                                 BelowHeight((tempNumbersOfPoints) + k) = triangleMeshArray(currentPosition, k)
                                                             Next

                                                             numbersOfPointsBelowHeight += 1

                                                         Else
                                                             tempNumbersOfPoints = numbersOfPointsEqualHeight * 3
                                                             For k = 0 To 2
                                                                 EqualHeight((tempNumbersOfPoints) + k) = triangleMeshArray(currentPosition, k)
                                                             Next

                                                             numbersOfPointsEqualHeight += 1

                                                         End If
                                                     Next

                                                     tempNumbersofLines = arrayNumOfLines(p) * 6
                                                     'remove equalHeight.Count = 3 to have equilivant function to notSameTriangles(?)
                                                     If numbersOfPointsEqualHeight = 2 Then

                                                         For k = 0 To 5
                                                             'tempLinesList((tempNumbersofLines) + k) = EqualHeight(k)
                                                             parallelFortempLineList(p, tempNumbersofLines + k) = EqualHeight(k)
                                                         Next

                                                         'numbersOfLines += 1
                                                         arrayNumOfLines(p) += 1

                                                     ElseIf (numbersOfPointsAboveHeight = 1 And numbersOfPointsBelowHeight = 1) And (numbersOfPointsEqualHeight = 1) Then

                                                         firstSingleArrayPointCrossZPlane = functionSingleArrayPointCrossesZ(BelowHeight, AboveHeight, h)

                                                         For k = 0 To 2
                                                             'tempLinesList((tempNumbersofLines) + k) = EqualHeight(k)
                                                             'tempLinesList((tempNumbersofLines) + k + 3) = firstSingleArrayPointCrossZPlane(k)
                                                             parallelFortempLineList(p, tempNumbersofLines + k) = EqualHeight(k)
                                                             parallelFortempLineList(p, tempNumbersofLines + k + 3) = firstSingleArrayPointCrossZPlane(k)
                                                         Next

                                                         'numbersOfLines += 1
                                                         arrayNumOfLines(p) += 1

                                                     ElseIf (numbersOfPointsAboveHeight > 0 And numbersOfPointsBelowHeight > 0) And (numbersOfPointsEqualHeight = 0) Then

                                                         If numbersOfPointsAboveHeight = 2 Then
                                                             For k = 0 To 2
                                                                 firstTempSingleArrayPoint(k) = AboveHeight(k)
                                                                 secondTempSingleArrayPoint(k) = AboveHeight(k + 3)
                                                             Next

                                                             firstSingleArrayPointCrossZPlane = functionSingleArrayPointCrossesZ(BelowHeight, firstTempSingleArrayPoint, h)
                                                             secondsingleArrayPointCrossZPlane = functionSingleArrayPointCrossesZ(BelowHeight, secondTempSingleArrayPoint, h)

                                                         ElseIf numbersOfPointsBelowHeight = 2 Then
                                                             For k = 0 To 2
                                                                 firstTempSingleArrayPoint(k) = BelowHeight(k)
                                                                 secondTempSingleArrayPoint(k) = BelowHeight(k + 3)
                                                             Next

                                                             firstSingleArrayPointCrossZPlane = functionSingleArrayPointCrossesZ(firstTempSingleArrayPoint, AboveHeight, h)
                                                             secondsingleArrayPointCrossZPlane = functionSingleArrayPointCrossesZ(secondTempSingleArrayPoint, AboveHeight, h)

                                                         End If

                                                         For k = 0 To 2
                                                             'tempLinesList((tempNumbersofLines) + k) = firstSingleArrayPointCrossZPlane(k)
                                                             'tempLinesList((tempNumbersofLines) + k + 3) = secondsingleArrayPointCrossZPlane(k)
                                                             parallelFortempLineList(p, tempNumbersofLines + k) = firstSingleArrayPointCrossZPlane(k)
                                                             parallelFortempLineList(p, tempNumbersofLines + k + 3) = secondsingleArrayPointCrossZPlane(k)
                                                         Next

                                                         'numbersOfLines += 1
                                                         arrayNumOfLines(p) += 1

                                                     End If
                                                 Next

                                             End Sub)


        totalNumOfLines = arrayNumOfLines.Sum
        totalArraySize = totalNumOfLines * 6

        ReDim finalLinesList(totalArraySize - 1)

        Parallel.For(0, numbersOfProcessors, Sub(i)
                                                 Dim parallelforInitialArrayPosition As Integer
                                                 Dim parallelforFinalArrayPosition As Integer

                                                 parallelforInitialArrayPosition = 0

                                                 If i > 0 Then
                                                     For j = 0 To i - 1
                                                         parallelforInitialArrayPosition += arrayNumOfLines(j) * 6
                                                     Next
                                                 End If

                                                 parallelforFinalArrayPosition = parallelforInitialArrayPosition + arrayNumOfLines(i) * 6

                                                 For k = parallelforInitialArrayPosition To parallelforFinalArrayPosition - 1
                                                     finalLinesList(k) = parallelFortempLineList(i, k - parallelforInitialArrayPosition)
                                                 Next

                                             End Sub)


        Return finalLinesList

    End Function

    Private Function functionLinesSingleArrayOnZHeightSerialFor(ByVal triangleMeshArray(,) As Single, ByVal h As Integer) As Single()

        Dim finalLinesList() As Single
        Dim totalArraySize As Integer
        Dim tempLinesList((6 * Numtri) - 1) As Single
        Dim numbersOfPointsAboveHeight As Integer
        Dim numbersOfPointsEqualHeight As Integer
        Dim numbersOfPointsBelowHeight As Integer
        Dim numbersOfLines As Integer
        Dim firstSingleArrayPointCrossZPlane(2) As Single
        Dim secondsingleArrayPointCrossZPlane(2) As Single
        Dim firstTempSingleArrayPoint(2) As Single
        Dim secondTempSingleArrayPoint(2) As Single
        Dim totalArraySizeByte As Integer

        For i = 0 To Numtri - 1
            Dim AboveHeight(8) As Single
            Dim EqualHeight(8) As Single
            Dim BelowHeight(8) As Single
            Dim currentPosition As Integer
            Dim tempNumbersOfPoints As Integer
            Dim tempNumbersofLines As Integer

            numbersOfPointsAboveHeight = 0
            numbersOfPointsEqualHeight = 0
            numbersOfPointsBelowHeight = 0

            For j = 0 To 2
                currentPosition = (3 * i) + j

                If triangleMeshArray(currentPosition, 2) > h Then
                    'one d array with (x1, y1, z1) represents a coordination point
                    tempNumbersOfPoints = numbersOfPointsAboveHeight * 3
                    For k = 0 To 2
                        AboveHeight((tempNumbersOfPoints) + k) = triangleMeshArray(currentPosition, k)
                    Next

                    numbersOfPointsAboveHeight += 1

                ElseIf triangleMeshArray(currentPosition, 2) < h Then
                    tempNumbersOfPoints = numbersOfPointsBelowHeight * 3
                    For k = 0 To 2
                        BelowHeight((tempNumbersOfPoints) + k) = triangleMeshArray(currentPosition, k)
                    Next

                    numbersOfPointsBelowHeight += 1

                Else
                    tempNumbersOfPoints = numbersOfPointsEqualHeight * 3
                    For k = 0 To 2
                        EqualHeight((tempNumbersOfPoints) + k) = triangleMeshArray(currentPosition, k)
                    Next

                    numbersOfPointsEqualHeight += 1

                End If
            Next

            tempNumbersofLines = numbersOfLines * 6
            'remove equalHeight.Count = 3 to have equilivant function to notSameTriangles(?)
            If numbersOfPointsEqualHeight = 2 Then

                For k = 0 To 5
                    tempLinesList((tempNumbersofLines) + k) = EqualHeight(k)
                    'parallelFortempLineList(p, tempNumbersofLines + k) = EqualHeight(k)
                Next

                numbersOfLines += 1
                'arrayNumOfLines(p) += 1

            ElseIf (numbersOfPointsAboveHeight = 1 And numbersOfPointsBelowHeight = 1) And (numbersOfPointsEqualHeight = 1) Then

                firstSingleArrayPointCrossZPlane = functionSingleArrayPointCrossesZ(BelowHeight, AboveHeight, h)

                For k = 0 To 2
                    tempLinesList((tempNumbersofLines) + k) = EqualHeight(k)
                    tempLinesList((tempNumbersofLines) + k + 3) = firstSingleArrayPointCrossZPlane(k)
                    'parallelFortempLineList(p, tempNumbersofLines + k) = EqualHeight(k)
                    'parallelFortempLineList(p, tempNumbersofLines + k + 3) = firstSingleArrayPointCrossZPlane(k)
                Next

                numbersOfLines += 1
                'arrayNumOfLines(p) += 1

            ElseIf (numbersOfPointsAboveHeight > 0 And numbersOfPointsBelowHeight > 0) And (numbersOfPointsEqualHeight = 0) Then

                If numbersOfPointsAboveHeight = 2 Then
                    For k = 0 To 2
                        firstTempSingleArrayPoint(k) = AboveHeight(k)
                        secondTempSingleArrayPoint(k) = AboveHeight(k + 3)
                    Next

                    firstSingleArrayPointCrossZPlane = functionSingleArrayPointCrossesZ(BelowHeight, firstTempSingleArrayPoint, h)
                    secondsingleArrayPointCrossZPlane = functionSingleArrayPointCrossesZ(BelowHeight, secondTempSingleArrayPoint, h)

                ElseIf numbersOfPointsBelowHeight = 2 Then
                    For k = 0 To 2
                        firstTempSingleArrayPoint(k) = BelowHeight(k)
                        secondTempSingleArrayPoint(k) = BelowHeight(k + 3)
                    Next

                    firstSingleArrayPointCrossZPlane = functionSingleArrayPointCrossesZ(firstTempSingleArrayPoint, AboveHeight, h)
                    secondsingleArrayPointCrossZPlane = functionSingleArrayPointCrossesZ(secondTempSingleArrayPoint, AboveHeight, h)

                End If

                For k = 0 To 2
                    tempLinesList((tempNumbersofLines) + k) = firstSingleArrayPointCrossZPlane(k)
                    tempLinesList((tempNumbersofLines) + k + 3) = secondsingleArrayPointCrossZPlane(k)
                    'parallelFortempLineList(p, tempNumbersofLines + k) = firstSingleArrayPointCrossZPlane(k)
                    'parallelFortempLineList(p, tempNumbersofLines + k + 3) = secondsingleArrayPointCrossZPlane(k)
                Next

                numbersOfLines += 1
                'arrayNumOfLines(p) += 1

            End If
        Next

        totalArraySize = numbersOfLines * 6
        totalArraySizeByte = totalArraySize * 4

        ReDim finalLinesList(totalArraySize - 1)

        'For i = 0 To totalArraySize - 1
        '    finalLinesList(i) = tempLinesList(i)
        'Next

        Buffer.BlockCopy(tempLinesList, 0, finalLinesList, 0, totalArraySizeByte)

        Return finalLinesList

    End Function

    Private Function functionSingleArrayPointCrossesZ(ByVal singleArrayP1() As Single, ByVal singleArrayP2() As Single, ByVal h As Integer) As Single()

        Dim distance As Single
        Dim intersectSingleArrayP(2) As Single

        If singleArrayP1(2) = singleArrayP2(2) Then
            distance = 0.0
        Else
            distance = (h - singleArrayP1(2)) / (singleArrayP2(2) - singleArrayP1(2))
        End If

        For i = 0 To 1
            intersectSingleArrayP(i) = singleArrayP1(i) + distance * (singleArrayP2(i) - singleArrayP1(i))
        Next
        intersectSingleArrayP(2) = h

        Return intersectSingleArrayP

    End Function

    Private Function functionLinesToVoxels(ByVal linesList() As Single, ByVal currentBMPColumnNumbers As Integer, ByVal currentBMPRowNumbers As Integer) As Boolean()

        Dim returned1DPixels(bmpTotalPxs - 1) As Boolean
        Dim pixelSizeforEachProcessor As Integer

        'paralle for
        pixelSizeforEachProcessor = eachBMPImageWidthPxs \ numbersOfProcessors

        Parallel.For(0, numbersOfProcessors, Sub(i)
                                                 Dim initialPixelPosition As New Integer
                                                 Dim finalPixelPosition As Integer
                                                 Dim relevantLines() As Single
                                                 Dim singleRelevantLine(5) As Single
                                                 Dim numbersOfRelevantLines As Integer
                                                 'Dim isBlack As Boolean
                                                 Dim targetedYs() As Integer
                                                 Dim numbersOfTargetedYs As Integer
                                                 Dim tempI As Integer
                                                 Dim returnedPixelPosition As Long
                                                 Dim testPositionList As New List(Of Integer)
                                                 Dim testPositionArray() As Integer
                                                 Dim numbersOfFinalTargets As New Integer
                                                 Dim tempReturnPixel() As Boolean
                                                 Dim isBlackPixelSizes As Integer

                                                 initialPixelPosition = i * pixelSizeforEachProcessor

                                                 If i = (numbersOfProcessors - 1) Then
                                                     finalPixelPosition = eachBMPImageWidthPxs
                                                 Else
                                                     finalPixelPosition = initialPixelPosition + pixelSizeforEachProcessor
                                                 End If

                                                 For x = initialPixelPosition To finalPixelPosition - 1
                                                     'isBlack = False

                                                     numbersOfTargetedYs = 0
                                                     numbersOfFinalTargets = 0
                                                     isBlackPixelSizes = 0

                                                     'at a particular x, find the line going through this x
                                                     'call this line the relevant line
                                                     relevantLines = findRelevantLines(linesList, x)
                                                     numbersOfRelevantLines = (relevantLines.Length \ 6)

                                                     ReDim targetedYs(numbersOfRelevantLines - 1)

                                                     'at a particular x, find the corresponding y on this relevant line
                                                     'for each tuple Item1 = x, Item2 = y, Item3 = z
                                                     For j = 0 To numbersOfRelevantLines - 1
                                                         tempI = j * 6
                                                         If (relevantLines(tempI) <> relevantLines((tempI) + 3)) Then
                                                             For k = 0 To 5
                                                                 singleRelevantLine(k) = relevantLines(tempI + k)
                                                             Next

                                                             targetedYs(numbersOfTargetedYs) = calculateY(singleRelevantLine, x)
                                                             'targetedYs(numbersOfTargetedYs) = calculateX(singleRelevantLine, x)
                                                             numbersOfTargetedYs += 1

                                                         End If
                                                     Next

                                                     'set the pixel (x, y) on the relevant line to true
                                                     'y is between targetedYs
                                                     numbersOfFinalTargets = 0
                                                     ReDim testPositionArray(numbersOfTargetedYs * numbersOfRelevantLines - 1)

                                                     Array.Sort(targetedYs)


                                                     For m = 0 To numbersOfTargetedYs - 1
                                                         For j = 0 To numbersOfRelevantLines - 1
                                                             tempI = j * 6
                                                             For k = 0 To 5
                                                                 singleRelevantLine(k) = relevantLines(tempI + k)
                                                             Next
                                                             If (isOnTheLine(singleRelevantLine, x, targetedYs(m))) Then
                                                                 returnedPixelPosition = bmpTotalPxs - 1 - (x * eachBMPImageHeightPxs + (eachBMPImageHeightPxs - 1 - targetedYs(m)))
                                                                 'returnedPixelPosition = bmpTotalPxs - 1 - (targetedYs(m) * eachBMPImageWidthPxs + (eachBMPImageWidthPxs - 1 - x))
                                                                 testPositionArray(numbersOfFinalTargets) = returnedPixelPosition
                                                                 numbersOfFinalTargets += 1
                                                             End If
                                                             'If (isOnTheLine(singleRelevantLine, targetedYs(m), x)) Then
                                                             '    'returnedPixelPosition = bmpTotalPxs - 1 - (x * eachBMPImageHeightPxs + (eachBMPImageHeightPxs - 1 - targetedYs(m)))
                                                             '    returnedPixelPosition = bmpTotalPxs - 1 - (targetedYs(m) * eachBMPImageWidthPxs + (eachBMPImageWidthPxs - 1 - x))
                                                             '    testPositionArray(numbersOfFinalTargets) = returnedPixelPosition
                                                             '    numbersOfFinalTargets += 1
                                                             'End If
                                                         Next
                                                     Next

                                                     For n = 0 To numbersOfFinalTargets - 2 Step 2
                                                         isBlackPixelSizes = testPositionArray(n + 1) - testPositionArray(n)
                                                         ReDim tempReturnPixel(isBlackPixelSizes - 1)
                                                         For p = 0 To isBlackPixelSizes - 1
                                                             tempReturnPixel(p) = True
                                                         Next
                                                         Buffer.BlockCopy(tempReturnPixel, 0, returned1DPixels, testPositionArray(n), isBlackPixelSizes)
                                                     Next
                                                 Next

                                             End Sub)

        Return returned1DPixels

    End Function

    Private Function functionLinesToVoxelsAlongX(ByVal linesList() As Single, ByVal currentBMPColumnNumbers As Integer, ByVal currentBMPRowNumbers As Integer) As Boolean()

        Dim returned1DPixels(bmpTotalPxs - 1) As Boolean
        Dim pixelSizeforEachProcessor As Integer

        'paralle for
        'pixelSizeforEachProcessor = eachBMPImageWidthPxs \ numbersOfProcessors
        pixelSizeforEachProcessor = eachBMPImageHeightPxs \ numbersOfProcessors

        Parallel.For(0, numbersOfProcessors, Sub(i)
                                                 Dim initialPixelPosition As New Integer
                                                 Dim finalPixelPosition As Integer
                                                 Dim relevantLines() As Single
                                                 Dim singleRelevantLine(5) As Single
                                                 Dim numbersOfRelevantLines As Integer
                                                 Dim targetedXs() As Integer
                                                 Dim numbersOfTargetedXs As Integer
                                                 Dim tempI As Integer
                                                 Dim returnedPixelPosition As Long
                                                 Dim testPositionList As New List(Of Integer)
                                                 Dim testPositionArray() As Integer
                                                 Dim numbersOfFinalTargets As New Integer
                                                 Dim tempReturnPixel() As Boolean
                                                 Dim isBlackPixelSizes As Integer

                                                 initialPixelPosition = i * pixelSizeforEachProcessor

                                                 If i = (numbersOfProcessors - 1) Then
                                                     'finalPixelPosition = eachBMPImageWidthPxs
                                                     finalPixelPosition = eachBMPImageHeightPxs
                                                 Else
                                                     finalPixelPosition = initialPixelPosition + pixelSizeforEachProcessor
                                                 End If

                                                 For y = initialPixelPosition To finalPixelPosition - 1
                                                     'isBlack = False

                                                     'numbersOfTargetedYs = 0
                                                     numbersOfTargetedXs = 0
                                                     numbersOfFinalTargets = 0
                                                     isBlackPixelSizes = 0

                                                     'at a particular y, find the line going through this y
                                                     'call this line the relevant line
                                                     'relevantLines = findRelevantLines(linesList, x)
                                                     relevantLines = findRelevantLinesX(linesList, y)
                                                     numbersOfRelevantLines = (relevantLines.Length \ 6)

                                                     'ReDim targetedYs(numbersOfRelevantLines - 1)
                                                     ReDim targetedXs(numbersOfRelevantLines - 1)

                                                     'at a particular x, find the corresponding y on this relevant line
                                                     'for each tuple Item1 = x, Item2 = y, Item3 = z
                                                     For j = 0 To numbersOfRelevantLines - 1
                                                         tempI = j * 6
                                                         If (relevantLines(tempI + 1) <> relevantLines((tempI) + 3 + 1)) Then
                                                             For k = 0 To 5
                                                                 singleRelevantLine(k) = relevantLines(tempI + k)
                                                             Next

                                                             targetedXs(numbersOfTargetedXs) = calculateX(singleRelevantLine, y)
                                                             numbersOfTargetedXs += 1

                                                         End If
                                                     Next

                                                     'set the pixel (x, y) on the relevant line to true
                                                     'x is between targetedxs
                                                     numbersOfFinalTargets = 0
                                                     ReDim testPositionArray(numbersOfTargetedXs * numbersOfRelevantLines - 1)

                                                     Array.Sort(targetedXs)

                                                     For m = 0 To numbersOfTargetedXs - 1
                                                         For j = 0 To numbersOfRelevantLines - 1
                                                             tempI = j * 6
                                                             For k = 0 To 5
                                                                 singleRelevantLine(k) = relevantLines(tempI + k)
                                                             Next
                                                             If (isOnTheLineX(singleRelevantLine, targetedXs(m), y)) Then
                                                                 returnedPixelPosition = bmpTotalPxs - 1 - (y * eachBMPImageWidthPxs + (eachBMPImageWidthPxs - 1 - targetedXs(m)))
                                                                 'returnedPixelPosition = bmpTotalPxs - 1 - (targetedYs(m) * eachBMPImageWidthPxs + (eachBMPImageWidthPxs - 1 - x))
                                                                 testPositionArray(numbersOfFinalTargets) = returnedPixelPosition
                                                                 numbersOfFinalTargets += 1
                                                             End If
                                                         Next
                                                     Next

                                                     For n = 0 To numbersOfFinalTargets - 2 Step 2
                                                         isBlackPixelSizes = testPositionArray(n + 1) - testPositionArray(n)
                                                         ReDim tempReturnPixel(isBlackPixelSizes)
                                                         For p = 0 To isBlackPixelSizes
                                                             tempReturnPixel(p) = True
                                                         Next
                                                         Buffer.BlockCopy(tempReturnPixel, 0, returned1DPixels, testPositionArray(n), isBlackPixelSizes + 1)
                                                     Next
                                                 Next

                                             End Sub)

        Return returned1DPixels

    End Function

    Private Function functionLinesToVoxelsAlongXSerialFor(ByVal linesList() As Single, ByVal currentBMPColumnNumbers As Integer, ByVal currentBMPRowNumbers As Integer) As Boolean()

        Dim returned1DPixels(bmpTotalPxs - 1) As Boolean
        Dim pixelSizeforEachProcessor As Integer

        'paralle for
        'pixelSizeforEachProcessor = eachBMPImageWidthPxs \ numbersOfProcessors
        pixelSizeforEachProcessor = eachBMPImageHeightPxs \ numbersOfProcessors

        Dim initialPixelPosition As New Integer
        Dim finalPixelPosition As Integer
                                                 Dim relevantLines() As Single
                                                 Dim singleRelevantLine(5) As Single
                                                 Dim numbersOfRelevantLines As Integer
                                                 'Dim isBlack As Boolean
                                                 Dim targetedYs() As Integer
                                                 Dim targetedXs() As Integer
                                                 Dim numbersOfTargetedYs As Integer
                                                 Dim numbersOfTargetedXs As Integer
                                                 Dim tempI As Integer
                                                 Dim returnedPixelPosition As Long
                                                 Dim testPositionList As New List(Of Integer)
                                                 Dim testPositionArray() As Integer
                                                 Dim numbersOfFinalTargets As New Integer
                                                 Dim tempReturnPixel() As Boolean
                                                 Dim isBlackPixelSizes As Integer

        'initialPixelPosition = i * pixelSizeforEachProcessor

        'If i = (numbersOfProcessors - 1) Then
        '    'finalPixelPosition = eachBMPImageWidthPxs
        '    finalPixelPosition = eachBMPImageHeightPxs
        'Else
        '    finalPixelPosition = initialPixelPosition + pixelSizeforEachProcessor
        'End If

        finalPixelPosition = eachBMPImageHeightPxs

        For y = initialPixelPosition To finalPixelPosition - 1
            'isBlack = False

            'numbersOfTargetedYs = 0
            numbersOfTargetedXs = 0
            numbersOfFinalTargets = 0
            isBlackPixelSizes = 0

            'at a particular y, find the line going through this y
            'call this line the relevant line
            'relevantLines = findRelevantLines(linesList, x)
            relevantLines = findRelevantLinesX(linesList, y)
            numbersOfRelevantLines = (relevantLines.Length \ 6)

            'ReDim targetedYs(numbersOfRelevantLines - 1)
            ReDim targetedXs(numbersOfRelevantLines - 1)

            'at a particular x, find the corresponding y on this relevant line
            'for each tuple Item1 = x, Item2 = y, Item3 = z
            For j = 0 To numbersOfRelevantLines - 1
                tempI = j * 6
                If (relevantLines(tempI + 1) <> relevantLines((tempI) + 3 + 1)) Then
                    For k = 0 To 5
                        singleRelevantLine(k) = relevantLines(tempI + k)
                    Next

                    'targetedYs(numbersOfTargetedYs) = calculateY(singleRelevantLine, x)
                    'targetedYs(numbersOfTargetedYs) = calculateX(singleRelevantLine, x)
                    targetedXs(numbersOfTargetedXs) = calculateX(singleRelevantLine, y)
                    numbersOfTargetedXs += 1

                End If
            Next

            'set the pixel (x, y) on the relevant line to true
            'x is between targetedxs
            numbersOfFinalTargets = 0
            'ReDim testPositionArray(numbersOfTargetedYs * numbersOfRelevantLines - 1)
            ReDim testPositionArray(numbersOfTargetedXs * numbersOfRelevantLines - 1)

            'Array.Sort(targetedYs)
            Array.Sort(targetedXs)

            For m = 0 To numbersOfTargetedXs - 1
                For j = 0 To numbersOfRelevantLines - 1
                    tempI = j * 6
                    For k = 0 To 5
                        singleRelevantLine(k) = relevantLines(tempI + k)
                    Next
                    If (isOnTheLineX(singleRelevantLine, targetedXs(m), y)) Then
                        returnedPixelPosition = bmpTotalPxs - 1 - ((y) * eachBMPImageWidthPxs + (eachBMPImageWidthPxs - 1 - targetedXs(m)))
                        testPositionArray(numbersOfFinalTargets) = returnedPixelPosition
                        numbersOfFinalTargets += 1
                    End If
                Next
            Next

            For n = 0 To numbersOfFinalTargets - 2 Step 2
                isBlackPixelSizes = testPositionArray(n + 1) - testPositionArray(n) + 1
                ReDim tempReturnPixel(isBlackPixelSizes - 1)
                For p = 0 To isBlackPixelSizes - 1
                    tempReturnPixel(p) = True
                Next
                Buffer.BlockCopy(tempReturnPixel, 0, returned1DPixels, testPositionArray(n), isBlackPixelSizes)
            Next
        Next


        Return returned1DPixels

    End Function

    Private Function findRelevantLines(ByVal lineList() As Single, ByVal x As Integer) As Single()

        Dim same As Boolean
        Dim above As Boolean
        Dim below As Boolean
        Dim tempRelevantLinesSingleArray() As Single
        Dim finalRelevantLineSingleArray() As Single
        Dim numbersOfInputLines As Integer
        Dim numbersOfRelevantLines As New Integer
        Dim tempPosition1 As Integer
        Dim tempPosition2 As Integer
        Dim tempPosition3 As Integer
        Dim totalNumLinesPoints As Integer
        Dim finalOutputArraySize As Integer
        Dim finalOutputArraySizeByte As Integer

        totalNumLinesPoints = lineList.Length

        numbersOfInputLines = (totalNumLinesPoints \ 6)

        ReDim tempRelevantLinesSingleArray(totalNumLinesPoints - 1)

        For i = 0 To numbersOfInputLines - 1
            same = New Boolean
            above = New Boolean
            below = New Boolean
            tempPosition1 = i * 6
            tempPosition2 = numbersOfRelevantLines * 6

            For j = 0 To 1
                tempPosition3 = tempPosition1 + (j * 3)
                If (lineList(tempPosition3) > x) Then
                    above = True
                ElseIf (lineList(tempPosition3) = x) Then
                    same = True
                Else
                    below = True
                End If
            Next

            If (above And below) Or (same And above) Then

                For k = 0 To 5
                    tempRelevantLinesSingleArray((tempPosition2) + k) = lineList(tempPosition1 + k)
                Next

                numbersOfRelevantLines += 1

            End If
        Next

        finalOutputArraySize = 6 * numbersOfRelevantLines
        finalOutputArraySizeByte = finalOutputArraySize * 4

        ReDim finalRelevantLineSingleArray(finalOutputArraySize - 1)

        Buffer.BlockCopy(tempRelevantLinesSingleArray, 0, finalRelevantLineSingleArray, 0, finalOutputArraySizeByte)

        Return finalRelevantLineSingleArray


    End Function

    Private Function findRelevantLinesX(ByVal lineList() As Single, ByVal y As Integer) As Single()

        Dim same As Boolean
        Dim above As Boolean
        Dim below As Boolean
        Dim tempRelevantLinesSingleArray() As Single
        Dim finalRelevantLineSingleArray() As Single
        Dim numbersOfInputLines As Integer
        Dim numbersOfRelevantLines As New Integer
        Dim tempPosition1 As Integer
        Dim tempPosition2 As Integer
        Dim tempPosition3 As Integer
        Dim totalNumLinesPoints As Integer
        Dim finalOutputArraySize As Integer
        Dim finalOutputArraySizeByte As Integer

        totalNumLinesPoints = lineList.Length

        numbersOfInputLines = (totalNumLinesPoints \ 6)

        ReDim tempRelevantLinesSingleArray(totalNumLinesPoints - 1)

        For i = 0 To numbersOfInputLines - 1
            same = New Boolean
            above = New Boolean
            below = New Boolean
            tempPosition1 = i * 6
            tempPosition2 = numbersOfRelevantLines * 6

            For j = 0 To 1
                tempPosition3 = tempPosition1 + (j * 3) + 1
                If (lineList(tempPosition3) > y) Then
                    above = True
                ElseIf (lineList(tempPosition3) = y) Then
                    same = True
                Else
                    below = True
                End If
            Next

            If (above And below) Or (same And above) Then

                For k = 0 To 5
                    tempRelevantLinesSingleArray((tempPosition2) + k) = lineList(tempPosition1 + k)
                Next

                numbersOfRelevantLines += 1

            ElseIf (y = eachBMPImageHeightPxs - 1) And (same And below) Then

                For k = 0 To 5
                    tempRelevantLinesSingleArray((tempPosition2) + k) = lineList(tempPosition1 + k)
                Next

                numbersOfRelevantLines += 1

            End If
        Next

        finalOutputArraySize = 6 * numbersOfRelevantLines
        finalOutputArraySizeByte = finalOutputArraySize * 4

        ReDim finalRelevantLineSingleArray(finalOutputArraySize - 1)

        Buffer.BlockCopy(tempRelevantLinesSingleArray, 0, finalRelevantLineSingleArray, 0, finalOutputArraySizeByte)

        Return finalRelevantLineSingleArray


    End Function

    Private Function isOnTheLine(ByVal line() As Single, ByVal x As Integer, ByVal y As Integer) As Boolean

        Dim estY As Integer
        Dim maxX As Integer
        Dim minX As Integer
        Dim maxY As Integer
        Dim minY As Integer

        'line(0) = x1, line(1) = y1, line(2) = z1, line(3) = x2, line(4) = y2, line(5) = z2
        If line(0) > line(3) Then
            maxX = Math.Round(line(0))
            minX = Math.Round(line(3))
        Else
            maxX = Math.Round(line(3))
            minX = Math.Round(line(0))
        End If

        If line(1) > line(4) Then
            maxY = Math.Round(line(1))
            minY = Math.Round(line(4))
        Else
            maxY = Math.Round(line(4))
            minY = Math.Round(line(1))
        End If

        estY = calculateY(line, x)

        If (estY <> y) Then
            Return False
        End If
        If (Math.Round(line(0)) <> x) And (Math.Round(line(3)) <> x) And (maxX < x Or minX > x) Then
            Return False
        End If
        If (Math.Round(line(1)) <> y) And (Math.Round(line(4)) <> y) And (maxY < y Or minY > y) Then
            Return False
        End If

        Return True


    End Function

    Private Function isOnTheLineX(ByVal line() As Single, ByVal x As Integer, ByVal y As Integer) As Boolean

        Dim estY As Integer
        Dim estX As Integer
        Dim maxX As Integer
        Dim minX As Integer
        Dim maxY As Integer
        Dim minY As Integer

        'line(0) = x1, line(1) = y1, line(2) = z1, line(3) = x2, line(4) = y2, line(5) = z2
        If line(0) > line(3) Then
            maxX = Math.Round(line(0))
            minX = Math.Round(line(3))
        Else
            maxX = Math.Round(line(3))
            minX = Math.Round(line(0))
        End If

        If line(1) > line(4) Then
            maxY = Math.Round(line(1))
            minY = Math.Round(line(4))
        Else
            maxY = Math.Round(line(4))
            minY = Math.Round(line(1))
        End If

        'estY = calculateY(line, x)
        estX = calculateX(line, y)

        If (estX <> x) Then
            Return False
        End If
        If (Math.Round(line(0)) <> x) And (Math.Round(line(3)) <> x) And (maxX < x Or minX > x) Then
            Return False
        End If
        If (Math.Round(line(1)) <> y) And (Math.Round(line(4)) <> y) And (maxY < y Or minY > y) Then
            Return False
        End If

        Return True


    End Function

    Private Function calculateY(ByVal line() As Single, ByVal x As Integer) As Integer

        Dim ratio As Single
        Dim calY As Integer


        'line(0) = x1, line(1) = y1, line(2) = z1, line(3) = x2, line(4) = y2, line(5) = z2
        ratio = ((x - line(0)) * (line(4) - line(1)) / (line(3) - line(0)))

        calY = Math.Round(line(1) + ratio)

        Return calY

    End Function

    Private Function calculateX(ByVal line() As Single, ByVal y As Integer) As Integer

        Dim ratio As Single
        Dim calX As Integer


        'line(0) = x1, line(1) = y1, line(2) = z1, line(3) = x2, line(4) = y2, line(5) = z2
        ratio = ((y - line(1)) * (line(3) - line(0)) / (line(4) - line(1)))

        calX = Math.Round(line(0) + ratio)

        Return calX

    End Function

    Private Function functionCalNormSum1DArray(ByVal Sum1DArray() As Byte, ByVal totalResistLayerThicknessUM As Single, ByVal patternThicknessUM As Single) As Byte()

        Dim normSum1DArray(bmpTotalPxs - 1) As Byte
        Dim maxSum As Byte
        Dim ratioPatternToResistThickness As Single
        Dim pixelsForEachProcessor As Integer
        Dim parallelForMaxSum(numbersOfProcessors - 1) As Byte
        'Dim testPartitioner = Partitioner.Create(0, Sum1DArray.Length)



        ratioPatternToResistThickness = patternThicknessUM / totalResistLayerThicknessUM

        'renormalised the array, to 255 * (pattern thickness/resist total layer thickness)

        pixelsForEachProcessor = bmpTotalPxs \ numbersOfProcessors

        For i = 0 To bmpTotalPxs - 1
            If Sum1DArray(i) > maxSum Then
                maxSum = Sum1DArray(i)
            End If
        Next

        If maxSum > 0 Then
            Parallel.For(0, numbersOfProcessors, Sub(i)
                                                     Dim initialPixelPosition As Integer
                                                     Dim finalPixelPosition As Integer
                                                     Dim tempByteArray() As Byte
                                                     Dim currentByteArrayLength As Integer

                                                     initialPixelPosition = i * pixelsForEachProcessor

                                                     If i = (numbersOfProcessors - 1) Then
                                                         finalPixelPosition = bmpTotalPxs
                                                     Else
                                                         finalPixelPosition = initialPixelPosition + pixelsForEachProcessor
                                                     End If

                                                     currentByteArrayLength = finalPixelPosition - initialPixelPosition

                                                     ReDim tempByteArray(currentByteArrayLength - 1)

                                                     For j = initialPixelPosition To finalPixelPosition - 1

                                                         normSum1DArray(j) = ((Sum1DArray(j) / maxSum) * 255 * (ratioPatternToResistThickness))

                                                         If isInverted Then
                                                             normSum1DArray(j) = Not normSum1DArray(j)
                                                         End If

                                                     Next

                                                 End Sub)
        End If

        If rbnPreview.Checked = True Then
            ReDim output1DByteArray(bmpTotalPxs - 1)
            For i = 0 To bmpTotalPxs - 1
                output1DByteArray(i) = normSum1DArray(i)
            Next
        End If

        Return normSum1DArray

    End Function



End Class
