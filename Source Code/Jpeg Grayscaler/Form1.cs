using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace Jpeg_Grayscaler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // make sure they're actually dropping files (not text or anything else)
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                // allow them to continue
                // (without this, the cursor stays a "NO" symbol
                e.Effect = DragDropEffects.All;
        }

        private void listBox1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // transfer the filenames to a string array
            // (yes, everything to the left of the "=" can be put in the 
            // foreach loop in place of "files", but this is easier to understand.)
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // loop through the string array, adding each filename to the ListBox
            foreach (string file in files)
            {
                FileInfo finfo = new FileInfo(file);
                if (finfo.Exists == true)
                {
                    bool addfile = true;
                    foreach (string cf in listBox1.Items)
                    {
                        if (file == cf)
                        {
                            addfile = false;
                        }
                    }
                    if (addfile == true)
                    {
                        listBox1.Items.Add(file);
                    }
                }
                finfo = null;
            }
            if ((listBox1.SelectedIndex == -1) & (listBox1.SelectedIndices.Count < 1) & (listBox1.Items.Count > 0))
                listBox1.SelectedIndex = 0;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            } return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Enabled = false;
            button1.Enabled = false;
            foreach (string file in listBox1.Items)
            {
                try
                {
                    FileInfo finfo = new FileInfo(file);
                   
                    pb_color.Image = new Bitmap(finfo.FullName);
                    Bitmap b = (Bitmap)pb_color.Image;


                    int width = b.Size.Width;
                    int height = b.Size.Height;

                    for (int j = 0; j < height; j++)
                    {
                        for (int i = 0; i < width; i++)
                        {
                            Color col;
                            col = b.GetPixel(i, j);

                            b.SetPixel(i, j, Color.FromArgb((col.R + col.G + col.B) / 3, (col.R + col.G + col.B) / 3, (col.R + col.G + col.B) / 3));
                        }
                    }

                   



                    System.Drawing.Imaging.Encoder qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameter enc = new EncoderParameter(qualityEncoder, 100L);
                    EncoderParameters encs = new EncoderParameters(1);
                    encs.Param[0] = enc;
                    ImageCodecInfo coder;
                    coder = GetEncoderInfo("image/jpeg");
                    //MessageBox.Show(finfo.FullName.Remove(finfo.FullName.Length - finfo.Extension.Length) + ".jpg");
                    string savename = finfo.FullName.Remove(finfo.FullName.Length - finfo.Extension.Length) + ".jpg";
                    bool continu = false;
                    int counter = 0;
                    while (continu == false)
                    {
                        counter = counter + 1;
                        FileInfo tinfo = new FileInfo(savename);
                        if (tinfo.Exists == true)
                        {
                            continu = false;
                            savename = tinfo.FullName.Remove(tinfo.FullName.Length - tinfo.Extension.Length) + "(" + counter + ").jpg";
                        }
                        else
                        {
                            continu = true;
                        }
                        tinfo = null;
                    }


                    b.Save(savename, coder, encs);

                    finfo = null;
                }
                catch
                {
                    FileInfo finfo = new FileInfo(file);
                    MessageBox.Show("There was an error in processing " + finfo.Name, "Error Trapped", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    finfo = null;
                }
            }
         if (listBox1.Items.Count > 0)
         {
                                 MessageBox.Show("All files have been processed.", "Operation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                               
         }
         listBox1.Enabled = true;
         button1.Enabled = true;
         listBox1.Focus();
         if (listBox1.Items.Count > 0)
         {
                          listBox1.SelectedIndex = 0;
         }
        }

        private void DeleteItem()
        {
         if (listBox1.SelectedIndices.Count > 0)
                {
                    ListBox.SelectedIndexCollection indices = listBox1.SelectedIndices;
                    ArrayList arr = new ArrayList();
                    foreach (int i in indices)
                    {
                        arr.Add(i);
                    }
                    arr.Reverse();
                    foreach (int i in arr)
                    {
                        listBox1.Items.RemoveAt(i);

                    }
                    arr.Clear();
                    arr = null;
                    pb_color.Image.Dispose();
                    pb_color.Image = null;
                }
                else
                {
                    if (listBox1.SelectedIndex > -1)
                    {
                        listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                        pb_color.Image.Dispose();
                        pb_color.Image = null;
                    }
                }
    }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteItem();

   

            }
            if (e.KeyCode == Keys.Up)
            {
                if ((listBox1.Items.Count > 0) & (listBox1.SelectedIndex == -1))
                {
                    listBox1.SelectedIndex = 0;
                }
                int indices0 = 0;
                if (listBox1.SelectedIndices.Count > 0)
                {
                    indices0 = listBox1.SelectedIndices[0];
                }
                int indicesCount = listBox1.SelectedIndices.Count;
                int indice = listBox1.SelectedIndex;
                
                listBox1.SelectedIndex = -1;
                listBox1.SelectedIndices.Clear();

                if (indicesCount > 0)
                {
                    if ((indices0 - 1) > -1)
                    {
                        listBox1.SelectedIndex = indices0 - 1;
                    }
                    else
                    {
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    }
        
                }
                else
                {
                    if (indice > -1)
                    {
                        if ((indice - 1) > -1)
                        {
                            listBox1.SelectedIndex = indice - 1;
                        }
                        else
                        {
                            listBox1.SelectedIndex = listBox1.Items.Count - 1;
                        }
                    }
                }

            }
            if (e.KeyCode == Keys.Down)
            {
                if ((listBox1.Items.Count > 0) && (listBox1.SelectedIndex == -1))
                {
                    listBox1.SelectedIndex = 0;
                }
                int indices0 = 0;
                if (listBox1.SelectedIndices.Count > 0)
                {
                    indices0 = listBox1.SelectedIndices[0];
                }
                int indicesCount = listBox1.SelectedIndices.Count;
                int indice = listBox1.SelectedIndex;

                listBox1.SelectedIndex = -1;
                listBox1.SelectedIndices.Clear();

                if (indicesCount > 0)
                {
                    if ((indices0 + 1) < (listBox1.Items.Count))
                    {
                        listBox1.SelectedIndex = indices0 + 1;
                    }
                    else
                    {
                        listBox1.SelectedIndex = 0;
                    }

                }
                else
                {
                    if (indice > -1)
                    {
                        if ((indice + 1) > listBox1.Items.Count)
                        {
                            listBox1.SelectedIndex = indice + 1;
                        }
                        else
                        {
                            listBox1.SelectedIndex = 0;
                        }
                    }
                }

            }
            e.Handled = true;

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndices.Count > 0)
                {
                
                    pb_color.Image = Image.FromFile(listBox1.Items[listBox1.SelectedIndices[0]].ToString());
                }
                else
                {
                    if (listBox1.SelectedIndex > -1)
                    {
                                        pb_color.Image = Image.FromFile(listBox1.Items[listBox1.SelectedIndex].ToString());
                    }
                }
            }
            catch
            {
                if (listBox1.SelectedIndices.Count > 0)
                {
                    FileInfo finfo = new FileInfo(listBox1.Items[listBox1.SelectedIndices[0]].ToString());
                    MessageBox.Show("There was an error in processing " + finfo.Name + ". The file has been removed from the list of files to be processed.", "Error Trapped", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                    finfo = null;

                }
                else
                {
                    if (listBox1.SelectedIndex > -1)
                    {
                        FileInfo finfo = new FileInfo(listBox1.Items[listBox1.SelectedIndex].ToString());
                        MessageBox.Show("There was an error in processing " + finfo.Name + ". The file has been removed from the list of files to be processed.", "Error Trapped", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                        finfo = null;

                    }
                }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Focus();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ArrayList arr = new ArrayList();
            foreach (string i in listBox1.Items)
            {
                arr.Add(i);
            }
            listBox1.Items.Clear();
            arr.Sort();
            foreach (string i in arr)
            {
                listBox1.Items.Add(i);
            }
            arr.Clear();
            arr = null;
        }

    }


}