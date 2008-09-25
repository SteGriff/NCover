Imports System

Namespace ANamespace

    Public Class Class1

        Sub AMethod()
            Try
                If True Then Throw New System.ApplicationException
            Catch ex As Exception 'need here

            End Try

            Dim attempts As Integer
            Try
                If True Then Throw New System.ApplicationException
            Catch ex As Exception When attempts > 2 'need here

            End Try

            If True AndAlso True Then System.Console.Out.WriteLine("Hello")

            If True AndAlso True Then

            ElseIf True AndAlso True Then
            Else
                'need here
            End If

            Dim b As Boolean = True
            While b 'could be a comment here
                b = False
            End While

            For x As Integer = 1 To 10
                'need here
            Next

            Dim xs() As Integer = {1, 2, 3}
            For Each x As Integer In xs

            Next

            'Dim xxxx As Boolean = IIf(True, True, False)

            If True Then Return

            Dim xy$ = "fred" : Dim z As Integer : z = 2 : z = z + 1

            Select Case xy
                Case "fred" AndAlso True
                    'need here
                Case "May"
                    'need here
                Case Else
                    'need here
            End Select

        End Sub
    End Class
End Namespace











