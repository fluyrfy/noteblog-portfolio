<%@ Page Title="Note" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Note.aspx.cs" Inherits="noteblog.Note" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Note.css" rel="stylesheet" />

    <%--scirpt hightlight--%>
    <%--<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.8.0/styles/default.min.css">--%>
    <link rel="stylesheet" href="Shared/highlight/stackoverflow-light.css">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.8.0/highlight.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.8.0/languages/go.min.js"></script>
    <%--<link href="https://cdn.jsdelivr.net/npm/prismjs@1.25.0/themes/prism.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/prismjs@1.25.0/prism.js"></script>--%>

    <%--clipboard js--%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/clipboard.js/2.0.11/clipboard.min.js"></script>
    
    <script>
      $(function() {
        // Initialize Prism.js Highlight.js
        hljs.highlightAll();
        hljs.configure({
            ignoreUnescapedHTML: true
        });
        const codeElements = document.querySelectorAll('pre');
        codeElements.forEach(code => {
      })
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
            btn.classList.remove('fa-clipboard-list');
            btn.classList.add('fa-clipboard-check');
            setTimeout(() => {
                btn.classList.remove('fa-clipboard-check');
                btn.classList.add('fa-clipboard-list');
            }, 2000)
        });
      });
  </script>
    <main>
        <!-- !PAGE CONTENT! -->
        <div class="w3-main main-text">
            <h1 class="title">
                <asp:Literal ID="litTitle" runat="server" />
                <span class="note-info">Posted by
                    <asp:Literal ID="litAuthor" runat="server" />
                    on
                    <asp:Literal ID="litCreatedAt" runat="server" /></span>
            </h1>
            <asp:Literal ID="litContent" runat="server" Mode="PassThrough" />
        </div>
    </main>
</asp:Content>
