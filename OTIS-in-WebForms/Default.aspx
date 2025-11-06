<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OTIS_in_WebForms.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="/Content/Site.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager1" />
        <asp:UpdatePanel class="chat-container" runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <% if (MessageHistory.Count <= 0)
                    { %>
                <div class="new-chat">
                    <h2 class="new-chat-head">Chat to Otis</h2>
                    <h3 class="new-chat-sub">Get help with your mental health!</h3>
                </div>
                <% }
                    else
                    { %>
                <div class="message-history">
                    <%
                        MessageHistory.ForEach((string m) =>
                        {   %>
                    <div class="message">
                        <%: m %>
                    </div>
                    <% }); %>

                    <% if (isSending)
                        { %>
                    <div class="message typing-area">
                        <div class="typing"></div>
                        <div class="typing"></div>
                        <div class="typing"></div>
                    </div>
                    <% } %>
                </div>
                <%
                    } %>

                <div class="input-form" id="clientForm" onsubmit="return false;">
                    <asp:TextBox runat="server" ID="InputTextBox" class="input" TextMode="MultiLine" placeholder="Ask for help with your mental health"></asp:TextBox>
                    <asp:Button runat="server" ID="SendButton" class="input-button" Text="S" OnClick="SendButton_Click" Style="display: inline-block;" />
                </div>

            </ContentTemplate>

            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="SendButton" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>

    </form>

    <script type="text/javascript">
        function wireUpChatEvents() {
            var clientForm = document.getElementById('clientForm');
            var input = document.getElementById('<%= InputTextBox.ClientID %>');
            var sendBtn = document.getElementById('<%= SendButton.ClientID %>');

            if (!input || !sendBtn) return;

            // Handle Enter key for sending
            input.addEventListener('keypress', function (ev) {
                Resize();

                if (ev.key === 'Enter' && !ev.shiftKey) {
                    ev.preventDefault();
                    sendBtn.click();
                }
            });

            // Handle auto-resize
            function Resize() {
                input.style.height = '';
                input.style.height = input.scrollHeight + "px";
            }

            Resize();

            // Intercept the form submit to prevent full postback
            if (clientForm) {
                clientForm.addEventListener('submit', function (ev) {
                    ev.preventDefault();
                    sendBtn.click();
                });
            }
        }

        // Run once on initial load
        document.addEventListener('DOMContentLoaded', wireUpChatEvents);

        // Re-run after every async postback (UpdatePanel refresh)
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            wireUpChatEvents();

            // Optional: auto-scroll to bottom of chat
            var history = document.querySelector('.message-history');
            if (history) history.scrollTop = history.scrollHeight;
        });
    </script>

</body>
</html>
