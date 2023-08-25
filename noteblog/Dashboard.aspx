﻿<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="noteblog.Dashboard" EnableEventValidation="false" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Shared/bootstrap.min.css">
    <!----css3---->
    <link rel="stylesheet" href="Shared/custom.css">


    <!--google fonts -->
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap" rel="stylesheet">

    <!--google material icon-->
    <link href="https://fonts.googleapis.com/css2?family=Material+Icons" rel="stylesheet">

    <div class="wrapper">
        <div class="body-overlay"></div>

        <!-------sidebar--design------------>
        <div id="sidebar">
            <div class="sidebar-header">
                <asp:HyperLink NavigateUrl="/" runat="server">
                    <h3>
                        <img src="img/logo.png" class="img-fluid" /><asp:Label ID="lblUser" runat="server" /></h3>
                </asp:HyperLink>
            </div>
            <ul class="list-unstyled component m-0">
                <li class="active">
                    <a href="#" class="dashboard"><i class="material-icons">dashboard</i>dashboard </a>
                </li>

                <li class="dropdown">
                    <a href="#homeSubmenu1" data-toggle="collapse" aria-expanded="false"
                        class="dropdown-toggle">
                        <i class="material-icons">aspect_ratio</i>Layouts
                    </a>
                    <ul class="collapse list-unstyled menu" id="homeSubmenu1">
                        <li><a href="#">layout 1</a></li>
                        <li><a href="#">layout 2</a></li>
                        <li><a href="#">layout 3</a></li>
                    </ul>
                </li>


                <li class="dropdown">
                    <a href="#homeSubmenu2" data-toggle="collapse" aria-expanded="false"
                        class="dropdown-toggle">
                        <i class="material-icons">apps</i>widgets
                    </a>
                    <ul class="collapse list-unstyled menu" id="homeSubmenu2">
                        <li><a href="#">Apps 1</a></li>
                        <li><a href="#">Apps 2</a></li>
                        <li><a href="#">Apps 3</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="#homeSubmenu3" data-toggle="collapse" aria-expanded="false"
                        class="dropdown-toggle">
                        <i class="material-icons">equalizer</i>charts
                    </a>
                    <ul class="collapse list-unstyled menu" id="homeSubmenu3">
                        <li><a href="#">Pages 1</a></li>
                        <li><a href="#">Pages 2</a></li>
                        <li><a href="#">Pages 3</a></li>
                    </ul>
                </li>


                <li class="dropdown">
                    <a href="#homeSubmenu4" data-toggle="collapse" aria-expanded="false"
                        class="dropdown-toggle">
                        <i class="material-icons">extension</i>UI Element
                    </a>
                    <ul class="collapse list-unstyled menu" id="homeSubmenu4">
                        <li><a href="#">Pages 1</a></li>
                        <li><a href="#">Pages 2</a></li>
                        <li><a href="#">Pages 3</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="#homeSubmenu5" data-toggle="collapse" aria-expanded="false"
                        class="dropdown-toggle">
                        <i class="material-icons">border_color</i>forms
                    </a>
                    <ul class="collapse list-unstyled menu" id="homeSubmenu5">
                        <li><a href="#">Pages 1</a></li>
                        <li><a href="#">Pages 2</a></li>
                        <li><a href="#">Pages 3</a></li>
                    </ul>
                </li>

                <li class="dropdown">
                    <a href="#homeSubmenu6" data-toggle="collapse" aria-expanded="false"
                        class="dropdown-toggle">
                        <i class="material-icons">grid_on</i>tables
                    </a>
                    <ul class="collapse list-unstyled menu" id="homeSubmenu6">
                        <li><a href="#">table 1</a></li>
                        <li><a href="#">table 2</a></li>
                        <li><a href="#">table 3</a></li>
                    </ul>
                </li>


                <li class="dropdown">
                    <a href="#homeSubmenu7" data-toggle="collapse" aria-expanded="false"
                        class="dropdown-toggle">
                        <i class="material-icons">content_copy</i>Pages
                    </a>
                    <ul class="collapse list-unstyled menu" id="homeSubmenu7">
                        <li><a href="#">Pages 1</a></li>
                        <li><a href="#">Pages 2</a></li>
                        <li><a href="#">Pages 3</a></li>
                    </ul>
                </li>


                <li class="">
                    <a href="#" class=""><i class="material-icons">date_range</i>copy </a>
                </li>
                <li class="">
                    <a href="#" class=""><i class="material-icons">library_books</i>calender </a>
                </li>

            </ul>
        </div>

        <!-------sidebar--design- close----------->



        <!-------page-content start----------->

        <div id="content">

            <!------top-navbar-start----------->

            <div class="top-navbar">
                <div class="xd-topbar">
                    <div class="row">
                        <div class="col-2 col-md-1 col-lg-1 order-2 order-md-1 align-self-center">
                            <div class="xp-menubar">
                                <span class="material-icons text-white">signal_cellular_alt</span>
                            </div>
                        </div>

                        <div class="col-md-5 col-lg-3 order-3 order-md-2">
                            <div class="xp-searchbar">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control" placeholder="Search" type="search" />
                                    <asp:HiddenField ID="hdnSearch" runat="server" />
                                    <div class="input-group-append">
                                        <asp:Button runat="server" CssClass="btn" ID="btnSearch" Text="Go" OnClick="btnSearch_Click" OnClientClick="convertToBase64()" />
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="col-10 col-md-6 col-lg-8 order-1 order-md-3">
                            <div class="xp-profilebar text-right">
                                <nav class="navbar p-0">
                                    <ul class="nav navbar-nav flex-row ml-auto">
                                        <li class="dropdown nav-item active">
                                            <a class="nav-link" href="#" data-toggle="dropdown">
                                                <span class="material-icons">notifications</span>
                                                <span class="notification">4</span>
                                            </a>
                                            <ul class="dropdown-menu">
                                                <li><a href="#">You Have 4 New Messages</a></li>
                                                <li><a href="#">You Have 4 New Messages</a></li>
                                                <li><a href="#">You Have 4 New Messages</a></li>
                                                <li><a href="#">You Have 4 New Messages</a></li>
                                            </ul>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" href="#">
                                                <span class="material-icons">question_answer</span>
                                            </a>
                                        </li>
                                        <li class="dropdown nav-item">
                                            <a class="nav-link" href="#" data-toggle="dropdown">
                                                <img src="img/user.jpg" style="width: 40px; border-radius: 50%;" />
                                                <span class="xp-user-live"></span>
                                            </a>
                                            <ul class="dropdown-menu small-menu">
                                                <li><a href="#">
                                                    <span class="material-icons">person_outline</span>
                                                    Profile
                                                </a></li>
                                                <li><a href="#">
                                                    <span class="material-icons">settings</span>
                                                    Settings
                                                </a></li>
                                                <li>
                                                    <asp:LinkButton runat="server" OnClick="btnOut_Click">
                                                        <span class="material-icons">logout</span>
                                                        Logout
                                                    </asp:LinkButton></li>
                                            </ul>
                                        </li>
                                    </ul>
                                </nav>
                            </div>
                        </div>
                    </div>

                    <div class="xp-breadcrumbbar text-center">
                        <h4 class="page-title">Dashboard</h4>
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item">
                                <asp:HyperLink runat="server" NavigateUrl="~/Default.aspx" ID="hlkUser"></asp:HyperLink></li>
                            <li class="breadcrumb-item active" aria-curent="page">Dashboard</li>
                        </ol>
                    </div>


                </div>
            </div>
            <!------top-navbar-end----------->

            <!------main-content-start----------->

            <div class="main-content">
                <div class="row">
                    <div class="col-md-12">
                        <div class="table-wrapper">

                            <div class="table-title">
                                <div class="row">
                                    <div class="col-sm-6 p-0 flex justify-content-lg-start justify-content-center">
                                        <h2 class="ml-lg-2">Manage  Notes</h2>
                                    </div>
                                    <div class="col-sm-6 p-0 flex justify-content-lg-end justify-content-center">
                                        <asp:HyperLink runat="server" NavigateUrl="Take.aspx" class="btn btn-success">
                                            <i class="material-icons">&#xE147;</i>
                                            <span>Add New Notes</span>
                                        </asp:HyperLink>
                                        <a href="#deleteNoteModal" class="btn btn-danger disabled" data-toggle="modal" onclick="setNoteIds()" id="MultiDelete" runat="server">
                                            <i class="material-icons">&#xE15C;</i>
                                            <span>Delete</span>
                                        </a>
                                    </div>
                                </div>
                            </div>

                            <table class="table table-striped table-hover text-center">
                                <thead>
                                    <tr>
                                        <th><span class="custom-checkbox"></span>
                                            <asp:CheckBox ID="cbSelectAll" AutoPostBack="true" runat="server" OnCheckedChanged="cbSelectAll_CheckedChanged" CssClass="cb-note" />
                                            <%--<input type="checkbox" id="selectAll">--%>
                                            <%--<label for="selectAll"></label>--%>
                                        </th>
                                        <th>Id</th>
                                        <th>Title</th>
                                        <th>Development</th>
                                        <th>Updated At</th>
                                        <th>Actions</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:Repeater ID="repNotes" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <th><span class="custom-checkbox"></span>
                                                    <asp:CheckBox runat="server" ID="cbNote" AutoPostBack="true" OnCheckedChanged="cbNote_CheckedChanged" CssClass="cb-note" />
                                                    <%--<input type="checkbox" name="option[]" value="1">--%>
                                                    <%--<label for="checkbox"></label>--%>
                                                </th>
                                                <th>
                                                    <asp:Literal Text='<%# Eval("id") %>' runat="server" ID="litNoteId" />
                                                </th>
                                                <th>
                                                    <a href='<%# "Note.aspx?id=" + Eval("id") %>' class="link-opacity-25-hover link-offset-2 underline" onmouseover="this.style.textDecoration='underline';"
                                                        onmouseout="this.style.textDecoration='none';">
                                                        <%# Eval("title") %>
                                                    </a>
                                                </th>
                                                <th><%# Eval("development") %></th>
                                                <th><%# DataBinder.Eval(Container.DataItem, "updated_at", "{0:yyyy-MM-dd HH:mm:ss}") %></th>
                                                <th>
                                                    <asp:HyperLink runat="server" NavigateUrl='<%# "Modify.aspx?id=" + Eval("id") %>' CssClass="edit">
                                                        <i class="material-icons" data-toggle="tooltip" title="Edit">&#xE254;</i>
                                                    </asp:HyperLink>
                                                    <a href="#deleteNoteModal" class="delete" data-toggle="modal" onclick="setNoteIds(<%# Eval("id") %>)">
                                                        <i class="material-icons" data-toggle="tooltip" title="Delete">&#xE872;</i>
                                                    </a>
                                                </th>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                            <div class="clearfix">
                                <div class="hint-text">
                                    showing <b>
                                        <asp:Literal ID="litPageSize" runat="server" /></b> out of <b>
                                            <asp:Literal ID="litDataCount" runat="server" /></b>
                                </div>
                                <ul class="pagination">
                                    <li class="page-item">
                                        <asp:LinkButton runat="server" Text="Previous" CssClass="page-link btn disabled" OnCommand="btnPreNext_Command" CommandName="Previous" ID="btnPrevious" /></li>
                                    <asp:Repeater runat="server" ID="repPage">
                                        <ItemTemplate>
                                            <li class="page-item">
                                                <asp:LinkButton runat="server" CssClass="page-link active" Text='<%# Container.DataItem %>' OnClick="btnPage_Click" CommandArgument='<%# Container.DataItem %>' ID="btnPage" /></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <li class="page-item">
                                        <asp:LinkButton runat="server" Text="Next" CssClass="page-link btn" OnCommand="btnPreNext_Command" CommandName="Next" ID="btnNext" /></li>
                                </ul>
                            </div>
                        </div>
                    </div>

                    <!----add-modal start--------->
                    <%--<div class="modal fade" tabindex="-1" id="addEmployeeModal" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Add Notes</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <div class="form-group">
                                        <label>Name</label>
                                        <input type="text" class="form-control" required>
                                    </div>
                                    <div class="form-group">
                                        <label>Email</label>
                                        <input type="emil" class="form-control" required>
                                    </div>
                                    <div class="form-group">
                                        <label>Address</label>
                                        <textarea class="form-control" required></textarea>
                                    </div>
                                    <div class="form-group">
                                        <label>Phone</label>
                                        <input type="text" class="form-control" required>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                                    <button type="button" class="btn btn-success">Add</button>
                                </div>
                            </div>
                        </div>
                    </div>--%>

                    <!----edit-modal end--------->

                    <!----edit-modal start--------->
                    <%--<div class="modal fade" tabindex="-1" id="editEmployeeModal" role="dialog">
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Edit Notes</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <div class="form-group">
                                        <label>Name</label>
                                        <input type="text" class="form-control" required>
                                    </div>
                                    <div class="form-group">
                                        <label>Email</label>
                                        <input type="email" class="form-control" required>
                                    </div>
                                    <div class="form-group">
                                        <label>Address</label>
                                        <textarea class="form-control" required></textarea>
                                    </div>
                                    <div class="form-group">
                                        <label>Phone</label>
                                        <input type="text" class="form-control" required>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                                    <button type="button" class="btn btn-success">Save</button>
                                </div>
                            </div>
                        </div>
                    </div>--%>

                    <!----edit-modal end--------->

                    <!----delete-modal start--------->
                    <div class="modal fade" tabindex="-1" id="deleteNoteModal" role="dialog">

                        <input type="hidden" name="noteId" id="noteId" />
                        <div class="modal-dialog" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Delete Notes</h5>
                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">&times;</span>
                                    </button>
                                </div>
                                <div class="modal-body">
                                    <p>Are you sure you want to delete this Records</p>
                                    <p class="text-warning"><small>this action Cannot be Undone</small></p>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                                    <asp:Button ID="btnNoteDelete" runat="server" CssClass="btn btn-success" Text="Delete" OnClick="btnNoteDelete_Click" UseSubmitBehavior="false" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!----delete-modal end--------->

                </div>
            </div>

            <!------main-content-end----------->

            <!----footer-design------------->

            <%--            <footer class="footer">
                <div class="container-fluid">
                    <div class="footer-in">
                        <p class="mb-0">&copy 2021 Vishweb Design . All Rights Reserved.</p>
                    </div>
                </div>
            </footer>--%>
        </div>
    </div>


    <script type="text/javascript">
        function setNoteIds(noteIds = 0) {
            document.getElementById("noteId").value = noteIds == 0 ? 0 : noteIds;
        }

        function convertToBase64() {
            if ($("#MainContent_txtSearch").val() == "") {
                return false
            }
            var userInput = $("#MainContent_txtSearch").val();
            var encodedInput = btoa(encodeURIComponent(userInput)); // 將用戶輸入的內容進行 Base64 編碼
            $("#MainContent_hdnSearch").val(encodedInput);
            $("#MainContent_txtSearch").val("");
            return true;
        }

        $(document).ready(function () {
            $(".xp-menubar").on('click', function () {
                $("#sidebar").toggleClass('active');
                $("#content").toggleClass('active');
            });

            $('.xp-menubar,.body-overlay').on('click', function () {
                $("#sidebar,.body-overlay").toggleClass('show-nav');
            });

            $('.cb-note').on('click', function () {
                var selectedCheckBoxes = $('.cb-note:checked');
                var multiDelete = $('.multi-delete');
                if (selectedCheckBoxes.length > 0) {
                    multiDelete.removeClass('disabled');
                } else {
                    multiDelete.addClass('disabled');
                }
            });
        });
    </script>

</asp:Content>

