namespace MinimalPlanner
{
    public partial class EditPopupForm : Form
    {
        private readonly Entry _entry;

        public EditPopupForm(Entry entry)
        {
            InitializeComponent();
            _entry = entry;
        }


        private void EditPopupForm_Load(object sender, EventArgs e)
        {
            txtDescription.Text = _entry.Description;
            dateTimePicker.Value = _entry.TimeOfEvent;

            if (dateTimePicker.Value < DateTime.Now)
                editButton.Enabled = false;
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            DataManager.Update(_entry.Id, new Entry { 
                Description = txtDescription.Text, 
                TimeOfEvent = dateTimePicker.Value 
            });

            NotificationManager.EditDate(_entry);

            DialogResult = DialogResult.OK;
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker.Value > DateTime.Now)
                editButton.Enabled = true;
            else
                editButton.Enabled = false;            
        }
    }
}
