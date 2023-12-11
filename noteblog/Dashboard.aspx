<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="noteblog.Dashboard" EnableEventValidation="false" %>


<%@ Register Src="~/Controls/PaginationControl.ascx" TagName="PaginationControl" TagPrefix="uc" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Shared/bootstrap.min.css">
    <link rel="stylesheet" href="Shared/custom.css">
    <link rel="stylesheet" href="Shared/Stats.css">


    <!--google fonts -->
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600&display=swap" rel="stylesheet">

    <!--google material icon-->
    <link href="https://fonts.googleapis.com/css2?family=Material+Icons" rel="stylesheet">

    <!-- chart -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js"></script>
    <script src="Utils/js/chart.js" defer></script>

    <div class="wrapper">
        <asp:HiddenField ID="hidActiveView" runat="server" Value="0" ClientIDMode="Static" />
        <asp:HiddenField ID="hidActiveSidebarItem" runat="server" Value="notes" ClientIDMode="Static" />
        <div class="body-overlay"></div>

        <!-------sidebar--design------------>
        <div id="sidebar">
            <div class="sidebar-header">
                <asp:HyperLink NavigateUrl="/" runat="server">
                    <h3>
                        <asp:Image runat="server" CssClass="img-fluid rounded-circle" ID="imgAvatar" />
                        <asp:Label ID="lblUser" runat="server" />
                    </h3>
                </asp:HyperLink>
            </div>
            <ul class="list-unstyled component m-0 sidebar-list">
                <li class="active sidebar-item" data-sidebar-item="notes">
                    <asp:LinkButton runat="server" OnCommand="lbtnView_Command" CommandArgument="0" CssClass="dashboard" ID="lbtnManageNotes"><i class="material-icons">article</i>notes</asp:LinkButton></li>
                <asp:Panel runat="server" Visible="false" ID="pnlAdmin">
                    <li class="sidebar-item" data-sidebar-item="users">
                        <asp:LinkButton runat="server" OnCommand="lbtnView_Command" CommandArgument="1" CssClass="dashboard" ID="lbtnManageUsers"><i class="material-icons">manage_accounts</i>users</asp:LinkButton></li>
                    <li class="sidebar-item" data-sidebar-item="categories">
                        <asp:LinkButton runat="server" OnCommand="lbtnView_Command" CommandArgument="2" CssClass="dashboard"><i class="material-icons">category</i>categories</asp:LinkButton></li>
                </asp:Panel>
                <li class="sidebar-item" data-sidebar-item="profile">
                    <asp:LinkButton runat="server" OnCommand="lbtnView_Command" CommandArgument="3" CssClass="dashboard"><i class="material-icons">badge</i>profile</asp:LinkButton></li>
                <li class="sidebar-item" data-sidebar-item="logs">
                    <asp:LinkButton runat="server" OnCommand="lbtnView_Command" CommandArgument="4" CssClass="dashboard"><i class="material-icons">receipt_long</i>logs</asp:LinkButton></li>
                <li class="sidebar-item" data-sidebar-item="stats">
                    <asp:LinkButton runat="server" OnCommand="lbtnView_Command" CommandArgument="5" CssClass="dashboard"><i class="material-icons">query_stats</i>stats</asp:LinkButton>
                </li>
                <li class="sidebar-item" data-sidebar-item="email">
                    <asp:LinkButton runat="server" OnCommand="lbtnView_Command" CommandArgument="6" CssClass="dashboard"><i class="material-icons">mail</i>email</asp:LinkButton>
                </li>
            </ul>
        </div>
        <!-------page-content start----------->
        <div id="content">

            <!------top-navbar----------->
            <div class="top-navbar">
                <div class="xd-topbar">
                    <div class="row justify-content-between">
                        <div class="col-2 col-md-1 col-lg-1 order-2 order-md-1 align-self-center">
                            <div class="xp-menubar">
                                <span class="material-icons text-white">signal_cellular_alt</span>
                            </div>
                        </div>
                        <div class="col-10 col-md-6 col-lg-8 order-1 order-md-3">
                            <div class="xp-profilebar text-right">
                                <nav class="navbar p-0">
                                    <ul class="nav navbar-nav flex-row ml-auto">
                                        <li class="dropdown nav-item active">
                                            <a class="nav-link" href="#" data-toggle="dropdown">
                                                <span class="material-icons">notifications</span> <span class="notification">4</span> </a>
                                            <ul class="dropdown-menu">
                                                <li><a href="#">You Have 4 New Messages</a></li>
                                                <li><a href="#">You Have 4 New Messages</a></li>
                                                <li><a href="#">You Have 4 New Messages</a></li>
                                                <li><a href="#">You Have 4 New Messages</a></li>
                                            </ul>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" href="#">
                                                <span class="material-icons">question_answer</span> </a></li>
                                        <li class="dropdown nav-item">
                                            <a class="nav-link" href="#" data-toggle="dropdown">
                                                <span class="material-icons">person</span> <%--<span class="xp-user-live"></span>--%></a><ul class="dropdown-menu small-menu">
                                                    <%--												<li><a href="#">
													<span class="material-icons">person_outline</span>
													Profile
												</a></li>
												<li><a href="#">
													<span class="material-icons">settings</span>
													Settings
												</a></li>--%>
                                                    <li>
                                                        <asp:LinkButton runat="server" OnClick="btnOut_Click">
                                                            <span class="material-icons">logout</span>
                                                            Logout
                                                        </asp:LinkButton>
                                                    </li>
                                                </ul>
                                        </li>
                                    </ul>
                                </nav>
                            </div>
                        </div>
                    </div>
                    <%--                    <div class="xp-breadcrumbbar text-center">
						<h4 class="page-title">Notes</h4>--%><%--                        <ol class="breadcrumb">
							<li class="breadcrumb-item active" aria-curent="page">Notes</li>
						</ol>--%><%--</div>--%>
                </div>
            </div>
            <!------main-content----------->
            <asp:MultiView ActiveViewIndex="0" runat="server" ID="mvMainContent">
                <%--manage notes--%>
                <asp:View runat="server" ID="vManageNotes" OnActivate="vManageNotes_Activate">
                    <div class="main-content">
                        <div class="col-md-5 col-lg-3 order-3 order-md-2 mx-auto">
                            <div class="xp-searchbar">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtSearch" CssClass="form-control" placeholder="Search" type="search" />
                                    <asp:HiddenField ID="hdnSearch" runat="server" />
                                    <div class="input-group-append">
                                        <asp:Button runat="server" CssClass="btn" ID="btnSearch" Text="Go" OnCommand="btnSearch_Command" CommandArgument="note" OnClientClick="convertToBase64()" />
                                    </div>
                                </div>

                            </div>
                        </div>
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
                                                <a href="#deleteNoteModal" class="btn btn-danger disabled" data-toggle="modal" onclick="setNoteIds()" id="MultiDelete" runat="server"><i class="material-icons">&#xE15C;</i> <span>Delete</span> </a>
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
                                                            <a href="#deleteNoteModal" class="delete" data-toggle="modal" onclick="setNoteIds(<%# Eval("id") %>)"><i class="material-icons" data-toggle="tooltip" title="Delete">&#xE872;</i> </a></th>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                    <%--pagination--%><uc:PaginationControl ID="PaginationControlNotes" runat="server" />
                                    <%--<div class="clearfix">
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
									</div>--%>
                                </div>
                            </div>

                            <!----delete-modal--------->
                            <div class="modal fade" tabindex="-1" id="deleteNoteModal" role="dialog">
                                <input type="hidden" name="noteId" id="noteId" />
                                <div class="modal-dialog" role="document">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title">Delete Notes</h5>
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">&times;</span></button>
                                        </div>
                                        <div class="modal-body">
                                            <p>Are you sure you want to delete this Records</p>
                                            <p class="text-warning"><small>this action Cannot be Undone</small></p>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button><asp:Button ID="btnNoteDelete" runat="server" CssClass="btn btn-success" Text="Delete" OnClick="btnNoteDelete_Click" UseSubmitBehavior="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>

                <%--manage users--%>
                <asp:View ID="vManageUsers" runat="server" OnActivate="vManageUsers_Activate">
                    <asp:HiddenField ID="hidUserId" runat="server" ClientIDMode="Static" Value="-1" />
                    <div class="main-content">
                        <div class="col-md-5 col-lg-3 order-3 order-md-2 mx-auto">
                            <div class="xp-searchbar">
                                <div class="input-group">
                                    <asp:TextBox runat="server" ID="txtSearchUser" CssClass="form-control" placeholder="Search" type="search" />
                                    <asp:HiddenField ID="hdnSearchUser" runat="server" />
                                    <div class="input-group-append">
                                        <asp:Button runat="server" CssClass="btn" ID="btnSearchUser" Text="Go" OnCommand="btnSearch_Command" CommandArgument="user" OnClientClick="convertToBase64()" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-wrapper">
                                    <div class="table-title">
                                        <div class="row">
                                            <div class="col-sm-6 p-0 flex justify-content-lg-start justify-content-center">
                                                <h2 class="ml-lg-2">Manage  Users</h2>
                                            </div>
                                        </div>
                                    </div>
                                    <table class="table table-striped table-hover text-center">
                                        <thead>
                                            <tr>
                                                <th>Name</th>
                                                <th>Email</th>
                                                <th>Role</th>
                                                <th>Is Verified</th>
                                                <th>Updated At</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="repUsers" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Eval("name") %></td>
                                                        <td><%# Eval("email") %></td>
                                                        <td><%# Eval("role") %></td>
                                                        <td><%# Eval("isVerified") %></td>
                                                        <td><%# DataBinder.Eval(Container.DataItem, "updatedAt", "{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                                        <td>
                                                            <a href="#editUserModal" class="edit" data-toggle="modal" onclick="setId('User', <%# Eval("id") %>);">
                                                                <i class="material-icons" data-toggle="tooltip" title="Edit">&#xE254;</i> </a><a href="#deleteUserModal" class="delete" data-toggle="modal" onclick="setId('User', <%# Eval("id") %>)"><i class="material-icons" data-toggle="tooltip" title="Delete">&#xE872;</i> </a></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                    <%--pagination--%><uc:PaginationControl ID="PaginationControlUsers" runat="server" />

                                    <!----edit-modal--------->
                                    <div class="modal fade" tabindex="-1" id="editUserModal" role="dialog">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title">Edit User</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span></button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="form-group">
                                                        <label>Name</label>
                                                        <input type="text" class="form-control" name="editUserName" id="editUserName" placeholder="new name" />
                                                    </div>
                                                    <div class="form-group">
                                                        <label>Avatar</label>
                                                        <input type="file" name="editUserAvatar" />
                                                    </div>
                                                    <div class="form-group">
                                                        <label>Role</label>
                                                        <input type="text" class="form-control" name="editUserRole" id="editUserRole" placeholder="new role">
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button><asp:Button Text="Save" runat="server" CssClass="btn btn-success" OnCommand="btnManageUser_Command" CommandArgument="update" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!----delete-modal--------->
                                    <div class="modal fade" tabindex="-1" id="deleteUserModal" role="dialog">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title">Delete User</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span></button>
                                                </div>
                                                <div class="modal-body">
                                                    <p>Are you sure you want to delete this User</p>
                                                    <p class="text-warning"><small>this action Cannot be Undone</small></p>
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button><asp:Button ID="btnUserDelete" runat="server" CssClass="btn btn-success" Text="Delete" OnCommand="btnManageUser_Command" CommandArgument="delete" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>

                <%--manage categories--%>
                <asp:View ID="vManageCategories" runat="server" OnActivate="vManageCategories_Activate">
                    <asp:HiddenField ID="hidCategoryId" runat="server" ClientIDMode="Static" Value="-1" />
                    <div class="main-content">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-wrapper">
                                    <div class="table-title">
                                        <div class="row">
                                            <div class="col-sm-6 p-0 flex justify-content-lg-start justify-content-center">
                                                <h2 class="ml-lg-2">Manage  Categories</h2>
                                            </div>
                                            <div class="col-sm-6 p-0 flex justify-content-lg-end justify-content-center">
                                                <a href="#addCategoryModal" class="btn btn-success" data-toggle="modal">
                                                    <i class="material-icons">&#xE147;</i> <span>Add New Category</span> </a>
                                            </div>
                                        </div>
                                    </div>
                                    <table class="table table-striped table-hover text-center">
                                        <thead>
                                            <tr>
                                                <th>Name</th>
                                                <th>Description</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="repCategories" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%# Eval("name") %></td>
                                                        <td><%# Eval("description") %></td>
                                                        <td>
                                                            <a href="#editCategoryModal" class="edit" data-toggle="modal" onclick="setId('Category', <%# Eval("id") %>); getCategoryValue(<%# Eval("id") %>)">
                                                                <i class="material-icons" data-toggle="tooltip" title="Edit">&#xE254;</i> </a><a href="#deleteCategoryModal" class="delete" data-toggle="modal" onclick="setId('Category', <%# Eval("id") %>)"><i class="material-icons" data-toggle="tooltip" title="Delete">&#xE872;</i> </a></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                    <%--pagination--%><uc:PaginationControl ID="PaginationControlCategories" runat="server" />

                                    <!----add-modal--------->
                                    <div class="modal fade" tabindex="-1" id="addCategoryModal" role="dialog">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title">Add Category</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span></button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="form-group">
                                                        <label>Name</label>
                                                        <input name="insertCategoryName" class="form-control" placeholder="new name" />
                                                    </div>
                                                    <div class="form-group">
                                                        <label>Description</label>
                                                        <input name="insertCategoryDescription" class="form-control" placeholder="new description" />
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button><asp:Button runat="server" CssClass="btn btn-success" Text="Add" OnCommand="btnManageCategory_Command" CommandArgument="insert" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!----edit-modal--------->
                                    <div class="modal fade" tabindex="-1" id="editCategoryModal" role="dialog">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title">Edit Category</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span></button>
                                                </div>
                                                <div class="modal-body">
                                                    <div class="form-group">
                                                        <label>Name</label>
                                                        <input type="text" class="form-control" name="editCategoryName" placeholder="new name" id="editCategoryName" />
                                                    </div>
                                                    <div class="form-group">
                                                        <label>Description</label>
                                                        <input type="text" class="form-control" name="editCategoryDescription" placeholder="new description" id="editCategoryDescription">
                                                    </div>
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button><asp:Button Text="Save" runat="server" type="button" CssClass="btn btn-success" OnCommand="btnManageCategory_Command" CommandArgument="update" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!----delete-modal--------->
                                    <div class="modal fade" tabindex="-1" id="deleteCategoryModal" role="dialog">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title">Delete Category</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span></button>
                                                </div>
                                                <div class="modal-body">
                                                    <p>Are you sure you want to delete this Category</p>
                                                    <p class="text-warning"><small>this action Cannot be Undone</small></p>
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button><asp:Button runat="server" CssClass="btn btn-success" Text="Delete" OnCommand="btnManageCategory_Command" CommandArgument="delete" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>

                <%--manage profile--%>
                <asp:View ID="vManageProfile" runat="server" OnActivate="vManageProfile_Activate">
                    <div class="container bootstrap snippets bootdey">
                        <h1 class="text-info">Edit Profile</h1>
                        <hr>
                        <div class="row">
                            <!-- left column -->
                            <div class="col-md-3">
                                <div class="text-center">
                                    <asp:Image ID="imgProfileAvatar" runat="server" CssClass="avatar img-circle img-thumbnail" alt="avatar" Style="width: 200px; height: 200px" />
                                    <h6>Upload a different photo...</h6>
                                    <asp:FileUpload ID="fuEditProfileAvatar" runat="server" CssClass="form-control" accept=".png,.jpg,.jpeg" />
                                </div>
                            </div>

                            <!-- edit form column -->
                            <div class="col-md-9 personal-info">
                                <%--<div class="alert alert-info alert-dismissable">
							  <a class="panel-close close" data-dismiss="alert">×</a> 
							  <i class="fa fa-coffee"></i>
							  This is an <strong>.alert</strong>. Use this to show important messages to the user.
							</div>--%>
                                <h3>Personal info</h3>
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label class="col-lg-3 control-label">Name:</label>
                                        <div class="col-lg-8">
                                            <asp:TextBox ID="txtEditProfileName" runat="server" CssClass="form-control" type="text" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                                <div class="text-center">
                                    <asp:Button runat="server" CssClass="btn btn-primary" Text="Update" OnCommand="btnManageProfile_Command" CommandArgument="update" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <hr>
                </asp:View>

                <%--manage log--%>
                <asp:View ID="vManageLogs" runat="server" OnActivate="vManageLogs_Activate">
                    <div class="main-content">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-wrapper">
                                    <div class="table-title">
                                        <div class="row">
                                            <div class="col-sm-6 p-0 flex justify-content-lg-start justify-content-center">
                                                <h2 class="ml-lg-2">Logs</h2>
                                            </div>
                                            <div class="col-sm-6 p-0 flex justify-content-lg-end justify-content-center">
                                                <a href="#clearLogsModal" class="btn btn-danger" data-toggle="modal" runat="server">
                                                    <i class="material-icons">&#xE15C;</i> <span>Clear</span> </a>
                                            </div>
                                        </div>
                                    </div>
                                    <table class="table table-striped table-hover text-center">
                                        <thead>
                                            <tr class="d-flex">
                                                <th class="ellipsis-td">Level</th>
                                                <th class="ellipsis-td">Message</th>
                                                <th class="ellipsis-td">Exception</th>
                                                <th class="ellipsis-td">Created At</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="repLogs" runat="server">
                                                <ItemTemplate>
                                                    <tr class="d-flex">
                                                        <td class="ellipsis-td"><%# Eval("level") %></td>
                                                        <td class="ellipsis-td"><a href="#logContentModal" onclick="setLogContent('<%# Eval("message") %>')" data-toggle="modal" class="ellipsis"><%# Eval("message") %></a></td>
                                                        <td class="ellipsis-td"><a href="#logContentModal" onclick="setLogContent('<%# Eval("exception") %>')" data-toggle="modal" class="ellipsis"><%# Eval("exception") %></a></td>
                                                        <td style="font-size: 14px;" class="ellipsis-td"><%# Eval("createdAt") %></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>

                                    <%--pagination--%>
                                    <uc:PaginationControl ID="PaginationControlLogs" runat="server" />

                                    <!----detail-modal--------->
                                    <div class="modal fade" tabindex="-1" id="logContentModal" role="dialog">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title">Log Detail</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span></button>
                                                </div>
                                                <div class="modal-body">
                                                    <p id="logContent" style="word-wrap: break-word;"></p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!----delete-modal--------->
                                    <div class="modal fade" tabindex="-1" id="clearLogsModal" role="dialog">
                                        <div class="modal-dialog" role="document">
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <h5 class="modal-title">Clear Logs</h5>
                                                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span></button>
                                                </div>
                                                <div class="modal-body">
                                                    <p>Are you sure you want to clear Logs</p>
                                                    <p class="text-warning"><small>this action Cannot be Undone</small></p>
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button><asp:Button ID="btnLogsClear" runat="server" CssClass="btn btn-success" Text="Clear" OnClick="btnLogsClear_Click" UseSubmitBehavior="false" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:View>

                <%--manage stats--%>
                <asp:View ID="vManageStats" runat="server">
                    <div class="main-content">
                        <section class="block-date">
                          Date: <input type="text" name="date" id="startDate" required> - <input type="text" name="date" id="endDate" required><button class="btn-search" type="button"><span class="material-icons">
                            search
                            </span></button>
                        </section>
                        <section class="block-chart">
                          <div class="container-canvas">
                            <canvas id="notesChart"></canvas>
                          </div>
                          <div class="container-canvas">
                            <canvas id="visitsChart"></canvas>
                          </div>
                          <div class="container-canvas">
                            <canvas id="regionsChart"></canvas>
                          </div>
                          <div class="container-canvas">
                            <canvas id="ctrChart"></canvas>
                          </div>
                        </section>
                    </div>
                </asp:View>

                <%--manage email--%>
                <asp:View ID="vManageEmail" runat="server">
                    <div class="main-content">
                        <section>
                        </section>
                    </div>
                </asp:View>

            </asp:MultiView>

        <footer class="footer">
          <div class="container-fluid">
            <div class="footer-in">
              <p class="mb-0">&copy 2021 Vishweb Design . All Rights Reserved.</p>
            </div>
          </div>
			  </footer>
      </div>
    </div>
    <script src="Utils/js/dashboard.js"></script>
</asp:Content>

