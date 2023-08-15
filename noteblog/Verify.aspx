<%@ Page Title="Verify" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Verify.aspx.cs" Inherits="noteblog.Verify" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link rel="stylesheet" href="Shared/Verify.css" />

    <div class="jumbotron">
        <div class="container">
            <h1>Welcome to F.L.</h1>
            <p>
                <asp:Literal ID="litMsg" runat="server" />, You will be automatically redirected in 5 seconds to log in page.</p>
            <div class="progress progress-striped active">
                <div class="progress-bar progress-bar-primary" role="progressbar" id="progress" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 1%"></div>
            </div>
        </div>
    </div>
    <script>
        //only run script when the entire page and images are loaded
        $(window).on("load ", function () {
            setTimeout(function () {
                $("#progress").css("width", "100%");
            }, 500);
        });
    </script>
</asp:Content>

