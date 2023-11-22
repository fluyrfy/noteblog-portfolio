<%@ Page Title="Sign" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Sign.aspx.cs" Inherits="noteblog.Sign" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <link href="Shared/bootstrap.min.css" rel="stylesheet" />--%>
    <link href="Shared/Sign.css" rel="stylesheet" />
    <script src="https://challenges.cloudflare.com/turnstile/v0/api.js?onload=onloadTurnstileCallback" defer></script>
    <script src="Utils/js/turnstile.js" type="module"></script>
    <script>        
        $(document).ready(function () {
            $('.signup').keypress(function (e) {
                if (e.keyCode == '13') {
                    $(this).find('.btnLogin').click();
                }
            });
            $('.login').keypress(function (e) {
                if (e.keyCode == '13') {
                    $(this).find('.btnSignup').click();
                }
            });
            $(".toggle-reset-password").click(function () {
                $(this).toggleClass("fa-eye-slash fa-eye");
                var input = $($(this).attr("toggle"));
                if (input.attr("type") == "password") {
                    input.attr("type", "text");
                } else {
                    input.attr("type", "password");
                }
            });

            $(".toggle-reset-password-confirm").click(function () {

                $(this).toggleClass("fa-eye-slash fa-eye");
                var input = $($(this).attr("toggle"));
                if (input.attr("type") == "password") {
                    input.attr("type", "text");
                } else {
                    input.attr("type", "password");
                }
            });

            const loginBtn = document.getElementById('login');
            const signupBtn = document.getElementById('signup');

            loginBtn?.addEventListener('click', (e) => {
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

            signupBtn?.addEventListener('click', (e) => {
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
        });
    </script>

    <div class="form-structor">
        <asp:Panel runat="server" ID="pnlResetPwd" ClientIDMode="Static">
            <%--reset password block--%>
            <div class="signup">
                <h2 class="form-title" id="">Reset Password</h2>
                <%--confirm & send verify email--%>
                <asp:Panel runat="server" ID="pnlConfirmEmail">
                    <div class="form-holder">
                        <asp:TextBox runat="server" type="email" CssClass="input" placeholder="Email" ID="txtConfirmEmail"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Email is required" ValidationGroup="Confirm" ControlToValidate="txtConfirmEmail" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ValidationGroup="Confirm"
                        ControlToValidate="txtConfirmEmail"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ErrorMessage="Please enter a valid email" CssClass="hidden-validator" Display="Dynamic" />
                    <asp:Label runat="server" CssClass="hint" ID="lblConfirmHint" />
                    <asp:Button ID="btnConfirmEmail" ValidationGroup="Confirm" runat="server" CssClass="submit-btn" Text="Send Confirmation Email" OnClick="btnConfirmEmail_Click" />
                    <asp:HyperLink NavigateUrl="~/Sign.aspx" runat="server" Text="New here? Sign up" CssClass="btn-forgot" />
                </asp:Panel>
                <%--reset password--%>
                <asp:Panel runat="server" ID="pnlResetExistPwd">
                    <div class="form-holder">
                        <asp:TextBox runat="server" type="password" CssClass="input" placeholder="Password" ID="txtResetPwd"></asp:TextBox>
                        <span toggle="#MainContent_txtResetPwd" class="fa-regular fa-eye-slash field-icon toggle-reset-password"></span>
                        <asp:TextBox runat="server" type="password" CssClass="input" placeholder="Confirm Password" ID="txtResetPwdConfirm"></asp:TextBox>
                        <span toggle="#MainContent_txtResetPwdConfirm" class="fa-regular fa-eye-slash field-icon toggle-reset-password-confirm"></span>
                    </div>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Password is required" ValidationGroup="Reset" ControlToValidate="txtResetPwd" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Confirm Password is required" ValidationGroup="Reset" ControlToValidate="txtResetPwdConfirm" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CompareValidator runat="server" ErrorMessage="Passwords must match" CssClass="hidden-validator" Display="Dynamic" ControlToValidate="txtResetPwdConfirm" ControlToCompare="txtResetPwd" Operator="Equal" Type="String"></asp:CompareValidator>
                    <asp:Button ID="btnResetPwd" ValidationGroup="Reset" runat="server" CssClass="submit-btn" Text="Reset Password" OnClick="btnResetPwd_Click" />
                </asp:Panel>
            </div>
        </asp:Panel>

        <asp:Panel runat="server" ID="pnlSign" ClientIDMode="Static">
            <%-- log in --%>
            <asp:Panel runat="server" ID="pnlLogIn">
                <div class="signup">
                    <h2 class="form-title" id="signup"><span>or</span>Log in</h2>
                    <div class="form-holder">
                        <asp:TextBox runat="server" type="email" CssClass="input" placeholder="Email" ID="txtInEmail"></asp:TextBox>
                        <asp:TextBox runat="server" type="password" CssClass="input" placeholder="Password" ID="txtInPwd"></asp:TextBox>
                        <span toggle="#MainContent_txtInPwd" class="fa-regular fa-eye-slash field-icon toggle-in-password"></span>
                    </div>
                    <span style="display: flex; justify-content: space-between">

                        <asp:CheckBox ID="cbRememberMe" runat="server" Text="Remember Me" CssClass="cb-remember" />
                        <asp:HyperLink Text="Forgot password?" NavigateUrl="~/Sign.aspx?reset=1" runat="server" CssClass="btn-forgot"></asp:HyperLink>
                    </span>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Email is required" ValidationGroup="Login" ControlToValidate="txtInEmail" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="Password is required" ValidationGroup="Login" ControlToValidate="txtInPwd" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblInHint" runat="server" CssClass="hint"></asp:Label>
                    <button id="btnLogIn" runat="server" class="submit-btn loading-btn" onserverclick="btnLogIn_Click" validationgroup="Login">Login</button>
                </div>
            </asp:Panel>

            <%--register--%>
            <asp:Panel runat="server" ID="pnlSignUp">
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
                        <div class="cfturnstile"></div>
                        <button id="btnSignUp" validationgroup="Register" runat="server" class="submit-btn disabled loading-btn signup-btn" onserverclick="btnSignUp_Click" disabled>Sign up</button>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>

        <!-- hint modal -->
        <asp:Panel ID="modalBgPanel" runat="server" CssClass="modal-bg" Visible="false">
            <asp:Panel ID="modalContentPanel" runat="server" CssClass="modal-content" Visible="false">

                <div class="modal-header">
                    <h5>
                        <asp:Literal ID="litModalTitle" runat="server" /></h5>
                    <asp:Button ID="btnClose" runat="server" Text="X" CssClass="close-btn" type="button" OnClick="btnClose_Click" />
                </div>

                <div class="modal-body">
                    <p>
                        <asp:Literal ID="litModalContent" runat="server" />
                    </p>
                </div>
            </asp:Panel>
        </asp:Panel>
    </div>
</asp:Content>

