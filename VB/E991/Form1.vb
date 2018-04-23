Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Namespace E991
	Partial Public Class Form1
		Inherits Form
		Public Sub New()
			InitializeComponent()
			'You may use the PopupSize property to set the popup size. In this demo I'm using the default one.
			Dim TempGridViewQuickColumnCustomization As DevExpress.XtraGrid.Helpers.GridViewQuickColumnCustomization = New DevExpress.XtraGrid.Helpers.GridViewQuickColumnCustomization(gridView1)
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			' TODO: This line of code loads data into the 'nwindDataSet.Orders' table. You can move, or remove it, as needed.
			Me.ordersTableAdapter.Fill(Me.nwindDataSet.Orders)
		End Sub
	End Class
End Namespace