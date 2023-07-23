<%@ Page Title="New Post" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="noteblog.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/ckeditor.js"></script>
    <main>
        <div class="w3-container w3-padding-large w3-grey">
            <h4 id="contact"><b>New Post</b></h4>
            <hr class="w3-opacity">
            <div class="w3-section" aria-orientation="horizontal">
                <label>Development</label>
                <asp:RadioButtonList ID="rdlDevelopment" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="F">Front-End</asp:ListItem>
                    <asp:ListItem Value="B">Back-End</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="w3-section">
                <label>Cover Photo</label>
                <asp:FileUpload ID="fuCoverPhoto" runat="server" accept=".png,.jpg,.jpeg,.gif" />
            </div>
            <div class="w3-section">
                <label>Title</label>
                <asp:TextBox ID="txtTitle" runat="server" class="w3-input w3-border" required="required"></asp:TextBox>
            </div>
            <div class="w3-section">
                <label>Keyword</label>
                <asp:TextBox ID="txtKeyword" runat="server" class="w3-input w3-border" required="required"></asp:TextBox>
            </div>
            <div class="w3-section">
                <label>Content</label>
                <asp:TextBox ID="txtContent" class="w3-input w3-border" TextMode="MultiLine" runat="server"></asp:TextBox>
            </div>
            <div style="width: 100%; text-align: center;">
                <button class="w3-button w3-black w3-round" runat="server" onserverclick="btnSubmit_Click"><i class="fa fa-pencil w3-margin-right"></i>Submit</button>
            </div>
        </div>
    </main>

    <script>
        window.addEventListener("load", (e) => {
            CKSource.Editor.create(document.querySelector('#MainContent_txtContent'))
                .then(editor => {
                    //console.log(editor);
                })
                .catch(error => {
                    console.error(error);
                });
        });
    </script>
</asp:Content>

