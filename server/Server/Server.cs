using System;
using System.Text;
using System.Drawing;

using System.Windows.Forms;
using SimpleTCP;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Drawing.Drawing2D;
namespace Server
{
    public partial class Server : Form
    {
        private SimpleTcpServer server;
        private List<string> loggedInUsers;
        

        public Server()
        {
            InitializeComponent();
            loggedInUsers = new List<string>();
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
        }




        private void Server_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13; // Enter key
            server.StringEncoder = Encoding.UTF8;
            server.DataReceived += Server_DataReceived;
            
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                string message = e.MessageString.Trim();

                // Check if the message indicates a user login
                if (message.Contains("has logged in as"))
                {
                    // Extract the username from the message
                    int startIndex = message.IndexOf(':') + 1;
                    string username = message.Substring(startIndex).Trim();

                    // Add the username to the list of logged-in users
                    if (!loggedInUsers.Contains(username))
                    {
                        loggedInUsers.Add(username);
                        UpdateUserList();
                    }
                }

                txtStatus.Text = message;
                e.ReplyLine($"You said: {message}");
            });



        }



        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                BeginInvoke((Action)(() => { txtStatus.Text += "Server Starting ...."; }));
                System.Net.IPAddress ip = System.Net.IPAddress.Parse(txtHost.Text);

                server.Start(ip, Convert.ToInt32(txtPort.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error starting server: " + ex.Message);
            }
        }
        private void UpdateUserList()
        {
            // Clear the existing user list
            txtStatus.Items.Clear();

            // Add the updated list of logged-in users to the ListBox
            foreach (string user in loggedInUsers)
            {
                txtStatus.Items.Add(user);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void empImg_Click(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source=DESKTOP-VCQGRJ0\\SQLEXPRESS;Initial Catalog=dbOHMS;Integrated Security=True";
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string Query = "SELECT TOP (1000) [id], [empCode], [empSname], [empFname], [empOname], [empDOB], [empGender], [empContact], [empEmail], [empNationality], [empDateJoined], [empDepartment], [empDesignation], [empQualification], [empResidenceAddress], [empReferenceName], [empReferenceContact] FROM [dbOHMS].[dbo].[tblEmployees]";
            SqlCommand cmd = new SqlCommand(Query, con);
            var reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            con.Close();
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source=DESKTOP-VCQGRJ0\\SQLEXPRESS;Initial Catalog=dbOHMS;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string Query = "SELECT TOP (1000) [epmPhoto] FROM [dbOHMS].[dbo].[tblEmployees]";
                using (SqlCommand cmd = new SqlCommand(Query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);
                        dataGridView1.DataSource = table;
                    }
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "epmPhoto" && e.Value != null)
            {
                // Check if the value is a byte array
                if (e.Value is byte[])
                {
                    // Convert the value to a byte array
                    byte[] imageData = (byte[])e.Value;

                    // Resize the image
                    Image resizedImage = ResizeImage(imageData, 160, 117); // Set the dimensions you want here (160x117)

                    // Set the resized image to the cell
                    e.Value = resizedImage;
                }
            }
        }


        private Image ResizeImage(byte[] imageData, int width, int height)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                // Create an image from the byte array
                Image originalImage = Image.FromStream(ms);

                // Resize the image
                Image resizedImage = originalImage.GetThumbnailImage(width, height, null, IntPtr.Zero);

                return resizedImage;
            }
        }



        // Method to convert byte array to Image
        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source=DESKTOP-VCQGRJ0\\SQLEXPRESS;Initial Catalog=dbOHMS;Integrated Security=True";
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string Query = "SELECT TOP (1000) [id]\r\n      ,[patID]\r\n      ,[DocCode]\r\n      ,[consultDate]\r\n      ,[consurtTime]\r\n      ,[diagnoseDetails]\r\n      ,[Treatment]\r\n      ,[medication]\r\n      ,[testImage]\r\n  FROM [dbOHMS].[dbo].[tblConsultation]";
            SqlCommand cmd = new SqlCommand(Query, con);
            var reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView3.DataSource = table;
            con.Close();
        }


        private void dataGridView3_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView3.Columns[e.ColumnIndex].Name == "testImage" && e.Value != null)
            {
                // Check if the value is a byte array
                if (e.Value is byte[])
                {
                    // Convert the value to a byte array
                    byte[] imageData = (byte[])e.Value;

                    // Resize the image
                    Image resizedImage = ResizeImage(imageData, 160, 117); // Set the dimensions you want here (160x117)

                    // Set the resized image to the cell
                    e.Value = resizedImage;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source=DESKTOP-VCQGRJ0\\SQLEXPRESS;Initial Catalog=dbOHMS;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                string Query = "SELECT TOP (1000) [testImage] FROM [dbOHMS].[dbo].[tblConsultation]";
                using (SqlCommand cmd = new SqlCommand(Query, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable table = new DataTable();
                        table.Load(reader);
                        dataGridView3.DataSource = table;
                    }
                }
            }
        }

     

        private void button3_Click(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source=DESKTOP-VCQGRJ0\\SQLEXPRESS;Initial Catalog=dbOHMS;Integrated Security=True";
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string Query = "SELECT TOP (1000) [patID]\r\n      ,[patName]\r\n      ,[bmi]\r\n      ,[pressure]\r\n      ,[temperature]\r\n      ,[measuredOnDate]\r\n      ,[measuredOnTime]\r\n  FROM [dbOHMS].[dbo].[PatientWeight]";
            SqlCommand cmd = new SqlCommand(Query, con);
            var reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView2.DataSource = table;
            con.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source=DESKTOP-VCQGRJ0\\SQLEXPRESS;Initial Catalog=dbOHMS;Integrated Security=True";
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string Query = "SELECT TOP (1000) [id]\r\n      ,[supCode]\r\n      ,[supName]\r\n      ,[supContact]\r\n      ,[supType]\r\n      ,[supPersonInCharge]\r\n      ,[supContactPersonInCharge]\r\n      ,[supCountry]\r\n      ,[supEmail]\r\n      ,[supAddress]\r\n      ,[supAgreementDate]\r\n  FROM [dbOHMS].[dbo].[tblSupplier]";
            SqlCommand cmd = new SqlCommand(Query, con);
            var reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView4.DataSource = table;
            con.Close();
        }

      
    }
}
