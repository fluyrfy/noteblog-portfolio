<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaginationControl.ascx.cs" Inherits="noteblog.Controls.PaginationControl" %>

<%--pagination--%>
<asp:HiddenField ID="hidNowPage" runat="server" ClientIDMode="Static" Value="1" />
<div class="clearfix">
    <div class="hint-text">
        showing <b>
            <asp:Literal ID="litPageSize" runat="server" /></b> out of <b>
                <asp:Literal ID="litDataCount" runat="server" /></b>
    </div>
    <ul class="pagination">
        <li class="page-item">
            <asp:LinkButton runat="server" Text="Previous" CssClass="page-link btn disabled page-previous" OnCommand="btnPreNext_Command" CommandName="Previous" ID="btnPrevious" /></li>
        <asp:Repeater runat="server" ID="repPage">
            <ItemTemplate>
                <li class="page-item">
                    <asp:LinkButton runat="server" CssClass="page-link page-number" Text='<%# Container.DataItem %>' OnCommand="btnPage_Command" CommandArgument='<%# Container.DataItem %>' /></li>
            </ItemTemplate>
        </asp:Repeater>
        <li class="page-item">
            <asp:LinkButton runat="server" Text="Next" CssClass="page-link btn page-next" OnCommand="btnPreNext_Command" CommandName="Next" ID="btnNext" /></li>
    </ul>
</div>

<script>
    $(document).ready(function () {
        $(".sidebar-item").on("click", function () {
            $("#hidNowPage").val(1);
        })
        var maxIdx = $(".page-link").length - 2;
        var nowPage = parseInt($("#hidNowPage").val());
        var totalButtons = $(".page-number").length;
        $(".page-next").toggleClass("disabled", nowPage == maxIdx);
        $(".page-previous").toggleClass("disabled", nowPage == "1");
        var startPage = Math.max(1, nowPage - 2);
        var endPage = Math.min(nowPage + 2, totalButtons);
        $(".page-number").hide();
        for (var i = startPage; i <= endPage; i++) {
            $(".page-number").eq(i - 1).show();
        }
        if (startPage > 1) {
            $(".page-number").eq(startPage - 1).before('<span>...</span>');
        }
        if (endPage < totalButtons) {
            $(".page-number").eq(endPage - 1).after('<span>...</span>');
        }
        $(".page-number").each(function () {
            $(this).removeClass("active");
            if ($(this).text() == $("#hidNowPage").val()) {
                $(this).addClass("active");
            }
        })
    })
</script>
