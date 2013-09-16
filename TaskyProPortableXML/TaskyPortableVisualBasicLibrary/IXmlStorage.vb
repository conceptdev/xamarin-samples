Option Explicit On
Public Interface IXmlStorage

    Function ReadXml(filename As String) As List(Of Task)
    Sub WriteXml(tasks As List(Of Task), filename As String)

End Interface
