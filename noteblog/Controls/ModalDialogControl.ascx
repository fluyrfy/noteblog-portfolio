<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalDialogControl.ascx.cs" Inherits="noteblog.Controls.ModalDialogControl" %>
<asp:Panel ID="modalBgPanel" runat="server" CssClass="modal-bg" Visible="false">
    <asp:Panel ID="modalContentPanel" runat="server" CssClass="modal-content" Visible="false">

        <div class="modal-header">
            <h5>
                <asp:Literal ID="litModalTitle" runat="server" /></h5>
            <asp:Button ID="btnClose" runat="server" Text="X" CssClass="close-btn" type="button" OnClick="btnClose_Click" />
        </div>

        <div class="modal-body">
            <p>
                <asp:Literal ID="litModalContent" runat="server" />
            </p>
        </div>

    </asp:Panel>
</asp:Panel>
