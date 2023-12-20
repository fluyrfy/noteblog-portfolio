<%@ Page Title="F.L." Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="noteblog._Default" %>

<%@ Register Src="~/Controls/NoteListControl.ascx" TagName="NoteListControl" TagPrefix="uc" %>

<%@ OutputCache Duration="86400" Location="Server" VaryByHeader="Referer" VaryByParam="uid" VaryByCustom="“none”" VaryByControl="ctl00$MainContent$NoteListControl1$hidLastUpdateTime;ctl00$MainContent$NoteListControl1$hidCategoryName;ctl00$MainContent$NoteListControl1$hidPageNumber;ctl00$MainContent$NoteListControl1$repPagination;ctl00$MainContent$NoteListControl1$repNote" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Default.css" rel="stylesheet" />
    <script src="Utils/js/download.js" async></script>
    <script data-name="BMC-Widget" data-cfasync="false" src="https://cdnjs.buymeacoffee.com/1.0.0/widget.prod.min.js" data-id="FrankLiao" data-description="Support me on Buy me a coffee!" data-message="" data-color="#5F7FFF" data-position="Right" data-x_margin="18" data-y_margin="18"></script>
    <script>
        $(function () {      // 取得父元素和子元素
            function init() {
                var parentElement = document.getElementById("donate-container");
                var childElement = document.getElementById("bmc-wbtn");
                var iframeElement = document.getElementById("bmc-iframe");
                var iconElement = document.createElement("i");
                iconElement.className = "fa fa-credit-card";
                iconElement.setAttribute("aria-hidden", "true");
                childElement.innerHTML = "Send me a tip"
                childElement.appendChild(iconElement);
                parentElement.appendChild(childElement);
                parentElement.appendChild(iframeElement);
            }
            init();
            $("#bmc-wbtn").click(function () {
                init();
            })
            let closeButton = document.getElementById("bmc-close-btn");
            closeButton.parentElement.addEventListener("click", function () {
                init()
            });

            $(".btn-clear-cache").click(function () {
                $(this).addClass("refresh");
                $.ajax({
                    url: `/api/status/clearCache`,
                    type: "GET",
                }).done(function () { }).always(function () {
                    location.reload();
                })
            })
        })
    </script>
    <main>
        <!-- Overlay effect when opening sidebar on small screens -->

        <!-- !PAGE CONTENT! -->
        <div class="w3-main" style="margin-left: 300px">
            <div class="w3-container">
                <h1 style="display: flex; justify-content: space-between; align-items: center;">
                    <b>My Portfolio</b>
                </h1>
                <span>last output cache time: <%= DateTime.Now.ToString() %>
                    <span class="btn-clear-cache">
                        <i class="ri-refresh-line"></i>
                    </span>
                </span>
                <uc:NoteListControl ID="NoteListControl1" runat="server" />
            </div>
            <div class="w3-container w3-padding-large" style="margin-bottom: 32px" id="about">
                <h4><b>About Me</b></h4>
                <p style="text-align: justify;">
                    <asp:Label ID="lblBiography" runat="server" Style="display: block; white-space: pre-wrap;" />
                </p>
                <hr>

                <h4>Technical Skills</h4>
                <!-- Progress bars / Skills -->
                <asp:Repeater runat="server" ID="repUserSkills" OnItemDataBound="repUserSkills_ItemDataBound">
                    <ItemTemplate>
                        <p><%# Eval("name") %></p>
                        <div class="w3-grey">
                            <div style='<%# "width: " + Eval("percent") %>' class="w3-container w3-dark-grey w3-padding w3-center"><%# Eval("percent") %></div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <p>
                    <button class="w3-button w3-dark-grey w3-padding-large w3-margin-top w3-margin-bottom loading-btn" runat="server" onclick="downloadResume()" type="button">
                        <i class="fa fa-download w3-margin-right"></i>Download Resume
                    </button>
                </p>
                <hr>
            </div>

            <!-- Contact Section -->
            <div class="w3-container w3-padding-large w3-grey">
                <h4 id="contact"><b>Contact Me</b><div id="donate-container"></div>
                </h4>
                <div class="w3-row-padding w3-center w3-padding-24" style="margin: 0 -16px">
                    <div class="w3-third w3-dark-grey clickable">
                        <a href="mailto:yufanliaocestlavie@gmail.com">
                            <p><i class="fa fa-envelope w3-xxlarge w3-text-light-grey"></i></p>
                            <p>
                                <asp:Literal ID="litContactEmail" runat="server" />
                            </p>
                        </a>
                    </div>
                    <div class="w3-third w3-teal">
                        <p><i class="fa fa-map-marker w3-xxlarge w3-text-light-grey"></i></p>
                        <p>
                            <asp:HyperLink runat="server" ID="hlkContactRegionLink" Target="_blank">
                                <asp:Literal ID="litContactRegionName" runat="server" />
                            </asp:HyperLink>
                        </p>
                    </div>
                    <div class="w3-third w3-dark-grey">
                        <p><i class="fa fa-phone w3-xxlarge w3-text-light-grey"></i></p>
                        <p>
                            <a href="tel:+886-965605173">
                                <asp:Literal ID="litContactPhone" runat="server" /></a>
                        </p>
                    </div>
                </div>
            </div>
            <div class="w3-black w3-center w3-padding-16">Powered by <a href="https://www.w3schools.com/w3css/default.asp" title="W3.CSS" target="_blank" class="w3-hover-opacity">w3.css</a></div>
            <!-- End page content -->
        </div>
    </main>
</asp:Content>
