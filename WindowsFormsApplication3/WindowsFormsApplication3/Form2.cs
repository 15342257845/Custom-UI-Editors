using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public string FilePath;
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.filepath = this.textBox1.Text;
            f1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //初始化一个OpenFileDialog类
            OpenFileDialog fileDialog = new OpenFileDialog();

            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                //获取用户选择文件的后缀名
                string extension = Path.GetExtension(fileDialog.FileName);
                //声明允许的后缀名
                string[] str = new string[] { ".xml" };
                if (!((IList)str).Contains(extension))
                {
                    MessageBox.Show("仅能选择xml格式的文件！");
                }
                else
                {
                    //获取用户选择的文件，fileInfo.Length是以字节为单位的
                    FileInfo fileInfo = new FileInfo(fileDialog.FileName);
                    string filepath = fileInfo.ToString();
                    textBox1.Text = filepath;

                }
            }
           
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            string path = @"C:\Users\admin\Desktop\save";
            if (Directory.Exists(path))
            {
                deleteAll(path);
            }
            else
            {
                Directory.CreateDirectory(@"C:\Users\admin\Desktop\save");
            }


          

        }

        public void deleteAll(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                    {
                     
                        File.Delete(d); //直接删除其中的文件
                    }
                    else
                     
                    deleteAll(d); //递归删除子文件夹
                }
               
            }
            Directory.Delete(dir);
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
           
           
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            
          
        }
    }
}
