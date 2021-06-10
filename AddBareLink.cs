using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlphamaConverter
{
	public partial class AddBareLink: Form
	{
        public string text;

        public AddBareLink()
		{
			InitializeComponent();
		}

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            text = GetLinks();
            this.Close();

        }

        public string GetLinks()
        {
            string result = String.Empty;

            string link = this.textBox1.Text;
            if (link != "") result += "<ref>" + link + "</ref>";
           
            link = this.textBox2.Text;
            if (link != "") result += "<ref>" + link + "</ref>";

            link = this.textBox3.Text;
            if (link != "") result += "<ref>" + link + "</ref>";
            
            return result;

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "A")
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                this.KeyPreview = true;
                this.textBox1.SelectAll();
            }
            if (e.KeyCode == Keys.Enter)
            {
                text = GetLinks();
                this.Close();
                e.Handled = true;
                e.SuppressKeyPress = true;

            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "A")
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                this.KeyPreview = true;
                this.textBox2.SelectAll();
            }
            if (e.KeyCode == Keys.Enter)
            {
                text = GetLinks();
                this.Close();
                e.Handled = true;
                e.SuppressKeyPress = true;

            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "A")
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                this.KeyPreview = true;
                this.textBox3.SelectAll();
            }

            if (e.KeyCode == Keys.Enter)
            {
                text = GetLinks();
                this.Close();
                e.Handled = true;
                e.SuppressKeyPress = true;
                
            }

        }
	}
}
