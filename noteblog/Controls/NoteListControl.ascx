<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NoteListControl.ascx.cs" Inherits="noteblog.Controls.NoteListControl" %>

<asp:HiddenField ID="hidCategoryName" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hidLastUpdateTime" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hidPageNumber" runat="server" ClientIDMode="Static" />

<%--category--%>
<div class="w3-section w3-bottombar w3-padding-16" id="filter">
    <span class="w3-margin-right">Filter:</span>
    <asp:Repeater runat="server" ID="repCategory" OnItemCreated="repCategory_ItemCreated">
        <ItemTemplate>
            <div style="display: inline-block; position: relative;">
                <i runat="server" class='<%# "fa category-icon " + Eval("iconClass") %>' aria-hidden="true"></i>
                <asp:LinkButton runat="server" OnCommand="btnFilter_Command" CommandArgument='<%# Eval("name") %>' Text='<%# Eval("name") %>' CssClass="w3-button w3-white category-item" UseSubmitBehavior="false" />
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

<%--notes--%>
<div class="w3-row-padding">
    <asp:Repeater ID="repNote" runat="server" Visible="true">
        <ItemTemplate>
            <div class="w3-third w3-container w3-margin-bottom rep-item float-shadow">
                <a href='<%# "Note.aspx?id=" + Eval("id") %>' target="_blank">
                    <img src="data:image/png;base64,<%# System.Convert.ToBase64String((byte[])Eval("pic"))%>" class="w3-hover-opacity cover-photo" alt="cover photo">
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
                </a>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

<!-- pagination -->
<asp:Panel runat="server" ID="pnlPagination">
    <div class="w3-center w3-padding-32">
        <div class="w3-bar">
            <asp:Button runat="server" CssClass="w3-bar-item w3-button w3-hover-black page-previous" Text="«" OnCommand="btnNavigation_Command" CommandArgument="Previous" ID="btnPrevious" ClientIDMode="Static" />
            <asp:Repeater runat="server" ID="repPagination" OnItemDataBound="repPagination_ItemDataBound">
                <ItemTemplate>
                    <asp:Button runat="server" ID="btnPage" ClientIDMode="Static" CssClass="w3-bar-item w3-button w3-hover-black page-item" Text='<%# Container.DataItem %>' OnClientClick='<%# "toggleChangePage(" + Container.DataItem + ");" %>' OnCommand="btnPage_Command" CommandArgument='<%# Container.DataItem %>' />
                </ItemTemplate>
            </asp:Repeater>
            <asp:Button runat="server" class="w3-bar-item w3-button w3-hover-black page-next" Text="»" OnCommand="btnNavigation_Command" CommandArgument="Next" ID="btnNext" ClientIDMode="Static" />
        </div>
    </div>
</asp:Panel>

<link rel="stylesheet" href="Shared/effect/hover.css" />
<script src="Utils/js/note.js"></script>
<script>
    function toggleChangePage(number) {
        $("#hidPageNumber").val(number);
    }

    $(document).ready(function () {
        getLastUpdateTime();

        $(".category-item").on('click', function () {
            $('#hidCategoryName').val($(this).attr("id").split("btn")[1])
            getLastUpdateTime();
            $("#hidPageNumber").val(1);
        })

        var pageItem = $(".page-item");
        var hidPageNumber = $("#hidPageNumber");
        $("#btnPrevious").toggle(pageItem.length > 1);
        $("#btnNext").toggle(pageItem.length > 1);
        pageItem.each(function (index) {
            $(this).toggleClass("w3-black", index == (hidPageNumber.val() - 1));
        })
        $(".page-previous").on("click", function () {
            if (hidPageNumber.val() > 1) {
                hidPageNumber.val(hidPageNumber.val() - 1);
            }
        })
        $(".page-next").on("click", function () {
            if (hidPageNumber.val() < $(".page-item").length) {
                hidPageNumber.val(hidPageNumber.val() + 1);
            }
        })


        function toggleFilterClass() {
            const category = $('#hidCategoryName');
            $(".category-item").each(function () {
                $(this).removeClass("w3-black");
                $(this).siblings('i').removeClass("active");
            })
            $(`#btn${category.val()}`).addClass("w3-black");
            $(`#btn${category.val()}`).siblings('i').addClass("active");
        }

        toggleFilterClass();

        startPolling();
    })
</script>

