using SelectPdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Doctor : Form
    {
        public Doctor()
        {
            InitializeComponent();
        }

        private void Doctor_Load(object sender, EventArgs e)
        {

        }

        private void empImg_Click_1(object sender, EventArgs e)
        {
            string ConnectionString = "Data Source=DESKTOP-VCQGRJ0\\SQLEXPRESS;Initial Catalog=dbOHMS;Integrated Security=True";
            SqlConnection con = new SqlConnection(ConnectionString);
            con.Open();
            string Query = "SELECT TOP (1000) [id]\r\n      ,[patID]\r\n      ,[DocCode]\r\n      ,[consultDate]\r\n      ,[consurtTime]\r\n      ,[diagnoseDetails]\r\n      ,[Treatment]\r\n      ,[medication]  FROM[dbOHMS].[dbo].[tblConsultation]";

            SqlCommand cmd = new SqlCommand(Query, con);
            var reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView1.DataSource = table;
            con.Close();
        }


        private void button2_Click_1(object sender, EventArgs e)
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
                        dataGridView1.DataSource = table;
                    }
                }
            }
        }

        private void showImg_Click_1(object sender, EventArgs e)
        {


            try
            {
                // Connection string to SQL Server
                string connectionString = "Data Source=DESKTOP-VCQGRJ0\\SQLEXPRESS;Initial Catalog=dbOHMS;Integrated Security=True";

                // SQL query to retrieve images from the database
                string query = "SELECT TOP (1000) [testImage] FROM [dbOHMS].[dbo].[tblConsultation] ORDER BY [id] DESC";

                string imagePath = @"D:\project\MediNet+\hospital-app\Hospital app\Server\x rays";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        byte[] imageData = (byte[])reader["testImage"];
                        string imageName = $"Image_{reader["testImage"]}.jpg"; // Customize the image name as per your requirement
                        string imagePathWithName = Path.Combine(imagePath, imageName);
                        File.WriteAllBytes(imagePathWithName, imageData);
                        imgPath.Text = "Image sent successfully";
                    }
                    else
                    {
                        Console.WriteLine("No image found in the database.");
                    }
                }


            }
            catch (Exception ex)
            {
                // Handle any exceptions and display the error message
                imgPath.Text = $"Error saving images: {ex.Message}";
            }
        }
        private void btnDownload_Click_1(object sender, EventArgs e)
        {
            // Connection string to SQL Server
            string connectionString = "Data Source=DESKTOP-VCQGRJ0\\SQLEXPRESS;Initial Catalog=dbOHMS;Integrated Security=True";

            /// SQL query to retrieve data from SQL Server
       
            string query = "SELECT TOP (1000) [id], [patID], [DocCode], [consultDate], [consurtTime], [diagnoseDetails], [Treatment], [medication] FROM [dbOHMS].[dbo].[tblConsultation]";

            // Create an HTML string to hold the table structure
            string htmlContent = "<html><head><title>Consultation Data</title></head><body><table border='1'><tr><th>ID</th><th>Patient ID</th><th>Doctor Code</th><th>Consultation Date</th><th>Consultation Time</th><th>Diagnosis Details</th><th>Treatment</th><th>Medication</th></tr>";

            // Retrieve data from SQL Server and append it to the HTML content
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    htmlContent += "<tr>";
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        htmlContent += "<td>" + reader[i].ToString() + "</td>";
                    }
                    htmlContent += "</tr>";
                }
            }

            // Close the table tag and the HTML document
            htmlContent += "</table></body></html>";



            // Convert HTML to PDF
            HtmlToPdf converter = new HtmlToPdf();
            PdfDocument pdfDocument = converter.ConvertHtmlString(htmlContent);

            // Path to save the PDF file
            string pdfPath = @"D:\project\MediNet+\hospital-app\Hospital app\Server\operated patient\reports.pdf";

            // Save the PDF to the specified path
            pdfDocument.Save(pdfPath);

            // Close the PDF document
            pdfDocument.Close();

            // Display a message in the text box
            Console.WriteLine("PDF file generated successfully.");
            Console.WriteLine($"PDF saved at: {pdfPath}");
            // Assuming there is a TextBox called "showPdf" in your UI
            showPdf.Text = $"PDF saved at Employee Data";
        }

 
    }
}
