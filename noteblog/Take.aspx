<%@ Page Title="New Post" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Take.aspx.cs" Inherits="noteblog.Take" ValidateRequest="false" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                <asp:TextBox ID="txtContent" class="w3-input w3-border ck-editor" TextMode="MultiLine" runat="server" AutoPostBack="true"></asp:TextBox>
            </div>
            <div style="width: 100%; text-align: center;">
                <button class="w3-button w3-black w3-round" runat="server" onserverclick="btnSubmit_Click"><i class="fa fa-pencil w3-margin-right"></i>Submit</button>
            </div>
            <div id="displayArea"></div>
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
                        console.log(data);
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


    </script>
</asp:Content>

