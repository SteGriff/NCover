Option Explicit On 
Option Strict On

Imports ANamespace
Imports NUnit.Framework

<TestFixture()> _
Public Class Class1Test

    <Test()> _
    Public Sub ATest()
        Dim c As New Class1

        c.AMethod()
    End Sub


    <Test()> _
    Public Sub AnotherTest()
        Dim c As New ExampleCSharpLibrary.Class1

        c.AMethod()
    End Sub

End Class
