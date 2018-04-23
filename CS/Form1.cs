using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace LookupBoundToXPO {
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class Form1 : System.Windows.Forms.Form {
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private UnitOfWork unitOfWork1;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Form1() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            InitObjects();
            _Project = unitOfWork1.FindObject<Project>(null);

            InitLookUpEdit();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing ) {
            if( disposing ) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.lookUpEdit = new DevExpress.XtraEditors.LookUpEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.unitOfWork1 = new DevExpress.Xpo.UnitOfWork();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitOfWork1)).BeginInit();
            this.SuspendLayout();
            // 
            // lookUpEdit
            // 
            this.lookUpEdit.Location = new System.Drawing.Point(13, 14);
            this.lookUpEdit.Name = "lookUpEdit";
            this.lookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit.Size = new System.Drawing.Size(187, 20);
            this.lookUpEdit.TabIndex = 0;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(227, 14);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(62, 20);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "Save";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(300, 50);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.lookUpEdit);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.unitOfWork1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
            Application.Run(new Form1());
        }


        private void InitObjects() {
            using(UnitOfWork uow = new UnitOfWork())
            {
                Project proj = uow.FindObject<Project>(null);

                if(proj == null) {

                    ProjectStatus status = new ProjectStatus(uow);
                    status.Name = "New";

                    proj = new Project(uow);
                    proj.Status = status;

                    // more status items
                    status = new ProjectStatus(uow);
                    status.Name = "Designed";

                    status = new ProjectStatus(uow);
                    status.Name = "Under construction";

                    status = new ProjectStatus(uow);
                    status.Name = "Ready";

                    status = new ProjectStatus(uow);
                    status.Name = "Released";

                    status = new ProjectStatus(uow);
                    status.Name = "Discontinued";

                    uow.CommitChanges();
                }
            }
        }

        private Project _Project;
        public Project Project {
            get { return _Project; }
            set {
                _Project = value;
            }
        }       

        private void InitLookUpEdit() {
            lookUpEdit.Properties.ValueMember = "This"; // attention!
            lookUpEdit.Properties.DisplayMember = "Name";
            lookUpEdit.Properties.Columns.Add(new LookUpColumnInfo("Name", 200));
            lookUpEdit.Properties.DataSource = new XPCollection<ProjectStatus>(unitOfWork1);
            lookUpEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
			
            lookUpEdit.DataBindings.Add("EditValue", this.Project, "Status!"); // attention!
        }

        private void simpleButton1_Click(object sender, System.EventArgs e) {
            this.Project.Save();
            unitOfWork1.CommitChanges();
        }
    }

    public class ProjectStatus : XPObject {
        public ProjectStatus(Session session) : base(session) { }

        private string _Name;
        public string Name {
            get {
                return _Name;
            }
            set {
                SetPropertyValue("Name", ref _Name, value);
            }
        }
        private string _Description;
        public string Description {
            get {
                return _Description;
            }
            set {
                SetPropertyValue("Description", ref _Description, value);
            }
        }
    }

    public class Project : XPObject {
        public Project(Session session) : base(session) { }

        private ProjectStatus _Status;
        public ProjectStatus Status {
            get {
                return _Status;
            }
            set {
                SetPropertyValue("Status", ref _Status, value);
            }
        }
    }
}