using OTIS.Chat;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace OTIS_in_WebForms
{
    public partial class Default : System.Web.UI.Page
    {
        // Persist the message history in ViewState so it survives async postbacks
        public List<string> MessageHistory
        {
            get
            {
                var list = ViewState["MessageHistory"] as List<string>;
                if (list == null)
                {
                    list = new List<string> { "hi" };
                    ViewState["MessageHistory"] = list;
                }
                return list;
            }
            set { ViewState["MessageHistory"] = value; }
        }

        public bool isSending
        {
            get { return (ViewState["isSending"] as bool?) ?? false; }
            set { ViewState["isSending"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // initial welcome call (runs once)
                isSending = true;
                string response = Chat.Message(new Request("Hello, how are you?", MessageHistory));
                MessageHistory.Add(response);
                isSending = false;
            }
        }

        protected void SendButton_Click(object sender, EventArgs e)
        {
            // This runs on the server during async postback triggered by JS
            isSending = true;


            var message = (InputTextBox.Text ?? string.Empty).Trim();
            if (!string.IsNullOrEmpty(message))
            {

                MessageHistory.Add(message);

                string response = Chat.Message(new Request(message, MessageHistory));
                
                MessageHistory.Add(response);
            }

            InputTextBox.Text = string.Empty;
            isSending = false;
        }
    }
}