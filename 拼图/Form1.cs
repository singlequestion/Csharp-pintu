using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 拼图
{
	public partial class Form1 : Form
	{
		const int button_size = 60;
		Button[,] cell_buttons = new Button[4, 4];

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			cell_buttons_initialize();	//初始化按钮矩阵
		}

		private void button1_Click(object sender, EventArgs e)
		{
			cell_buttons_upset();   //打乱按钮
			//激活按钮
			for ( short id = 0; id < 15; id++ )
				cell_buttons[id / 4, id % 4].Enabled = true; 
			this.button1.Visible = false;
		}

		void cell_buttons_initialize()
		{
			var x = this.Width / 2 - 2 * button_size;
			var y = (this.Height - 100) / 2 - 2 * button_size;
			for (int button_id = 0; button_id < 16; button_id++)
			{
				if (button_id != 0 && button_id % 4 == 0)
				{
					x -= 4 * button_size;
					y += button_size;
				}
				Button btn = new Button();
				int button_text = button_id + 1;
				//btn.Text = button_text.ToString();
				btn.Top = y;
				btn.Left = x;
				btn.Width = button_size;
				btn.Height = button_size;
				btn.Visible = true;
				btn.Tag = button_id;
				btn.Enabled = false;
				//按钮样式：
				btn.Margin = new Padding(0);
				btn.FlatStyle = FlatStyle.Flat;
				btn.FlatAppearance.BorderSize = 1;
				btn.FlatAppearance.BorderColor = Color.White;
				btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
				btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
				btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;  //图片大小自适应
//				var img = "pic/1/(" + button_text.ToString() + ").jpg";		//路径方式获取图片
//				btn.BackgroundImage = Image.FromFile(img);
				var img = "_" + button_text.ToString() + "_";	//资源调用方式获取图片
				btn.BackgroundImage = (Image)Properties.Resource.ResourceManager.GetObject(img);

				if (button_id == 15)
					btn.Visible = false;
				else
					btn.Click += new EventHandler(cell_button_Click);

				cell_buttons[button_id / 4, button_id % 4] = btn;
				this.Controls.Add(btn);
				x += button_size;
			}
		}

		void cell_buttons_upset()
		{
			Random rnd = new Random();
			for (int i = 0; i < 32; i++)
				cell_button_exchange(cell_buttons[rnd.Next(4), rnd.Next(4)], cell_buttons[rnd.Next(4), rnd.Next(4)]);
		}

		//按钮点击事件
		void cell_button_Click(object sender, EventArgs e)
		{
			Button btn = sender as Button;		//当前点中的按钮
			Button blank = cell_buttons[3, 3];	//空白按钮
			if (button_is_near(btn, blank))
				cell_button_exchange(btn, blank);
			if (is_finish())
			{
				for (short id = 0; id < 15; id++)
					cell_buttons[id / 4, id % 4].Enabled = false;
				MessageBox.Show("恭喜你获得无聊大王称号");
				button1.Visible = true;
			}
		}

		void cell_button_exchange(Button btn1, Button btn2)
		{
			int temp;
			temp = btn1.Top;
			btn1.Top = btn2.Top;
			btn2.Top = temp;
			temp = btn1.Left;
			btn1.Left = btn2.Left;
			btn2.Left = temp;

			temp = (int)btn1.Tag;
			btn1.Tag = btn2.Tag;
			btn2.Tag = temp;
		}

		bool button_is_near(Button btn1, Button btn2)
		{
			int r1 = (int)btn1.Tag / 4;
			int r2 = (int)btn2.Tag / 4;
			int c1 = (int)btn1.Tag % 4;
			int c2 = (int)btn2.Tag % 4;

			if (r1 == r2 && ( c1 - c2 == -1 || c1 - c2 == 1 ) )	//行相邻
				return true;
			if (c1 == c2 && ( r1 - r2 == -1 || r1 - r2 == 1 ) )  //列相邻
				return true;
			else
				return false;
		}

		bool is_finish()
		{
			for ( int id = 0; id < 15; id++)
			{
				if (id != (int)cell_buttons[id / 4, id % 4].Tag)
					return false;
			}
			return true;
		}
	}
}
