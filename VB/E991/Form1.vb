Imports DevExpress.XtraGrid.Views.Grid
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
			Dim tempVar As New DevExpress.XtraGrid.Helpers.GridViewQuickColumnCustomization(gridView1)
		End Sub

		Private Function CreateTable() As DataTable
			Dim table As New DataTable()
			Dim dataRow As DataRow

			table.Columns.Add("Prod_NO", GetType(String))
			table.Columns.Add("Prod_Name", GetType(String))
			table.Columns.Add("Order_Date", GetType(String))
			table.Columns.Add("Quantity", GetType(String))

			dataRow = table.NewRow()
			dataRow("Prod_NO") = "101"
			dataRow("Prod_Name") = "Product1"
			dataRow("Order_Date") = "12/06/2012"
			dataRow("Quantity") = "50"
			table.Rows.Add(dataRow)

			dataRow = table.NewRow()
			dataRow("Prod_NO") = "102"
			dataRow("Prod_Name") = "Product2"
			dataRow("Order_Date") = "15/06/2012"
			dataRow("Quantity") = "70"
			table.Rows.Add(dataRow)

			dataRow = table.NewRow()
			dataRow("Prod_NO") = "102"
			dataRow("Prod_Name") = "Product2"
			dataRow("Order_Date") = "15/06/2012"
			dataRow("Quantity") = "70"
			table.Rows.Add(dataRow)

			dataRow = table.NewRow()
			dataRow("Prod_NO") = "103"
			dataRow("Prod_Name") = "Product3"
			dataRow("Order_Date") = "18/06/2012"
			dataRow("Quantity") = "30"
			table.Rows.Add(dataRow)

			dataRow = table.NewRow()
			dataRow("Prod_NO") = "104"
			dataRow("Prod_Name") = "Product4"
			dataRow("Order_Date") = "25/06/2012"
			dataRow("Quantity") = "20"
			table.Rows.Add(dataRow)

			Return table
		End Function

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
			gridControl1.DataSource = CreateTable()
		End Sub


	End Class
End Namespace