<%@ Page Title="New Post" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Take.aspx.cs" Inherits="noteblog.Take" ValidateRequest="false" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Take.css" rel="stylesheet" />
    <main>
        <div class="w3-main" style="margin-left: 300px">
            <div class="w3-container w3-padding-large w3-grey">
                <h4 id="contact"><b>New Post</b></h4>
                <hr class="w3-opacity">
                <div class="w3-section" aria-orientation="horizontal">
                    <label>Category</label>
                    <asp:RadioButtonList ID="rdlCategory" runat="server" RepeatDirection="Horizontal">
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rfvDevelopment" runat="server" ErrorMessage="Development is required" ControlToValidate="rdlCategory" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
                <div class="w3-section">
                    <label>Cover Image</label>
                    <asp:FileUpload ID="fuCoverPhoto" runat="server" accept=".png,.jpg,.jpeg" />
                    <%--                    <asp:RequiredFieldValidator ID="rfvCI" runat="server" ErrorMessage="Cover Image is required" ControlToValidate="fuCoverPhoto" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    <asp:Label ID="lblPhotoMsg" runat="server" />
                    <br />
                    <asp:Image ID="imgCover" runat="server" CssClass="cover-photo" />
                    <asp:HiddenField ID="hdnImgData" runat="server" />
                </div>
                <div class="w3-section">
                    <label>Title</label>
                    <asp:TextBox ID="txtTitle" runat="server" class="w3-input w3-border" required="required"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ErrorMessage="Title is required" ControlToValidate="txtTitle" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
                <div class="w3-section">
                    <label>Keyword</label>
                    <asp:TextBox ID="txtKeyword" runat="server" class="w3-input w3-border"></asp:TextBox>
                </div>
                <div class="w3-section">
                    <label>Content</label>
                    <asp:TextBox ID="txtContent" name="editor1" class="w3-input w3-border ck-editor" TextMode="MultiLine" runat="server" AutoPostBack="true"></asp:TextBox>
                    <%-- <asp:RequiredFieldValidator ID="rfvContent" runat="server" ErrorMessage="Content is required" ControlToValidate="txtContent" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                </div>
                <div style="width: 100%; text-align: center;">
                    <button class="w3-button w3-black w3-round" runat="server" onserverclick="btnSubmit_Click"><i class="fa fa-pencil w3-margin-right"></i>Submit</button>
                </div>
            </div>
        </div>

    </main>

    <script src="Utils/js/draft.js"></script>
    <script>
        var contentEditor;

        const watchdog = new CKSource.EditorWatchdog();

        window.watchdog = watchdog;

        watchdog.setCreator((element, config) => {
            return CKSource.Editor
                .create(element, config)
                .then(editor => {
                    // 在这里可以绑定其他事件或进行其他操作
                    editor.model.document.on('change:data', () => {
                        const data = editor.getData();
                    })
                    contentEditor = editor

                    return editor;
                })
        });

        watchdog.setDestructor(editor => {
            return editor.destroy();
        });

        watchdog.on('error', handleError);

        watchdog
            .create(document.querySelector('.ck-editor'), {

                licenseKey: '',
            })
            .catch(handleError);

        function handleError(error) {
            console.error('Oops, something went wrong!');
            console.error('Please, report the following error on https://github.com/ckeditor/ckeditor5/issues with the build id and the error stack trace:');
            console.warn('Build id: 31o3b59jzmiv-rq35etlei2s');
            console.error(error);
        }
        $(document).ready(function () {
            $("#MainContent_imgCover").hide();

            $("#MainContent_fuCoverPhoto").on("change", function () {
                var fileInput = $(this)[0];
                if (fileInput.files && fileInput.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        // 將讀取的圖片數據設置為 <img> 的 src
                        $("#MainContent_imgCover").attr("src", e.target.result);
                        $("#MainContent_imgCover").show(); // 顯示圖片
                        $("#<%= hdnImgData.ClientID %>").val(e.target.result.split(',')[1])
                    };
                    // 讀取選擇的文件
                    reader.readAsDataURL(fileInput.files[0]);
                } else {
                    $("#MainContent_imgCover").hide();
                }
            })

            const element = {
                noteId: 0,
                category: $("#<%= rdlCategory.ClientID %>"),
                pic: $('#<%= fuCoverPhoto.ClientID %>'),
                title: $('#<%= txtTitle.ClientID %>'),
                keyword: $('#<%= txtKeyword.ClientID %>'),
                content: contentEditor,
                preImg: $('#<%= imgCover.ClientID %>'),
                hdnImg: $('#<%= hdnImgData.ClientID %>'),
            }

            draft(element);
        })

    </script>

</asp:Content>

