namespace MinimalPlanner
{
    public partial class AddPopupForm : Form
    {
        public AddPopupForm()
        {
            InitializeComponent();
        }

        private void AddPopupForm_Load(object sender, EventArgs e)
        {
            txtDescription_SetDefaultText();
            dateTimePicker.Value = DateTime.Now.AddMinutes(2);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var entry = new Entry { 
                Description = txtDescription.Text,
                TimeOfEvent = DateTime.ParseExact(dateTimePicker.Text, "dd.MM.yyyy HH:mm", null)
            };
            DataManager.Add(entry);
            NotificationManager.AddDate(entry);

            DialogResult = DialogResult.OK;
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimePicker.Value > DateTime.Now)
                addButton.Enabled = true;
            else
                addButton.Enabled = false;
        }

        private void txtDescription_Enter(object sender, EventArgs e)
        {
            if (txtDescription.ForeColor == Color.Black)
                return;
            txtDescription.Text = "";
            txtDescription.ForeColor = Color.Black;
        }

        private void txtDescription_Leave(object sender, EventArgs e)
        {
            if (txtDescription.Text.Trim() == "")
                txtDescription_SetDefaultText();
        }

        private void txtDescription_SetDefaultText()
        {
            txtDescription.Text = "New event";
            txtDescription.ForeColor = Color.Gray;
        }
    }
}
