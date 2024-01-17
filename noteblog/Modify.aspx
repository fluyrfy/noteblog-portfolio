<%@ Page Title="Modify Post" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Modify.aspx.cs" Inherits="noteblog.Modify" ValidateRequest="false" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Take.css" rel="stylesheet" />
    <%--<link rel="stylesheet" href="Shared/bootstrap.min.css">--%>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.12.9/dist/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
    <script src="Scripts/ckeditor/ckeditor.js"></script>
    <script src="Utils/js/ckeditor.js" defer></script>
    <script src="Utils/js/coAuthor.js"></script>
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
                coAuthor: $('#input-co-author'),
                content: contentEditor,
                preImg: $('#<%= imgCover.ClientID %>'),
                hdnImg: $('#<%= hdnImgData.ClientID %>'),
            }
            draft(element, () => {
                $("#input-co-author").children().each(function () {
                    selectedCoAuthorUserIds.push(parseInt($(this).attr("id")));
                    $(this).find('.fa-times').on('click', function () {
                        let userId = parseInt($(this).parent().attr('id'));
                        selectedCoAuthorUserIds = selectedCoAuthorUserIds.filter(existingUserId => existingUserId !== userId);
                        $(this).parent().remove();
                    });
                })
            });
        });
    </script>

    <main>
        <div class="w3-main" style="margin-left: 300px">
            <div class="w3-container w3-padding-large w3-grey">
                <asp:Panel runat="server" ID="pnlCoAuthorAlert" Visible="false">
                    <div class="alert alert-danger alert-dismissible fade show" role="alert">
                        <strong>Notice:</strong> You are a collaborator of this note. Please edit responsibly.
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    </div>
                </asp:Panel>
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
                <asp:Panel runat="server" ID="pnlCoAuthor">
                    <div class="w3-section">
                        <label>Co-Author</label>
                        <div id="input-co-author" contenteditable class="w3-input w3-border"></div>
                        <div id="coAuthorContainer"></div>
                    </div>
                </asp:Panel>
                <div class="w3-section">
                    <label>Content</label>
                    <asp:TextBox ID="txtContent" class="w3-input w3-border ck-editor" TextMode="MultiLine" runat="server" AutoPostBack="true" ClientIDMode="Static"></asp:TextBox>
                </div>
                <div style="width: 100%; text-align: center;">
                    <button class="w3-button w3-black w3-round loading-btn submit-btn" runat="server" onclick="addDataLanguage();" onserverclick="btnSubmit_Click"><i class="fa fa-pencil w3-margin-right"></i>Submit</button>
                    <asp:HiddenField ID="hdnContent" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hdnSelectedCoAuthorUserIds" ClientIDMode="Static" />
                    <asp:HiddenField runat="server" ID="hdnSelectedCoAuthorUser" ClientIDMode="Static" />
                </div>
            </div>
        </div>
        <div class="overlay"></div>
        <div class="spanner">
          <div class="loader"></div>
          <p>Retrieve draft please be patient.</p>
        </div>
    </main>
</asp:Content>

