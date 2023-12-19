<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProfileControl.ascx.cs" Inherits="noteblog.Controls.ProfileControl" %>

<link href="Shared/Profile.css" rel="stylesheet" />
<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
<script>
    $(function () {
        $("#btn-avatar-upload").click(function () {
            $("#fuEditProfileAvatar").click()
        })
        $("#btn-resume-upload").click(function () {
            $("#fuEditResume").click()
        })
        $("#imgProfileAvatar").toggle($("#imgProfileAvatar")[0].src.length > 0);

        $("#fuEditResume").on("change", function (event) {
            $("#resumeName").text(event.target.files[0].name);
        })
        $("#fuEditProfileAvatar").on("change", function (event) {
            var input = event.target;

            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $("#imgProfileAvatar").attr("src", e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
            }
        })

    })
</script>

<div class="container">
    <div class="row flex-lg-nowrap">
        <div class="col">
            <div class="row">
                <div class="col m-mb-3">
                    <div class="card">
                        <div class="card-body">
                            <div class="e-profile">
                                <div class="row">
                                    <div class="col-12 col-sm-auto mb-3">
                                        <div class="mx-auto" style="width: 140px;">
                                            <div class="d-flex justify-content-center align-items-center rounded" style="height: 140px; background-color: rgb(233, 236, 239);">
                                                <span style="color: rgb(166, 168, 170); font: bold 8pt Arial;">140x140</span>
                                                <asp:Image ID="imgProfileAvatar" ClientIDMode="Static" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col d-flex flex-column flex-sm-row justify-content-between mb-3 align-items-center align-items-md-start">
                                        <div class="text-center text-sm-left mb-2 mb-sm-0">
                                          <div class="d-flex align-items-center justify-content-md-start justify-content-center">
                                            <h4 class="pt-sm-2 pb-1 mb-0 text-nowrap mr-3">
                                                <asp:Literal ID="litUserName" runat="server" /></h4>
                                                <button runat="server" onserverclick="btnDirectSelfHome_Click" type="button" class="btn btn-outline-dark btn-sm rounded-circle">
                                                  <i class="fa fa-link" aria-hidden="true"></i>
                                                </button>
                                          </div>
                                            <p class="mb-0">
                                                <asp:Literal ID="litEmail" runat="server" />
                                            </p>
                                            <!-- <div class="text-muted"><small>Last seen 2 hours ago</small></div> -->
                                            <div class="mt-2 d-flex" style="gap: 5px">
                                                <asp:FileUpload ID="fuEditProfileAvatar" runat="server" CssClass="form-control" accept=".png,.jpg,.jpeg" ClientIDMode="Static" />
                                                <button class="btn btn-primary" id="btn-avatar-upload" type="button">
                                                    <i class="fa fa-fw fa-camera"></i>
                                                    <span>Change Photo</span>
                                                </button>
                                                <asp:FileUpload ID="fuEditResume" runat="server" CssClass="form-control" accept=".pdf" ClientIDMode="Static" />
                                                <button class="btn btn-primary" id="btn-resume-upload" type="button">
                                                    <i class="fa fa-fw fa-upload"></i>
                                                    <span>Upload Resume</span>
                                                </button>
                                                <span id="resumeName"></span>
                                            </div>
                                        </div>
                                        <div class="text-center text-sm-right">
                                            <span class="badge badge-secondary">
                                                <asp:Literal runat="server" ID="litRole" /></span>
                                            <div class="text-muted">
                                                <small>Joined
                                                <asp:Literal runat="server" ID="litCreatedAt" /></small>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <ul class="nav nav-tabs">
                                    <li class="nav-item active nav-link">Settings</li>
                                </ul>
                                <div class="tab-content pt-3">
                                    <div class="tab-pane active">
                                        <form class="form" novalidate="">
                                            <div class="row">
                                                <div class="col">
                                                    <!-- username -->
                                                    <div class="row">
                                                        <div class="col">
                                                            <div class="form-group">
                                                                <label>Username</label>
                                                                <asp:TextBox runat="server" ID="txtEditProfileName" ClientIDMode="Static" CssClass="form-control" placeholder="Frank"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- phone -->
                                                    <div class="row">
                                                        <div class="col">
                                                            <div class="form-group">
                                                                <label>Phone</label>
                                                                <asp:TextBox runat="server" ID="txtPhone" ClientIDMode="Static" CssClass="form-control" placeholder="+886-965605173"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- region  -->
                                                    <div class="row">
                                                        <div class="col">
                                                            <div class="form-group">
                                                                <label>Region</label>
                                                                <asp:TextBox runat="server" ID="txtRegionName" CssClass="form-control" placeholder="HSC, TW"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col">
                                                            <div class="form-group">
                                                                <label>Google Map Link</label>
                                                                <asp:TextBox ID="txtRegionLink" CssClass="form-control" placeholder="https://www.google.com/maps/place/..." runat="server"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- github -->
                                                    <div class="row">
                                                        <div class="col">
                                                            <div class="form-group">
                                                                <label>GitHub</label>
                                                                <asp:TextBox ID="txtGitHubLink" runat="server" CssClass="form-control" placeholder="https://github.com/user"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- job -->
                                                    <div class="row">
                                                        <div class="col">
                                                            <div class="form-group">
                                                                <label>Job Title</label>
                                                                <asp:TextBox ID="txtJobTitle" CssClass="form-control" runat="server" placeholder="Web Developer"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- about -->
                                                    <div class="row">
                                                        <div class="col mb-3">
                                                            <div class="form-group">
                                                                <label>About</label>
                                                                <asp:TextBox runat="server" ID="txtAbout" CssClass="form-control" TextMode="MultiLine" placeholder="My Bio"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- skills -->
                                                    <div class="row">
                                                        <div class="col mb-3">
                                                            <label>Technical Skills</label>
                                                            <div class="accordion" id="accordionExample">
                                                                <div class="card">
                                                                    <div class="card-header" id="headingOne">
                                                                        <h2 class="mb-0">
                                                                            <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                                                                Skill #1
                                                                            </button>
                                                                        </h2>
                                                                    </div>
                                                                    <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample">
                                                                        <div class="card-body">
                                                                            <div class="row">
                                                                                <div class="col">
                                                                                    <div class="form-group">
                                                                                        <label>Name</label>
                                                                                        <asp:TextBox runat="server" CssClass="form-control" placeholder="Front-End" ID="txtSkill1Name"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col">
                                                                                    <div class="form-group">
                                                                                        <label>Percentage</label>
                                                                                        <asp:TextBox runat="server" CssClass="form-control" placeholder="95%" ID="txtSkill1Percent"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="card">
                                                                    <div class="card-header" id="headingTwo">
                                                                        <h2 class="mb-0">
                                                                            <button class="btn btn-link btn-block text-left collapsed" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                                                                Skill #2
                                                                            </button>
                                                                        </h2>
                                                                    </div>
                                                                    <div id="collapseTwo" class="collapse" aria-labelledby="headingTwo" data-parent="#accordionExample">
                                                                        <div class="card-body">
                                                                            <div class="row">
                                                                                <div class="col">
                                                                                    <div class="form-group">
                                                                                        <label>Name</label>
                                                                                        <asp:TextBox runat="server" CssClass="form-control" placeholder="Back-End" ID="txtSkill2Name"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col">
                                                                                    <div class="form-group">
                                                                                        <label>Percentage</label>
                                                                                        <asp:TextBox runat="server" CssClass="form-control" placeholder="90%" ID="txtSkill2Percent"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="card">
                                                                    <div class="card-header" id="headingThree">
                                                                        <h2 class="mb-0">
                                                                            <button class="btn btn-link btn-block text-left collapsed" type="button" data-toggle="collapse" data-target="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                                                                                Skill #3
                                                                            </button>
                                                                        </h2>
                                                                    </div>
                                                                    <div id="collapseThree" class="collapse" aria-labelledby="headingThree" data-parent="#accordionExample">
                                                                        <div class="card-body">
                                                                            <div class="row">
                                                                                <div class="col">
                                                                                    <div class="form-group">
                                                                                        <label>Name</label>
                                                                                        <asp:TextBox runat="server" CssClass="form-control" placeholder="Other" ID="txtSkill3Name"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col">
                                                                                    <div class="form-group">
                                                                                        <label>Percentage</label>
                                                                                        <asp:TextBox runat="server" CssClass="form-control" placeholder="80%" ID="txtSkill3Percent"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col d-flex justify-content-end">
                                                    <button runat="server" onserverclick="btnUpdateProfile_Click" class="btn btn-primary loading-btn">Save Changes</button>
                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
