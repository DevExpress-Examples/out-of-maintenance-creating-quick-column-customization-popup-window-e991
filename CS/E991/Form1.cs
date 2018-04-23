using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace E991 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            //You may use the PopupSize property to set the popup size. In this demo I'm using the default one.
            new DevExpress.XtraGrid.Helpers.GridViewQuickColumnCustomization(gridView1);
        }

        private void Form1_Load(object sender, EventArgs e) {
            // TODO: This line of code loads data into the 'nwindDataSet.Orders' table. You can move, or remove it, as needed.
            this.ordersTableAdapter.Fill(this.nwindDataSet.Orders);
        }
    }
}