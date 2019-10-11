Imports System.Threading

Public Class GameOfLife

    Dim iSize As Integer = 30
    Dim aInitialPositions(iSize, iSize) As Integer
    Dim aFinalPositions(iSize, iSize) As Integer

   
    Private Sub GameOfLife_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Reset the two arrays
        resetArrays()

        'create PictureBoxes dynamically  
        createPictureBoxes()

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As System.EventArgs) Handles Timer1.Tick

        For i = 0 To iSize - 1
            For j = 0 To iSize - 1
                algoGameOfLife(i, j)
            Next
        Next

        copyFinalArrayIntoInitialArray()
        updateDisplay()
    End Sub

    Private Sub algoGameOfLife(ByVal line As Integer, ByVal column As Integer)

        Dim nbAliveCells As Integer = 0

        nbAliveCells = howManyAliveCellsAround(line, column)
        ' A death cell that has at least 3 alive cells around it becomes alive
        If (nbAliveCells = 3) And (Not isCellAlive(line, column)) Then
            aFinalPositions(line, column) = 1
        End If

        'An alive cell that has 2 or 3 alive cells around it stays alive else it dies
        If (isCellAlive(line, column)) Then
            If nbAliveCells = 2 Or nbAliveCells = 3 Then
                'the cell stays alive
                aFinalPositions(line, column) = 1
            Else
                'the cell dies
                aFinalPositions(line, column) = 0
            End If
        End If

    End Sub


    'Create dynamically as many pictureboxes as defined
    'Each picturebox contains 1 pixel
    Private Sub createPictureBoxes()

        For i = 0 To iSize - 1
            For j = 0 To iSize - 1
                Dim obj As New System.Windows.Forms.PictureBox

                With obj
                    .Name = "PB" & i & "_" & j 'nom de ta picturebox (PB1, PB2, PB3, ...)
                    .Left = (j - ((j \ iSize) * iSize)) * 15 + 10 ' 'position par rapport au rebord gauche de l'UserForm
                    .Top = (i) * 15 + 10   'position par rapport au haut de l'UserForm
                    .Width = 10 'largeur de la zone d'écriture
                    .Height = 10 'hauteur de la zone d'écriture
                    'tu peux rajouter ou enlever des propriétés de l'objet
                    '.Text = "A"
                    If aInitialPositions(i, j) <> 0 Then
                        .BackColor = Color.Blue
                    Else
                        .BackColor = Color.Black
                    End If
                    .Visible = True

                End With

                'ajout du controle à la PictureBox1
                Me.Controls.Add(obj)

                'only for debug purpose
                'AddHandler obj.MouseClick, AddressOf obj_MouseClick

            Next
        Next
    End Sub

    'only for debug purpose
    Private Sub obj_MouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.Click
        'sender.Name()
    End Sub

    Private Sub updateDisplay()

        For i = 0 To iSize - 1
            For j = 0 To iSize - 1
                If aInitialPositions(i, j) = 1 Then
                    CType(Me.Controls("PB" & i & "_" & j), PictureBox).BackColor = Color.Blue
                Else
                    CType(Me.Controls("PB" & i & "_" & j), PictureBox).BackColor = Color.Black
                End If
            Next
        Next

    End Sub

    Private Sub copyFinalArrayIntoInitialArray()
        For k = 0 To iSize - 1
            For l = 0 To iSize - 1
                aInitialPositions(k, l) = aFinalPositions(k, l)
            Next
        Next
    End Sub

    Private Sub resetArrays()
        For k = 0 To iSize - 1
            For l = 0 To iSize - 1
                aInitialPositions(k, l) = 0
                aFinalPositions(k, l) = 0
            Next
        Next

    End Sub
    Private Function isCellAlive(ByVal param1 As Integer, ByVal param2 As Integer) As Boolean
        Dim result As Boolean = False

        If (aInitialPositions(param1, param2) <> 0) Then result = True

        Return result

    End Function


    Private Function howManyAliveCellsAround(ByVal line As Integer, ByVal column As Integer) As Integer

        Dim result As Integer = 0
        Dim saveParam1 = line
        Dim saveParam2 = column

        For i = -1 To 1
            For j = -1 To 1

                line = saveParam1 + i
                If (line = -1) Then
                    line = iSize - 1
                End If
                If (line = iSize) Then
                    line = 0
                End If

                column = saveParam2 + j
                If (column = -1) Then
                    column = iSize - 1
                End If
                If (column = iSize) Then
                    column = 0
                End If

                If (aInitialPositions(line, column) <> 0) Then
                    result = result + 1
                End If
            Next
        Next
        If (aInitialPositions(saveParam1, saveParam2) <> 0) Then result = result - 1

        Return result

    End Function

    'The random initialisation
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        'initial positions (first solution)
        'Randomize()
        'For i = 0 To 200
        '    aInitialPositions((Int(iSize - 1) * Rnd()), (Int(iSize - 1) * Rnd())) = 1
        'Next

        'initial positions (second solution)
        Randomize()

        For iCpt = 0 To iSize - 1
            For jCpt = 0 To iSize - 1
                If Int(100 * Rnd() < 20) Then
                    aInitialPositions(iCpt, jCpt) = 1
                Else
                    aInitialPositions(iCpt, jCpt) = 0
                End If
            Next
        Next

        'Set and start a Timer
        Timer1.Interval = 100
        Timer1.Enabled = True
        Timer1.Start()

    End Sub

    'Test (Step by step)
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        For i = 0 To iSize - 1
            For j = 0 To iSize - 1
                algoGameOfLife(i, j)
            Next
        Next

        copyFinalArrayIntoInitialArray()
        updateDisplay()

    End Sub

    'The cross initialisation
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

       
        SetCrossConfiguration()

        'Set and start a Timer
        Timer1.Interval = 100
        Timer1.Enabled = True
        Timer1.Start()

    End Sub

    'Set test configuration
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Timer1.Enabled = False
        'Reset the two arrays
        resetArrays()
        aInitialPositions(1, 2) = 1
        aInitialPositions(2, 1) = 1
        aInitialPositions(2, 2) = 1
        aInitialPositions(3, 2) = 1
        updateDisplay()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        SetCrossConfiguration()
        updateDisplay()
    End Sub


    Private Sub SetCrossConfiguration()

        Timer1.Enabled = False
        'Reset the two arrays
        resetArrays()

        For iCpt = 0 To iSize - 1
            For jCpt = 0 To iSize - 1
                aInitialPositions(iCpt, jCpt) = 0
                If iCpt = jCpt Then
                    aInitialPositions(iCpt, jCpt) = 1
                End If
                If iCpt + jCpt = iSize - 1 Then
                    aInitialPositions(iCpt, jCpt) = 1
                End If
            Next
        Next
    End Sub
End Class
