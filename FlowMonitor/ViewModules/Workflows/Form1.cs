using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Diagram.Controls.DiagramControl;
using Diagram.Controls.PropertyEditor;
using Diagram.DiagramModel;
using Diagram.Dialogue;
using Diagram.PaletteManager;
using Newtonsoft.Json;
using View = Diagram.DiagramView.View;

namespace DiagramTest
{
    public partial class Form1 : Form
    {
        private PaletteManager paletteMgr;
        private PropertyEditor propertyEditor;
	    private Model model;
        private View view;
        private DiagramControl diagram;

        public Form1()
        {
            InitializeComponent();

            propertyEditor = new PropertyEditor();
            propertyEditor.Dock = DockStyle.Fill;
            propertyEditor.Name = "propertyEditor";
            splitContainer1.Panel2.Controls.Add(propertyEditor);

            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var reader = new JsonTextReader(new StreamReader(Path.Combine(path, "palette.json")));
            var paletteItems = new JsonSerializer().Deserialize<List<PaletteObj>>(reader);
            paletteMgr = new PaletteManager(symbolPalette, paletteItems);
        }

        private void LoadModel()
        {
            splitContainer1.Panel1.Controls.Clear();
            propertyEditor.Workflow = Model.Workflow;
            view = new View(model, propertyEditor);
            diagram = new DiagramControl(view);
            diagram.Name = "DiagramControl";
            diagram.BackColor = SystemColors.Window;
            diagram.Dock = DockStyle.Fill;
			splitContainer1.Panel1.Controls.Add(diagram);

            model.Router = new ConnectionRouter(model);
            model.RouteConnections(model.Router);
            
            Text = $"{Model.Workflow.Name} {Model.Workflow.Version}";
        }

		private void SaveWorkflow(object sender, System.EventArgs e)
		{
			model.Save();
		}

        private void SaveToFile(object sender, System.EventArgs e)
        {
            saveFileDialog1.FileName = $"{Model.Workflow.Name}_{Model.Workflow.Version}.json";
            if(saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                model.SaveToFile(saveFileDialog1.FileName);
        }

        private void OpenFromFile(object sender, System.EventArgs e)
        {
	        if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
	        {
		        model = new Model(openFileDialog1.FileName);
		        LoadModel();
	        }
        }

        private void OpenWorkflow(object sender, System.EventArgs e)
        {
            var dialogue = new WorkflowSelector();
            if(dialogue.ShowDialog() == DialogResult.OK)
            {
                var workflow = dialogue.Workflow;
                model = new Model(workflow.Name, workflow.Version);
                LoadModel();
            }
            
        }

        private void NewWorkflow(object sender, System.EventArgs e)
        {
            model = new Model();
            LoadModel();
        }

        private void DeleteSelected(object sender, System.EventArgs e)
        {
            diagram?.Document.DeleteSelected();
        }

        private void EnableEditItems(object sender, System.EventArgs e)
        {
            deleteSelectedMenuItem.Enabled = (view?.Selected != null);
        }

        private void EnableWorkflowItems(object sender, System.EventArgs e)
        {
            var enable = model != null;
            workflowSaveMenuItem.Enabled = enable;
            workflowSaveFileMenuItem.Enabled = enable;
        }
    }
}
