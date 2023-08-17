﻿<%@ Page Title="Sign" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Sign.aspx.cs" Inherits="noteblog.Sign" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <link href="Shared/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="Shared/Sign.css" rel="stylesheet" />

    <div class="form-structor">
        <%--log in--%>
        <asp:Panel runat="server" DefaultButton="btnLogIn">
            <div class="signup">
                <h2 class="form-title" id="signup"><span>or</span>Log in</h2>
                <div class="form-holder">
                    <asp:TextBox runat="server" type="email" CssClass="input" placeholder="Email" ID="txtInEmail"></asp:TextBox>
                    <asp:TextBox runat="server" type="password" CssClass="input" placeholder="Password" ID="txtInPwd"></asp:TextBox>
                    <span toggle="#MainContent_txtInPwd" class="fa-regular fa-eye-slash field-icon toggle-in-password"></span>
                </div>
                <asp:CheckBox ID="cbRememberMe" runat="server" Text="Remember Me" CssClass="cb-remember" />
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Email is required" ValidationGroup="Login" ControlToValidate="txtInEmail" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Password is required" ValidationGroup="Login" ControlToValidate="txtInPwd" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:Label ID="lblInHint" runat="server" CssClass="hint"></asp:Label>
                <asp:Button ID="btnLogIn" ValidationGroup="Login" runat="server" CssClass="submit-btn" Text="Log in" OnClick="btnLogIn_Click" />
            </div>
        </asp:Panel>

        <%--register--%>
        <asp:Panel runat="server" DefaultButton="btnSignUp">
            <div class="login slide-up">
                <div class="center">
                    <h2 class="form-title" id="login"><span>or</span>Sign up</h2>
                    <div class="form-holder">
                        <asp:TextBox type="text" CssClass="input" ID="txtUpName" runat="server" placeholder="Name"></asp:TextBox>
                        <asp:TextBox type="email" CssClass="input" ID="txtUpEmail" runat="server" placeholder="Email"></asp:TextBox>
                        <asp:TextBox type="password" CssClass="input" ID="txtUpPwd" runat="server" placeholder="Password"></asp:TextBox>
                        <span toggle="#MainContent_txtUpPwd" class="fa-regular fa-eye-slash field-icon toggle-up-password"></span>
                    </div>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Name is required" ValidationGroup="Register" ControlToValidate="txtUpName" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Email is required" ValidationGroup="Register" ControlToValidate="txtUpEmail" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Password is required" ValidationGroup="Register" ControlToValidate="txtUpPwd" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ValidationGroup="Register"
                        ControlToValidate="txtUpEmail"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ErrorMessage="Please enter a valid email" CssClass="hidden-validator" Display="Dynamic" />
                    <asp:Label ID="lblUpHint" runat="server" CssClass="hint"></asp:Label>
                    <asp:Button ID="btnSignUp" ValidationGroup="Register" runat="server" CssClass="submit-btn" Text="Sign up" OnClick="btnSignUp_Click" />
                </div>
            </div>
        </asp:Panel>

        <!-- hint modal -->
        <asp:Panel ID="modalBgPanel" runat="server" CssClass="modal-bg" Visible="false">
            <asp:Panel ID="modalContentPanel" runat="server" CssClass="modal-content" Visible="false">

                <div class="modal-header">
                    <h5>Sign up success</h5>
                    <asp:Button ID="btnClose" runat="server" Text="X" CssClass="close-btn" type="button" OnClick="btnClose_Click" />
                </div>

                <div class="modal-body">
                    <p>Please check your email and verify your account</p>
                </div>

            </asp:Panel>
        </asp:Panel>
    </div>


    <script>
        const loginBtn = document.getElementById('login');
        const signupBtn = document.getElementById('signup');

        loginBtn.addEventListener('click', (e) => {
            let parent = e.target.parentNode.parentNode;
            Array.from(e.target.parentNode.parentNode.classList).find((element) => {
                if (element !== "slide-up") {
                    parent.classList.add('slide-up')
                } else {
                    signupBtn.parentNode.classList.add('slide-up')
                    parent.classList.remove('slide-up')
                }
            });
        });

        signupBtn.addEventListener('click', (e) => {
            let parent = e.target.parentNode;
            Array.from(e.target.parentNode.classList).find((element) => {
                if (element !== "slide-up") {
                    parent.classList.add('slide-up')
                } else {
                    loginBtn.parentNode.parentNode.classList.add('slide-up')
                    parent.classList.remove('slide-up')
                }
            });
        });

        $(".toggle-in-password").click(function () {

            $(this).toggleClass("fa-eye-slash fa-eye");
            var input = $($(this).attr("toggle"));
            if (input.attr("type") == "password") {
                input.attr("type", "text");
            } else {
                input.attr("type", "password");
            }
        });

        $(".toggle-up-password").click(function () {

            $(this).toggleClass("fa-eye-slash fa-eye");
            var input = $($(this).attr("toggle"));
            if (input.attr("type") == "password") {
                input.attr("type", "text");
            } else {
                input.attr("type", "password");
            }
        });
    </script>
</asp:Content>

