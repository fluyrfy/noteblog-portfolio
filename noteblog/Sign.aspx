<%@ Page Title="Sign" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Sign.aspx.cs" Inherits="noteblog.Sign" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link href="Shared/Sign.css" rel="stylesheet" />

    <div class="form-structor">
        <div class="signup">
            <h2 class="form-title" id="signup"><span>or</span>Log in</h2>
            <div class="form-holder">
                <asp:TextBox runat="server" type="email" CssClass="input" placeholder="Email" ID="txtInEmail"></asp:TextBox>
                <asp:TextBox runat="server" type="password" CssClass="input" placeholder="Password" ID="txtInPwd"></asp:TextBox>
            </div>
            <asp:CheckBox ID="cbRememberMe" runat="server" Text="Remember Me" CssClass="cb-remember" />
            <asp:RequiredFieldValidator runat="server" ErrorMessage="Email is required" ValidationGroup="Login" ControlToValidate="txtInEmail" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator runat="server" ErrorMessage="Password is required" ValidationGroup="Login" ControlToValidate="txtInPwd" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:Label ID="lblInHint" runat="server"></asp:Label>
            <asp:Button ValidationGroup="Login" runat="server" CssClass="submit-btn" Text="Log in" OnClick="btnLogIn_Click" />
        </div>
        <div class="login slide-up">
            <div class="center">
                <h2 class="form-title" id="login"><span>or</span>Sign up</h2>
                <div class="form-holder">
                    <asp:TextBox type="text" CssClass="input" ID="txtUpName" runat="server" placeholder="Name"></asp:TextBox>
                    <asp:TextBox type="email" CssClass="input" ID="txtUpEmail" runat="server" placeholder="Email"></asp:TextBox>
                    <asp:TextBox type="password" CssClass="input" ID="txtUpPwd" runat="server" placeholder="Password"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Name is required" ValidationGroup="Register" ControlToValidate="txtUpName" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Email is required" ValidationGroup="Register" ControlToValidate="txtUpEmail" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator runat="server" ErrorMessage="Password is required" ValidationGroup="Register" ControlToValidate="txtUpPwd" CssClass="hidden-validator" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ValidationGroup="Register"
                    ControlToValidate="txtUpEmail"
                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    ErrorMessage="Please enter a valid email" CssClass="hidden-validator" Display="Dynamic" />
                <asp:Button ValidationGroup="Register" runat="server" CssClass="submit-btn" Text="Sign up" OnClick="btnSignUp_Click" />
            </div>
        </div>
    </div>


    <script>
        console.clear();

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
    </script>
</asp:Content>

