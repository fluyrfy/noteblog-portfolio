<%@ Page Title="Note" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Note.aspx.cs" Inherits="noteblog.Note" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Note.css" rel="stylesheet" />

    <main>
        <!-- Overlay effect when opening sidebar on small screens -->
        <div class="w3-overlay w3-hide-large w3-animate-opacity" onclick="w3_close()" style="cursor: pointer" title="close side menu" id="myOverlay"></div>

        <!-- !PAGE CONTENT! -->
        <div class="w3-main main-text">
            <h1 class="title">
                <asp:Literal ID="litTitle" runat="server" /></h1>
            <asp:Literal ID="litContent" runat="server" Mode="PassThrough" />
        </div>

        <script>
            // Script to open and close sidebar
            function w3_open() {
                document.getElementById("mySidebar").style.display = "block";
                document.getElementById("myOverlay").style.display = "block";
            }

            function w3_close() {
                document.getElementById("mySidebar").style.display = "none";
                document.getElementById("myOverlay").style.display = "none";
            }

            const codeElements = document.querySelectorAll('pre');
            codeElements.forEach(code => {
                const btn = document.createElement('button');
                btn.classList.add('copy-btn', 'fa-solid', 'fa-clipboard-list');
                btn.setAttribute("type", "button");
                code.appendChild(btn);
                const codeBlock = code.querySelector('code');
                const clipboard = new ClipboardJS(btn, {
                    text: function () {
                        return codeBlock.textContent;
                    }
                });

                clipboard.on('success', function (e) {
                    e.clearSelection();
                    // 這裡可以加入一些提示或反饋，例如按鈕文字更改
                    btn.classList.remove('fa-clipboard-list');
                    btn.classList.add('fa-clipboard-check');
                    setTimeout(() => {
                        btn.classList.remove('fa-clipboard-check');
                        btn.classList.add('fa-clipboard-list');
                    }, 2000)
                });
            });
        </script>
    </main>
</asp:Content>
