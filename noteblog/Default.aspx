<%@ Page Title="F.L." Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="noteblog._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Default.css" rel="stylesheet" />

    <main>
        <!-- Overlay effect when opening sidebar on small screens -->
        <div class="w3-overlay w3-hide-large w3-animate-opacity" onclick="w3_close()" style="cursor: pointer" title="close side menu" id="myOverlay"></div>

        <!-- !PAGE CONTENT! -->
        <div class="w3-main" style="margin-left: 300px">
            <asp:UpdatePanel ID="updatePanel1" runat="server">
                <ContentTemplate>
                    <div class="w3-container">
                        <h1><b>My Portfolio</b></h1>
                        <div class="w3-section w3-bottombar w3-padding-16" id="filter">
                            <span class="w3-margin-right">Filter:</span>
                            <asp:LinkButton ID="btnAll" runat="server" OnCommand="btnFilter_Command" CommandArgument="NotesAll" type="button" Text="ALL"></asp:LinkButton>
                            <asp:LinkButton ID="btnFrontEnd" OnCommand="btnFilter_Command" runat="server" CommandArgument="NotesFront"><i class="fa fa-code w3-margin-right" type="button"></i>Front-End</asp:LinkButton>
                            <asp:LinkButton ID="btnBackEnd" OnCommand="btnFilter_Command" CommandArgument="NotesBack" runat="server"><i class="fa fa-database w3-margin-right" type="button"></i>Back-End</asp:LinkButton>
                        </div>
                    </div>
                    <div class="w3-row-padding">
                        <asp:Repeater ID="repNote" runat="server" Visible="true">
                            <ItemTemplate>
                                <div class="w3-third w3-container w3-margin-bottom rep-item" onclick="redirectPage(<%# Eval("id") %>)">
                                    <asp:LinkButton runat="server" ID="lnkNote" CommandName="ReadNote" CommandArgument='<%# Eval("id") %>' />
                                    <img src="data:image/png;base64,<%# System.Convert.ToBase64String((byte[])Eval("pic"))%>" class="w3-hover-opacity cover-photo">
                                    <div class="w3-container w3-white">
                                        <p>
                                            <b class="title">
                                                <asp:Literal Text='<%# Eval("title").ToString() %>' runat="server" />
                                            </b>
                                        </p>
                                        <p class="content">
                                            <asp:Literal ID="litContent" Text='<%# Eval("content").ToString() %>' runat="server" />
                                        </p>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <!-- Pagination -->
                    <asp:Panel runat="server" ID="pnlPagination">
                        <div class="w3-center w3-padding-32">
                            <div class="w3-bar">
                                <asp:Button runat="server" CssClass="w3-bar-item w3-button w3-hover-black" Text="«" OnCommand="btnNavigation_Command" CommandArgument="Previous" ID="btnPrevious" />
                                <asp:Repeater runat="server" ID="repPagination" OnItemDataBound="repPagination_ItemDataBound">
                                    <ItemTemplate>
                                        <asp:Button runat="server" ID="btnPage" ClientIDMode="AutoID" CssClass="w3-bar-item w3-button w3-hover-black" Text='<%# Container.DataItem %>' OnClick="btnPage_Click" CommandArgument='<%# Container.DataItem %>' />
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Button runat="server" class="w3-bar-item w3-button w3-hover-black" Text="»" OnCommand="btnNavigation_Command" CommandArgument="Next" ID="btnNext" />
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!-- Images of Me -->
            <%--            <div class="w3-row-padding w3-padding-16" id="about">
                <div class="w3-col m6">
                    <img src="/w3images/avatar_g.jpg" alt="Me" style="width: 100%">
                </div>
                <div class="w3-col m6">
                    <img src="/w3images/me2.jpg" alt="Me" style="width: 100%">
                </div>
            </div>--%>

            <div class="w3-container w3-padding-large" style="margin-bottom: 32px" id="about">
                <h4><b>About Me</b></h4>
                <p>Just me, myself and I, exploring the universe of unknownment. I have a heart of love and an interest of lorem ipsum and mauris neque quam blog. I want to share my world with you. Praesent tincidunt sed tellus ut rutrum. Sed vitae justo condimentum, porta lectus vitae, ultricies congue gravida diam non fringilla. Praesent tincidunt sed tellus ut rutrum. Sed vitae justo condimentum, porta lectus vitae, ultricies congue gravida diam non fringilla.</p>
                <hr>

                <h4>Technical Skills</h4>
                <!-- Progress bars / Skills -->
                <p>Front-End</p>
                <div class="w3-grey">
                    <div class="w3-container w3-dark-grey w3-padding w3-center" style="width: 95%">95%</div>
                </div>
                <p>Back-End</p>
                <div class="w3-grey">
                    <div class="w3-container w3-dark-grey w3-padding w3-center" style="width: 90%">90%</div>
                </div>
                <p>Other</p>
                <div class="w3-grey">
                    <div class="w3-container w3-dark-grey w3-padding w3-center" style="width: 80%">80%</div>
                </div>
                <p>
                    <button class="w3-button w3-dark-grey w3-padding-large w3-margin-top w3-margin-bottom" runat="server" onserverclick="btnDownload_Click" type="button">
                        <i class="fa fa-download w3-margin-right"></i>Download Resume
                    </button>
                </p>
                <hr>

                <!-- <h4>How much I charge</h4> -->
                <!-- Pricing Tables -->
                <!-- <div class="w3-row-padding" style="margin:0 -16px">
      <div class="w3-third w3-margin-bottom">
        <ul class="w3-ul w3-border w3-white w3-center w3-opacity w3-hover-opacity-off">
          <li class="w3-black w3-xlarge w3-padding-32">Basic</li>
          <li class="w3-padding-16">Web Design</li>
          <li class="w3-padding-16">Photography</li>
          <li class="w3-padding-16">1GB Storage</li>
          <li class="w3-padding-16">Mail Support</li>
          <li class="w3-padding-16">
            <h2>$ 10</h2>
            <span class="w3-opacity">per month</span>
          </li>
          <li class="w3-light-grey w3-padding-24">
            <button class="w3-button w3-teal w3-padding-large w3-hover-black">Sign Up</button>
          </li>
        </ul>
      </div>
      
      <div class="w3-third w3-margin-bottom">
        <ul class="w3-ul w3-border w3-white w3-center w3-opacity w3-hover-opacity-off">
          <li class="w3-teal w3-xlarge w3-padding-32">Pro</li>
          <li class="w3-padding-16">Web Design</li>
          <li class="w3-padding-16">Photography</li>
          <li class="w3-padding-16">50GB Storage</li>
          <li class="w3-padding-16">Endless Support</li>
          <li class="w3-padding-16">
            <h2>$ 25</h2>
            <span class="w3-opacity">per month</span>
          </li>
          <li class="w3-light-grey w3-padding-24">
            <button class="w3-button w3-teal w3-padding-large w3-hover-black">Sign Up</button>
          </li>
        </ul>
      </div>
      
      <div class="w3-third">
        <ul class="w3-ul w3-border w3-white w3-center w3-opacity w3-hover-opacity-off">
          <li class="w3-black w3-xlarge w3-padding-32">Premium</li>
          <li class="w3-padding-16">Web Design</li>
          <li class="w3-padding-16">Photography</li>
          <li class="w3-padding-16">Unlimited Storage</li>
          <li class="w3-padding-16">Endless Support</li>
          <li class="w3-padding-16">
            <h2>$ 25</h2>
            <span class="w3-opacity">per month</span>
          </li>
          <li class="w3-light-grey w3-padding-24">
            <button class="w3-button w3-teal w3-padding-large w3-hover-black">Sign Up</button>
          </li>
        </ul>
      </div>
    </div> -->
            </div>

            <!-- Contact Section -->
            <div class="w3-container w3-padding-large w3-grey">
                <h4 id="contact"><b>Contact Me</b></h4>
                <div class="w3-row-padding w3-center w3-padding-24" style="margin: 0 -16px">
                    <div class="w3-third w3-dark-grey clickable" onclick="mailTo()">
                        <p><i class="fa fa-envelope w3-xxlarge w3-text-light-grey"></i></p>
                        <p>yufanliaocestlavie@gmail.com</p>
                    </div>
                    <div class="w3-third w3-teal">
                        <p><i class="fa fa-map-marker w3-xxlarge w3-text-light-grey"></i></p>
                        <p>HSC, TW</p>
                    </div>
                    <div class="w3-third w3-dark-grey">
                        <p><i class="fa fa-phone w3-xxlarge w3-text-light-grey"></i></p>
                        <p>+886 965605173</p>
                    </div>
                </div>
                <%--<hr class="w3-opacity">--%>
                <%--<form action="/action_page.php" target="_blank">
                    <div class="w3-section">
                        <label>Name</label>
                        <input class="w3-input w3-border" type="text" name="Name" required>
                    </div>
                    <div class="w3-section">
                        <label>Email</label>
                        <input class="w3-input w3-border" type="text" name="Email" required>
                    </div>
                    <div class="w3-section">
                        <label>Message</label>
                        <input class="w3-input w3-border" type="text" name="Message" required>
                    </div>
                    <button type="submit" class="w3-button w3-black w3-margin-bottom"><i class="fa fa-paper-plane w3-margin-right"></i>Send Message</button>
                <%--</form>--%>
            </div>
            <div class="w3-black w3-center w3-padding-16">Powered by <a href="https://www.w3schools.com/w3css/default.asp" title="W3.CSS" target="_blank" class="w3-hover-opacity">w3.css</a></div>
            <!-- End page content -->
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

            // 使用事件委托，在整个 Repeater 区域监听点击事件
            function redirectPage(noteId) {
                console.log(noteId)
                window.location.href = 'Note.aspx?id=' + encodeURIComponent(noteId);
            }
            function mailTo() {
                window.location.href = "mailto:yufanliaocestlavie@gmail.com";
            }
        </script>
    </main>
</asp:Content>
