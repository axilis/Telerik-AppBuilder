using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PushSender
{
    public partial class frmPushTokens : Form
    {
        public frmPushTokens()
        {
            InitializeComponent();
        }

        public string PushToken
        {
            get
            {
                return (string)dataGridView1.CurrentRow.Cells["pushTokenDataGridViewTextBoxColumn"].Value; ;
            }
        }

        public int DeviceId
        {
            get
            {
                return (int)dataGridView1.CurrentRow.Cells["deviceIdDataGridViewTextBoxColumn"].Value; ;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.DialogResult = DialogResult.OK;

        }


        string serviceUri = "http://wcf.programski-kod.com/odata/";

        private async void loadData()
        {

            this.Text = "Ucitavam";


            List<PushSender.Service.Members.EF.Model.Device> oDeviceList = null;


            await Task.Run(() =>
            {
                var container = new Service.Default.Container(new Uri(serviceUri));

                var Query = (Microsoft.OData.Client.DataServiceQuery<Service.Members.EF.Model.Device>)container.Device;

                if (cbWithPushToken.Checked)
                {

                    Query = (Microsoft.OData.Client.DataServiceQuery<Service.Members.EF.Model.Device>)Query.Where(q => !q.PushToken.Equals(null) && q.PushToken != "");

                }

                oDeviceList = Query.ToList();

            });


            dataGridView1.DataSource = oDeviceList;

            this.Text = "Gotovo";
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            loadData();
        }

        private void frmPushTokens_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount == 0)
                return;

            deleteData();


        }

        async void deleteData()
        {
            await Task.Run(() =>
                      {
                          var container = new Service.Default.Container(new Uri(serviceUri));

                          var Query = container.Device.Where(x => x.DeviceId == DeviceId).First();

                          container.DeleteObject(Query);

                          container.SaveChanges();
                      });


            loadData();

        }

    }
}
