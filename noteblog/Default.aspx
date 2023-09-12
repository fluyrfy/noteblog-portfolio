<%@ Page Title="F.L." Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="noteblog._Default" %>

<%@ OutputCache Duration="300" VaryByParam="none" VaryByControl="ddlCategory" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Default.css" rel="stylesheet" />

    <main>
        <!-- Overlay effect when opening sidebar on small screens -->

        <!-- !PAGE CONTENT! -->
        <div class="w3-main" style="margin-left: 300px">
            <asp:DropDownList runat="server" ID="ddlCategory" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true" EnableViewState="False">
                <asp:ListItem Text="ALL" Value="ALL" />
                <asp:ListItem Text="Front-End" Value="Front-End" />
                <asp:ListItem Text="Back-End" Value="Back-End" />
            </asp:DropDownList>
            Client Time:
            <script type="text/javascript">
                document.write(Date());
            </script>
            <br />
            <asp:Label Text="" ID="time" runat="server" />
            <%--            <asp:UpdatePanel ID="updatePanel1" runat="server">
                <ContentTemplate>--%>
            <asp:HiddenField ID="hidCategoryName" runat="server" />
            <div class="w3-container">
                <h1 style="display: flex; justify-content: space-between; align-items: center;">
                    <b>My Portfolio</b>
                </h1>

                <%--category--%>
                <div class="w3-section w3-bottombar w3-padding-16" id="filter">
                    <span class="w3-margin-right">Filter:</span>
                    <asp:Repeater runat="server" ID="repCategory" OnItemCreated="repCategory_ItemCreated">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" OnCommand="btnFilter_Command" CommandArgument='<%# Eval("name") %>' type="button" Text='<%# Eval("name") %>' CssClass="w3-button w3-white category-item"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <%--notes--%>
            <div class="w3-row-padding">
                <asp:Repeater ID="repNote" runat="server" Visible="true">
                    <ItemTemplate>
                        <div class="w3-third w3-container w3-margin-bottom rep-item" onclick="redirectPage(<%# Eval("id") %>)">
                            <%--<div class="skeleton">
                                    </div>--%>
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

            <!-- pagination -->
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
            <%--</ContentTemplate>
            </asp:UpdatePanel>--%>

            <div class="w3-container w3-padding-large" style="margin-bottom: 32px" id="about">
                <h4><b>About Me</b></h4>
                <p>
                    Hi, I'm Frank Liao, and the website name F.L. is an abbreviation of my name.
                </p>
                <p style="text-align: justify">
                    F.L. is an exciting name representing various meanings. For instance, it could stand for Fast Learning, signifying my enthusiasm for rapidly acquiring new skills and knowledge. It could also mean Fun Life, expressing my enjoyment of life's pleasures and adventures. Furthermore, it could indicate Future Leader, denoting my leadership abilities and ambitious aspirations.
                </p>
                <p style="text-align: justify">
                    In defining our strengths, I chose three keywords for F.L.: "code," "recode," and "record." "Code" exemplifies our ability to create and solve problems using programming languages, while "Recode" underscores our continuous efforts to enhance and refine code, ensuring it's both efficient and lucid. Meanwhile, "Record" portrays our commitment to documenting and sharing our journey and accomplishments while learning, allowing anyone interested to gain insights from our experiences.
                </p>
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
            </div>

            <!-- Contact Section -->
            <div class="w3-container w3-padding-large w3-grey">
                <h4 id="contact"><b>Contact Me</b></h4>
                <div class="w3-row-padding w3-center w3-padding-24" style="margin: 0 -16px">
                    <div class="w3-third w3-dark-grey clickable">
                        <a href="mailto:yufanliaocestlavie@gmail.com">
                            <p><i class="fa fa-envelope w3-xxlarge w3-text-light-grey"></i></p>
                            <p>yufanliaocestlavie@gmail.com</p>
                        </a>
                    </div>
                    <div class="w3-third w3-teal">
                        <p><i class="fa fa-map-marker w3-xxlarge w3-text-light-grey"></i></p>
                        <p><a href="https://www.google.com/maps/place/%E6%96%B0%E7%AB%B9/@24.8015925,120.9690134,17z/data=!3m1!4b1!4m6!3m5!1s0x346835e9c2e07205:0x5e8cb484291aeeba!8m2!3d24.8015877!4d120.9715883!16s%2Fm%2F04lf1bd?hl=zh-TW&entry=ttu" target="_blank">HSC, TW</a></p>
                    </div>
                    <div class="w3-third w3-dark-grey">
                        <p><i class="fa fa-phone w3-xxlarge w3-text-light-grey"></i></p>
                        <p><a href="tel:+886-965605173">+886 965605173</a></p>
                    </div>
                </div>
            </div>
            <div class="w3-black w3-center w3-padding-16">Powered by <a href="https://www.w3schools.com/w3css/default.asp" title="W3.CSS" target="_blank" class="w3-hover-opacity">w3.css</a></div>
            <!-- End page content -->
        </div>

        <script>
            $(document).ready(function () {
                function redirectPage(noteId) {
                    console.log(noteId)
                    window.location.href = 'Note.aspx?id=' + encodeURIComponent(noteId);
                }

                function toggleFilterClass() {
                    const category = $('#<%= hidCategoryName.ClientID %>');
                    $(".category-item").each(function () {
                        $(this).removeClass("w3-black");
                    })
                    $(`#btn${category.val()}`).addClass("w3-black");
                }

                toggleFilterClass();


                var prm = Sys.WebForms.PageRequestManager.getInstance();
                prm.add_endRequest(function () {
                    toggleFilterClass();
                });
            })
        </script>
    </main>
</asp:Content>
