using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using helper;
using System.Diagnostics;
using System.Threading;

namespace CaroForm
{
    
    public partial class Form1 : Form
    {
       // CreateCaro.Caro = new CreateCaro();
        private static CreateCaro instance = new CreateCaro();
 
        public Form1()
        {
            
           
            InitializeComponent();

            assgin();

        }

        private void assgin()
        {

            instance.setup();

            instance.MouseDown += new MouseEventHandler(instance.click_handel);

            //this.MouseDown += new MouseEventHandler(instance.click_handel);

            this.Controls.Add(instance);

            this.Controls.Add(instance.TableControl);
        }







    }
}
