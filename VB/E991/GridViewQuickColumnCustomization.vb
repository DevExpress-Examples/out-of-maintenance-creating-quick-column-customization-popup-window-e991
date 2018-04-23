Imports Microsoft.VisualBasic
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports DevExpress.XtraEditors.Popup
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports System.Drawing
Imports DevExpress.XtraGrid.Columns
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Views.Grid.Drawing

Namespace DevExpress.XtraGrid.Helpers

	Public Class GridViewQuickColumnCustomization

		Private Enum ColumnCustomizationState
			None
			Pressed
			Shown
		End Enum

		Private view_Renamed As GridView
		Private state_Renamed As ColumnCustomizationState = ColumnCustomizationState.None
		Private containerEdit_Renamed As GridViewQuickColumnCustomizationContainerEdit
		Private popupSize_Renamed As Size = Size.Empty

		Public Sub New(ByVal view As GridView)
			Me.view_Renamed = view
			AddHandler Me.View.CustomDrawRowIndicator, AddressOf View_CustomDrawRowIndicator
			AddHandler Me.View.MouseDown, AddressOf View_MouseDown
			AddHandler Me.View.MouseUp, AddressOf View_MouseUp
		End Sub
		Public ReadOnly Property View() As GridView
			Get
				Return view_Renamed
			End Get
		End Property
		Public Property PopupSize() As Size
			Get
				Return popupSize_Renamed
			End Get
			Set(ByVal value As Size)
				popupSize_Renamed = value
			End Set
		End Property

		Private Property State() As ColumnCustomizationState
			Get
				Return state_Renamed
			End Get
			Set(ByVal value As ColumnCustomizationState)
				If State = value Then
					Return
				End If
				state_Renamed = value
				InvalidateIndicator()
				If State = ColumnCustomizationState.Shown Then
					ShowCustomization()
				End If
			End Set
		End Property

		Private Sub View_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
			If IsCursorOnColumnButton(e) Then
				State = ColumnCustomizationState.Pressed
			Else
				State = ColumnCustomizationState.None
			End If
		End Sub

		Private Sub View_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
			If State <> ColumnCustomizationState.Pressed Then
				Return
			End If
			If IsCursorOnColumnButton(e) Then
				State = ColumnCustomizationState.Shown
			Else
				State = ColumnCustomizationState.None
			End If
		End Sub
		Private Sub View_CustomDrawRowIndicator(ByVal sender As Object, ByVal e As RowIndicatorCustomDrawEventArgs)
			If e.RowHandle = GridControl.InvalidRowHandle Then
				' You may assign your own image list e.Info.Images to show the custom image.
				e.Info.ImageIndex = GridPainter.IndicatorNewItemRow
				e.Info.Appearance.ForeColor = Color.Blue
				If State <> ColumnCustomizationState.None Then
					e.Info.State = DevExpress.Utils.Drawing.ObjectState.Pressed
				End If
			End If
		End Sub
		Private Sub ShowCustomization()
			SetupContainerEdit()
			ContainerEdit.ShowPopup()
		End Sub
		Private Function GetColumnButtonBounds() As Rectangle
			Dim vi As GridViewInfo = TryCast(View.GetViewInfo(), GridViewInfo)
			For i As Integer = 0 To vi.ColumnsInfo.Count - 1
				If vi.ColumnsInfo(i).Type = GridColumnInfoType.Indicator Then
					Return vi.ColumnsInfo(i).Bounds
				End If
			Next i
			Return Rectangle.Empty
		End Function
		Private Sub InvalidateIndicator()
			View.InvalidateRect(GetColumnButtonBounds())
		End Sub
		Private columnButtonLoation As Point = Point.Empty
		Private Function IsCursorOnColumnButton(ByVal e As System.EventArgs) As Boolean
			Dim de As DXMouseEventArgs = DXMouseEventArgs.GetMouseArgs(View.GridControl, e)
			Dim o As Object = View.CalcHitInfo(de.Location)
			Return View.CalcHitInfo(de.Location).HitTest = GridHitTest.ColumnButton
		End Function
		Protected ReadOnly Property ContainerEdit() As GridViewQuickColumnCustomizationContainerEdit
			Get
				Return containerEdit_Renamed
			End Get
		End Property
		Private Sub SetupContainerEdit()
			Me.containerEdit_Renamed = New GridViewQuickColumnCustomizationContainerEdit(View)
			ContainerEdit.Text = String.Empty
			ContainerEdit.Properties.AutoHeight = False
			ContainerEdit.Properties.LookAndFeel.ParentLookAndFeel = View.GridControl.LookAndFeel
			ContainerEdit.Properties.Appearance.BackColor = Color.Transparent
			ContainerEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
			ContainerEdit.Properties.Buttons.Clear()
			AddHandler ContainerEdit.Closed, AddressOf OnClosed
			ContainerEdit.Bounds = GetColumnButtonBounds()
			If (Not PopupSize.IsEmpty) Then
				ContainerEdit.Properties.PopupStartSize = PopupSize
			End If
			ContainerEdit.Parent = View.GridControl
		End Sub

		Private Sub OnClosed(ByVal sender As Object, ByVal e As ClosedEventArgs)
			ContainerEdit.Dispose()
			Me.containerEdit_Renamed = Nothing
			State = ColumnCustomizationState.None
		End Sub
	End Class

	Public Class GridViewQuickColumnCustomizationContainerEdit
		Inherits BlobBaseEdit
		Private view As GridView
		Public Sub New(ByVal view As GridView)
			Me.view = view
		End Sub
		Protected Overrides Function CreatePopupForm() As PopupBaseForm
			Return New GridViewQuickColumnCustomizationPopup(Me, view)
		End Function
		Protected Overrides Sub OnPopupClosing(ByVal e As CloseUpEventArgs)
			If e.AcceptValue Then
				CType(PopupForm, GridViewQuickColumnCustomizationPopup).Apply()
			End If
			MyBase.OnPopupClosing(e)
		End Sub
	End Class
	Public Class GridViewQuickColumnCustomizationPopup
		Inherits BlobBasePopupForm
		Private view_Renamed As GridView
		Private checkListBox_Renamed As CheckedListBoxControl

		Public Sub New(ByVal ownerEdit As BlobBaseEdit, ByVal view As GridView)
			MyBase.New(ownerEdit)
			Me.view_Renamed = view
			Me.checkListBox_Renamed = New CheckedListBoxControl()
			Me.checkListBox_Renamed.BorderStyle = BorderStyles.Simple
			Me.checkListBox_Renamed.Appearance.Assign(ownerEdit.Properties.AppearanceDropDown)
			Me.checkListBox_Renamed.LookAndFeel.ParentLookAndFeel = Me.OwnerEdit.LookAndFeel
			Me.checkListBox_Renamed.Visible = False
			Me.checkListBox_Renamed.CheckOnClick = True
			AddHandler checkListBox_Renamed.ItemCheck, AddressOf OnCheckListBoxItemCheck
			Me.Controls.Add(checkListBox_Renamed)
			UpdateCheckListBox()
			FillList()
			OkButton.Enabled = True
		End Sub
		Public Sub Apply()
			View.BeginUpdate()
			Try
				For i As Integer = 0 To CheckListBox.Items.Count - 1
					Dim column As GridColumn = CType(CheckListBox.Items(i).Value, GridColumn)
					column.Visible = CheckListBox.Items(i).CheckState = CheckState.Checked
				Next i
			Finally
				View.EndUpdate()
			End Try
		End Sub
		Protected ReadOnly Property View() As GridView
			Get
				Return view_Renamed
			End Get
		End Property
		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				If CheckListBox IsNot Nothing Then
					Me.checkListBox_Renamed.Dispose()
					Me.checkListBox_Renamed = Nothing
				End If
			End If
			MyBase.Dispose(disposing)
		End Sub
		Protected Overrides ReadOnly Property EmbeddedControl() As Control
			Get
				Return CheckListBox
			End Get
		End Property
		Public ReadOnly Property CheckListBox() As CheckedListBoxControl
			Get
				Return checkListBox_Renamed
			End Get
		End Property
		Protected Sub UpdateCheckListBox()
			OkButton.Enabled = True
			CheckListBox.BeginUpdate()
			Try
				CheckListBox.Appearance.Assign(ViewInfo.PaintAppearanceContent)
			Finally
				CheckListBox.EndUpdate()
			End Try
		End Sub
		Public Overrides Sub ProcessKeyDown(ByVal e As KeyEventArgs)
			If e.KeyCode = Keys.Enter Then
				e.Handled = True
				OwnerEdit.ClosePopup()
				Return
			End If
			MyBase.ProcessKeyDown(e)
		End Sub

		Public Overrides Sub ShowPopupForm()
			BeginControlUpdate()
			Try
				UpdateCheckListBox()
			Finally
				EndControlUpdate()
			End Try
			MyBase.ShowPopupForm()
			FocusFormControl(CheckListBox)
			OkButton.Enabled = True
		End Sub
		Private Sub FillList()
			CheckListBox.BeginUpdate()
			Try
				For Each column As GridColumn In View.Columns
					If column.OptionsColumn.ShowInCustomizationForm Then
						If column.Visible Then
							CheckListBox.Items.Add(column, column.GetTextCaption(),CheckState.Checked, True)
						Else
							CheckListBox.Items.Add(column, column.GetTextCaption(),CheckState.Unchecked, True)
						End If
					End If
				Next column
			Finally
				CheckListBox.EndUpdate()
			End Try
		End Sub
		Private Sub OnCheckListBoxItemCheck(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ItemCheckEventArgs)
			Dim hasVisibleButton As Boolean = False
			For i As Integer = 0 To CheckListBox.Items.Count - 1
				If CheckListBox.Items(i).CheckState = System.Windows.Forms.CheckState.Checked Then
					hasVisibleButton = True
					Exit For
				End If
			Next i
			OkButton.Enabled = hasVisibleButton
		End Sub
	End Class
End Namespace