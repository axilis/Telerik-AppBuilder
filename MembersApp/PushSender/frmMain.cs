using PushSharp;
using PushSharp.Android;
using PushSharp.Core;
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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }



        private void NotificationSent(object sender, INotification notification)
        {
            txtLogs.Invoke((MethodInvoker)(() =>
            {
                txtLogs.AppendText("NotificationSent" + Environment.NewLine);
            }));
        }
        private void ChannelException(object sender, IPushChannel pushChannel, Exception error)
        {
            txtLogs.Invoke((MethodInvoker)(() =>
            {
                txtLogs.AppendText("ChannelException: " + error.Message + Environment.NewLine);
            }));

        }
        private void ServiceException(object sender, Exception error)
        {
            txtLogs.Invoke((MethodInvoker)(() =>
            {
                txtLogs.AppendText("ServiceException: " + error.Message + Environment.NewLine);
            }));

        }
        private void NotificationFailed(object sender, INotification notification, Exception error)
        {
            txtLogs.Invoke((MethodInvoker)(() =>
            {
                txtLogs.AppendText("NotificationFailed: " + error.Message + Environment.NewLine);
            }));

        }
        private void DeviceSubscriptionExpired(object sender, string expiredSubscriptionId, DateTime expirationDateUtc, INotification notification)
        {
            txtLogs.Invoke((MethodInvoker)(() =>
            {
                txtLogs.AppendText("DeviceSubscriptionExpired" + Environment.NewLine);
            }));

        }
        private void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            txtLogs.Invoke((MethodInvoker)(() =>
            {
                txtLogs.AppendText("DeviceSubscriptionChanged" + Environment.NewLine);
            }));

        }
        private void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            txtLogs.Invoke((MethodInvoker)(() =>
            {
                txtLogs.AppendText("ChannelCreated" + Environment.NewLine);
            }));

        }
        private void ChannelDestroyed(object sender)
        {
            txtLogs.Invoke((MethodInvoker)(() =>
            {
                txtLogs.AppendText("ChannelDestroyed" + Environment.NewLine);
            }));
        }



        private void butSend_Click(object sender, EventArgs e)
        {
            sendPush();
        }

        PushBroker push;

        void sendPush()
        {

            if (string.IsNullOrEmpty(txtApiKey.Text))
            {
                MessageBox.Show("Please specify API Key.");
                txtApiKey.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtRegistrationID.Text))
            {
                MessageBox.Show("Please specify Registration Key.");
                txtRegistrationID.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtMessage.Text))
            {
                MessageBox.Show("Please specify message text.");
                txtMessage.Focus();
                return;
            }

            if (push == null)
            {
                push = new PushBroker();
                push.OnNotificationSent += NotificationSent;
                push.OnChannelException += ChannelException;
                push.OnServiceException += ServiceException;
                push.OnNotificationFailed += NotificationFailed;
                push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
                push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
                push.OnChannelCreated += ChannelCreated;
                push.OnChannelDestroyed += ChannelDestroyed;

                //txtApiKey.Text is API KEY for server applications from Google Developers Console
                push.RegisterGcmService(new GcmPushChannelSettings(txtApiKey.Text));
            }

            
            string message = string.Format("{{\"message\":\"{0}\",\"title\":\"{1}\",\"collapseKey\":\"{2}\",\"msgcnt\":\"{3}\",\"soundname\":\"{4}\"}}", txtMessage.Text, txtTitle.Text, "demo", txtBadge.Text, txtSound.Text);

            push.QueueNotification(new GcmNotification().ForDeviceRegistrationId(txtRegistrationID.Text).WithJson(message));

        }

        private void btnPushTokens_Click(object sender, EventArgs e)
        {

            frmPushTokens oForm = new frmPushTokens();

            if (oForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                txtRegistrationID.Text = oForm.PushToken;

            }

        }
    }
}
