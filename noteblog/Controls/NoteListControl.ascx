<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NoteListControl.ascx.cs" Inherits="noteblog.Controls.NoteListControl" %>

<asp:HiddenField ID="hidCategoryName" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hidLastUpdateTime" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hidPageNumber" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hidTotalPages" runat="server" ClientIDMode="Static" />

<link rel="stylesheet" href="Shared/effect/hover.css" />
<script>
    function toggleChangePage(number) {
        $("#hidPageNumber").val(number);
    }

    $(window).on("load", function () {
        $(".category-item").on('click', function () {
            $('#hidCategoryName').val($(this).attr("id").split("btn")[1])
            $("#hidPageNumber").val(1);
        })

        //var pageItem = $(".page-item");
        var hidPageNumber = $("#hidPageNumber");
        let curNumber = parseInt(hidPageNumber.val());
        let totalPages = parseInt($("#hidTotalPages").val());
        $("#btnPrevious, #btnNext").toggle(totalPages > 1);
        $("#btnPrevious").prop("disabled", curNumber == 1);
        $("#btnNext").prop("disabled", curNumber == totalPages);
        //pageItem.each(function (index) {
        //    $(this).toggleClass("w3-black", index == (hidPageNumber.val() - 1));
        //    $(this).on("click", function () {
        //        hidPageNumber.val(index + 1);
        //    });
        //})
        $(".page-previous").on("click", function () {
            if (curNumber > 1) {
                curNumber = curNumber--;
                hidPageNumber.val(curNumber);
            }
        })
        $(".page-next").on("click", function () {
            if (curNumber < totalPages) {
                curNumber = curNumber++;
                hidPageNumber.val(curNumber);
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

    })
</script>

<%--category--%>
<div class="w3-section w3-bottombar w3-padding-16" id="filter">
    <span class="w3-margin-right">Filter:</span>
    <asp:Repeater runat="server" ID="repCategory" OnItemCreated="repCategory_ItemCreated">
        <ItemTemplate>
            <div style="display: inline-block; position: relative;" class="loading-btn">
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
                    <img src='<%# Eval("pic") == DBNull.Value ? "Images/cover/default.jpg" : $"data:image/png;base64,{System.Convert.ToBase64String((byte[])Eval("pic"))}" %>' class="w3-hover-opacity cover-photo" alt="cover photo">
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
    <div class="w3-center">
        <div class="w3-bar">
            <asp:Button runat="server" CssClass="w3-bar-item w3-button w3-hover-black page-previous" Text="«" OnCommand="btnNavigation_Command" CommandArgument="Previous" ID="btnPrevious" ClientIDMode="Static" />
<%--            <asp:Repeater runat="server" ID="repPagination">
                <ItemTemplate>
                    <asp:Button runat="server" ID="btnPage" ClientIDMode="Static" CssClass="w3-bar-item w3-button w3-hover-black page-item loading-btn" Text='<%# Container.DataItem %>' OnClientClick='<%# "toggleChangePage(" + Container.DataItem + ");" %>' OnCommand="btnPage_Command" CommandArgument='<%# Container.DataItem %>' />
                </ItemTemplate>
            </asp:Repeater>--%>
            <asp:Button runat="server" class="w3-bar-item w3-button w3-hover-black page-next" Text="»" OnCommand="btnNavigation_Command" CommandArgument="Next" ID="btnNext" ClientIDMode="Static" />
        </div>
    </div>
</asp:Panel>



