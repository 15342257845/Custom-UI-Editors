using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;




namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }



        //记录x坐标: 
        int xPos;
        //记录y坐标:
        int yPos;
        //记录是否按下鼠标:
        bool MoveFlag;
       
        //全局xml
        XmlDocument xml;
       
        //文件路径
        public string filepath;
      
     
        //xmlnodellist
        XmlNodeList nodeList;
     
       
        //节点名的数组
        string[] nodenameArray = new string[0];

        public string FilePath;

       
       
        XmlNode node;
        int b;
        int c;
        //XML每行的内容
      
        Panel p;

        Panel pchildrens;
        Panel pchildrens1;
        Panel pchildrens2;
        int[] array;
    

        int[] array1;
        int[] array2;



        int imgnum = 0;
  
        private readonly Dictionary<int, int> _indexList = new Dictionary<int, int>();
        private MLibrary _library;
        private MLibrary.MImage _selectedImage;


       

        bool startclick = false;
        int oldc;
        int[,] erweiarray = new int[100, 100];


        private void ClearInterface()
        {
            _selectedImage = null;          
        }
        
        private void Form1_Load_1(object sender, EventArgs e)
        {
           
            if (filepath!="")
            {

                //获取从第一个页面传过来的filepath的值
                this.DesktopBounds = Screen.GetWorkingArea(this);
                Form2 f2 = new Form2();


                xml = new XmlDocument();
                xml.Load(filepath);
                //获取根节点
                node = xml.SelectSingleNode("Config");
                //获得node下面的所有子节点
                nodeList = node.ChildNodes;
                RecursionTreeControl(xml.DocumentElement, treeView1.Nodes);//将加载完成的XML文件显示在TreeView控件中\\\

               
                for (int l = 0; l < 100; l++)
                {
                    for (int j = 0; j < l; j++)
                    {
                        erweiarray[l, j] = 0;
                       
                    }

                }

                for (int i = 0; i < 100; i++)
                {
                    array = new int[100];
                     array1 = new int[100];
                     array2= new int[100];
                    array[i] = 0;
                    array1[i] = 0;

                    array2[i] = 0;
                }

            }
            else
            {
                MessageBox.Show("请选择正确的路径!");
            } 
        }
        


        private void RecursionTreeControl(XmlNode xmlNode, TreeNodeCollection nodes)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)//循环遍历当前元素的子元素集合
            {
                TreeNode new_child = new TreeNode();//定义一个TreeNode节点对象
                new_child.Name = node.Attributes["ID"].Value;
                new_child.Text = node.Attributes["nodecaption"].Value;
                nodes.Add(new_child);//向当前TreeNodeCollection集合中添加当前节点
                RecursionTreeControl(node, new_child.Nodes);//调用本方法进行递归
                
            }

        }

 
        private void p_LocationChanged(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            g.Clear(p.BackColor);
            g.Dispose();//释放资源
        }


        private void pchildrens_LocationChanged(object sender, EventArgs e)
        {

        }
            



       

        //点击右边展示中间 的内容
        public void refresh()
        {
           
            panel1.Controls.Clear();
            string ID = treeView1.SelectedNode.Name;

            //如果id是子节点ID，那么就把父节点id给到c
            if (ID.IndexOf("_") != -1)
            {
                b = ID.IndexOf("_");
                c = int.Parse(ID.Substring(0, b)) - 1;//点击的最父节点的name
            }
            else
            {
                c = int.Parse(ID.Substring(0)) - 1;
            }
            if (c != oldc)
            {
                startclick = false;
            }
          

            if (startclick == false)
            {
                Directory.CreateDirectory(@"C:\Users\admin\Desktop\save\img" + imgnum);
            }
           

            //创建父节点panel 
            p = new Panel();
            p.Name = nodeList.Item(c).Attributes["ID"].Value;
            string iconpath = nodeList.Item(c).Attributes["iconpath"].Value;
            string beginimg = nodeList.Item(c).Attributes["beginimg"].Value;
            if (int.Parse(nodeList.Item(c).Attributes["width"].Value) == 0)
            {
                nodeList.Item(c).Attributes["width"].Value = "5";
            }
            if (int.Parse(nodeList.Item(c).Attributes["height"].Value) == 0)
            {
                nodeList.Item(c).Attributes["height"].Value = "5";
            }
            p.Left = int.Parse(nodeList.Item(c).Attributes["x"].Value);
            p.Top = int.Parse(nodeList.Item(c).Attributes["y"].Value);

            p.Width = int.Parse(nodeList.Item(c).Attributes["width"].Value);
            p.Height = int.Parse(nodeList.Item(c).Attributes["height"].Value);
            if (startclick == false)
            {

                _library = new MLibrary(nodeList.Item(c).Attributes["iconpath"].Value);
                if (_library.isnull == true) { }
                else
                {
                    _selectedImage = _library.GetMImage(int.Parse(nodeList.Item(c).Attributes["beginimg"].Value));

                    if (_selectedImage.Image == null) { }
                    else
                    {
                        string filename = @"C:\Users\admin\Desktop\save\img" + imgnum + "\\" + p.Name + "+" + pnum + ".jpg";

                        _selectedImage.Image.Save(filename);
                        p.BackgroundImage = Image.FromFile(filename);

                        p.BackgroundImageLayout = ImageLayout.Stretch;
                       
                    }
                }
            }
            else
            {
                try
                {
                    p.BackgroundImage = Image.FromFile("C:/Users/admin/Desktop/save/img" + (imgnum - 1) + "/" + p.Name + "+" + pnum + ".jpg");

                    p.BackgroundImageLayout = ImageLayout.Stretch;

                   
                }
                catch { }
               
                
            }




            p.BorderStyle = BorderStyle.FixedSingle;

            panel1.Controls.Add(p);

            p.MouseDown += new MouseEventHandler(rootmousedown);
            p.MouseMove += new MouseEventHandler(rootmousemove);
            p.MouseUp += new MouseEventHandler(rootmouseup);
            p.LocationChanged += new EventHandler(p_LocationChanged);


           

            for (int k = 0; k < nodeList.Item(c).ChildNodes.Count; k++)
            {
                
                pchildrens = new Panel();
                pchildrens.Name = nodeList.Item(c).ChildNodes[k].Attributes["ID"].Value;
                if (nodeList.Item(c).ChildNodes[k].Attributes["width"].Value=="0")
                {
                    nodeList.Item(c).ChildNodes[k].Attributes["width"].Value= "20";
                }
                if (nodeList.Item(c).ChildNodes[k].Attributes["height"].Value == "0")
                {
                    nodeList.Item(c).ChildNodes[k].Attributes["height"].Value = "20";
                }

                pchildrens.Left = int.Parse(nodeList.Item(c).ChildNodes[k].Attributes["x"].Value);
                pchildrens.Top = int.Parse(nodeList.Item(c).ChildNodes[k].Attributes["y"].Value);

                pchildrens.Width = int.Parse(nodeList.Item(c).ChildNodes[k].Attributes["width"].Value);
                pchildrens.Height = int.Parse(nodeList.Item(c).ChildNodes[k].Attributes["height"].Value);
                if (startclick == false)
                {
                    _library = new MLibrary(nodeList.Item(c).ChildNodes[k].Attributes["iconpath"].Value);

                    
                    string filename1 = @"C:\Users\admin\Desktop\save\img" + imgnum + "\\" + pchildrens.Name + "+" + array[k] + ".jpg";
                    if (_library.isnull == true) { }
                    else
                    {
                        _selectedImage = _library.GetMImage(int.Parse(nodeList.Item(c).ChildNodes[k].Attributes["beginimg"].Value));


                        if (_selectedImage.Image == null) { }
                        else
                        {
                            _selectedImage.Image.Save(filename1);
                            pchildrens.BackgroundImage = Image.FromFile(filename1);

                            pchildrens.BackgroundImageLayout = ImageLayout.Stretch;






                        }
                    }
                }
                else
                {
                    try
                    {
                        pchildrens.BackgroundImage = Image.FromFile(@"C:\Users\admin\Desktop\save\img" + (imgnum - 1) + "\\" + pchildrens.Name + "+" + array[k] + ".jpg");
                        pchildrens.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    catch 
                    {

                    } 
                   

                }

                


                pchildrens.BorderStyle = BorderStyle.FixedSingle;
                
                p.Controls.Add(pchildrens);



                pchildrens.MouseDown += new MouseEventHandler(mousedown);
                pchildrens.MouseMove += new MouseEventHandler(mousemove);
                pchildrens.MouseUp += new MouseEventHandler(mouseup);
                pchildrens.LocationChanged += new EventHandler(pchildrens_LocationChanged);

               

                if (nodeList.Item(c).ChildNodes[k].HasChildNodes)
                {
                    for (int i = 0; i < nodeList.Item(c).ChildNodes[k].ChildNodes.Count; i++)
                    {
                        pchildrens1 = new Panel();
                        pchildrens1.Name = nodeList.Item(c).ChildNodes[k].ChildNodes[i].Attributes["ID"].Value;
                       
                        pchildrens1.Left = int.Parse(nodeList.Item(c).ChildNodes[k].ChildNodes[i].Attributes["x"].Value);
                        pchildrens1.Top = int.Parse(nodeList.Item(c).ChildNodes[k].ChildNodes[i].Attributes["y"].Value);

                        pchildrens1.Width = int.Parse(nodeList.Item(c).ChildNodes[k].ChildNodes[i].Attributes["width"].Value);
                        pchildrens1.Height = int.Parse(nodeList.Item(c).ChildNodes[k].ChildNodes[i].Attributes["height"].Value);
                        pchildrens1.BorderStyle = BorderStyle.FixedSingle;

                        if (startclick == false)
                        {
                            _library = new MLibrary(nodeList.Item(c).ChildNodes[k].ChildNodes[i].Attributes["iconpath"].Value);


                            string filename1 = @"C:\Users\admin\Desktop\save\img" + imgnum + "\\"+pchildrens1.Name+"+" + array1[i] + ".jpg";
                            if (_library.isnull == true) { }
                            else
                            {
                                _selectedImage = _library.GetMImage(int.Parse(nodeList.Item(c).ChildNodes[k].ChildNodes[i].Attributes["beginimg"].Value));


                                if (_selectedImage.Image == null) { }
                                else
                                {
                                    _selectedImage.Image.Save(filename1);
                                    pchildrens1.BackgroundImage = Image.FromFile(filename1);

                                    pchildrens1.BackgroundImageLayout = ImageLayout.Stretch;
                                  
                                }
                            }

                            for (int l = 0; l < 100; l++)
                            {
                                for (int j = 0; j < l; j++)
                                {
                                    erweiarray[l, j] = 0;

                                }

                            }

                        }
                        else
                        {
                            try
                            {
                                pchildrens1.BackgroundImage = Image.FromFile(@"C:\Users\admin\Desktop\save\img" + (imgnum - 1) + "\\" + pchildrens1.Name + "+" + erweiarray[k, i] + ".jpg");
                                
                                pchildrens1.BackgroundImageLayout = ImageLayout.Stretch;
                            }
                            catch {}
                                
                         
                        }


                        pchildrens.Controls.Add(pchildrens1);

                        pchildrens1.MouseDown += new MouseEventHandler(mousedown);
                        pchildrens1.MouseMove += new MouseEventHandler(mousemove);
                        pchildrens1.MouseUp += new MouseEventHandler(mouseup);
                        pchildrens1.LocationChanged += new EventHandler(pchildrens_LocationChanged);




                        if (nodeList.Item(c).ChildNodes[k].ChildNodes[i].HasChildNodes)
                        {
                            for (int m = 0; m < nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes.Count; m++)
                            {
                                pchildrens2 = new Panel();
                                pchildrens2.Name = nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes[m].Attributes["ID"].Value;
                                
                                pchildrens2.Left = int.Parse(nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes[m].Attributes["x"].Value);
                                pchildrens2.Top = int.Parse(nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes[m].Attributes["y"].Value);
                                pchildrens2.Width = int.Parse(nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes[m].Attributes["width"].Value);
                                pchildrens2.Height = int.Parse(nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes[m].Attributes["height"].Value);
                                pchildrens2.BorderStyle = BorderStyle.FixedSingle;

                                if (startclick == false)
                                {
                                    _library = new MLibrary(nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes[m].Attributes["iconpath"].Value);


                                    string filename1 = @"C:\Users\admin\Desktop\save\img" + imgnum + "\\"+""+ + array2[m] + ".jpg";
                                    if (_library.isnull == true) { }
                                    else
                                    {
                                        _selectedImage = _library.GetMImage(int.Parse(nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes[m].Attributes["beginimg"].Value));


                                        if (_selectedImage.Image == null) { }
                                        else
                                        {
                                            _selectedImage.Image.Save(filename1);
                                            pchildrens2.BackgroundImage = Image.FromFile(filename1);

                                            pchildrens2.BackgroundImageLayout = ImageLayout.Stretch;
                                           
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        pchildrens2.BackgroundImage = Image.FromFile(@"C:\Users\admin\Desktop\save\img" + (imgnum - 1) + "\\" + pchildrens2.Name+"+"+ array2[m] + ".jpg");

                                        pchildrens2.BackgroundImageLayout = ImageLayout.Stretch;
                                      
                                    }
                                    catch
                                    { }
                                  
                                }

                                pchildrens1.Controls.Add(pchildrens2);

                                pchildrens2.MouseDown += new MouseEventHandler(mousedown);
                                pchildrens2.MouseMove += new MouseEventHandler(mousemove);
                                pchildrens2.MouseUp += new MouseEventHandler(mouseup);
                                pchildrens2.LocationChanged += new EventHandler(pchildrens_LocationChanged);


                            }
                        }
                    }
                }
            }

           
           
            oldc = c;

            if (startclick==false)
            {
                imgnum++;
            }
           
            
            startclick = true;
           
        }


      
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            string ID = treeView1.SelectedNode.Name;

            panel2.Controls.Clear();




            refreshleft(ID);

            refresh();
          
        }


        private void mouselosefocus(object sender, EventArgs e)
        {
          
        }



        
        int pnum = 0;
       
        private void txtkeypress(object sender, KeyPressEventArgs e)
        {


            if (e.KeyChar == System.Convert.ToChar(13))
            {
                TextBox TextBoxID = (TextBox)panel2.Controls.Find("ID", true)[0];
                TextBox iconpath = (TextBox)panel2.Controls.Find("iconpath", true)[0];
                TextBox beginimg = (TextBox)panel2.Controls.Find("beginimg", true)[0];
                TextBox x = (TextBox)panel2.Controls.Find("x", true)[0];
                TextBox y = (TextBox)panel2.Controls.Find("y", true)[0];
               
                TextBox width = (TextBox)panel2.Controls.Find("width", true)[0];
                TextBox height = (TextBox)panel2.Controls.Find("height", true)[0];


                string thisID = TextBoxID.Text;

                
                int appear = 0;

                for (int i = 0; i < thisID.Length; i++)

                {
                    if (thisID[i] == '_')
                    {
                        appear++;
                    }
                }
                

                if (appear == 1)
                {
                    int thisIDdownline = thisID.IndexOf("_");
                    int j = int.Parse(thisID.Substring(0,thisIDdownline))-1;
                    int q = int.Parse(thisID.Substring(thisIDdownline+1))-1;

                    array[q]++;
                    if (iconpath.Text != nodeList.Item(j).ChildNodes[q].Attributes["iconpath"].Value || beginimg.Text != nodeList.Item(j).ChildNodes[q].Attributes["beginimg"].Value)
                    {
                        nodeList.Item(j).ChildNodes[q].Attributes["iconpath"].Value = iconpath.Text;
                        nodeList.Item(j).ChildNodes[q].Attributes["beginimg"].Value = beginimg.Text;
                        _library = new MLibrary(nodeList.Item(j).ChildNodes[q].Attributes["iconpath"].Value);

                        if (_library.isnull == true) { }
                        else
                        {
                            _selectedImage = _library.GetMImage(int.Parse(nodeList.Item(j).ChildNodes[q].Attributes["beginimg"].Value));

                            string filename1 = "C:/Users/admin/Desktop/save/img" + (imgnum - 1) + "/" + TextBoxID.Text + "+" + array[q] + ".jpg";
                            if (_selectedImage.Image == null) { }
                            else
                            {

                                _selectedImage.Image.Save(filename1);

                                Panel pchildren = (Panel)panel1.Controls.Find(thisID, true)[0];

                                pchildren.BackgroundImage = Image.FromFile(filename1);

                                pchildren.BackgroundImageLayout = ImageLayout.Stretch;
                            }
                        }
                    }
                    else
                    {
                        Panel pchildren = (Panel)panel1.Controls.Find(thisID, true)[0];
                        nodeList.Item(c).ChildNodes[q].Attributes["width"].Value = width.Text;
                        nodeList.Item(c).ChildNodes[q].Attributes["height"].Value = height.Text;
                        nodeList.Item(c).ChildNodes[q].Attributes["x"].Value = x.Text;
                        nodeList.Item(c).ChildNodes[q].Attributes["y"].Value = y.Text;
                        pchildren.Width = int.Parse(width.Text);
                        pchildren.Height = int.Parse(height.Text);
                        pchildren.Left = int.Parse(x.Text);
                        pchildren.Top = int.Parse(y.Text);
                    }
                }


             



                if (appear == 0)
                {
                    int c = int.Parse(thisID)-1;
                    if (iconpath.Text != nodeList.Item(c).Attributes["iconpath"].Value || beginimg.Text != nodeList.Item(c).Attributes["beginimg"].Value)
                    {
                        nodeList.Item(c).Attributes["iconpath"].Value = iconpath.Text;
                        nodeList.Item(c).Attributes["beginimg"].Value = beginimg.Text;

                        _library = new MLibrary(nodeList.Item(c).Attributes["iconpath"].Value);
                        if (_library.isnull == true) { }
                        else
                        {
                            _selectedImage = _library.GetMImage(int.Parse(nodeList.Item(c).Attributes["beginimg"].Value));

                            string filename1 = "C:/Users/admin/Desktop/save/img" + (imgnum - 1) + "/" + p.Name + "+" + (pnum + 1) + ".jpg";
                            if (_selectedImage.Image == null) { }
                            else
                            {

                                _selectedImage.Image.Save(filename1);
                                Panel p = (Panel)panel1.Controls.Find(thisID, true)[0];
                                p.BackgroundImage = Image.FromFile(filename1);

                                p.BackgroundImageLayout = ImageLayout.Stretch;

                            }
                        }
                        pnum++;
                    }
                    else
                    {
                        nodeList.Item(c).Attributes["width"].Value = width.Text;
                        nodeList.Item(c).Attributes["height"].Value = height.Text;
                        nodeList.Item(c).Attributes["x"].Value = x.Text;
                        nodeList.Item(c).Attributes["y"].Value = y.Text;

                        p.Width = int.Parse(width.Text);
                        p.Height = int.Parse(height.Text);
                        p.Left = int.Parse(x.Text);
                        p.Top = int.Parse(y.Text);
                    }
                }




                if (appear == 2)
                {


                    string newstr;
                    newstr = thisID;
                    int q = 0;
                    int w = 0;

                    for (int i = 0; i < newstr.Length; i++)
                    {
                        newstr = newstr.Substring(newstr.IndexOf("_") + 1);
                        if (i == 0)
                        {
                            q = int.Parse(newstr.Substring(0, newstr.IndexOf("_"))) - 1;

                        }
                        if (i == 1)
                        {
                            w = int.Parse(newstr.Substring(newstr.IndexOf("_") + 1)) - 1;
                           
                        }
                    }





                    erweiarray[q, w]++;  
                    int thisIDdownline = thisID.IndexOf("_");
                    int j = int.Parse(thisID.Substring(0, thisIDdownline)) - 1;

                    if (iconpath.Text != nodeList.Item(j).ChildNodes[q].ChildNodes[w].Attributes["iconpath"].Value || beginimg.Text != nodeList.Item(j).ChildNodes[q].ChildNodes[w].Attributes["beginimg"].Value)
                    {
                        nodeList.Item(j).ChildNodes[q].ChildNodes[w].Attributes["iconpath"].Value = iconpath.Text;
                        nodeList.Item(j).ChildNodes[q].ChildNodes[w].Attributes["beginimg"].Value = beginimg.Text;


                        _library = new MLibrary(nodeList.Item(j).ChildNodes[q].ChildNodes[w].Attributes["iconpath"].Value);

                        if (_library.isnull == true) { }
                        else
                        {
                            _selectedImage = _library.GetMImage(int.Parse(nodeList.Item(j).ChildNodes[q].ChildNodes[w].Attributes["beginimg"].Value));

                            string filename1 = "C:/Users/admin/Desktop/save/img" + (imgnum - 1) + "/" + TextBoxID.Text + "+" + erweiarray[q,w] + ".jpg";
                            if (_selectedImage.Image == null) { }
                            else
                            {

                                _selectedImage.Image.Save(filename1);

                                Panel pchildrens1 = (Panel)panel1.Controls.Find(thisID, true)[0];

                                pchildrens1.BackgroundImage = Image.FromFile(filename1);

                                pchildrens1.BackgroundImageLayout = ImageLayout.Stretch;

                            }

                        }


                    }
                    else
                    {
                        Panel pchildrens1 = (Panel)panel1.Controls.Find(thisID, true)[0];
                        nodeList.Item(c).ChildNodes[q].ChildNodes[w].Attributes["width"].Value = width.Text;
                        nodeList.Item(c).ChildNodes[q].ChildNodes[w].Attributes["height"].Value = height.Text;
                        nodeList.Item(c).ChildNodes[q].ChildNodes[w].Attributes["x"].Value = x.Text;
                        nodeList.Item(c).ChildNodes[q].ChildNodes[w].Attributes["y"].Value = y.Text;
                        pchildrens1.Width = int.Parse(width.Text);
                        pchildrens1.Height = int.Parse(height.Text);
                        pchildrens1.Left = int.Parse(x.Text);
                        pchildrens1.Top = int.Parse(y.Text);
                    }
                }

                xml.Save(filepath);
            }
           
        }


        


        private void rootmousedown(object sender, MouseEventArgs e)
        {

            MoveFlag = true;//已经按下.
            xPos = e.X;//当前x坐标.
            yPos = e.Y;//当前y坐标.

        }

      
        private void rootmousemove(object sender, MouseEventArgs e)
        {

            if (MoveFlag)
            {
                Panel panel = (Panel)sender;
                Control q = (Control)panel1.Controls.Find(panel.Name, true)[0];
                q.Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                q.Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.

                Control xtextbox = (Control)panel2.Controls.Find("x", true)[0];
                xtextbox.Text = q.Left.ToString();
                Control ytextbox = (Control)panel2.Controls.Find("y", true)[0];
                ytextbox.Text = q.Top.ToString();
            }
        }


       


        private void rootmouseup(object sender, MouseEventArgs e)
        {
            MoveFlag = false;

            Panel panel = (Panel)sender;
            Control q = (Control)panel1.Controls.Find(panel.Name, true)[0];

            nodeList.Item(c).Attributes["x"].Value = q.Left.ToString();
            nodeList.Item(c).Attributes["y"].Value = q.Top.ToString();
            xml.Save(filepath);
            panel2.Controls.Clear();
            Panel ID = (Panel)sender;

            for (int v = 0; v < nodeList.Count; v++)
            {

                if (nodeList.Item(v).Attributes["ID"].Value == ID.Name)
                {
                    for (int vv = 0; vv < nodeList.Item(v).Attributes.Count; vv++)
                    {
                        Label lab = new Label();
                        TextBox txt = new TextBox();

                        lab.Name = "lable" + nodeList.Item(v).Attributes[vv].Name;
                        lab.Text = nodeList.Item(v).Attributes[vv].Name;


                        txt.Name = nodeList.Item(v).Attributes[vv].Name;
                        txt.Text = nodeList.Item(v).Attributes[vv].Value;


                        int yLocation = 30;

                        lab.Width = 80;
                        lab.Location = new Point(10, yLocation * (vv + 1));


                        txt.Width = 120;
                        txt.Location = new Point(95, yLocation * (vv + 1));
                        txt.LostFocus += new EventHandler(mouselosefocus);
                        txt.KeyPress += new KeyPressEventHandler(txtkeypress);

                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("filename") != -1)
                        {
                            string filenamet = "filename";
                            string filenamenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(filenamet.Length.ToString()));
                            lab.Text = "文件名" + filenamenum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("fnode") != -1)
                        {
                            string fnode = "fnode";
                            string fnodenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(fnode.Length.ToString()));
                            lab.Text = "f节点" + fnodenum;
                        }

                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("nodecaption") != -1)
                        {
                            string nodecaption = "nodecaption";
                            string nodecaptionnum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(nodecaption.Length.ToString()));
                            lab.Text = "节点中文名称" + nodecaptionnum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("nodename") != -1)
                        {
                            string nodeName = "nodename";
                            string nodeNamenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(nodeName.Length.ToString()));
                            lab.Text = "节点名" + nodeNamenum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("iconpath") != -1)
                        {
                            string iconPath = "iconpath";
                            string iconPathnum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(iconPath.Length.ToString()));
                            lab.Text = "图片路径" + iconPathnum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("imgfile") != -1)
                        {
                            string imgfile = "imgfile";
                            string imgfilenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(imgfile.Length.ToString()));
                            lab.Text = "图片文件名" + imgfilenum;

                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("beginimg") != -1)
                        {
                            string imgSubBegin = "beginimg";
                            string imgSubBeginnum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(imgSubBegin.Length.ToString()));
                            lab.Text = "图片开始" + imgSubBeginnum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("endimg") != -1)
                        {
                            string imgSubEnd = "endimg";
                            string imgSubEndnum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(imgSubEnd.Length.ToString()));
                            lab.Text = "图片结束" + imgSubEndnum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("x") != -1)
                        {
                            string x = "x";
                            string xnum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(x.Length.ToString()));
                            lab.Text = "x坐标" + xnum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("y") != -1)
                        {
                            string y = "y";
                            string ynum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(y.Length.ToString()));
                            lab.Text = "y坐标" + ynum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("ishide") != -1)
                        {
                            string isHide = "ishide";
                            string isHidenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(isHide.Length.ToString()));
                            lab.Text = "是否隐藏" + isHidenum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("width") != -1)
                        {
                            string Width = "width";
                            string isHidenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(Width.Length.ToString()));
                            lab.Text = "宽" + isHidenum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("height") != -1)
                        {
                            string Height = "height";
                            string isHidenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(Height.Length.ToString()));
                            lab.Text = "高" + isHidenum;
                        }

                        panel2.Controls.Add(lab);
                        panel2.Controls.Add(txt);
                        if (txt.Name == "ID" || txt.Name == "filename" || txt.Name == "fnode" || txt.Name == "nodecaption" || txt.Name == "nodename")
                        {
                            txt.Enabled = false;
                        }

                    }

                }
            }

        }



      
         

        private void mousedown(object sender, MouseEventArgs e)
        {
            MoveFlag = true;//已经按下.
            xPos = e.X;//当前x坐标.
            yPos = e.Y;//当前y坐标.
            
        }
        private void mousemove(object sender, MouseEventArgs e)
        {

            if (MoveFlag)
            {
                Panel panel = (Panel)sender;
                Control q = (Control)p.Controls.Find(panel.Name, true)[0]; 

                q.Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                q.Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.

                Control xtextbox = (Control)panel2.Controls.Find("x", true)[0];
                xtextbox.Text = q.Left.ToString();
                Control ytextbox = (Control)panel2.Controls.Find("y", true)[0];
                ytextbox.Text = q.Top.ToString();

              
            }
        }




        private void mouseup(object sender, MouseEventArgs e)
        {
            MoveFlag = false;
            Panel ID = (Panel)sender;
            Panel panel = (Panel)sender;

            Control q = (Control)panel1.Controls.Find(panel.Name, true)[0];


            //p.BackgroundImage = Image.FromFile(@"C:\Users\admin\Desktop\save\img" + (imgnum - 1) + "\\" + p.Name + ".jpg");
            //p.BackgroundImageLayout = ImageLayout.Stretch;


            for (int k = 0; k < nodeList.Item(c).ChildNodes.Count; k++)
            {

                if (nodeList.Item(c).ChildNodes[k].Attributes["ID"].Value == panel.Name)
                {
                    nodeList.Item(c).ChildNodes[k].Attributes["x"].Value = q.Left.ToString();
                    nodeList.Item(c).ChildNodes[k].Attributes["y"].Value = q.Top.ToString();

                }
                else
                {
                    if (nodeList.Item(c).ChildNodes[k].HasChildNodes)
                    {
                        for (int i = 0; i < nodeList.Item(c).ChildNodes[k].ChildNodes.Count; i++)
                        {

                            if (nodeList.Item(c).ChildNodes[k].ChildNodes[i].Attributes["ID"].Value == panel.Name)
                            {
                                nodeList.Item(c).ChildNodes[k].ChildNodes[i].Attributes["x"].Value = q.Left.ToString();
                                nodeList.Item(c).ChildNodes[k].ChildNodes[i].Attributes["y"].Value = q.Top.ToString();

                            }
                            else
                            {
                                if (nodeList.Item(c).ChildNodes[k].ChildNodes[i].HasChildNodes)
                                {
                                    for (int m = 0; m < nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes.Count; m++)
                                    {
                                        if (nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes[m].Attributes["ID"].Value == panel.Name)
                                        {
                                            nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes[m].Attributes["x"].Value = q.Left.ToString();
                                            nodeList.Item(c).ChildNodes[k].ChildNodes[i].ChildNodes[m].Attributes["y"].Value = q.Top.ToString();

                                        }

                                    }
                                }
                            }

                        }
                    }
                }

            }
            xml.Save(filepath);

            panel2.Controls.Clear();

            refreshleft(ID.Name);
        }

        public void refreshleft(string ID)
        {
            for (int v = 0; v < nodeList.Count; v++)
            {

                if (nodeList.Item(v).Attributes["ID"].Value == ID)
                {
                    for (int vv = 0; vv < nodeList.Item(v).Attributes.Count; vv++)
                    {
                        Label lab = new Label();
                        TextBox txt = new TextBox();
                        ComboBox cb = new ComboBox();

                        lab.Name = "lable" + nodeList.Item(v).Attributes[vv].Name;
                        lab.Text = nodeList.Item(v).Attributes[vv].Name;

                        //if (nodeList.Item(v).Attributes[vv].Name == "iconPath")
                        //{
                        //    cb.Name = nodeList.Item(v).Attributes[vv].Name;
                        //    cb.Text = nodeList.Item(v).Attributes[vv].Value;


                        //    int yLocation = 30;

                        //    lab.Width = 80;
                        //    lab.Location = new Point(10, yLocation * (vv + 1));


                        //    cb.Width = 100;
                        //    cb.Location = new Point(95, yLocation * (vv + 1));

                        //}
                        //else
                        //{

                        txt.Name = nodeList.Item(v).Attributes[vv].Name;
                        txt.Text = nodeList.Item(v).Attributes[vv].Value;


                        int yLocation = 30;

                        lab.Width = 80;
                        lab.Location = new Point(10, yLocation * (vv + 1));


                        txt.Width = 120;
                        txt.Location = new Point(95, yLocation * (vv + 1));
                        //   }

                        txt.LostFocus += new EventHandler(mouselosefocus);
                        txt.KeyPress += new KeyPressEventHandler(txtkeypress);
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("filename") != -1)
                        {
                            string filenamet = "filename";
                            string filenamenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(filenamet.Length.ToString()));
                            lab.Text = "文件名" + filenamenum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("fnode") != -1)
                        {
                            string fnode = "fnode";
                            string fnodenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(fnode.Length.ToString()));
                            lab.Text = "f节点" + fnodenum;
                        }

                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("nodecaption") != -1)
                        {
                            string nodecaption = "nodecaption";
                            string nodecaptionnum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(nodecaption.Length.ToString()));
                            lab.Text = "节点中文名称" + nodecaptionnum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("nodename") != -1)
                        {
                            string nodeName = "nodename";
                            string nodeNamenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(nodeName.Length.ToString()));
                            lab.Text = "节点名" + nodeNamenum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("iconpath") != -1)
                        {
                            string iconPath = "iconpath";
                            string iconPathnum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(iconPath.Length.ToString()));
                            lab.Text = "图片路径" + iconPathnum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("imgfile") != -1)
                        {
                            string imgfile = "imgfile";
                            string imgfilenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(imgfile.Length.ToString()));
                            lab.Text = "图片文件名" + imgfilenum;

                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("beginimg") != -1)
                        {
                            string imgSubBegin = "beginimg";
                            string imgSubBeginnum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(imgSubBegin.Length.ToString()));
                            lab.Text = "图片开始" + imgSubBeginnum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("endimg") != -1)
                        {
                            string imgSubEnd = "endimg";
                            string imgSubEndnum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(imgSubEnd.Length.ToString()));
                            lab.Text = "图片结束" + imgSubEndnum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("x") != -1)
                        {
                            string x = "x";
                            string xnum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(x.Length.ToString()));
                            lab.Text = "x坐标" + xnum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("y") != -1)
                        {
                            string y = "y";
                            string ynum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(y.Length.ToString()));
                            lab.Text = "y坐标" + ynum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("ishide") != -1)
                        {
                            string isHide = "ishide";
                            string isHidenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(isHide.Length.ToString()));
                            lab.Text = "是否隐藏" + isHidenum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("width") != -1)
                        {
                            string Width = "width";
                            string isHidenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(Width.Length.ToString()));
                            lab.Text = "宽" + isHidenum;
                        }
                        if (nodeList.Item(v).Attributes[vv].Name.IndexOf("height") != -1)
                        {
                            string Height = "height";
                            string isHidenum = nodeList.Item(v).Attributes[vv].Name.Substring(int.Parse(Height.Length.ToString()));
                            lab.Text = "高" + isHidenum;
                        }

                        panel2.Controls.Add(lab);
                        //if (nodeList.Item(v).Attributes[vv].Name == "iconPath")
                        //{
                        //    panel2.Controls.Add(cb);
                        //}
                        //else
                        //{
                        panel2.Controls.Add(txt);
                        // }
                        if (txt.Name == "ID" || txt.Name == "filename" || txt.Name == "fnode" || txt.Name == "nodecaption" || txt.Name == "nodename")
                        {
                            txt.Enabled = false;
                        }
                    }


                }
                else
                {
                    for (int vv = 0; vv < nodeList.Item(v).ChildNodes.Count; vv++)
                    {

                        if (nodeList.Item(v).ChildNodes[vv].Attributes["ID"].Value == ID)
                        {

                            for (int vvv = 0; vvv < nodeList.Item(v).ChildNodes[vv].Attributes.Count; vvv++)
                            {
                                Label lab = new Label();
                                TextBox txt = new TextBox();
                                ComboBox cb = new ComboBox();

                                lab.Name = "lable" + nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name;
                                lab.Text = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name;

                                //if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name == "iconPath")
                                //{
                                //    cb.Name = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name;
                                //    cb.Text = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Value;


                                //    int yLocation = 30;

                                //    lab.Width = 80;
                                //    lab.Location = new Point(10, yLocation * (vvv + 1));


                                //    cb.Width = 100;
                                //    cb.Location = new Point(95, yLocation * (vvv + 1));

                                //}
                                //else
                                //{

                                txt.Name = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name;
                                txt.Text = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Value;


                                int yLocation = 30;

                                lab.Width = 80;
                                lab.Location = new Point(10, yLocation * (vvv + 1));


                                txt.Width = 120;
                                txt.Location = new Point(95, yLocation * (vvv + 1));
                                //  }
                                txt.LostFocus += new EventHandler(mouselosefocus);
                                txt.KeyPress += new KeyPressEventHandler(txtkeypress);

                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("filename") != -1)
                                {
                                    string filenamet = "filename";
                                    string filenamenum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(filenamet.Length.ToString()));
                                    lab.Text = "文件名" + filenamenum;
                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("fnode") != -1)
                                {
                                    string fnode = "fnode";
                                    string fnodenum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(fnode.Length.ToString()));
                                    lab.Text = "f节点" + fnodenum;
                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("nodecaption") != -1)
                                {
                                    string nodecaption = "nodecaption";
                                    string nodecaptionnum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(nodecaption.Length.ToString()));
                                    lab.Text = "节点中文名称" + nodecaptionnum;
                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("nodename") != -1)
                                {
                                    string nodeName = "nodename";
                                    string nodeNamenum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(nodeName.Length.ToString()));
                                    lab.Text = "节点名" + nodeNamenum;
                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("iconpath") != -1)
                                {
                                    string iconPath = "iconpath";
                                    string iconPathnum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(iconPath.Length.ToString()));
                                    lab.Text = "图片路径" + iconPathnum;
                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("imgfile") != -1)
                                {
                                    string imgfile = "imgfile";
                                    string imgfilenum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(imgfile.Length.ToString()));
                                    lab.Text = "图片文件名" + imgfilenum;

                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("beginimg") != -1)
                                {
                                    string imgSubBegin = "beginimg";
                                    string imgSubBeginnum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(imgSubBegin.Length.ToString()));
                                    lab.Text = "图片开始" + imgSubBeginnum;
                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("endimg") != -1)
                                {
                                    string imgSubEnd = "endimg";
                                    string imgSubEndnum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(imgSubEnd.Length.ToString()));
                                    lab.Text = "图片结束" + imgSubEndnum;
                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("x") != -1)
                                {
                                    string x = "x";
                                    string xnum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(x.Length.ToString()));
                                    lab.Text = "x坐标" + xnum;
                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("y") != -1)
                                {
                                    string y = "y";
                                    string ynum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(y.Length.ToString()));
                                    lab.Text = "y坐标" + ynum;
                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("ishide") != -1)
                                {
                                    string isHide = "ishide";
                                    string isHidenum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(isHide.Length.ToString()));
                                    lab.Text = "是否隐藏" + isHidenum;
                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("width") != -1)
                                {
                                    string Width = "width";
                                    string isHidenum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(Width.Length.ToString()));
                                    lab.Text = "宽" + isHidenum;
                                }
                                if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.IndexOf("height") != -1)
                                {
                                    string Height = "height";
                                    string isHidenum = nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name.Substring(int.Parse(Height.Length.ToString()));
                                    lab.Text = "高" + isHidenum;
                                }


                                panel2.Controls.Add(lab);
                                //if (nodeList.Item(v).ChildNodes[vv].Attributes[vvv].Name == "iconPath")
                                //{
                                //    panel2.Controls.Add(cb);
                                //}
                                //else
                                //{
                                panel2.Controls.Add(txt);
                                //   }

                                if (txt.Name == "ID" || txt.Name == "filename" || txt.Name == "fnode" || txt.Name == "nodecaption" || txt.Name == "nodename")
                                {
                                    txt.Enabled = false;
                                }
                            }

                        }
                        else
                        {
                            for (int vvvv = 0; vvvv < nodeList.Item(v).ChildNodes[vv].ChildNodes.Count; vvvv++)
                            {
                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes["ID"].Value == ID)
                                {

                                    for (int vvvvv = 0; vvvvv < nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes.Count; vvvvv++)
                                    {
                                        Label lab = new Label();
                                        TextBox txt = new TextBox();
                                        ComboBox cb = new ComboBox();

                                        lab.Name = "lable" + nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name;
                                        lab.Text = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name;

                                        //if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name == "iconPath")
                                        //{
                                        //    cb.Name = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name;
                                        //    cb.Text = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Value;


                                        //    int yLocation = 30;

                                        //    lab.Width = 80;
                                        //    lab.Location = new Point(10, yLocation * (vvvvv + 1));


                                        //    cb.Width = 100;
                                        //    cb.Location = new Point(95, yLocation * (vvvvv + 1));

                                        //}
                                        //else
                                        //{
                                        txt.Name = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name;
                                        txt.Text = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Value;


                                        int yLocation = 30;

                                        lab.Width = 80;
                                        lab.Location = new Point(10, yLocation * (vvvvv + 1));


                                        txt.Width = 120;
                                        txt.Location = new Point(95, yLocation * (vvvvv + 1));
                                        //   }

                                        txt.LostFocus += new EventHandler(mouselosefocus);
                                        txt.KeyPress += new KeyPressEventHandler(txtkeypress);
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("filename") != -1)
                                        {
                                            string filenamet = "filename";
                                            string filenamenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(filenamet.Length.ToString()));
                                            lab.Text = "文件名" + filenamenum;
                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("fnode") != -1)
                                        {
                                            string fnode = "fnode";
                                            string fnodenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(fnode.Length.ToString()));
                                            lab.Text = "f节点" + fnodenum;
                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("nodecaption") != -1)
                                        {
                                            string nodecaption = "nodecaption";
                                            string nodecaptionnum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(nodecaption.Length.ToString()));
                                            lab.Text = "节点中文名称" + nodecaptionnum;
                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("nodename") != -1)
                                        {
                                            string nodeName = "nodename";
                                            string nodeNamenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(nodeName.Length.ToString()));
                                            lab.Text = "节点名" + nodeNamenum;
                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("iconpath") != -1)
                                        {
                                            string iconPath = "iconpath";
                                            string iconPathnum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(iconPath.Length.ToString()));
                                            lab.Text = "图片路径" + iconPathnum;
                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("imgfile") != -1)
                                        {
                                            string imgfile = "imgfile";
                                            string imgfilenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(imgfile.Length.ToString()));
                                            lab.Text = "图片文件名" + imgfilenum;

                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("beginimg") != -1)
                                        {
                                            string imgSubBegin = "beginimg";
                                            string imgSubBeginnum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(imgSubBegin.Length.ToString()));
                                            lab.Text = "图片开始" + imgSubBeginnum;
                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("endimg") != -1)
                                        {
                                            string imgSubEnd = "endimg";
                                            string imgSubEndnum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(imgSubEnd.Length.ToString()));
                                            lab.Text = "图片结束" + imgSubEndnum;
                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("x") != -1)
                                        {
                                            string x = "x";
                                            string xnum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(x.Length.ToString()));
                                            lab.Text = "x坐标" + xnum;
                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("y") != -1)
                                        {
                                            string y = "y";
                                            string ynum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(y.Length.ToString()));
                                            lab.Text = "y坐标" + ynum;
                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("ishide") != -1)
                                        {
                                            string isHide = "ishide";
                                            string isHidenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(isHide.Length.ToString()));
                                            lab.Text = "是否隐藏" + isHidenum;
                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("width") != -1)
                                        {
                                            string Width = "width";
                                            string isHidenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(Width.Length.ToString()));
                                            lab.Text = "宽" + isHidenum;
                                        }
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.IndexOf("height") != -1)
                                        {
                                            string Height = "height";
                                            string isHidenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name.Substring(int.Parse(Height.Length.ToString()));
                                            lab.Text = "高" + isHidenum;
                                        }

                                        panel2.Controls.Add(lab);

                                        //if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].Attributes[vvvvv].Name == "iconPath")
                                        //{
                                        //    panel2.Controls.Add(cb);
                                        //}
                                        //else
                                        //{
                                        panel2.Controls.Add(txt);
                                        //   }

                                        if (txt.Name == "ID" || txt.Name == "filename" || txt.Name == "fnode" || txt.Name == "nodecaption" || txt.Name == "nodename")
                                        {
                                            txt.Enabled = false;
                                        }
                                    }

                                }
                                else
                                {
                                    for (int vvvvvv = 0; vvvvvv < nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes.Count; vvvvvv++)
                                    {
                                        if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes["ID"].Value == ID)
                                        {

                                            for (int vvvvvvvvv = 0; vvvvvvvvv < nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes.Count; vvvvvvvvv++)
                                            {
                                                Label lab = new Label();
                                                TextBox txt = new TextBox();
                                                ComboBox cb = new ComboBox();

                                                lab.Name = "lable" + nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name;
                                                lab.Text = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name;


                                                //if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name == "iconPath")
                                                //{
                                                //    cb.Name = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name;
                                                //    cb.Text = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Value;


                                                //    int yLocation = 30;

                                                //    lab.Width = 80;
                                                //    lab.Location = new Point(10, yLocation * (vvvvvvvvv + 1));


                                                //    cb.Width = 100;
                                                //    cb.Location = new Point(95, yLocation * (vvvvvvvvv + 1));

                                                //}
                                                //else
                                                //{
                                                txt.Name = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name;
                                                txt.Text = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Value;


                                                int yLocation = 30;

                                                lab.Width = 80;
                                                lab.Location = new Point(10, yLocation * (vvvvvvvvv + 1));


                                                txt.Width = 120;
                                                txt.Location = new Point(95, yLocation * (vvvvvvvvv + 1));
                                                //  }


                                                txt.LostFocus += new EventHandler(mouselosefocus);
                                                txt.KeyPress += new KeyPressEventHandler(txtkeypress);
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("filename") != -1)
                                                {
                                                    string filenamet = "filename";
                                                    string filenamenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(filenamet.Length.ToString()));
                                                    lab.Text = "文件名" + filenamenum;
                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("fnode") != -1)
                                                {
                                                    string fnode = "fnode";
                                                    string fnodenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(fnode.Length.ToString()));
                                                    lab.Text = "f节点" + fnodenum;
                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("nodecaption") != -1)
                                                {
                                                    string nodecaption = "nodecaption";
                                                    string nodecaptionnum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(nodecaption.Length.ToString()));
                                                    lab.Text = "节点中文名称" + nodecaptionnum;
                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("nodename") != -1)
                                                {
                                                    string nodeName = "nodename";
                                                    string nodeNamenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(nodeName.Length.ToString()));
                                                    lab.Text = "节点名" + nodeNamenum;
                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("iconpath") != -1)
                                                {
                                                    string iconPath = "iconpath";
                                                    string iconPathnum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(iconPath.Length.ToString()));
                                                    lab.Text = "图片路径" + iconPathnum;
                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("imgfile") != -1)
                                                {
                                                    string imgfile = "imgfile";
                                                    string imgfilenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(imgfile.Length.ToString()));
                                                    lab.Text = "图片文件名" + imgfilenum;

                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("beginimg") != -1)
                                                {
                                                    string imgSubBegin = "beginimg";
                                                    string imgSubBeginnum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(imgSubBegin.Length.ToString()));
                                                    lab.Text = "图片开始" + imgSubBeginnum;
                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("endimg") != -1)
                                                {
                                                    string imgSubEnd = "endimg";
                                                    string imgSubEndnum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(imgSubEnd.Length.ToString()));
                                                    lab.Text = "图片结束" + imgSubEndnum;
                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("x") != -1)
                                                {
                                                    string x = "x";
                                                    string xnum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(x.Length.ToString()));
                                                    lab.Text = "x坐标" + xnum;
                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("y") != -1)
                                                {
                                                    string y = "y";
                                                    string ynum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(y.Length.ToString()));
                                                    lab.Text = "y坐标" + ynum;
                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("ishide") != -1)
                                                {
                                                    string isHide = "ishide";
                                                    string isHidenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(isHide.Length.ToString()));
                                                    lab.Text = "是否隐藏" + isHidenum;
                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("width") != -1)
                                                {
                                                    string Width = "width";
                                                    string isHidenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(Width.Length.ToString()));
                                                    lab.Text = "宽" + isHidenum;
                                                }
                                                if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.IndexOf("height") != -1)
                                                {
                                                    string Height = "height";
                                                    string isHidenum = nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name.Substring(int.Parse(Height.Length.ToString()));
                                                    lab.Text = "高" + isHidenum;
                                                }


                                                panel2.Controls.Add(lab);
                                                //if (nodeList.Item(v).ChildNodes[vv].ChildNodes[vvvv].ChildNodes[vvvvvv].Attributes[vvvvvvvvv].Name == "iconPath")
                                                //{
                                                //    panel2.Controls.Add(cb);
                                                //}
                                                //else
                                                //{
                                                panel2.Controls.Add(txt);
                                                // }

                                                if (txt.Name == "ID" || txt.Name == "filename" || txt.Name == "fnode" || txt.Name == "nodecaption" || txt.Name == "nodename")
                                                {
                                                    txt.Enabled = false;
                                                }
                                            }

                                        }
                                    }
                                }

                            }

                        }

                    }
                }
            }

        }



        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
           

        }
    }

}
