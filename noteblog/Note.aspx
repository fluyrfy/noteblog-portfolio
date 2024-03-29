﻿<%@ Page Title="Note" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Note.aspx.cs" Inherits="noteblog.Note" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Note.css" rel="stylesheet" />

    <%--clipboard js--%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/clipboard.js/2.0.11/clipboard.min.js" defer></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js" integrity="sha512-GsLlZN/3F2ErC5ifS5QtgpiJtWd43JWSuIgh7mbzZ8zBps+dvLusV+eNQATqgA/HdeKFVgA5v3S/cIrLF7QnIg==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="Utils/js/noteNavigation.js" defer></script>
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

            $(window).scroll(function() {
              if ($(this).scrollTop() > 100) {
                $('#scrollToTopBtn').fadeIn();
              } else {
                $('#scrollToTopBtn').fadeOut();
              }
            });
            $('#scrollToTopBtn').click(function() {
              $('html, body').animate({scrollTop : 0}, 800);
              return false;
            });

            $("#htmlToPDF").click(function() {
              const btnPDF = $(this);
              let element = document.querySelector("#main-text > div");
              let filename = $("h1.title").text().trim();
              let opt = {
                filename: `${filename}.pdf`,
                margin: 10,
                image: {
                  type: 'jpeg',
                  quality: 1
                },
                jsPDF: { unit: "mm", autoPaging: true, format: "a4", orientation: "portrait"},
                html2canvas: { useCORS: true },
                pagebreak: { mode: ['avoid-all', 'css', 'legacy'] }
              };
              
              // New Promise-based usage:*/
              html2pdf().then(function() {
                  element.classList.add("print");
                  btnPDF.hide();
                }).set(opt).from(element).save().then(function() {
                  element.classList.remove("print");
                btnPDF.show();
              });
            })
        });
    </script>
    <main>
        <!-- !PAGE CONTENT! -->
        <div class="w3-main main-text" id="main-text">
          <div class="container-navi">
            <button class="toggle-navi" type="button">
            </button>
            <div class="noteNavigation">
              <ul>
              </ul>
            </div>
          </div>
            <div>
              <h1 class="title">
                <asp:Literal ID="litTitle" runat="server" ClientIDMode="Static"/>
              </h1>
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
                                ,&nbsp;
                              <span class="author">
                                <%# Eval("name") %>
                                  <div class="profile-card">
                                      <div class="our-team">
                                        <div class="picture">
                                          <img class="img-fluid" src='<%#(byte[])Eval("avatar") == null ? "/Images/ico/user.png" : string.IsNullOrEmpty(Convert.ToBase64String((byte[])Eval("avatar"))) ? "/Images/ico/user.png" : "data:image/png;base64," + Convert.ToBase64String((byte[])Eval("avatar")) %>' />
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
            </s>
            <asp:Literal ID="litContent" runat="server" Mode="PassThrough" />
            <button id="htmlToPDF" type="button">
              <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
            </button>
          </div>
          <button id="scrollToTopBtn">
            <i class="fa fa-chevron-up" aria-hidden="true"></i>
          </button>
    </main>
</asp:Content>
