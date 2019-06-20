using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HexDump
{
    public partial class Form1 : Form
    {
        string CurFileName = "";
        int FileLength = 0;
        byte[] ByteData;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            statusStrip1.Items.Add("");

            textBox1.Multiline = true;
            textBox1.Dock = DockStyle.Fill;
        }

        private void FileOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = CurFileName;
            openFileDialog1.Filter = "すべてのファイル (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                CurFileName = openFileDialog1.FileName;

                this.Cursor = Cursors.WaitCursor;

                LoadFile();
                DispFile();

                this.Cursor = Cursors.Default;

                // ステータスストリップにファイル名とバイト数を表示
                string s = string.Format("{0}({1}バイト)",
                    CurFileName, FileLength);
                statusStrip1.Items[0].Text = s;
            }
        }

        /// <summary>
        /// ファイルをロードする
        /// </summary>
        private void LoadFile()
        {
            FileStream ifs = new FileStream(CurFileName, FileMode.Open);
            long filelen = ifs.Length;
            ByteData = new byte[filelen];

            BinaryReader br = new BinaryReader(ifs);

            int count = 0;
            while (true)
            {
                try
                {
                    ByteData[count] = br.ReadByte();
                }
                catch (EndOfStreamException /*ex*/)
                {
                    break;
                }
                count++;
            }

            ifs.Close();
            FileLength = count;
        }

        /// <summary>
        /// ファイルの内容を16進数で表示する
        /// </summary>
        private void DispFile()
        {
            StringBuilder DispData = new StringBuilder();
            string LineData = "";
            int count = 0;
            int address = 0;

            for (int i = 0; i < FileLength; i++)
            {
                if (count == 0)
                {
                    
                    DispData.Append(string.Format("{0:X6}", address).ToString() + " ");
                    address += 0x10;
                }

                byte b = ByteData[i];
                string s = string.Format("{0:X2} ", b);
                LineData += s;
                if (++count == 16)
                {
                    LineData.Trim();
                    DispData.Append(LineData + Environment.NewLine);
                    count = 0;
                    LineData = "";

                }
            }
            textBox1.Text = DispData.ToString();
        }

        private void FileExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
