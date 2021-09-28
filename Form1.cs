using PdfiumViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFtoBitmapEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //VaryQualityLevel((Bitmap)pictureBox1.Image);
        }

        //edit picture
        private void button1_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            button2.Enabled = true;

            ConvertPDF();


        }

        //save edited single image
        private void button2_Click(object sender, EventArgs e)
        {
            //string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            SaveImageCustomPath(pictureBox2.Image);

                //string path = Path.Combine(saveFileDialog1, "newimg.jpg");

                //SaveEdit(pictureBox2.Image, path);
            
        }

        //open specific file
        private void button3_Click(object sender, EventArgs e)
        {
            var test = CreateBitmapAtRuntime((Bitmap)pictureBox1.Image);
            pictureBox2.Image = test;
        }

        private void SaveEdit(Image edit, String path)
        {
            edit.Save(path);
        }

    private Bitmap CreateBitmapAtRuntime(Bitmap edit)
        {
            Bitmap img = edit;
            Bitmap newImage;
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            newImage = img;
            Graphics gr = Graphics.FromImage(newImage);

            gr.DrawImageUnscaled(img, new Rectangle(160, 0, 200, 200));
          //  gr.DrawImageUnscaledAndClipped(img, new Rectangle(30, 0, 200, 200));
            return newImage;
        }

        private void ConvertPDF()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                var fileStream = openFileDialog.OpenFile();

                using (var pdfDocument = PdfiumViewer.PdfDocument.Load(@filePath))
                {
                   
                    var bitmapImage = pdfDocument.Render(0, 1000, 1000, PdfRenderFlags.CorrectFromDpi);
                    // bitmapImage.Save(@"image.bmp", ImageFormat.Bmp);
                    /////   pictureBox1.Size = bitmapImage.Size;
                    /////  pictureBox1.Image = bitmapImage;
                    pictureBox1.Image = bitmapImage;
                }
            }
        }


        private Image ConvertPDF(string filePath)
        {
       
                using (var pdfDocument = PdfiumViewer.PdfDocument.Load(@filePath))
                {
                    var bitmapImage = pdfDocument.Render(0, 1000, 1000, PdfRenderFlags.CorrectFromDpi);
                    // bitmapImage.Save(@"image.bmp", ImageFormat.Bmp);
                    /////   pictureBox1.Size = bitmapImage.Size;
                    /////  pictureBox1.Image = bitmapImage;
                    return bitmapImage;
                }
        
        }

        //edit multiple
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var progress = 0;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.DefaultExt = System.Drawing.Imaging.ImageFormat.Jpeg.ToString();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            // saveFileDialog1.Filter = "Tiff Image|*.tiff";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.FileName = "new.jpg";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var directory = Path.GetDirectoryName(openFileDialog.FileName);

                    string[] files = Directory.GetFiles(directory, "*.pdf");

                    progressBar1.Maximum = files.Count();

                    foreach (var file in files)
                    {
                        Image test = ConvertPDF(file);
                        var newimage = CreateBitmapAtRuntime((Bitmap)test);
                        SaveImageCustomPath(newimage, saveFileDialog1.FileName);
                        progress++;
                        progressBar1.Value = progress;
                    }
                }
            }
        }


        private void SaveImageCustomPath(Image edit)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.DefaultExt = System.Drawing.Imaging.ImageFormat.Jpeg.ToString();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.FileName = "new.jpg";



            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                if (saveFileDialog1.FileName != "")
                {
                    System.IO.FileStream fs =
                        (System.IO.FileStream)saveFileDialog1.OpenFile();

                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 1:
                            edit.Save(fs,
                              System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;

                        case 2:
                            edit.Save(fs,
                              System.Drawing.Imaging.ImageFormat.Bmp);
                            break;

                        case 3:
                            edit.Save(fs,
                              System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                    }

                    //string path = Path.Combine(saveFileDialog1, "newimg.jpg");

                    //SaveEdit(pictureBox2.Image, path);
                }
            }

        }

        private void SaveImageCustomPath(Bitmap edit, String path)
        {

            var count = 0;

            var newpath = path;

            while (System.IO.File.Exists(newpath))
            {
                count++;
               newpath =  Path.GetDirectoryName(path)
                     + Path.DirectorySeparatorChar
                     + Path.GetFileNameWithoutExtension(path)
                     + count.ToString()
                     + Path.GetExtension(path);
            }


            VaryQualityLevel(edit, newpath);
            //edit.Save(newpath,System.Drawing.Imaging.ImageFormat.Tiff);
               

            }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }


        private void VaryQualityLevel(Bitmap bmp, String path)
        {
            using (bmp)
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                System.Drawing.Imaging.Encoder myEncoder =
                    System.Drawing.Imaging.Encoder.Quality;

                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);

                myEncoderParameter = new EncoderParameter(myEncoder, 100L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp.Save(path, jpgEncoder, myEncoderParameters);

                // zero compression
                //myEncoderParameter = new EncoderParameter(myEncoder, 0L);
                //myEncoderParameters.Param[0] = myEncoderParameter;
                //bmp.Save(path, jpgEncoder, myEncoderParameters);
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


    }
    }

