namespace MinimalPlanner
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            entryBindingSource.DataSource = DataManager.GetBindingList();
            dataGridView1.Columns[2].DefaultCellStyle.Format = "dd.MM.yyyy HH:mm";
            
            NotificationManager.RegisterHandler(
                (entry) => MessageBox.Show(
                    $"{Math.Floor((entry.TimeOfEvent - DateTime.Now).TotalMinutes)}:" +
                    $"{Math.Truncate(((entry.TimeOfEvent - DateTime.Now).TotalSeconds)%60)} " +
                    $"left until \"{entry.Description}\"", "Event starts soon!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                );
            NotificationManager.RegisterDataSource(DataManager.GetBindingList());
            
            deleteButton.Enabled = false;
            editButton.Enabled = false;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            NotificationManager.ParseDate();
            if (dataGridView1.SelectedRows.Count == 1)
            {
                deleteButton.Enabled = true;
                editButton.Enabled = true;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var popup = new AddPopupForm();
            popup.ShowDialog();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            var selInd = dataGridView1.SelectedCells[0].RowIndex;
            var entry = dataGridView1.Rows[selInd].DataBoundItem as Entry;

            DataManager.Remove(entry!.Id);
            NotificationManager.RemoveDate(entry);
            dataGridView1.Refresh();
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            var selInd = dataGridView1.SelectedCells[0].RowIndex;
            var entry = dataGridView1.Rows[selInd].DataBoundItem as Entry;

            var popup = new EditPopupForm(entry!);
            
            var result = popup.ShowDialog();
            if (result == DialogResult.OK)
            {
                dataGridView1.Refresh();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                deleteButton.Enabled = true;
                editButton.Enabled = true;
            }
            else
            {
                deleteButton.Enabled = false;
                editButton.Enabled = false;
            }
        }
    }
}