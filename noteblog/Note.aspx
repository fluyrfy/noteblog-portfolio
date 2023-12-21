<%@ Page Title="Note" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Note.aspx.cs" Inherits="noteblog.Note" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Note.css" rel="stylesheet" />

    <%--clipboard js--%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/clipboard.js/2.0.11/clipboard.min.js"></script>

    <script>
        $(function () {
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
                    btn.classList.remove('fa-clipboard-list');
                    btn.classList.add('fa-clipboard-check');
                    setTimeout(() => {
                        btn.classList.remove('fa-clipboard-check');
                        btn.classList.add('fa-clipboard-list');
                    }, 2000)
                });
            })
        });
    </script>
    <main>
        <!-- !PAGE CONTENT! -->
        <div class="w3-main main-text">
            <h1 class="title">
                <asp:Literal ID="litTitle" runat="server" />
                <span class="note-info">Posted by
                    <span class="author">
                        <asp:Literal ID="litAuthor" runat="server" />
                      
                        <div class="profile-card">
                            <div class="our-team">
                              <div class="picture">
                                  <asp:Image ID="imgAuthorAvatar" CssClass="img-fluid" runat="server" />
                              </div>
                              <div class="team-content">
                                  <h3 class="name">
                                      <asp:Literal ID="litAuthorName" ClientIDMode="Static" runat="server" /></h3>
                                  <h4 class="title">
                                      <asp:Literal ID="litAuthorJobTitle" ClientIDMode="Static" runat="server" /></h4>
                              </div>                        
                                <ul class="social">
                                    <li>
                                        <asp:LinkButton ID="lbtnAuthorProfile" OnClick="lbtnAuthorProfile_Click" runat="server" CssClass="fa fa-user" aria-hidden="true" /></li>                                        
                                    <li>
                                        <asp:HyperLink ID="hlkAuthorGitHub" CssClass="fa fa-github" aria-hidden="true" runat="server" Target="_blank" /></li>
                                    <li>
                                        <asp:HyperLink ID="hlkAuthorEmail" CssClass="fa fa-envelope" aria-hidden="true" runat="server" Target="_blank" /></li>
                                    <li>
                                        <asp:HyperLink ID="hlkAuthorResume" class="fa fa-file-text" aria-hidden="true" runat="server" />
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </span>
                    <span>
                        <asp:Repeater runat="server" ID="repCoAuthor">
                            <ItemTemplate>
                              <span class="author">
                            ,&nbsp;<%# Eval("name") %>
                                  <div class="profile-card">
                                      <div class="our-team">
                                        <div class="picture">
                                          <img class="img-fluid" src='<%#string.IsNullOrEmpty(Convert.ToBase64String((byte[])Eval("avatar"))) ? "/Images/ico/user.png" : "data:image/png;base64," + Convert.ToBase64String((byte[])Eval("avatar")) %>' />
                                        </div>
                                        <div class="team-content">
                                            <h3 class="name">
                                              <%# Eval("name") %></h3>
                                            <h4 class="title">
                                              <%# Eval("jobTitle") %>
                                            </h4>
                                         </div>                                          
                                          <ul class="social">
                                              <li>
                                                <a onclick='<%#String.Format("redirectWithCleanCache(\"{0}\", event);", "/Default.aspx?uid=" + Eval("id")) %>'  class="fa fa-user" aria-hidden="true"></a>
                                              </li>
                                              <li>
                                                <a href='<%# Eval("githubLink") %>' class="fa fa-github" aria-hidden="true" target="_blank"></a>
                                              <li>
                                                  <a href='mailto:<%# Eval("email") %>' class="fa fa-envelope" aria-hidden="true"></a></li>
                                              <li>
                                                  <a onclick='<%#"downloadResume(" + Eval("id") + ")" %>' class="fa fa-file-text" aria-hidden="true"></a>
                                              </li>
                                          </ul>
                                      </div>
                                  </div>
                              </span>
                            </ItemTemplate>
                        </asp:Repeater>
                    </span>
                    on
                    <asp:Literal ID="litCreatedAt" runat="server" /></span>
            </h1>
            <asp:Literal ID="litContent" runat="server" Mode="PassThrough" />
        </div>
    </main>
</asp:Content>
