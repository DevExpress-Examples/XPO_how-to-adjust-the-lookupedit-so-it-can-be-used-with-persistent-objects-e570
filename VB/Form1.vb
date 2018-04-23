Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports DevExpress.Xpo
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Namespace LookupBoundToXPO
	''' <summary>
	''' Summary description for Form1.
	''' </summary>
	Public Class Form1
		Inherits System.Windows.Forms.Form
		Private lookUpEdit As DevExpress.XtraEditors.LookUpEdit
		Private WithEvents simpleButton1 As DevExpress.XtraEditors.SimpleButton
		Private unitOfWork1 As UnitOfWork
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.Container = Nothing

		Public Sub New()
			'
			' Required for Windows Form Designer support
			'
			InitializeComponent()

			InitObjects()
			_Project = unitOfWork1.FindObject(Of Project)(Nothing)

			InitLookUpEdit()
		End Sub

		''' <summary>
		''' Clean up any resources being used.
		''' </summary>
		Protected Overrides Overloads Sub Dispose(ByVal disposing As Boolean)
			If disposing Then
				If components IsNot Nothing Then
					components.Dispose()
				End If
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Windows Form Designer generated code"
		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.lookUpEdit = New DevExpress.XtraEditors.LookUpEdit()
			Me.simpleButton1 = New DevExpress.XtraEditors.SimpleButton()
			Me.unitOfWork1 = New DevExpress.Xpo.UnitOfWork()
			CType(Me.lookUpEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
			CType(Me.unitOfWork1, System.ComponentModel.ISupportInitialize).BeginInit()
			Me.SuspendLayout()
			' 
			' lookUpEdit
			' 
			Me.lookUpEdit.Location = New System.Drawing.Point(13, 14)
			Me.lookUpEdit.Name = "lookUpEdit"
			Me.lookUpEdit.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() { New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
			Me.lookUpEdit.Size = New System.Drawing.Size(187, 20)
			Me.lookUpEdit.TabIndex = 0
			' 
			' simpleButton1
			' 
			Me.simpleButton1.Location = New System.Drawing.Point(227, 14)
			Me.simpleButton1.Name = "simpleButton1"
			Me.simpleButton1.Size = New System.Drawing.Size(62, 20)
			Me.simpleButton1.TabIndex = 1
			Me.simpleButton1.Text = "Save"
'			Me.simpleButton1.Click += New System.EventHandler(Me.simpleButton1_Click);
			' 
			' Form1
			' 
			Me.ClientSize = New System.Drawing.Size(300, 50)
			Me.Controls.Add(Me.simpleButton1)
			Me.Controls.Add(Me.lookUpEdit)
			Me.Name = "Form1"
			Me.Text = "Form1"
			CType(Me.lookUpEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
			CType(Me.unitOfWork1, System.ComponentModel.ISupportInitialize).EndInit()
			Me.ResumeLayout(False)

		End Sub
		#End Region

		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		<STAThread> _
		Shared Sub Main()
			XpoDefault.DataLayer = XpoDefault.GetDataLayer(DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema)
			Application.Run(New Form1())
		End Sub


		Private Sub InitObjects()
			Using uow As New UnitOfWork()
				Dim proj As Project = uow.FindObject(Of Project)(Nothing)

				If proj Is Nothing Then

					Dim status As New ProjectStatus(uow)
					status.Name = "New"

					proj = New Project(uow)
					proj.Status = status

					' more status items
					status = New ProjectStatus(uow)
					status.Name = "Designed"

					status = New ProjectStatus(uow)
					status.Name = "Under construction"

					status = New ProjectStatus(uow)
					status.Name = "Ready"

					status = New ProjectStatus(uow)
					status.Name = "Released"

					status = New ProjectStatus(uow)
					status.Name = "Discontinued"

					uow.CommitChanges()
				End If
			End Using
		End Sub

		Private _Project As Project
		Public Property Project() As Project
			Get
				Return _Project
			End Get
			Set(ByVal value As Project)
				_Project = value
			End Set
		End Property

		Private Sub InitLookUpEdit()
			lookUpEdit.Properties.ValueMember = "This" ' attention!
			lookUpEdit.Properties.DisplayMember = "Name"
			lookUpEdit.Properties.Columns.Add(New LookUpColumnInfo("Name", 200))
			lookUpEdit.Properties.DataSource = New XPCollection(Of ProjectStatus)(unitOfWork1)
			lookUpEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True

			lookUpEdit.DataBindings.Add("EditValue", Me.Project, "Status!") ' attention!
		End Sub

		Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles simpleButton1.Click
			Me.Project.Save()
			unitOfWork1.CommitChanges()
		End Sub
	End Class

	Public Class ProjectStatus
		Inherits XPObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub

		Private _Name As String
		Public Property Name() As String
			Get
				Return _Name
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Name", _Name, value)
			End Set
		End Property
		Private _Description As String
		Public Property Description() As String
			Get
				Return _Description
			End Get
			Set(ByVal value As String)
				SetPropertyValue("Description", _Description, value)
			End Set
		End Property
	End Class

	Public Class Project
		Inherits XPObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub

		Private _Status As ProjectStatus
		Public Property Status() As ProjectStatus
			Get
				Return _Status
			End Get
			Set(ByVal value As ProjectStatus)
				SetPropertyValue("Status", _Status, value)
			End Set
		End Property
	End Class
End Namespace