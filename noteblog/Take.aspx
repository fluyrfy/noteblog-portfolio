<%@ Page Title="New Post" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Take.aspx.cs" Inherits="noteblog.Take" ValidateRequest="false" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Take.css" rel="stylesheet" />

    <main>
        <!-- Overlay effect when opening sidebar on small screens -->
        <div class="w3-overlay w3-hide-large w3-animate-opacity" onclick="w3_close()" style="cursor: pointer" title="close side menu" id="myOverlay"></div>

        <div class="w3-main" style="margin-left: 300px">
            <div class="w3-container w3-padding-large w3-grey">
                <h4 id="contact"><b>New Post</b></h4>
                <hr class="w3-opacity">
                <div class="w3-section" aria-orientation="horizontal">
                    <label>Development</label>
                    <asp:RadioButtonList ID="rdlDevelopment" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="F">Front-End</asp:ListItem>
                        <asp:ListItem Value="B">Back-End</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rfvDevelopment" runat="server" ErrorMessage="Development is required" ControlToValidate="rdlDevelopment" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
                <div class="w3-section">
                    <label>Cover Image</label>
                    <asp:FileUpload ID="fuCoverPhoto" runat="server" accept=".png,.jpg,.jpeg" />
<%--                    <asp:RequiredFieldValidator ID="rfvCI" runat="server" ErrorMessage="Cover Image is required" ControlToValidate="fuCoverPhoto" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    <br />
                    <asp:Image ID="imgCover" runat="server" CssClass="cover-photo" />
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
                    <asp:TextBox ID="txtContent" class="w3-input w3-border ck-editor" TextMode="MultiLine" runat="server" AutoPostBack="true"></asp:TextBox>
                    <%-- <asp:RequiredFieldValidator ID="rfvContent" runat="server" ErrorMessage="Content is required" ControlToValidate="txtContent" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                </div>
                <div style="width: 100%; text-align: center;">
                    <button class="w3-button w3-black w3-round" runat="server" onserverclick="btnSubmit_Click"><i class="fa fa-pencil w3-margin-right"></i>Submit</button>
                </div>
            </div>
        </div>

    </main>

    <script>
        // 在页面加载完成后执行初始化

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
            //$("#MainContent_imgCover")
            // 監聽 file input 的變化事件
            $("#MainContent_fuCoverPhoto").on("change", function () {
                var fileInput = $(this)[0];
                if (fileInput.files && fileInput.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        // 將讀取的圖片數據設置為 <img> 的 src
                        $("#MainContent_imgCover").attr("src", e.target.result);
                        $("#MainContent_imgCover").show(); // 顯示圖片
                    };
                    // 讀取選擇的文件
                    reader.readAsDataURL(fileInput.files[0]);
                } else {
                    $("#MainContent_imgCover").hide();
                }
            });
        });

    </script>
</asp:Content>

