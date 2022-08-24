using Ionic.Zlib;
using ManagedSquish;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public sealed class MLibrary
    {
        public const int LibVersion = 1;
        public static bool Load = true;
        public string FileName;

        public List<MImage> Images = new List<MImage>();
        public List<int> IndexList = new List<int>();
        public int Count;
        private bool _initialized;

        private BinaryReader _reader_wzl, _reader_wzx;
        private FileStream _stream_wzl, _stream_wzx;

      public  bool isnull = false;
        public MLibrary(string filename)//sharpdx    slimdx 这2个都可以托管绘制图形，跟dx差不多。
        {
            FileName = filename;//"D:\\热血传奇客户端\\热血传奇\\Data\\Tiles.Lib"   "D:\\热血传奇客户端\\热血传奇\\Data\\Mon19.Lib"
            if (FileName=="")
            {
                isnull=true;
                return;
            }
            else
            {
                
                Initialize();
                Close();
            }
        }
            

        public void Initialize()
        {
            //int CurrentVersion; D:\热血传奇客户端\十五周年客户端\热血传奇\cbohum5.wzx
            _initialized = true;

            if (!File.Exists(FileName))//"D:\\热血传奇客户端\\热血传奇\\Data\\Mon19.Lib"
                return;
           
            //--------------------------------------------
            Images = new List<MImage>();
            IndexList = new List<int>();


            _stream_wzx = new FileStream(Path.ChangeExtension(FileName, null) + ".wzx", FileMode.Open, FileAccess.ReadWrite);////"D:\\热血传奇客户端\\热血传奇\\Data\\Tiles.wil"
            _stream_wzx.Seek(0, SeekOrigin.Begin);
            try
            {
                using (BinaryReader reader = new BinaryReader(_stream_wzx))
                {
                    _stream_wzx.Seek(48, SeekOrigin.Begin);//80// 【Title和indexCount一共48字节,后面就是数据】  wix 其中前44字节为 INDX v1.0-WEMADE Entertainment inc.  后4字节为资源索引数组的长度占4个字节。。48字节以后的就是资源索引了。
                    //  int aa = reader.ReadInt32();
                    // long a1 = (_stream_wzx.Length - 48 )/ 4;

                    _stream_wzx = null;

                    //for (int i = 0; i < Count; i++)  //索引集合
                    //{
                    //    IndexList.Add(reader.ReadInt32());
                    //    Images.Add(null);
                    //}
                    while (reader.BaseStream.Position <= reader.BaseStream.Length - 4)//2288
                    {
                        IndexList.Add(reader.ReadInt32());  //加载list<int> 整形索引集合
                        Images.Add(null);
                    }

                }
            }
            finally
            {
                if (_stream_wzx != null)
                    _stream_wzx.Dispose();
            }

            //-------------------------------------wil-----------------------------------------
            _stream_wzl = new FileStream(Path.ChangeExtension(FileName, null) + ".wzl", FileMode.Open, FileAccess.ReadWrite);
            _reader_wzl = new BinaryReader(_stream_wzl);

            _stream_wzl.Seek(0, SeekOrigin.Begin);


            for (int i = 0; i < IndexList.Count; i++)//560   13200
            {
                CheckImage(i);
            }



        }

        private void CheckImage(int index)
        {

            if (!_initialized)
                Initialize();

            if (Images == null || index < 0 || index >= Images.Count)
                return;

            if (Images[index] == null)
            {
                _stream_wzl.Position = IndexList[index];
                Images[index] = new MImage(_reader_wzl, IndexList[index], _stream_wzl, index);
            }

            if (!Load) return;

            //MImage mi = Images[index];
            //if (!mi.TextureValid)
            //{
            //    _stream_wil.Seek(IndexList[index] + 12, SeekOrigin.Begin);//向前偏移12个
            //    mi.CreateTexture(_reader_wil);// 使用 FBytes   创建bitmap图片
            //}
        }

        public int[] _palette;

        public void Close()
        {
            if (_stream_wzl != null)
                _stream_wzl.Dispose();
            // if (_reader != null)
            //     _reader.Dispose();
        }

     


        public MImage GetMImage(int index)
        {
            if (index < 0 || index >= Images.Count)
                return null;

            return Images[index];
        }

     

    
  


        public sealed class MImage
        {
            public short Width { get; set; }
            public short Height { get; set; }
            public short X { get; set; }
            public short Y { get; set; }
           
           

            public Bitmap Image, Preview;//查看

        
        
            public bool bo16bit { get; set; }
            public int nSize { get; set; }

            public static int[] _palette = new int[256] { 0, -8388608, -16744448, -8355840, -16777088, -8388480, -16744320, -4144960, -11173737, -6440504, -8686733, -13817559, -10857902, -10266022, -12437191, -14870504, -15200240, -14084072, -15726584, -886415, -2005153, -42406, -52943, -2729390, -7073792, -7067368, -13039616, -9236480, -4909056, -4365486, -12445680, -21863, -10874880, -9225943, -5944783, -7046285, -4369871, -11394800, -8703720, -13821936, -7583183, -7067392, -4378368, -3771566, -9752296, -3773630, -3257856, -5938375, -10866408, -14020608, -15398912, -12969984, -16252928, -14090240, -11927552, -6488064, -2359296, -2228224, -327680, -6524078, -7050422, -9221591, -11390696, -7583208, -7846895, -11919104, -14608368, -2714534, -3773663, -1086720, -35072, -5925756, -12439263, -15200248, -14084088, -14610432, -13031144, -7576775, -12441328, -9747944, -8697320, -7058944, -7568261, -9739430, -11910599, -14081768, -12175063, -4872812, -8688806, -3231340, -5927821, -7572646, -4877197, -2710157, -1071798, -1063284, -8690878, -9742791, -4352934, -10274560, -2701651, -11386327, -7052520, -1059155, -5927837, -10266038, -4348549, -10862056, -4355023, -13291223, -7043997, -8688822, -5927846, -10859991, -6522055, -12439280, -1069791, -15200256, -14081792, -6526208, -7044006, -11386344, -9741783, -8690911, -6522079, -2185984, -10857927, -13555440, -3228293, -10266055, -7044022, -3758807, -15688680, -12415926, -13530046, -15690711, -16246768, -16246760, -16242416, -15187415, -5917267, -9735309, -15193815, -15187382, -13548982, -10238242, -12263937, -7547153, -9213127, -532935, -528500, -530688, -9737382, -10842971, -12995089, -11887410, -13531979, -13544853, -2171178, -4342347, -7566204, -526370, -16775144, -16246727, -16248791, -16246784, -16242432, -16756059, -16745506, -15718070, -15713941, -15707508, -14591323, -15716006, -15711612, -13544828, -15195855, -11904389, -11375707, -14075549, -15709474, -14079711, -11908551, -14079720, -11908567, -8684734, -6513590, -10855895, -12434924, -13027072, -10921728, -3525332, -9735391, -14077696, -13551344, -13551336, -12432896, -11377896, -10849495, -13546984, -15195904, -15191808, -15189744, -10255286, -9716406, -10242742, -10240694, -10838966, -11891655, -10238390, -10234294, -11369398, -13536471, -10238374, -11354806, -15663360, -15193832, -11892662, -11868342, -16754176, -16742400, -16739328, -16720384, -16716288, -16712960, -11904364, -10259531, -8680234, -9733162, -8943361, -3750194, -7039844, -6515514, -13553351, -14083964, -15204220, -11910574, -11386245, -10265997, -3230217, -7570532, -8969524, -2249985, -1002454, -2162529, -1894477, -1040, -6250332, -8355712, -65536, -16711936, -256, -16776961, -65281, -16711681, -1 };

            private int WidthBytes(int nBit, int nWidth)
            {
                return (((nWidth * nBit) + 31) >> 5) * 4;
            }

        

            public static BitmapData data;

            public static MemoryStream output;

            public static Ionic.Zlib.ZlibStream deflateStream;

            public static int peet, index;

            public static int temp_Width;


       

            public byte[] bytes;





            //<summary>
            // 将 CreatePreview的创建取消了，所以图片有点大小不
            //</summary>
            //<param name="reader"></param>
            //<param name="index_long"></param>
            //<param name="fStream"></param>
            //<param name="_index"></param>
            public unsafe MImage(BinaryReader reader, long index_long, FileStream fStream, int _index)
            {

                if (reader.BaseStream.Position == 0) return;

                bo16bit = reader.ReadByte() == 5;
                reader.ReadBytes(3);

                Width = reader.ReadInt16();//80    8
                Height = reader.ReadInt16();//71   13
                X = reader.ReadInt16();
                Y = reader.ReadInt16();
                nSize = reader.ReadInt32();//2833

                if (Width * Height < 4)
                {
                    return;
                }


                fStream.Seek(index_long + (16), SeekOrigin.Begin);//80     127866- 127757=    109


                if (nSize == 0)
                {
                    bytes = reader.ReadBytes(this.bo16bit ? Width * Height * 2 : Width * Height);
                }
                else
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        using (ZlibStream deflateStream = new ZlibStream(output, Ionic.Zlib.CompressionMode.Decompress))
                        {
                            deflateStream.Write(reader.ReadBytes(nSize), 0, nSize);//得到图片数据大小,并且写入
                            bytes = output.ToArray();//--------得到解压后的原始数据      {by
                        }
                    }
                }

                
                this.Image = new Bitmap(Width, Height);
                BitmapData data = Image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                int index = 0;
                if (bytes.Length == Height * Width * 2.5)
                {
                    int HW_2 = Height * Width * 2;
                    int* scan0 = (int*)data.Scan0;
                    {
                        for (int y = Height - 1; y >= 0; y--)
                        {
                            //---------------------------------------------------------------------------
                            for (int x = 0; x < Width; x++)
                            {
                                if (bo16bit)
                                {
                                    scan0[y * Width + x] = convert16bitTo32bit2(bytes[index++] + ((bytes[index++] << 8)), bytes[HW_2 + y * Width / 2 + x / 2], x);
                                }
                                else
                                    scan0[y * Width + x] = _palette[bytes[index++]];
                            }
                            if ((Width % 4) > 0)
                                index += WidthBytes(bo16bit ? 16 : 8, Width) - (Width * (bo16bit ? 2 : 1));
                            //------------------------------------------------------------------------------------------

                        }
                    }
                }
                else
                {
                    int* scan0 = (int*)data.Scan0;
                    {
                        for (int y = Height - 1; y >= 0; y--)
                        {
                            //---------------------------------------------------------------------------
                            for (int x = 0; x < Width; x++)
                            {
                                if (bo16bit)
                                {
                                    scan0[y * Width + x] = convert16bitTo32bit3(bytes[index++] + (bytes[index++] << 8));
                                }
                                else
                                    scan0[y * Width + x] = _palette[bytes[index++]];
                            }
                            if ((Width % 4) > 0)
                                index += WidthBytes(bo16bit ? 16 : 8, Width) - (Width * (bo16bit ? 2 : 1));
                            //------------------------------------------------------------------------------------------

                        }
                    }
                }

                Image.UnlockBits(data);
            }




            private int convert16bitTo32bit3(int color)
            {
                byte red = (byte)((color & 0xf800) >> 8);
                byte green = (byte)((color & 0x07e0) >> 3);
                byte blue = (byte)((color & 0x001f) << 3);

                if (red == 0 && green == 0 && blue == 0) return 0;
                return ((red << 0x10) | (green << 0x8) | blue) | (255 << 24);
            }



            private int convert16bitTo32bit2(int temp, byte bb, int x) //16位分为5位红，5位蓝，6位绿。          2的16次方，可以表现65536种颜色
            {

                byte red = (byte)((temp & 0xf800) >> 8); // 0xf800  红   1111 1000 0000 0000 （右移动8位之后，就是0000 0000 1111 1000 然后强制转换为一个字节byte  既1111 1000）
                byte green = (byte)((temp & 0x07e0) >> 3);//0x07e0  绿         111 1110 0000 （右移3位就是 000 1111 1100转为byte）
                byte blue = (byte)((temp & 0x001f) << 3); //0x001f  蓝                1 1111  （左移3位就是    1111 1000转为byte）

                //int aa1 = ((bb & 0xf0) >> 4) * 17;
                //int aa2 = (int)((bb & 0xf) * 17);
                //byte alpha = (x % 2 != 0) ? ((byte)aa2) : ((byte)aa1);

                byte alpha = (x % 2 != 0) ? ((byte)((bb & 0xf) * 17)) : ((byte)(((bb & 0xf0) >> 4) * 17)); // 透明值

                return (
                        (red << 0x10)     // 0x10      1 0000    十进制 为 16   左移16位（原来在8位。左移16位就在24位上---16--24之间）
                      | (green << 0x8)   //  1000    十进制 为 8    左移16位（green原来在8位。左移16位就在24位上---8--16之间,blue在0--8之间）
                      | blue)  //0x8         1000    十进制 为 8    左移16位（green原来在8位。左移16位就在24位上---8--16之间,blue在0--8之间）
                      | (alpha << 24);//the final or is setting alpha to max so it'll display (since mir2 images have no alpha layer)

            }


        






      

        }
    }
}