<%@ Page Title="Modify Post" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Modify.aspx.cs" Inherits="noteblog.Modify" ValidateRequest="false" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Take.css" rel="stylesheet" />
    <script src="Scripts/ckeditor/ckeditor.js"></script>
    <script src="Utils/js/ckeditor.js" defer></script>
    <script type="module" src="Utils/js/edit.js"></script>
    <script type="module">
        import draft from './Utils/js/draft.js'
        $(document).ready(function () {
            const element = {
                noteId: new URLSearchParams(window.location.search).get('id'),
                category: $("#<%= rdlCategory.ClientID %>"),
                pic: $('#<%= fuCoverPhoto.ClientID %>'),
                title: $('#<%= txtTitle.ClientID %>'),
                keyword: $('#<%= txtKeyword.ClientID %>'),
                content: contentEditor,
                preImg: $('#<%= imgCover.ClientID %>'),
                hdnImg: $('#<%= hdnImgData.ClientID %>'),
            }

            draft(element);
        });
    </script>

    <main>
        <div class="w3-main" style="margin-left: 300px">
            <div class="w3-container w3-padding-large w3-grey">
                <h4 id="contact"><b>Modify Post</b></h4>
                <hr class="w3-opacity">
                <div class="w3-section" aria-orientation="horizontal">
                    <label>Category</label>
                    <asp:RadioButtonList ID="rdlCategory" runat="server" RepeatDirection="Horizontal" CssClass="category">
                    </asp:RadioButtonList>
                </div>
                <div class="w3-section">
                    <label>Cover Photo</label>
                    <asp:FileUpload ID="fuCoverPhoto" runat="server" accept=".png,.jpg,.jpeg" ClientIDMode="Static" /><br />
                    <div class="cover-container">
                        <asp:Image ID="imgCover" runat="server" CssClass="cover-photo" />
                        <button class="remove-img-btn" type="button">
                            <i class="fa fa-trash"></i>
                        </button>
                    </div>
                    <asp:Label ID="lblPhotoMsg" runat="server" />
                    <asp:HiddenField ID="hdnImgData" runat="server" ClientIDMode="Static" />
                </div>
                <div class="w3-section">
                    <label>Title</label>
                    <asp:TextBox ID="txtTitle" runat="server" class="w3-input w3-border" required="required"></asp:TextBox>
                </div>
                <div class="w3-section">
                    <label>Keyword</label>
                    <asp:TextBox ID="txtKeyword" runat="server" class="w3-input w3-border"></asp:TextBox>
                </div>
                <div class="w3-section">
                    <label>Content</label>
                    <asp:TextBox ID="txtContent" class="w3-input w3-border ck-editor" TextMode="MultiLine" runat="server" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div style="width: 100%; text-align: center;">
                    <button class="w3-button w3-black w3-round loading-btn submit-btn" runat="server" onclick="addDataLanguage();" onserverclick="btnSubmit_Click"><i class="fa fa-pencil w3-margin-right"></i>Submit</button>
                    <asp:HiddenField ID="hdnContent" runat="server" ClientIDMode="Static" />
                </div>
            </div>
        </div>
    </main>
</asp:Content>

