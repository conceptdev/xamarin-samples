Option Explicit On
''' <summary>
''' Abstract manager for Task C.R.U.D.
''' </summary>
Public Class TodoItemManager

    Private _repository As TodoItemRepositoryXML

    Public Sub New(filename As String, storage As IXmlStorage)
        _repository = New TodoItemRepositoryXML(filename, storage)
    End Sub

    Public Function GetTask(id As Integer) As TodoItem
        Return _repository.GetTask(id)
    End Function

    Public Function GetTasks() As List(Of TodoItem)
        Return New List(Of TodoItem)(_repository.GetTasks())
    End Function

    Public Function SaveTask(item As TodoItem) As Integer
        Return _repository.SaveTask(item)
    End Function

    Public Function DeleteTask(item As TodoItem) As Integer
        Return _repository.DeleteTask(item.ID)
    End Function
End Class
