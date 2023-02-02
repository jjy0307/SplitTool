using System;


namespace SplitTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.SelectedPath = @"C:\ResizedImages";
            folderBrowserDialog1.ShowNewFolderButton = true;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedPath = textBox1.Text;
            string savePath = @"C:\SplittedImages";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            string[] files = Directory.GetFiles(selectedPath, "*");
            foreach (string file in files)
            {
                try
                {
                    using (Image image = Image.FromFile(file))
                    {
                        int imgW = image.Width;
                        int imgH = image.Height;

                        int gridW = 300;
                        int gridH = 300;

                        int rangeW = (int)Math.Ceiling((double)imgW / gridW);
                        int rangeH = (int)Math.Ceiling((double)imgH / gridH);

                        for (int w = 0; w < rangeW; w++)
                        {
                            for (int h = 0; h < rangeH; h++)
                            {
                                Rectangle cropRect = new Rectangle(w * gridW, h * gridH, gridW, gridH);
                                Bitmap cropImage = new Bitmap(cropRect.Width, cropRect.Height);

                                using (Graphics g = Graphics.FromImage(cropImage))
                                {
                                    g.DrawImage(image, new Rectangle(0, 0, cropImage.Width, cropImage.Height), cropRect, GraphicsUnit.Pixel);
                                }
                                string folderName = Path.GetFileNameWithoutExtension(file);
                                string folderPath = Path.Combine(savePath, folderName);
                                if (!Directory.Exists(folderPath))
                                {
                                    Directory.CreateDirectory(folderPath);
                                }
                                cropImage.Save(Path.Combine(folderPath, $"{w * rangeH + h + 1}_{Path.GetFileName(file)}"));
                            }
                        }
                    }
                }
                catch(Exception) { }
            }
            MessageBox.Show("Splitting completed");

            //경로 열기
            System.Diagnostics.Process.Start("explorer.exe", savePath);
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}